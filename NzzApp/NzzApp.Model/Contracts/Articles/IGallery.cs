using System.Collections.ObjectModel;
using NzzApp.Model.Contracts.Images;

namespace NzzApp.Model.Contracts.Articles
{
    public interface IGallery : IRelatedContent
    {
        string Id { get; set; }
        string Title { get; set; }
        ObservableCollection<IImage> Images { get; }
        string GalleryHeight { get; }
    }
}