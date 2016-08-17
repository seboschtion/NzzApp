using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NzzApp.UWP.Controls;
using NzzApp.UWP.ViewModels;

namespace NzzApp.UWP.Views
{
    public sealed partial class HomeView : WaitingPage
    {
        public HomeViewModel HomeViewModel => (HomeViewModel)this.DataContext;

        public HomeView()
        {
            this.InitializeComponent();
            
            Windows.ApplicationModel.Core.CoreApplication.Resuming += CoreApplicationOnResuming;
        }

        public override void OnInitialized()
        {
            GotoState(WaitStates.Wait);
            if (HomeViewModel.AppSettings.SuccessfullInitialization)
            {
                GotoState(WaitStates.EndWait);
            }
        }

        private bool IsGotoStartPageEnabled => MainPivot?.SelectedIndex != 0;

        public override void ReceiveMessage(object message)
        {
            var homeViewModelMessage = (HomeViewModelMessage) message;
            switch (homeViewModelMessage)
            {
                case HomeViewModelMessage.SyncStarted:
                    GotoState(WaitStates.Wait);
                    break;
                case HomeViewModelMessage.SyncEnded:
                    GotoState(WaitStates.EndWait);
                    break;
            }
        }

        private void CoreApplicationOnResuming(object sender, object o)
        {
            SyncCurrentPivotItem(true);
        }

        private void MainPivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IsGotoStartPageEnabled));
            SyncCurrentPivotItem(false);
        }

        private void SyncCurrentPivotItem(bool force)
        {
            var itemViewModel = (HomeItemViewModel)MainPivot.SelectedItem;
            itemViewModel?.SyncArticles(force);
        }
        
        protected override ContentPresenter GetContentPresenter()
        {
            return this.ContentPresenter;
        }

        protected override string GetDisplayStatus()
        {
            return HomeViewModel.LoadingStatus;
        }

        protected override void OnWaitEnded()
        {
            FindName(HomeViewModel.AppSettings.SuccessfullInitialization ? nameof(MainPivot) : nameof(NoDataStackPanel));
            if (HomeViewModel.AppSettings.SuccessfullInitialization && NoDataStackPanel != null)
            {
                NoDataStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void HomeButton_OnClick(object sender, RoutedEventArgs e)
        {
            MainPivot.SelectedIndex = 0;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            throw new Exception("This exception was thrown manually.");
        }
    }
}