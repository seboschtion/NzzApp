using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using HtmlAgilityPack;
using NzzApp.UWP.Styles.Themes;

namespace NzzApp.UWP.Controls
{
    public class HtmlRichTextBlockExtensions
    {
        public static readonly DependencyProperty HtmlProperty =
            DependencyProperty.RegisterAttached("Html", typeof (string), typeof (HtmlRichTextBlockExtensions),
                new PropertyMetadata(null, HtmlChanged));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof (ICommand), typeof (HtmlRichTextBlockExtensions),
                new PropertyMetadata(null));

        public static void SetHtml(DependencyObject obj, string source)
        {
            obj.SetValue(HtmlProperty, source);
        }

        public static string GetHtml(DependencyObject obj)
        {
            return (string)obj.GetValue(HtmlProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand source)
        {
            obj.SetValue(CommandProperty, source);
        }

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        private static void HtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var richTextBlock = d as RichTextBlock;
            if (richTextBlock == null)
            {
                return;
            }

            var body = e.NewValue as string;
            if (body == null)
            {
                return;
            }
            
            richTextBlock.Blocks.Clear();
            var blockParagraph = new Paragraph();
            var doc = new HtmlDocument();
            doc.LoadHtml(HtmlEntity.DeEntitize(body));
            foreach (var run in GenerateInlines(doc.DocumentNode, new NodeProperties()))
            {
                blockParagraph.Inlines.Add(run);
            }
            richTextBlock.Blocks.Add(blockParagraph);
        }

        private static IEnumerable<Inline> GenerateInlines(HtmlNode node, NodeProperties properties)
        {
            switch (node.Name)
            {
                case "em":
                    Em(properties);
                    break;
                case "strong":
                    Strong(properties);
                    break;
                case "a":
                    var href = node.GetAttributeValue("href", null);
                    var target = node.GetAttributeValue("target", null);
                    if (!string.IsNullOrWhiteSpace(href))
                    {
                        A(properties, href, target);
                    }
                    break;
            }

            if (!node.HasChildNodes)
            {
                yield return BuildInline(node.InnerText, properties);
            }

            if (node.HasChildNodes)
            {
                foreach (var childNode in node.ChildNodes)
                {
                    var inlines = GenerateInlines(childNode, properties.CloneBasic());
                    foreach (var inline in inlines)
                    {
                        yield return inline;
                    }
                }
            }
        }

        private static Inline BuildInline(string text, NodeProperties properties)
        {
            var run = new Run
            {
                Text = text.Replace("\n", " ").Replace("  ", " ").Replace("  ", " "),
                FontWeight = properties.FontWeight,
                FontStyle = properties.FontStyle
            };

            if(properties.IsLink)
            {
                if (properties.IsExternalLink)
                {
                    var link = new Hyperlink { NavigateUri = new Uri(properties.LinkTarget, UriKind.Absolute) };
                    link.Foreground = ThemeResource.GetBrush(CommonBrush.ThemeBrush);
                    link.Inlines.Add(run);
                    link.Inlines.Add(new Run {Text = "\xE788", FontFamily = new FontFamily("Segoe MDL2 Assets"), FontSize = 12});
                    return link;
                }
                else
                {
                    var link = new InternalLink { NavigateTo = properties.LinkTarget };
                    var hyperlink = new Hyperlink();
                    hyperlink.Inlines.Add(run);
                    hyperlink.Foreground = ThemeResource.GetBrush(CommonBrush.ThemeBrush);
                    link.AddHyperlink(hyperlink);
                    link.Click += LinkOnClick;
                    return link;
                }
            }

            return run;
        }

        private static void LinkOnClick(object sender, string s)
        {
            var hyperlink = (Hyperlink) sender;
            var richTextBlock = VisualTreeHelper.GetParent(hyperlink);
            var command = GetCommand(richTextBlock);
            command?.Execute(s);
        }

        private static void Em(NodeProperties style)
        {
            style.FontStyle = FontStyle.Italic;
        }

        private static void Strong(NodeProperties style)
        {
            style.FontWeight = FontWeights.Bold;
        }

        private static void A(NodeProperties style, string href, string target)
        {
            style.LinkTarget = href;
            style.IsExternalLink =
                (target != null
                && style.IsLink
                && target.Contains("_blank"))
                || !Regex.IsMatch(href, "(http|https):\\/\\/(www.|)nzz.ch\\/((.*)-|-|)([0-9]|ld).[0-9]*");
        }
    }

    public class NodeProperties
    {
        public FontStyle FontStyle { get; set; } = FontStyle.Normal;
        public FontWeight FontWeight { get; set; } = FontWeights.Normal;
        public bool IsLink => Uri.IsWellFormedUriString(LinkTarget, UriKind.Absolute);
        public string LinkTarget { get; set; }
        public bool IsExternalLink { get; set; }

        public NodeProperties CloneBasic()
        {
            return new NodeProperties()
            {
                FontStyle = FontStyle,
                FontWeight = FontWeight,
                LinkTarget = LinkTarget,
                IsExternalLink = IsExternalLink
            };
        }
    }
}
