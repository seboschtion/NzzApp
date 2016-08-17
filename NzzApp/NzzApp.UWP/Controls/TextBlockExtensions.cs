using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using HtmlAgilityPack;

namespace NzzApp.UWP.Controls
{
    public class TextBlockExtensions
    {
        public static readonly DependencyProperty HtmlProperty =
           DependencyProperty.RegisterAttached("Html", typeof(string), typeof(TextBlockExtensions),
               new PropertyMetadata(null, HtmlChanged));

        public static void SetHtml(DependencyObject obj, string source)
        {
            obj.SetValue(HtmlProperty, source);
        }

        public static string GetHtml(DependencyObject obj)
        {
            return (string)obj.GetValue(HtmlProperty);
        }

        private static void HtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = d as TextBlock;
            if (textBlock == null)
            {
                return;
            }

            var text = e.NewValue as string;
            if (text == null)
            {
                return;
            }

            text = HtmlEntity.DeEntitize(text);

            textBlock.Inlines.Clear();
            textBlock.Inlines.Add(new Run() {Text = text.Replace("\n", " ").Replace("  ", " ").Replace("  ", " ")});
        }
    }
}
