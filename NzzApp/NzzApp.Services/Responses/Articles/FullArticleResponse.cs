using System;
using Newtonsoft.Json;

namespace NzzApp.Services.Responses.Articles
{
    public class FullArticleResponse : Article
    {
        [JsonProperty("leadText")]
        public string LeadText { get; set; }
        [JsonProperty("body")]
        public Body[] Body { get; set; } = new Body[0];
        [JsonProperty("authors")]
        public Author[] Authors { get; set; } = new Author[0];
        [JsonProperty("relatedArticles")]
        public Article[] RelatedArticles { get; set; } = new Article[0];
        [JsonProperty("relatedContent")]
        public RelatedContent[] RelatedContent { get; set; } = new RelatedContent[0];
        [JsonProperty("webUrl")]
        public string WebUrl { get; set; }
        [JsonProperty("shortWebUrl")]
        public string ShortWebUrl { get; set; }
        [JsonProperty("agency")]
        public string Agency { get; set; }
    }

    public class Body
    {
        private object _boxesObject;

        [JsonProperty("style")]
        public string Style { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("items")]
        public string[] Items { get; set; } = new string[0];
        [JsonIgnore]
        public RelatedContent[] Boxes { get; set; } = new RelatedContent[0];
        [JsonProperty("boxes")]
        public object BoxesObject
        {
            get { return _boxesObject; }
            set
            {
                _boxesObject = value;
                Boxes = ParseObjectToArray<RelatedContent>(value);
            }
        }
        protected T[] ParseObjectToArray<T>(object ambiguousObject)
        {
            string json = ambiguousObject.ToString();
            if (!json.Contains("video") && !json.Contains("image") && !json.Contains("html") && !json.Contains("infobox"))
            {
                json = json.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            }
            if (string.IsNullOrWhiteSpace(json) || json.TrimStart().StartsWith("[["))
            {
                return new T[0];
            }
            return JsonConvert.DeserializeObject<T[]>(json);
        }
    }

    public class Video
    {
        [JsonProperty("authorId")]
        public string AuthorId { get; set; }
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }
        [JsonProperty("videoId")]
        public string VideoId { get; set; }
    }

    public class RelatedContent : Image
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        // Infobox
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }

        // Video
        [JsonProperty("authorId")]
        public string AuthorId { get; set; }
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }
        [JsonProperty("videoId")]
        public string VideoId { get; set; }

        // Gallery
        [JsonProperty("images")]
        public Image[] Images { get; set; }
    }

    public class Author
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }
    }
}