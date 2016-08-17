using System.Collections.Generic;

namespace NzzApp.Model.Contracts.Articles
{
    public interface IFullArticle
    {
        IArticle Article { get; }
        IList<IParagraph> Paragraphs { get; } 
        IList<IAuthor> Authors { get; set; }
        IList<IArticle> RelatedArticles { get; set; } 
        IList<IRelatedContent> RelatedContents { get; } 
        string LeadText { get; set; }
        string WebUrl { get; set; }
        string ShortWebUrl { get; set; }
        string Agency { get; set; }
        string AuthorsText { get; }
        bool IsAuthorAvailable { get; }
        bool HasRelatedArticles { get; }
        bool HasRelatedContent { get; }
    }
}