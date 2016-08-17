using NzzApp.Services;
using NzzApp.UWP.ViewModels;
using Sebastian.Toolkit.MVVM.Navigation;

namespace NzzApp.UWP.Helpers
{
    public static class NavigatorExtensions
    {
        public static void NavigateToArticleUrl(this INavigator navigator, string url)
        {
            var splitted = url.Split('-');
            if (splitted.Length > 0 && !string.IsNullOrWhiteSpace(splitted[splitted.Length - 1]))
            {
                var id = splitted[splitted.Length - 1];
                var path = NzzRestServiceUrls.ArticleRelative + id;
                navigator.Navigate<ArticleViewModel>(path);
            }
        }
    }
}