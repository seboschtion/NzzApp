using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NzzApp.Services.Responses.Articles;

namespace NzzApp.Services.Responses.BreakingNews
{
    public class BreakingNewsResponse
    {
        [JsonProperty("path")]
        public string Path { get; set; }
        [JsonProperty("speakingName")]
        public string SpeakingName { get; set; }
        [JsonProperty("articles")]
        public List<Article> Articles { get; set; }
    }

    public class Teaser
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Article
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
        public Teaser[] Teaser { get; set; }
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
}