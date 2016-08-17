using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace NzzApp.UWP.Styles.Themes
{
    public enum CommonBrush
    {
        ThemeBrush
    }

    public class ThemeResource
    {
        public static SolidColorBrush GetBrush(string key)
        {
            var theme = Application.Current.RequestedTheme.ToString();
            return ((ResourceDictionary) Application.Current.Resources.ThemeDictionaries[theme])[key] as SolidColorBrush;
        }

        public static SolidColorBrush GetBrush(CommonBrush commonBrush)
        {
            return GetBrush(ConvertCommonBrushToString(commonBrush));
        }

        private static string ConvertCommonBrushToString(CommonBrush commonBrush)
        {
            switch (commonBrush)
            {
                case CommonBrush.ThemeBrush:
                    return "ThemeBrush";
            }

            return string.Empty;
        }
    }
}
