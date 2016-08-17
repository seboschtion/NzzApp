using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Implementation.Articles;

namespace NzzApp.Model.Implementation.RelatedContents
{
    public class DefaultRelatedContent : RelatedContent
    {
        public override RelatedContentType Type => RelatedContentType.NotImplemented;
    }
}