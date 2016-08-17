using Windows.ApplicationModel.Store;
using Sebastian.Toolkit.Util;

namespace NzzApp.UWP.Helpers
{
    public class ProductItemWrapper : PropertyChangedBase
    {
        private bool _processing;
        private ProductListing _product;
        private string _imageUri;
        private string _productId;
        private bool _available;

        public string ImageUri
        {
            get { return _imageUri; }
            set
            {
                _imageUri = value;
                OnPropertyChanged();
            }
        }

        public string ProductId
        {
            get { return _productId; }
            set
            {
                _productId = value; 
                OnPropertyChanged();
            }
        }

        public ProductListing Product
        {
            get { return _product; }
            set
            {
                _product = value; 
                OnPropertyChanged();
            }
        }

        public bool Processing
        {
            get { return _processing; }
            set
            {
                _processing = value;
                OnPropertyChanged();
            }
        }

        public bool Available
        {
            get { return _available; }
            set
            {
                _available = value; 
                OnPropertyChanged();
            }
        }
    }
}