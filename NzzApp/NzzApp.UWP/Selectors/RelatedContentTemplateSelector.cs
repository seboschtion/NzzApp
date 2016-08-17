using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NzzApp.Model.Contracts.Articles;

namespace NzzApp.UWP.Selectors
{
    public class RelatedContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Video { get; set; }
        public DataTemplate Image { get; set; }
        public DataTemplate Html { get; set; }
        public DataTemplate InfoBox { get; set; }
        public DataTemplate Gallery { get; set; }
        public DataTemplate NotImplemented { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var relatedContent = item as IRelatedContent;

            if (relatedContent != null)
            {
                switch (relatedContent.Type)
                {
                    case RelatedContentType.Video:
                        return Video;
                    case RelatedContentType.Image:
                        return Image;
                    case RelatedContentType.Html:
                        return Html;
                    case RelatedContentType.Infobox:
                        return InfoBox;
                    case RelatedContentType.Gallery:
                        return Gallery;
                }
            }

            return NotImplemented;
        }
    }
}
