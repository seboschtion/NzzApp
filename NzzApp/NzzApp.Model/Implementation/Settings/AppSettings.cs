using System;
using NzzApp.Model.Contracts.Settings;

namespace NzzApp.Model.Implementation.Settings
{
    public class AppSettings : IAppSettings
    {
        public bool IsFirstAppStart { get; set; }
        public bool BreakingLiveTileEnabled { get; set; }
        public bool SuccessfullInitialization { get; set; }
        public bool DisableLiveTileTask { get; set; }
        public int ArticleFontSize { get; set; }
        public string ArticleFontFamily { get; set; }
        public DateTime LastLiveTileTaskExecutionDate { get; set; }
    }
}
