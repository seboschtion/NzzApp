using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using NzzApp.Model.Contracts.Articles;

namespace NzzApp.UWP.Controls
{
    public sealed partial class GalleryControl : UserControl
    {
        public static DependencyProperty GalleryItemProperty = DependencyProperty.Register("GalleryItem", typeof (IGallery),
            typeof (GalleryControl), new PropertyMetadata(null, GalleryItemChangedCallback));

        private bool _metadataVisible = true;

        public GalleryControl()
        {
            this.InitializeComponent();
        }

        public IGallery GalleryItem
        {
            get { return (IGallery) GetValue(GalleryItemProperty); }
            set { SetValue(GalleryItemProperty, value); }
        }

        private static void GalleryItemChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (GalleryControl) sender;
            var height = double.Parse(((IGallery) e.NewValue).GalleryHeight);
            control.Loaded += (s, a) =>
            {
                control.ImagesFlipView.Height = height > control.ActualHeight ? control.ActualHeight : height;
            };
        }

        private void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_metadataVisible)
            {
                HideElement(TitleTextBorder);
                HideElement(CaptionTextBorder);
            }
            else
            {
                ShowElement(TitleTextBorder);
                ShowElement(CaptionTextBorder);
            }

            _metadataVisible = !_metadataVisible;
        }

        private void HideElement(UIElement element)
        {
            var visual = ElementCompositionPreview.GetElementVisual(element);
            var compositor = visual.Compositor;
            var animation = compositor.CreateScalarKeyFrameAnimation();
            animation.InsertKeyFrame(1.00f, 0.00f);
            animation.InsertKeyFrame(0.00f, 1.00f);
            animation.Duration = TimeSpan.FromMilliseconds(500);
            visual.StartAnimation("Opacity", animation);
        }

        private void ShowElement(UIElement element)
        {
            var visual = ElementCompositionPreview.GetElementVisual(element);
            var compositor = visual.Compositor;
            var animation = compositor.CreateScalarKeyFrameAnimation();
            animation.InsertKeyFrame(0.00f, 0.00f);
            animation.InsertKeyFrame(1.00f, 1.00f);
            animation.Duration = TimeSpan.FromMilliseconds(500);
            visual.StartAnimation("Opacity", animation);
        }
    }
}
