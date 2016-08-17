using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using NzzApp.UWP.ViewModels;
using Sebastian.Toolkit.Application;

namespace NzzApp.UWP.Views
{
    public sealed partial class SettingsView : View
    {
        public SettingsViewModel SettingsViewModel => (SettingsViewModel) DataContext;

        public SettingsView()
        {
            this.InitializeComponent();
            this.FontFamilyToggleSwitch.Loaded += (sender, args) =>
            {
                FontFamilyToggleSwitch.Toggled += FontFamilyToggleSwitch_OnToggled;
            };
        }

        private void LiveTileToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            if (LiveTileToggleSwitch.IsOn)
            {
                SettingsViewModel.EnableLiveTile();
            }
            else
            {
                SettingsViewModel.DisableLiveTile();
            }
        }

        private void RangeBase_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            SettingsViewModel?.SetFontSize(e.NewValue);
        }

        private void FontFamilyToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            SettingsViewModel?.ToggleFontFamily();
        }
    }
}