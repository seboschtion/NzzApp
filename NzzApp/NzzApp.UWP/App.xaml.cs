using Windows.ApplicationModel.Activation;
using Windows.Graphics.Display;
using NzzApp.Data;
using NzzApp.Data.SQLite;
using NzzApp.Providers.Articles;
using NzzApp.Providers.BackgroundTasks;
using NzzApp.Providers.Departments;
using NzzApp.Providers.LiveTile;
using NzzApp.Providers.Settings;
using NzzApp.Providers.Synchonisation;
using NzzApp.Services;
using NzzApp.UWP.ViewModels;
using Sebastian.Toolkit.Application;
using Sebastian.Toolkit.MVVM.Container;
using Sebastian.Toolkit.MVVM.Navigation;

namespace NzzApp.UWP
{
    sealed partial class App : SebastianApplication
    {
        private ISyncProvider _syncProvider;
        private ISettingsProvider _settingsProvider;
        private IBackgroundTaskProvider _backgroundTaskProvider;

        public override void Initialize()
        {
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
        }

        public override void SetupContainer(IoC container)
        {
            container.Singleton<IDataProvider, SQLiteDataProvider>();
            _backgroundTaskProvider = container.Singleton<IBackgroundTaskProvider, BackgroundTaskProvider>();
            container.Singleton<INzzRestService, NzzRestService>();
            _syncProvider = container.Singleton<ISyncProvider, SyncProvider>();
            container.Singleton<ILiveTileProvider, LiveTileProvider>();
            _settingsProvider = container.Singleton<ISettingsProvider, SettingsProvider>();
            container.Singleton<IDepartmentProvider, DepartmentProvider>();
            container.Singleton<IArticleProvider, ArticleProvider>();

            SynchronizeData();
        }

        public override void NavigateToFirstView(INavigator navigator)
        {
            navigator.Navigate<HomeViewModel>();
        }

        public override void Launched(LaunchActivatedEventArgs e)
        {
            if (e.PreviousExecutionState == ApplicationExecutionState.NotRunning)
            {
                var settings = _settingsProvider.GetSettings();
                if (settings.DisableLiveTileTask)
                {
                    _backgroundTaskProvider.UnregisterBreakingLiveTileTask();
                    settings.DisableLiveTileTask = false;
                    _settingsProvider.SetSettings(settings);
                }
                if (settings.BreakingLiveTileEnabled)
                {
                    _backgroundTaskProvider.RegisterBreakingLiveTileTask();
                }
            }
        }

        private void SynchronizeData()
        {
            _syncProvider.DeleteExpiredData();
            _syncProvider.FetchDepartments();
        }
    }
}