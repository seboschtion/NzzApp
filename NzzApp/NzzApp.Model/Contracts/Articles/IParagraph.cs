using System.Collections.Generic;

namespace NzzApp.Model.Contracts.Articles
{
    public interface IParagraph
    {
        ParagraphType ParagraphType { get; set; }
        string Text { get; set; }
        IList<IRelatedContent> Boxes { get; set; }
        IList<string> Items { get; set; } 
    }
}
