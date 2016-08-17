using System;
using System.Linq;
using System.Windows.Input;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Contracts.BreakingNews;
using NzzApp.Model.Contracts.Departments;
using NzzApp.Model.Implementation.Articles;
using NzzApp.Providers.Articles;
using NzzApp.Providers.Helpers;
using NzzApp.Providers.LiveTile;
using NzzApp.Providers.Settings;
using NzzApp.Providers.Synchonisation;
using Sebastian.Toolkit.Application;
using Sebastian.Toolkit.MVVM.Navigation;
using Sebastian.Toolkit.Util;
using Sebastian.Toolkit.Util.Collections;

namespace NzzApp.UWP.ViewModels
{
    public class HomeItemViewModel : ViewModel
    {
        private readonly IArticleProvider _articleProvider;
        private readonly ISyncProvider _syncProvider;
        private readonly INavigator _navigator;
        private readonly ILiveTileProvider _liveTileProvider;
        private readonly ISettingsProvider _settingsProvider;

        private ReplaceableObservableCollection<ViewOptimizedArticle> _articles =
            new ReplaceableObservableCollection<ViewOptimizedArticle>(new ArticleEqualityComparer());

        private IDepartment _selectedDepartment;
        private IDepartment _department;
        private bool _synced;
        private IFullArticle _fullArticle;
        private IBreakingNews _breakingNews;

        public HomeItemViewModel(IDepartment department, IArticleProvider articleProvider, ISyncProvider syncProvider, INavigator navigator, ILiveTileProvider liveTileProvider, ISettingsProvider settingsProvider)
        {
            Department = department;
            _selectedDepartment = department;
            _articleProvider = articleProvider;
            _syncProvider = syncProvider;
            _navigator = navigator;
            _liveTileProvider = liveTileProvider;
            _settingsProvider = settingsProvider;

            Windows.ApplicationModel.Core.CoreApplication.Resuming += CoreApplicationOnResuming;

            LoadArticles();
            LoadBriefing();
        }

        private void CoreApplicationOnResuming(object sender, object o)
        {
            _synced = false;
        }

        public ReplaceableObservableCollection<ViewOptimizedArticle> Articles
        {
            get { return _articles; }
            set
            {
                _articles = value;
                OnPropertyChanged();
            }
        }

        public IBreakingNews BreakingNews
        {
            get { return _breakingNews; }
            set
            {
                _breakingNews = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasBreakingNews));
                if (HasBreakingNews && Articles.FirstOrDefault() != null && !Articles.FirstOrDefault().IsFreeSpace)
                {
                    Articles.Insert(0, new ViewOptimizedArticle { IsFreeSpace = true });
                }
            }
        }

        public bool HasBreakingNews => BreakingNews != null && Department.IsStartPage;

        public bool CanReload
            =>
                Department.DepartmentSerialisationType == DepartmentSerialisationType.Json ||
                Department.DepartmentSerialisationType == DepartmentSerialisationType.Article;

        public string HtmlDepartmentPath
        {
            get
            {
                if (Uri.IsWellFormedUriString(_selectedDepartment.Path, UriKind.Absolute))
                {
                    return _selectedDepartment.Path;
                }
                return $"http://headlines.nzz.ch{_selectedDepartment.Path}";
            }
        }

        public bool ShowSubDepartments => Department.SubDepartments.Count > 0;

        public IDepartment Department
        {
            get { return _department; }
            private set
            {
                _department = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanReload));
                OnPropertyChanged(nameof(HtmlDepartmentPath));
                OnPropertyChanged(nameof(ShowSubDepartments));
            }
        }

        public IFullArticle FullArticle
        {
            get { return _fullArticle; }
            set
            {
                _fullArticle = value;
                OnPropertyChanged();
            }
        }

        public bool HasItems => Articles != null && Articles.Count > 0;

        public void LoadArticles()
        {
            if (Department.DepartmentSerialisationType != DepartmentSerialisationType.Json)
            {
                return;
            }

            var articles = _articleProvider.GetArticles(Department);

            var newLead = articles.Max();
            if (newLead != null)
            {
                newLead.IsLeadArticle = newLead.Article.LeadImage.HasImage;
            }
            foreach (var article in Articles.ToList().Where(article => article.IsLeadArticle))
            {
                Articles.Remove(article);
            }

            if (Articles.Count > 0)
            {
                Articles.RemoveAt(0);
            }

            Articles.ReplaceItemsWithList(articles);

            if (HasBreakingNews || ShowSubDepartments)
            {
                Articles.Insert(0, new ViewOptimizedArticle { IsFreeSpace = true });
            }

            OnPropertyChanged(nameof(HasItems));
        }

        private void LoadBriefing()
        {
            if (Department.DepartmentSerialisationType != DepartmentSerialisationType.Article)
            {
                return;
            }

            SyncArticles(true);
        }

        public void SyncArticles(bool force)
        {
            if (_synced && !force)
            {
                return;
            }

            if (Department.DepartmentSerialisationType == DepartmentSerialisationType.Json)
            {
                _syncProvider.FetchArticles(new[] { Department });
                _synced = true;
            }
            else if (Department.DepartmentSerialisationType == DepartmentSerialisationType.Article)
            {
                _syncProvider.FetchFullArticleCompleted += SyncProviderOnFetchFullArticleCompleted;
                _syncProvider.FetchFullArticle(Department.Path);
                _synced = true;
            }
            if (Department.IsStartPage)
            {
                _syncProvider.BreakingNewsDownloadCompleted += SyncProviderOnBreakingNewsDownloadCompleted;
                _syncProvider.DownloadBreakingNews(5);
            }
        }

        private void SyncProviderOnBreakingNewsDownloadCompleted(object sender, TaskResult<IBreakingNews> taskResult)
        {
            _syncProvider.BreakingNewsDownloadCompleted -= SyncProviderOnBreakingNewsDownloadCompleted;
            if (taskResult.Success)
            {
                BreakingNews = taskResult.Value;
                if (_settingsProvider.GetSettings().BreakingLiveTileEnabled)
                {
                    _liveTileProvider.RefreshLiveTile(taskResult.Value);
                }
            }
        }

        private void SyncProviderOnFetchFullArticleCompleted(object sender, TaskResult<string> taskResult)
        {
            _syncProvider.FetchFullArticleCompleted -= SyncProviderOnFetchFullArticleCompleted;
            if (taskResult.Success)
            {
                var article = _articleProvider.GetArticleByPath(taskResult.Value);
                FullArticle = _articleProvider.GetFullArticle(article);
            }
        }

        public ICommand SyncArticlesCommand => new RelayCommand<object>(o =>
        {
            SyncArticles(true);
        });

        public ICommand ClickArticleCommand => new RelayCommand<ViewOptimizedArticle>(article =>
        {
            if (article != null)
            {
                _navigator.Navigate<ArticleViewModel>(article.Article.Path);
            }
        });

        public ICommand ClickDepartmentCommand => new RelayCommand<IDepartment>(department =>
        {
            _navigator.Navigate<DepartmentViewModel>(department);
        });

        public ICommand ClickBreakingNews => new RelayCommand<IBreakingNews>(breakingNews =>
        {
            _navigator.Navigate<BreakingNewsViewModel>(breakingNews);
        });

        public ICommand SelectSubDepartment => new RelayCommand<IDepartment>(department =>
        {
            _selectedDepartment = department;
            OnPropertyChanged(nameof(HtmlDepartmentPath));
        });
    }
}