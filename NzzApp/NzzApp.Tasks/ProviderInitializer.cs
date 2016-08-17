using NzzApp.Data;
using NzzApp.Data.SQLite;
using NzzApp.Providers.LiveTile;
using NzzApp.Providers.Synchonisation;
using NzzApp.Services;

namespace NzzApp.Tasks
{
    internal class ProviderInitializer
    {
        public ProviderInitializer()
        {
            NzzRestService = new NzzRestService();
            DataProvider = new SQLiteDataProvider();
            SyncProvider = new SyncProvider(DataProvider, NzzRestService);
            LiveTileProvider = new LiveTileProvider(SyncProvider);
        }

        public ISyncProvider SyncProvider { get; set; }
        public ILiveTileProvider LiveTileProvider { get; set; }
        public IDataProvider DataProvider { get; set; }
        public INzzRestService NzzRestService { get; set; }
    }
}