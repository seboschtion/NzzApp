using Windows.Graphics.Display;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Providers.Settings;
using Sebastian.Toolkit.Application;

namespace NzzApp.UWP.ViewModels
{
    public class GalleryViewModel : ViewModel
    {
        private readonly ISettingsProvider _settingsProvider;

        private IGallery _gallery;
        private DisplayOrientations _initialOrientations;
        private string _fontFamily;

        public GalleryViewModel(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public IGallery Gallery
        {
            get { return _gallery; }
            set
            {
                _gallery = value;
                OnPropertyChanged();
            }
        }

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
            base.OnViewAttached();

            FontFamily = _settingsProvider.GetSettings().ArticleFontFamily;
        }

        public override void OnActivated(object parameter)
        {
            base.OnActivated(parameter);

            _initialOrientations = DisplayInformation.AutoRotationPreferences;
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;

            var gallery = parameter as IGallery;
            if (gallery != null)
            {
                Gallery = gallery;
            }
        }

        public override void OnDeactivated()
        {
            DisplayInformation.AutoRotationPreferences = _initialOrientations;
        }
    }
}
