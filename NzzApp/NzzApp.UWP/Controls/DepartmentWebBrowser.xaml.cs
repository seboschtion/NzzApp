using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NzzApp.UWP.ViewModels;


namespace NzzApp.UWP.Controls
{
    public sealed partial class DepartmentWebBrowser : UserControl
    {
        public HomeItemViewModel HomeItemViewModel => (HomeItemViewModel)this.DataContext;

        public DepartmentWebBrowser()
        {
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (HomeItemViewModel.ShowSubDepartments)
            {
                FindName(nameof(SubDepartmentsControl));
            }
        }
    }
}
