using System;

namespace NzzApp.Model.Contracts.Settings
{
    public interface IAppSettings
    {
        bool IsFirstAppStart { get; set; }
        bool BreakingLiveTileEnabled { get; set; }
        bool SuccessfullInitialization { get; set; }
        bool DisableLiveTileTask { get; set; }
        int ArticleFontSize { get; set; }
        string ArticleFontFamily { get; set; }
        DateTime LastLiveTileTaskExecutionDate { get; set; }
    }
}