using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using NzzApp.UWP.Strings;

namespace NzzApp.UWP.Converters
{
    public class MediaElementStateToStringConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            MediaElementState state = (MediaElementState) value;
            var name = state.ToString();
            return IndependentUseResource.LoadString($"MediaElementState{name}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
