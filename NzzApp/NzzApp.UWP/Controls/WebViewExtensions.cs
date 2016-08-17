using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NzzApp.UWP.Controls
{
    public class WebViewExtensions
    {
        public static readonly DependencyProperty HtmlProperty =
            DependencyProperty.RegisterAttached("Html", typeof (string), typeof (WebViewExtensions),
                new PropertyMetadata(string.Empty, OnHtmlChanged));

        public static string GetHtml(DependencyObject obj)
        {
            return (string) obj.GetValue(HtmlProperty);
        }

        public static void SetHtml(DependencyObject obj, string value)
        {
            obj.SetValue(HtmlProperty, value);
        }

        private static void OnHtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var webView = (WebView) d;
            var content = (string) e.NewValue;
            webView.NavigateToString(content);
        }
    }
}