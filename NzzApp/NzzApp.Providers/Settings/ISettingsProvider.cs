using NzzApp.Model.Contracts.Settings;

namespace NzzApp.Providers.Settings
{
    public interface ISettingsProvider
    {
        bool IsFirstAppStart { get; }

        IAppSettings GetSettings();
        void SetSettings(IAppSettings settings);
        void EnableBreakingLiveTile();
        void DisableBreakingLiveTile();
    }
}
