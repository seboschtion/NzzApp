using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.Graphics.Display;
using Windows.System;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Contracts.Departments;
using NzzApp.Providers.Articles;
using NzzApp.Providers.Departments;
using NzzApp.Providers.Helpers;
using NzzApp.Providers.Settings;
using NzzApp.Providers.Synchonisation;
using NzzApp.UWP.Helpers;
using NzzApp.UWP.Strings;
using Sebastian.Toolkit.Application;
using Sebastian.Toolkit.MVVM.Navigation;
using Sebastian.Toolkit.Util;

namespace NzzApp.UWP.ViewModels
{
    public class ArticleViewModel : ViewModel
    {
        private readonly IArticleProvider _articleProvider;
        private readonly IDepartmentProvider _departmentProvider;
        private readonly ISyncProvider _syncProvider;
        private readonly INavigator _navigator;
        private readonly ISettingsProvider _settingsProvider;

        private IFullArticle _fullArticle;
        private ObservableCollection<IDepartment> _departments = new ObservableCollection<IDepartment>();
        private string _articlePath;
        private bool _loadSuccess;
        private int _fontSize;
        private string _fontFamily;

        public ArticleViewModel(
            IArticleProvider articleProvider,
            IDepartmentProvider departmentProvider,
            ISyncProvider syncProvider,
            INavigator navigator,
            ISettingsProvider settingsProvider)
        {
            _articleProvider = articleProvider;
            _departmentProvider = departmentProvider;
            _syncProvider = syncProvider;
            _navigator = navigator;
            _settingsProvider = settingsProvider;
        }

        public event EventHandler ArticleLoaded;
        public event EventHandler ArticleLoadError;
        public event EventHandler ArticleRequested;

        public IFullArticle FullArticle
        {
            get { return _fullArticle; }
            set
            {
                _fullArticle = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(SubtitleString));
                OnPropertyChanged(nameof(LoadSuccess));
                SetDepartments();
            }
        }

        public ObservableCollection<IDepartment> Departments
        {
            get { return _departments; }
            set
            {
                _departments = value;
                OnPropertyChanged();
            }
        }

        public bool LoadSuccess => FullArticle != null;

        public string SubtitleString => GetSubtitleString();

        public string LoadingStatus { private set; get; } = IndependentUseResource.LoadString("WaitingPageLoading");

        public int FontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(LineHeight));
            }
        }

        public int LineHeight => FontSize + 10;

        public string FontFamily
        {
            get { return _fontFamily; }
            set
            {
                _fontFamily = value;
                OnPropertyChanged();
            }
        }

        public override void OnActivated(object parameter)
        {
            LoadArticle((string)parameter);
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
        }

        public override void OnViewAttached()
        {
            FontSize = _settingsProvider.GetSettings().ArticleFontSize;
            FontFamily = _settingsProvider.GetSettings().ArticleFontFamily;
        }

        public override void OnDeactivated()
        {
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
        }

        public void LoadArticle(string articlePath)
        {
            LoadingStatus = IndependentUseResource.LoadString("WaitingPageLoading");
            OnArticleRequested();
            _articlePath = articlePath;
            if (!_articleProvider.IsFullArticleReady(_articlePath))
            {
                _syncProvider.FetchFullArticleCompleted += SyncProviderOnFetchFullArticleCompleted;
                _syncProvider.FetchFullArticle(_articlePath);
            }
            else
            {
                var article = _articleProvider.GetArticleByPath(_articlePath);
                SetFullArticle(article);
            }
        }

        private void SetFullArticle(IArticle article)
        {
            var fullArticle = _articleProvider.GetFullArticle(article);
            if (fullArticle != null)
            {
                FullArticle = fullArticle;
                OnArticleLoaded();
                LoadingStatus = IndependentUseResource.LoadString("WaitingPageSuccess");
            }
        }

        private void SetDepartments()
        {
            Departments.Clear();
            var departments = _departmentProvider.GetDepartmentsForArticle(FullArticle.Article);
            foreach (var department in departments)
            {
                Departments.Add(department);
            }
        }

        private void SyncProviderOnFetchFullArticleCompleted(object sender, TaskResult<string> taskResult)
        {
            _syncProvider.FetchFullArticleCompleted -= SyncProviderOnFetchFullArticleCompleted;
            if (taskResult.Success)
            {
                var articlePath = taskResult.Value;
                var article = _articleProvider.GetArticleByPath(articlePath);
                SetFullArticle(article);
            }
            else
            {
                DialogDispatcher.ShowDebugError(taskResult.ErrorType.ToString());
                LoadingStatus = IndependentUseResource.LoadString("WaitingPageNoInternet");
                OnArticleLoadError();
            }
        }

        private string GetSubtitleString()
        {
            if (FullArticle == null)
            {
                return string.Empty;
            }

            string limiter = !string.IsNullOrWhiteSpace(FullArticle.Article.SubTitle) && FullArticle.IsAuthorAvailable
                ? " • "
                : string.Empty;
            return $"{FullArticle.Article.SubTitle }{limiter}{FullArticle.AuthorsText}";
        }

        public ICommand OnLinkClickedCommand => new RelayCommand<string>(url =>
        {
            _navigator.NavigateToArticleUrl(url);
        });

        public async void OpenInBrowser()
        {
            await Launcher.LaunchUriAsync(new Uri(FullArticle.WebUrl, UriKind.Absolute));
        }

        public ICommand OpenRelatedArticleCommand => new RelayCommand<IArticle>(article =>
        {
            _navigator.Navigate<ArticleViewModel>(article.Path);
        });

        public ICommand NavigateToDepartmentCommand => new RelayCommand<IDepartment>(department =>
        {
            _navigator.Navigate<DepartmentViewModel>(department);
        });

        protected virtual void OnArticleLoaded()
        {
            ArticleLoaded?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnArticleLoadError()
        {
            ArticleLoadError?.Invoke(this, EventArgs.Empty);
        }

        public void Retry()
        {
            LoadArticle(_articlePath);
        }

        public ICommand FullScreenGallery => new RelayCommand<IGallery>(gallery =>
        {
            _navigator.Navigate<GalleryViewModel>(gallery);
        });

        protected virtual void OnArticleRequested()
        {
            ArticleRequested?.Invoke(this, EventArgs.Empty);
        }

        public async void TryOpenInBrowser()
        {
            await Launcher.LaunchUriAsync(new Uri("http://nzz.ch" + _articlePath));
        }

        public void GotoSettings()
        {
            _navigator.Navigate<SettingsViewModel>();
        }
    }
}