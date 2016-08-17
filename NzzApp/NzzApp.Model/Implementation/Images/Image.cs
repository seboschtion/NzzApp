using System;
using Windows.Graphics.Display;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Contracts.Images;
using NzzApp.Model.Implementation.Articles;

namespace NzzApp.Model.Implementation.Images
{
    public class Image : RelatedContent, IImage
    {
        private string _source;
        private string _mimeType = string.Empty;
        private string _caption;
        private string _pathTemplate;

        public Image(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public string Source
        {
            get { return _source; }
            set
            {
                _source = value;
                OnPropertyChanged();
            }
        }

        public string Caption
        {
            get { return _caption; }
            set
            {
                _caption = value; 
                OnPropertyChanged();
            }
        }

        public string MimeType
        {
            get { return _mimeType; }
            set
            {
                _mimeType = value ?? string.Empty;
                OnPropertyChanged();
            }
        }

        public string PathTemplate
        {
            get { return _pathTemplate; }
            set
            {
                _pathTemplate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GalleryPath));
                OnPropertyChanged(nameof(TopPath));
                OnPropertyChanged(nameof(SquarePath));
                OnPropertyChanged(nameof(HasImage));
            }
        }

        public bool HasImage => !string.IsNullOrWhiteSpace(PathTemplate);
        public int OriginalHeight { get; set; }
        public int OriginalWidth { get; set; }
        private bool IsReScr => OriginalWidth > 0 && OriginalHeight > 0;

        public string CaptionAndSource => !string.IsNullOrWhiteSpace(Source) ? $"{Caption} ({Source})" : Caption;
        public string GalleryPath => GetFormattedPath(ImageType.Gallery);
        public string TopPath => GetFormattedPath(ImageType.Top);
        public string SquarePath => GetFormattedPath(ImageType.Square);
        public string DensityUnawareGalleryPath => GetFormattedPath(ImageType.Gallery, 200);
        public string DensityUnawareTopPath => GetFormattedPath(ImageType.Top, 200);
        public string DensityUnawareSquarePath => GetFormattedPath(ImageType.Square, 200);

        private string GetFormattedPath(ImageType type, float customDensity = 0f)
        {
            if (PathTemplate == null)
            {
                return string.Empty;
            }

            const string width = "%width%";
            const string height = "%height%";
            const string format = "%format%";
            string path = MimeType.Contains("png") ? PathTemplate.Replace(".jpg", ".png") : PathTemplate;

            DensityAwareSize density;
            if (Math.Abs(customDensity) < 0.1)
            {
                var displayInformation = DisplayInformation.GetForCurrentView();
                density = new DensityAwareSize((int) displayInformation.LogicalDpi);
            }
            else
            {
                density = new DensityAwareSize((int) customDensity);
            }

            string result;
            switch (type)
            {
                case ImageType.Gallery:
                    result = path
                        .Replace(width, density.GetGalleryWidth())
                        .Replace(format, density.GetGalleryFormat());
                    result = IsReScr ? result.Replace($",H{height}", "") : result.Replace(height, density.GetGalleryHeight());
                    break;
                case ImageType.Square:
                    result = path
                        .Replace(width, density.GetSquareWidth())
                        .Replace(format, density.GetSquareFormat());
                    result = IsReScr ? result.Replace($",H{height}", "") : result.Replace(height, density.GetSquareHeight());
                    break;
                case ImageType.Top:
                    result = path
                        .Replace(width, density.GetTopWidth())
                        .Replace(format, density.GetTopFormat());
                    result = IsReScr ? result.Replace($",H{height}", "") : result.Replace(height, density.GetTopHeight());
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        public override RelatedContentType Type => RelatedContentType.Image;
    }
}
