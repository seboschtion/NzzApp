using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NzzApp.Model.Contracts.Departments;

namespace NzzApp.UWP.Controls
{
    public sealed partial class SubDepartmentsControl : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.RegisterAttached("ItemsSource", typeof(IEnumerable<IDepartment>), typeof(SubDepartmentsControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(SubDepartmentsControl),
                new PropertyMetadata(null));

        private bool _listIsOpen;

        public SubDepartmentsControl()
        {
            this.InitializeComponent();
            this.RootGrid.DataContext = this;
        }

        public IEnumerable<IDepartment> ItemsSource
        {
            get { return (IEnumerable<IDepartment>) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (_listIsOpen)
            {
                this.HideFullListStoryboard.Completed += HideFullListStoryboardOnCompleted;
                this.HideFullListStoryboard.Begin();
            }
            else
            {
                this.Overlay.Visibility = Visibility.Visible;
                this.ShowFullListStoryboard.Begin();
            }
            _listIsOpen = !_listIsOpen;
        }

        private void HideFullListStoryboardOnCompleted(object sender, object o)
        {
            this.HideFullListStoryboard.Completed -= HideFullListStoryboardOnCompleted;
            this.Overlay.Visibility = Visibility.Collapsed;
        }

        private void RichTextBlock_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (RichTextBlock.ActualHeight > 48)
            {
                FindName(nameof(ToggleButton));
            }
        }
    }
}
