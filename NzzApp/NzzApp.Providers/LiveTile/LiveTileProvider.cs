using System;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using NzzApp.Model.Contracts.BreakingNews;
using NzzApp.Providers.Synchonisation;
using Sebastian.Toolkit.Util;

namespace NzzApp.Providers.LiveTile
{
    public class LiveTileProvider : ILiveTileProvider
    {
        private readonly ISyncProvider _syncProvider;

        public LiveTileProvider(ISyncProvider syncProvider)
        {
            _syncProvider = syncProvider;
        }

        public async Task RefreshLiveTileAsync()
        {
            try
            {
                var breakingNews = await _syncProvider.DownloadBreakingNewsAsync(5);
                if (breakingNews == null)
                {
                    ClearLiveTile();
                    return;
                }

                RefreshLiveTile(breakingNews);
            }
            catch (Exception)
            {
                ClearLiveTile();
            }
        }

        public void RefreshLiveTile(IBreakingNews breakingNews)
        {
            ClearLiveTile();
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);

            foreach (var article in breakingNews.Articles)
            {
                var wideSrc = article.LeadImage != null && article.LeadImage.HasImage
                    ? $"<image src='{article.LeadImage.DensityUnawareTopPath}' placement='peek'></image>"
                    : string.Empty;
                var squareSrc = article.LeadImage != null && article.LeadImage.HasImage
                    ? $"<image src='{article.LeadImage.DensityUnawareSquarePath}' placement='peek'></image>"
                    : string.Empty;

                var wideSubtitle = article.SubTitle != null ? $"<text hint-wrap='true' hint-style='caption'>{article.SubTitle}</text>" : string.Empty;
                var wideTitle = article.Title != null ? $"<text hint-wrap='true' hint-style='captionSubtle'>{article.Title}</text>" : string.Empty;
                var squareText = article.Title != null && article.SubTitle != null
                    ? $@"<binding template='TileMedium'>
                             <text hint-wrap='true' hint-style='caption'>{article.SubTitle}: {article.Title}</text>
                             {squareSrc}
                         </binding>"
                    : string.Empty;

                var xml = string.Format($@"
                        <tile version='3'>
                            <visual branding='name'>
                                <binding template='TileWide'>
                                    {wideSubtitle}
                                    {wideTitle}
                                    {wideSrc}
                                </binding>
                                {squareText}
                            </visual>
                        </tile>");
                var doc = new XmlDocument();
                doc.LoadXml(xml);

                var notification = new TileNotification(doc);
                notification.ExpirationTime = new DateTimeOffset(DateTime.Now.AddMinutes(59));
                updater.Update(notification);
            }
        }

        public void ClearLiveTile()
        {
            var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            tileUpdater.Clear();
        }
    }
}
