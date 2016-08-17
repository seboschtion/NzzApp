using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace NzzApp.UWP.Controls
{
    public sealed partial class AnimatedImage : UserControl
    {
        public DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(AnimatedImage), new PropertyMetadata(Stretch.None, StretchPropertyChangedCallback));
        public DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(string), typeof(AnimatedImage), new PropertyMetadata(null, SourcePropertyChangedCallback));

        public AnimatedImage()
        {
            this.InitializeComponent();
        }

        public Stretch Stretch
        {
            get { return (Stretch) GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public string Source
        {
            get { return (string) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private static void StretchPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var animatedImage = (AnimatedImage) dependencyObject;
            animatedImage.Image.Stretch = (Stretch) e.NewValue;
        }

        private static void SourcePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var animatedImage = (AnimatedImage)dependencyObject;
            var url = (string) e.NewValue;
            if (!string.IsNullOrWhiteSpace(url))
            {
                animatedImage.SetImageSource(new Uri(url, UriKind.RelativeOrAbsolute));
            }
        }

        private void SetImageSource(Uri uri)
        {
            Image.ImageOpened += ImageOnImageOpened;
            Image.Source = new BitmapImage(uri);
        }

        private void ImageOnImageOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            Image.Loaded -= ImageOnImageOpened;
            ShowImageStoryBoard.Completed += ShowImageStoryBoardOnCompleted;
            ShowImageStoryBoard.Begin();
        }

        private void ShowImageStoryBoardOnCompleted(object sender, object o)
        {
            ShowImageStoryBoard.Completed -= ShowImageStoryBoardOnCompleted;
        }
    }
}
