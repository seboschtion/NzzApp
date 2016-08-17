namespace NzzApp.Providers.BackgroundTasks
{
    public interface IBackgroundTaskProvider
    {
        void RegisterBreakingLiveTileTask();
        void UnregisterBreakingLiveTileTask();
    }
}