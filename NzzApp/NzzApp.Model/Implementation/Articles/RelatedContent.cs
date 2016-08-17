
using NzzApp.Model.Contracts.Articles;
using Sebastian.Toolkit.Util;

namespace NzzApp.Model.Implementation.Articles
{
    public abstract class RelatedContent : PropertyChangedBase, IRelatedContent
    {
        public abstract RelatedContentType Type { get; }
    }
}