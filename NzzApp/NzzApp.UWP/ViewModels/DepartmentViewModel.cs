using System;
using System.Windows.Input;
using Windows.System;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Contracts.Departments;
using NzzApp.Model.Implementation.Articles;
using NzzApp.Providers.Articles;
using NzzApp.Providers.Helpers;
using NzzApp.Providers.Synchonisation;
using Sebastian.Toolkit.Application;
using Sebastian.Toolkit.MVVM.Navigation;
using Sebastian.Toolkit.Util;
using Sebastian.Toolkit.Util.Collections;

namespace NzzApp.UWP.ViewModels
{
    public class DepartmentViewModel : ViewModel
    {
        private readonly IArticleProvider _articleProvider;
        private readonly ISyncProvider _syncProvider;
        private readonly INavigator _navigator;

        private ReplaceableObservableCollection<ViewOptimizedArticle> _articles = new ReplaceableObservableCollection<ViewOptimizedArticle>(new ArticleEqualityComparer());
        private IDepartment _department;

        public DepartmentViewModel(IArticleProvider articleProvider, ISyncProvider syncProvider, INavigator navigator)
        {
            _articleProvider = articleProvider;
            _syncProvider = syncProvider;
            _navigator = navigator;
        }

        public ReplaceableObservableCollection<ViewOptimizedArticle> Articles
        {
            get { return _articles; }
            set
            {
                _articles = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasItems));
            }
        }

        public IDepartment Department
        {
            get { return _department; }
            set
            {
                _department = value;
                OnPropertyChanged();
            }
        }

        public bool HasItems => Articles.Count > 0;

        public ICommand OpenArticleCommand => new RelayCommand<IArticle>(article =>
        {
            _navigator.Navigate<ArticleViewModel>(article.Path);
        });

        public override void OnActivated(object parameter)
        {
            base.OnActivated(parameter);
            Department = (IDepartment)parameter;
            LoadArticles();
            SyncArticles();
        }

        private void LoadArticles()
        {
            Articles.ReplaceItemsWithList(_articleProvider.GetArticles(Department));
            OnPropertyChanged(nameof(HasItems));
        }

        public void SyncArticles()
        {
            _syncProvider.FetchArticlesForDepartmentCompleted += SyncProviderOnFetchArticlesForDepartmentCompleted;
            _syncProvider.FetchArticles(new[] { Department });
        }

        private void SyncProviderOnFetchArticlesForDepartmentCompleted(object sender, TaskResult<IDepartment[]> taskResult)
        {
            _syncProvider.FetchArticlesForDepartmentCompleted -= SyncProviderOnFetchArticlesForDepartmentCompleted;
            if (taskResult.Success)
            {
                LoadArticles();
            }
            else
            {
                DialogDispatcher.ShowDebugError(taskResult.ErrorType.ToString());
            }
        }

        public async void TryOpenInBrowser()
        {
            await Launcher.LaunchUriAsync(new Uri("http://nzz.ch" + Department.Path.Replace("/api/departments", "")));
        }
    }
}