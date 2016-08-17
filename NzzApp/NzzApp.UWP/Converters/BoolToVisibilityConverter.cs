using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace NzzApp.UWP.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool visible = (bool) value;
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var visibility = (Visibility) value;
            return visibility == Visibility.Visible;
        }
    }
}
