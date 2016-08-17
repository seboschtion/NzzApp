using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NzzApp.UWP.Helpers;

namespace NzzApp.UWP.Controls
{
    public sealed partial class IapDetailsContentDialog : ContentDialog, INotifyPropertyChanged
    {
        private ProductItemWrapper _productItemWrapper;

        public event EventHandler Accept;

        public IapDetailsContentDialog()
        {
            this.InitializeComponent();
        }

        public ProductItemWrapper ProductItemWrapper
        {
            get { return _productItemWrapper; }
            set
            {
                _productItemWrapper = value; 
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Accept?.Invoke(this, EventArgs.Empty);
        }
    }
}
