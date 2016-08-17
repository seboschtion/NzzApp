using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using NzzApp.Model.Contracts.Departments;
using NzzApp.Model.Contracts.Settings;
using NzzApp.Providers.Articles;
using NzzApp.Providers.Departments;
using NzzApp.Providers.Helpers;
using NzzApp.Providers.LiveTile;
using NzzApp.Providers.Settings;
using NzzApp.Providers.Synchonisation;
using NzzApp.UWP.ContentDialogs;
using NzzApp.UWP.Helpers;
using NzzApp.UWP.Strings;
using Sebastian.Toolkit.Application;
using Sebastian.Toolkit.Logging;
using Sebastian.Toolkit.MVVM.Navigation;
using Sebastian.Toolkit.Util;
using Sebastian.Toolkit.Util.Extensions;

namespace NzzApp.UWP.ViewModels
{
    public enum HomeViewModelMessage
    {
        SyncStarted,
        SyncEnded
    }

    public class HomeViewModel : ViewModel
    {
        private readonly IDepartmentProvider _departmentProvider;
        private readonly IArticleProvider _articleProvider;
        private readonly ISyncProvider _syncProvider;
        private readonly ISettingsProvider _settingsProvider;
        private readonly INavigator _navigator;
        private readonly ILiveTileProvider _liveTileProvider;

        private ObservableCollection<HomeItemViewModel> _itemViewModels = new ObservableCollection<HomeItemViewModel>();
        private bool _loadSuccess;
        private int _fontSize;
        private string _fontFamily;

        public HomeViewModel(IDepartmentProvider departmentProvider, IArticleProvider articleProvider, ISyncProvider syncProvider, ISettingsProvider settingsProvider, INavigator navigator, ILiveTileProvider liveTileProvider)
        {
            _departmentProvider = departmentProvider;
            _articleProvider = articleProvider;
            _syncProvider = syncProvider;
            _settingsProvider = settingsProvider;
            _navigator = navigator;
            _liveTileProvider = liveTileProvider;
            AppSettings = _settingsProvider.GetSettings();

            Initialize();

            this.Logger().Debug("asldk");
        }

        public IAppSettings AppSettings { get; private set; }

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

        public override void OnViewAttached()
        {
            FontSize = AppSettings.ArticleFontSize;
            FontFamily = AppSettings.ArticleFontFamily;
        }

        public ObservableCollection<HomeItemViewModel> ItemViewModels
        {
            get { return _itemViewModels; }
            set
            {
                _itemViewModels = value;
                OnPropertyChanged();
            }
        }

        public string LoadingStatus { get; private set; } = IndependentUseResource.LoadString("WaitingPageLoading");

        public bool LoadSuccess
        {
            get { return _loadSuccess; }
            private set
            {
                _loadSuccess = value;
                OnPropertyChanged();
            }
        }

        private void Initialize()
        {   
            _syncProvider.FetchDepartmentsCompleted += SyncProviderOnFetchDepartmentsCompleted;
            _syncProvider.FetchArticlesForDepartmentCompleted += SyncProviderOnFetchArticlesForDepartmentCompleted;

            LoadDepartments();
        }

        private void LoadDepartments()
        {
            LoadingStatus = IndependentUseResource.LoadString("WaitingPageAlmostDone");
            var departments = _departmentProvider.GetMainDepartments().ToObservableCollection();
            foreach (var department in departments)
            {
                if (!_itemViewModels.Any(i => i.Department.Equals(department))
                    && ShowToday(department))
                {
                    _itemViewModels.Add(new HomeItemViewModel(department, _articleProvider, _syncProvider, _navigator, _liveTileProvider, _settingsProvider));
                }
            }
            LoadSuccess = true;
            SendMessage(HomeViewModelMessage.SyncEnded);
        }

        private bool ShowToday(IDepartment department)
        {
            if (!department.ShowAlways)
            {
                var dayOfWeek = (int)DateTime.Today.DayOfWeek;
                return department.ShowOn.Contains(dayOfWeek.ToString());
            }
            return true;
        }

        private void SyncProviderOnFetchDepartmentsCompleted(object sender, TaskResult taskResult)
        {
            if (!taskResult.Success && taskResult.ErrorType == ErrorTypes.NoInternet)
            {
                LoadingStatus = IndependentUseResource.LoadString("WaitingPageNoInternet");
                if (AppSettings.SuccessfullInitialization)
                {
                    DialogDispatcher.Show(IndependentUseResource.LoadString("NoInternetAlertText"),
                        IndependentUseResource.LoadString("NoInternetAlertTitle"), taskResult.ErrorType.ToString());
                }
                SendMessage(HomeViewModelMessage.SyncEnded);
                return;
            }

            LoadingStatus = IndependentUseResource.LoadString("WaitingPageSuccess");
            AppSettings.SuccessfullInitialization = true;
            _settingsProvider.SetSettings(AppSettings);
            LoadDepartments();
        }

        private void SyncProviderOnFetchArticlesForDepartmentCompleted(object sender, TaskResult<IDepartment[]> taskResult)
        {
            if (!taskResult.Success)
            {
                DialogDispatcher.ShowDebugError(taskResult.ErrorType.ToString());
                return;
            }

            foreach (var department in taskResult.Value)
            {
                var itemViewModel = _itemViewModels.FirstOrDefault(vm => vm.Department == department);
                itemViewModel?.LoadArticles();
            }
        }

        public ICommand ReloadRequestedByUserCommand => new RelayCommand<HomeItemViewModel>(viewModel =>
        {
            viewModel?.SyncArticles(true);
        });

        public void GotoSettings()
        {
            _navigator.Navigate<SettingsViewModel>();
        }
        
        public async void OpenArticleByUrl()
        {
            var dialog = new OpenArticleByUrl();
            dialog.RequestOpenArticle += DialogOnRequestOpenArticle;
            await dialog.ShowAsync();
        }
        
        public void DepartmentsTryAgain()
        {
            LoadingStatus = IndependentUseResource.LoadString("WaitingPageLoading");
            _syncProvider.FetchDepartments();
            SendMessage(HomeViewModelMessage.SyncStarted);
        }

        private void DialogOnRequestOpenArticle(object sender, string s)
        {
             _navigator.NavigateToArticleUrl(s);
        }

        public ICommand OnLinkClickedCommand => new RelayCommand<string>(url =>
        {
            _navigator.NavigateToArticleUrl(url);
        });
    }
}