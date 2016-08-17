using System.ComponentModel;
using Windows.UI.Xaml;
using NzzApp.UWP.ViewModels;
using Sebastian.Toolkit.Application;

namespace NzzApp.UWP.Views
{
    public sealed partial class DepartmentView : View
    {
        public DepartmentViewModel DepartmentViewModel => (DepartmentViewModel) this.DataContext;

        public DepartmentView()
        {
            this.InitializeComponent();

            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            DepartmentViewModel.PropertyChanged += DepartmentViewModelOnPropertyChanged;
            ActivateControls();
        }

        private void DepartmentViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DepartmentViewModel.HasItems))
            {
                ActivateControls();
            }
        }

        private void ActivateControls()
        {
            FindName(DepartmentViewModel.HasItems ? nameof(ArticlesListView) : nameof(NoDataStackPanel));
        }
    }
}
