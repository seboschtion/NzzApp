using System.Collections.ObjectModel;
using System.Linq;
using Windows.Graphics.Display;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Contracts.Images;
using NzzApp.Model.Implementation.Articles;

namespace NzzApp.Model.Implementation.Images
{
    public class Gallery : Articles.RelatedContent, IGallery
    {
        private string _id;
        private string _title;

        public string Id
        {
            get { return _id; }
            set
            {
                _id = value; 
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value; 
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IImage> Images { get; } = new ObservableCollection<IImage>();
        public string GalleryHeight => GetGalleryHeight();
        public override RelatedContentType Type => RelatedContentType.Gallery;

        private string GetGalleryHeight()
        {
            var displayInformation = DisplayInformation.GetForCurrentView();
            var density = new DensityAwareSize((int)displayInformation.LogicalDpi);
            return density.GetGalleryHeight();
        }
    }
}
