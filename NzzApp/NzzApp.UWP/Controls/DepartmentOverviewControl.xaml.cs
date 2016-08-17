using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using NzzApp.Model.Implementation.Articles;
using NzzApp.UWP.ViewModels;
using Sebastian.Toolkit.Util.Extensions;

namespace NzzApp.UWP.Controls
{
    public sealed partial class DepartmentOverviewControl : UserControl, INotifyPropertyChanged
    {
        public HomeItemViewModel HomeItemViewModel => (HomeItemViewModel) this.DataContext;

        public DepartmentOverviewControl()
        {
            this.InitializeComponent();

            Loaded += OnLoaded;
        }

        public bool MoveContent => HomeItemViewModel.ShowSubDepartments || HomeItemViewModel.HasBreakingNews;

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            HomeItemViewModel.PropertyChanged += HomeItemViewModelOnPropertyChanged;
            LoadComponents();
        }

        private void HomeItemViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(HomeItemViewModel.HasItems)
                || propertyChangedEventArgs.PropertyName == nameof(HomeItemViewModel.HasBreakingNews))
            {
                LoadComponents();
            }
        }

        private void LoadComponents()
        {
            if (!HomeItemViewModel.HasItems)
            {
                FindName(nameof(NoDataStackPanel));
            }
            if (HomeItemViewModel.ShowSubDepartments && !HomeItemViewModel.HasBreakingNews)
            {
                FindName(nameof(SubDepartmentsControl));
                AttachParallaxBehavior(this.SubDepartmentsControl);
            }
            if (HomeItemViewModel.HasBreakingNews)
            {
                FindName(nameof(BreakingNewsControl));
                AttachParallaxBehavior(this.BreakingNewsControl);
            }
            OnPropertyChanged(nameof(MoveContent));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void List_OnChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            var article = args.Item as ViewOptimizedArticle;
            if (article != null)
            {
                if (article.IsFreeSpace)
                {
                    args.ItemContainer = new ListViewItem
                    {
                        Height = 72
                    };
                }
                else if (article.IsLeadArticle)
                {
                    args.ItemContainer = new ListViewItem
                    {
                        ContentTemplate =
                            Application.Current.Resources["LeadArticleDataTemplate"] as DataTemplate,
                        Style =
                            Application.Current.Resources["StretchedListViewItemStyle"] as Style,
                    };
                }
                else
                {
                    args.ItemContainer = new ListViewItem
                    {
                        ContentTemplate =
                            Application.Current.Resources["ArticleEntryDataTemplate"] as DataTemplate,
                        Style =
                            Application.Current.Resources["StretchedListViewItemStyle"] as Style,
                    };
                }
            }
            args.IsContainerPrepared = true;
        }

        private void AttachParallaxBehavior(UIElement element)
        {
            var behavior = new ParallaxBehavior();
            behavior.SetValue(ParallaxBehavior.ParallaxContentProperty, element);
            behavior.SetValue(ParallaxBehavior.ParallaxMultiplierProperty, 0.4);
            var behaviors = Interaction.GetBehaviors(List);
            if (behaviors.Count == 0)
            {
                behaviors.Add(behavior);
            }
        }

        private void List_OnLoaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = this.List.GetChildOfType<ScrollViewer>();
            if (scrollViewer != null)
            {
                scrollViewer.ViewChanged += ScrollViewerOnViewChanged;
            }
        }

        private void ScrollViewerOnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer) sender;
            ScrollToTopButton.Visibility = scrollViewer.VerticalOffset > 155
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void ScrollToTopButton_OnClick(object sender, RoutedEventArgs e)
        {
            var scrollViewer = this.List.GetChildOfType<ScrollViewer>();
            scrollViewer.ChangeView(null, 0, null);
        }
    }
}