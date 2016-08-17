using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NzzApp.Model.Contracts.Departments;
using NzzApp.UWP.ViewModels;

namespace NzzApp.UWP.Selectors
{
    public class DepartmentOverviewTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ArticlesListViewDataTemplate { get; set; }
        public DataTemplate BrowserDataTemplate { get; set; }
        public DataTemplate ArticleDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var viewModel = item as HomeItemViewModel;

            if (viewModel != null)
            {
                switch (viewModel.Department.DepartmentSerialisationType)
                {
                    case DepartmentSerialisationType.Html:
                        return BrowserDataTemplate;
                    case DepartmentSerialisationType.Json:
                        return ArticlesListViewDataTemplate;
                    case DepartmentSerialisationType.Article:
                        return ArticleDataTemplate;
                }
            }

            return null;
        }
    }
}
