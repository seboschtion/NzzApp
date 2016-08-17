using NzzApp.Data;
using NzzApp.Model.Contracts.Settings;
using NzzApp.Providers.BackgroundTasks;
using NzzApp.Providers.LiveTile;

namespace NzzApp.Providers.Settings
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly IDataProvider _dataProvider;
        private readonly IBackgroundTaskProvider _backgroundTaskProvider;
        private readonly ILiveTileProvider _liveTileProvider;

        private IAppSettings _appSettings;

        public SettingsProvider(IDataProvider dataProvider, IBackgroundTaskProvider backgroundTaskProvider, ILiveTileProvider liveTileProvider)
        {
            _dataProvider = dataProvider;
            _backgroundTaskProvider = backgroundTaskProvider;
            _liveTileProvider = liveTileProvider;
            _appSettings = _dataProvider.GetSettings();
            HandleFirstAppStart();
        }

        public bool IsFirstAppStart { get; private set; }

        public IAppSettings GetSettings()
        {
            return _appSettings;
        }

        public void SetSettings(IAppSettings settings)
        {
            _dataProvider.SetSettings(settings);
            _appSettings = settings;
        }

        private void HandleFirstAppStart()
        {
            IsFirstAppStart = _appSettings.IsFirstAppStart;
            if (IsFirstAppStart)
            {
                _appSettings.IsFirstAppStart = false;
                SetSettings(_appSettings);
            }
        }

        public async void EnableBreakingLiveTile()
        {
            if (!_appSettings.BreakingLiveTileEnabled)
            {
                _backgroundTaskProvider.RegisterBreakingLiveTileTask();
                _appSettings.BreakingLiveTileEnabled = true;
                SetSettings(_appSettings);
                await _liveTileProvider.RefreshLiveTileAsync();
            }
        }

        public void DisableBreakingLiveTile()
        {
            if (_appSettings.BreakingLiveTileEnabled)
            {
                _backgroundTaskProvider.UnregisterBreakingLiveTileTask();
                _appSettings.BreakingLiveTileEnabled = false;
                SetSettings(_appSettings);
                _liveTileProvider.ClearLiveTile();
            }
        }
    }
}
