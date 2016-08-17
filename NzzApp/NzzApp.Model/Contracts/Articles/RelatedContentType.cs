namespace NzzApp.Model.Contracts.Articles
{
    public enum RelatedContentType
    {
        Gallery,
        Image,
        Infobox,
        Video,
        Html,
        NotImplemented
    }

    public static class RelatedContentConverter
    {
        public static string GetString(RelatedContentType type)
        {
            switch (type)
            {
                case RelatedContentType.Gallery:
                    return "gallery";
                case RelatedContentType.Image:
                    return "image";
                case RelatedContentType.Infobox:
                    return "infobox";
                case RelatedContentType.Video:
                    return "video";
                case RelatedContentType.Html:
                    return "html";
            }
            return "not implemented";
        }

        public static RelatedContentType GetType(string name)
        {
            switch (name)
            {
                case "gallery":
                    return RelatedContentType.Gallery;
                case "image":
                    return RelatedContentType.Image;
                case "infobox":
                    return RelatedContentType.Infobox;
                case "video":
                    return RelatedContentType.Video;
                case "html":
                    return RelatedContentType.Html;
            }
            return RelatedContentType.NotImplemented;
        }
    }
}