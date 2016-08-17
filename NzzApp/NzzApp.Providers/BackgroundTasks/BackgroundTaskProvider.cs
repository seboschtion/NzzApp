using System;
using System.Linq;
using Windows.ApplicationModel.Background;

namespace NzzApp.Providers.BackgroundTasks
{
    public class BackgroundTaskProvider : IBackgroundTaskProvider
    {
        private const string BreakingLiveTileTaskName = "BreakingLiveTileBackgroundTask";
        private const string BreakingLiveTileTaskEntryPoint = "NzzApp.Tasks.LiveTileBackgroundTask";

        public async void RegisterBreakingLiveTileTask()
        {
            var access = await BackgroundExecutionManager.RequestAccessAsync();
            if (access.HasFlag(BackgroundAccessStatus.Denied))
            {
                return;
            }

            var taskRegistered = BackgroundTaskRegistration.AllTasks.Any(task => task.Value.Name == BreakingLiveTileTaskName);
            if (!taskRegistered)
            {
                var builder = new BackgroundTaskBuilder
                {
                    Name = BreakingLiveTileTaskName,
                    TaskEntryPoint = BreakingLiveTileTaskEntryPoint
                };
                var trigger = new TimeTrigger(60, false);
                builder.SetTrigger(trigger);
                var task = builder.Register();
            }
        }

        public void UnregisterBreakingLiveTileTask()
        {
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == BreakingLiveTileTaskName)
                {
                    task.Value.Unregister(true);
                }
            }
        }
    }
}
