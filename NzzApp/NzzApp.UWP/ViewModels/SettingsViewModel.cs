using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.Storage;
using NzzApp.Model.Contracts.Settings;
using NzzApp.Providers.Settings;
using NzzApp.Services;
using Sebastian.Toolkit.Application;
using Sebastian.Toolkit.MVVM.Navigation;
using static Windows.System.Launcher;

namespace NzzApp.UWP.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly INavigator _navigator;

        private readonly IAppSettings _appSettings;
        private bool _articleFontFamilyIsSerif;

        public SettingsViewModel(ISettingsProvider settingsProvider, INavigator navigator)
        {
            _settingsProvider = settingsProvider;
            _navigator = navigator;

            _appSettings = _settingsProvider.GetSettings();
        }

        public override void OnActivated(object parameter)
        {
            BreakingLiveTileEnabled = _appSettings.BreakingLiveTileEnabled;
            ArticleFontFamilyIsSerif = _appSettings.ArticleFontFamily == "Georgia";
        }

        public int SettingsFontSize => _appSettings.ArticleFontSize;

        public Version AppVersion => GetVersion();

        public bool BreakingLiveTileEnabled { get; private set; }

        public DateTime LastLiveTileTaskExecutionDate => _appSettings.LastLiveTileTaskExecutionDate;

        public bool ArticleFontFamilyIsSerif
        {
            get { return _articleFontFamilyIsSerif; }
            private set
            {
                _articleFontFamilyIsSerif = value;
                OnPropertyChanged();
            }
        }

        private Version GetVersion()
        {
            var assembly = typeof(App).AssemblyQualifiedName;
            var version = assembly.Substring(assembly.IndexOf("Version=") + "Version=".Length);
            version = version.Substring(0, version.IndexOf(","));
            return new Version(version);
        }

        public async void Copyright()
        {
            await LaunchUriAsync(new Uri(NzzRestServiceUrls.CopyrightAbsolute, UriKind.Absolute));
        }

        public async void Impressum()
        {
            await LaunchUriAsync(new Uri(NzzRestServiceUrls.ImpressumAbsolute, UriKind.Absolute));
        }

        public async void Contact()
        {
            await LaunchUriAsync(new Uri(NzzRestServiceUrls.ContactAbsolute, UriKind.Absolute));
        }

        public async void Feedback()
        {
            await LaunchUriAsync(new Uri("https://github.com/seboschtion/NzzApp", UriKind.Absolute));
        }

        private async Task ClearTemporaryFolderAsync()
        {
            var files = await ApplicationData.Current.TemporaryFolder.GetFilesAsync();
            foreach (var storageFile in files)
            {
                await storageFile.DeleteAsync();
            }
        }

        public async void RateApp()
        {
            var productId = CurrentApp.LinkUri.AbsolutePath.Split('/').Last();
            string url = "ms-windows-store://review/?ProductId=" + productId;
            await LaunchUriAsync(new Uri(url, UriKind.Absolute));
        }

        public async void GotoTwitterSebastian()
        {
            await LaunchUriAsync(new Uri("https://twitter.com/seboschtion", UriKind.Absolute));
        }

        public void EnableLiveTile()
        {
            _settingsProvider.EnableBreakingLiveTile();
        }

        public void DisableLiveTile()
        {
            _settingsProvider.DisableBreakingLiveTile();
        }

        public void SetFontSize(double newValue)
        {
            int value = Convert.ToInt32(newValue);
            _appSettings.ArticleFontSize = value;
            _settingsProvider.SetSettings(_appSettings);
        }

        public void ToggleFontFamily()
        {
            var newFontFamily = "Georgia";
            switch (_appSettings.ArticleFontFamily)
            {
                case "Georgia":
                    newFontFamily = "Segoe UI";
                    break;
                case "Segoe UI":
                    newFontFamily = "Georgia";
                    break;
            }
            _appSettings.ArticleFontFamily = newFontFamily;
            _settingsProvider.SetSettings(_appSettings);
        }
    }
}