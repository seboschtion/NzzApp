using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Sebastian.Toolkit.Util.Extensions;

namespace NzzApp.UWP.Controls
{

    public class ParallaxBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty ParallaxContentProperty = DependencyProperty.Register(
            "ParallaxContent", typeof (UIElement), typeof (ParallaxBehavior), new PropertyMetadata(null, OnParallaxContentChanged));

        public static readonly DependencyProperty ParallaxMultiplierProperty = DependencyProperty.Register(
            "ParallaxMultiplier", typeof (double), typeof (ParallaxBehavior), new PropertyMetadata(0.3));

        public double ParallaxMultiplier
        {
            get { return (double)GetValue(ParallaxMultiplierProperty); }
            set { SetValue(ParallaxMultiplierProperty, value); }
        }

        public UIElement ParallaxContent
        {
            get { return (UIElement)GetValue(ParallaxContentProperty); }
            set { SetValue(ParallaxContentProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssignParallax();
        }
   
        private static void OnParallaxContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var b = d as ParallaxBehavior;
            b?.AssignParallax();
        }

        private void AssignParallax()
        {
            if (ParallaxContent == null || AssociatedObject == null)
            {
                return;
            }

            var scroller = AssociatedObject as ScrollViewer ?? AssociatedObject.GetChildOfType<ScrollViewer>();
            if (scroller == null)
            {
                return;
            }

            var scrollerViewerManipulation = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scroller);
            var expression = scrollerViewerManipulation.Compositor.CreateExpressionAnimation(
                "ScrollManipulation.Translation.Y * ParallaxMultiplier");
            expression.SetScalarParameter("ParallaxMultiplier", (float)ParallaxMultiplier);
            expression.SetReferenceParameter("ScrollManipulation", scrollerViewerManipulation);

            var textVisual = ElementCompositionPreview.GetElementVisual(ParallaxContent);
            textVisual.StartAnimation("Offset.Y", expression);
        }
    }
}