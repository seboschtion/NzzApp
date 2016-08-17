namespace NzzApp.Model.Contracts.Images
{
    public interface IImage
    {
        string Id { get; }
        string Source { get; set; }
        string Caption { get; set; }
        string MimeType { get; set; }
        string PathTemplate { get; set; }
        bool HasImage { get; }
        int OriginalHeight { get; set; }
        int OriginalWidth { get; set; }
        string CaptionAndSource { get; }

        string GalleryPath { get; }
        string TopPath { get; }
        string SquarePath { get; }
        string DensityUnawareGalleryPath { get; }
        string DensityUnawareTopPath { get; }
        string DensityUnawareSquarePath { get; }
    }
}