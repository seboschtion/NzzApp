using System.Threading.Tasks;
using NzzApp.Model.Contracts.BreakingNews;

namespace NzzApp.Providers.LiveTile
{
    public interface ILiveTileProvider
    {
        Task RefreshLiveTileAsync();
        void RefreshLiveTile(IBreakingNews breakingNews);
        void ClearLiveTile();
    }
}