using System;
using Windows.ApplicationModel.Background;
using NzzApp.Data;
using Sebastian.Toolkit.Logging;

namespace NzzApp.Tasks
{
    public sealed class LiveTileBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            taskInstance.Canceled += TaskInstanceOnCanceled;
            this.Logger().Debug("Start background task");

            var initializer = new ProviderInitializer();
            var liveTileProvider = initializer.LiveTileProvider;
            var dataProvider = initializer.DataProvider;
            UpdateLastLiveTileTaskExecutionDate(dataProvider);

            await liveTileProvider.RefreshLiveTileAsync();

            this.Logger().Debug("End background task");
            deferral.Complete();
        }

        private void TaskInstanceOnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            this.Logger().Error("Canceled background task", reason.ToString());
        }

        private void UpdateLastLiveTileTaskExecutionDate(IDataProvider dataProvider)
        {
            var settings = dataProvider.GetSettings();
            settings.LastLiveTileTaskExecutionDate = DateTime.UtcNow;
            dataProvider.SetSettings(settings);
        }
    }
}