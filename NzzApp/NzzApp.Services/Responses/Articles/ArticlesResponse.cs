using System;
using Newtonsoft.Json;

namespace NzzApp.Services.Responses.Articles
{
    public class ArticlesResponse : ResponseBase
    {
        [JsonProperty("path")]
        public string Path { get; set; }
        [JsonProperty("speakingName")]
        public string SpeakingName { get; set; }
        [JsonProperty("articles")]
        public Article[] Articles { get; set; } = new Article[0];
    }

    public class Article : ResponseBase
    {
        [JsonProperty("path")]
        public string Path { get; set; }
        [JsonProperty("publicationDateTime")]
        public string PublicationDateTime { get; set; }
        [JsonProperty("isBreakingNews")]
        public bool IsBreakingNews { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("subTitle")]
        public string SubTitle { get; set; }
        [JsonProperty("teaser")]
        public string Teaser { get; set; }
        [JsonProperty("leadImage")]
        public Image LeadImage { get; set; }
        [JsonProperty("departments")]
        public string[] Departments { get; set; } = new string[0];
        [JsonProperty("hasGallery")]
        public bool HasGallery { get; set; }
        [JsonProperty("isReportage")]
        public bool IsReportage { get; set; }
        [JsonProperty("isBriefing")]
        public bool IsBriefing { get; set; }
    }

    public class Image
    {
        [JsonProperty("guid")]
        public string Guid { get; set; }
        [JsonProperty("path")]
        public string Path { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("caption")]
        public string Caption { get; set; }
        [JsonProperty("mimeType")]
        public string MimeType { get; set; }
        [JsonProperty("originalHeight")]
        public int? OriginalHeight { get; set; }
        [JsonProperty("originalWidth")]
        public int? OriginalWidth { get; set; }
    }
}