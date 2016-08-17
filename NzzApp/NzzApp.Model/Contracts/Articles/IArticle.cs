using System;
using NzzApp.Model.Contracts.Images;

namespace NzzApp.Model.Contracts.Articles
{
    public interface IArticle
    {
        string Path { get; }
        DateTime PublishedOn { get; set; }
        bool IsBreakingNews { get; set; }
        string Title { get; set; }
        string SubTitle { get; set; }
        string Teaser { get; set; }
        bool HasGallery { get; set; }
        bool IsReportage { get; set; }
        bool IsBriefing { get; set; }
        bool IsOnStartPage { get; set; }
        IImage LeadImage { get; set; }
    }
}
