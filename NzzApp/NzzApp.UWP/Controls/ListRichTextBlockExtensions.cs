using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using NzzApp.Model.Contracts.Departments;

namespace NzzApp.UWP.Controls
{
    public class ListRichTextBlockExtensions
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.RegisterAttached("ItemsSource", typeof(IEnumerable<IDepartment>), typeof(ListRichTextBlockExtensions),
                new PropertyMetadata(null, ItemsSourcePropertyChangedCallback));

        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.RegisterAttached("ButtonStyle", typeof(Style), typeof(ListRichTextBlockExtensions),
                new PropertyMetadata(null));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ListRichTextBlockExtensions),
                new PropertyMetadata(null));

        public static void SetItemsSource(DependencyObject obj, IEnumerable<IDepartment> source)
        {
            obj.SetValue(ItemsSourceProperty, source);
        }

        public static IEnumerable<IDepartment> GetItemsSource(DependencyObject obj)
        {
            return (IEnumerable<IDepartment>)obj.GetValue(ItemsSourceProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand source)
        {
            obj.SetValue(CommandProperty, source);
        }

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetButtonStyle(DependencyObject obj, Style source)
        {
            obj.SetValue(ButtonStyleProperty, source);
        }

        public static Style GetButtonStyle(DependencyObject obj)
        {
            return (Style)obj.GetValue(ButtonStyleProperty);
        }

        private static void ItemsSourcePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var richTextBlock = sender as RichTextBlock;
            var items = e.NewValue as IEnumerable<IDepartment>;
            if (richTextBlock == null || items == null)
            {
                return;
            }

            richTextBlock.Blocks.Clear();
            var block = new Paragraph();
            foreach (var department in items)
            {
                var button = new Button()
                {
                    Content = department.Name,
                    Style = GetButtonStyle(sender),
                    CommandParameter = department
                };
                button.Click += ButtonOnClick;
                var container = new InlineUIContainer
                {
                    Child = button
                };
                block.Inlines.Add(container);
            }
            richTextBlock.Blocks.Add(block);
        }

        private static void ButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var button = (Button)sender;
            var richTextBlock = VisualTreeHelper.GetParent(button);
            var command = GetCommand(richTextBlock);
            command?.Execute(button.CommandParameter);
        }
    }
}
