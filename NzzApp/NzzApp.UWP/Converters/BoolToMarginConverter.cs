using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace NzzApp.UWP.Converters
{
    public class BoolToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var margins = ((string) parameter).Split('-');
            var boolean = (bool) value;
            var margin = boolean ? margins[0] : margins[1];
            var marginValues = margin.Split(',');
            return
                new Thickness(
                    double.Parse(marginValues[0]),
                    double.Parse(marginValues[1]),
                    double.Parse(marginValues[2]),
                    double.Parse(marginValues[3]));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
