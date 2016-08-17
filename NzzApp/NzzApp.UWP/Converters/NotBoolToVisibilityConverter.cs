using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace NzzApp.UWP.Converters
{
    public class NotBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool visible = (bool) value;
            return visible ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var visibility = (Visibility) value;
            return visibility == Visibility.Collapsed;
        }
    }
}
