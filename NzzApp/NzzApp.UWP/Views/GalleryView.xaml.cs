using NzzApp.UWP.ViewModels;
using Sebastian.Toolkit.Application;

namespace NzzApp.UWP.Views
{
    public sealed partial class GalleryView : View
    {
        private GalleryViewModel GalleryViewModel => (GalleryViewModel) DataContext;

        public GalleryView()
        {
            this.InitializeComponent();
        }
    }
}