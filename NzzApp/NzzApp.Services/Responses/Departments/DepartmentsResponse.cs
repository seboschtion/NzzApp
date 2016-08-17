using Newtonsoft.Json;

namespace NzzApp.Services.Responses.Departments
{

    public class DepartmentsResponse : ResponseBase
    {
        [JsonProperty("menus")]
        public Menu[] Menus { get; set; }
        [JsonProperty("ads")]
        public Ads Ads { get; set; }
    }

    public class Menu
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("path")]
        public string Path { get; set; }
        [JsonProperty("show_on")]
        public string ShowOn { get; set; }
        [JsonProperty("menus")]
        public SubMenu[] SubMenus { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class SubMenu
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("path")]
        public string Path { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Ads
    {
        [JsonProperty("article")]
        public Article Article { get; set; }
        [JsonProperty("frontpage")]
        public Frontpage Frontpage { get; set; }
        [JsonProperty("section")]
        public Section Section { get; set; }
    }

    public class Article
    {
        [JsonProperty("formatIdBanner")]
        public int FormatIdBanner { get; set; }
        [JsonProperty("formatIdToaster")]
        public int FormatIdToaster { get; set; }
        [JsonProperty("formatIdInterstitial")]
        public int FormatIdInterstitial { get; set; }
    }

    public class Frontpage
    {
        [JsonProperty("formatIdBanner")]
        public int FormatIdBanner { get; set; }
    }

    public class Section
    {
        [JsonProperty("formatIdBanner")]
        public int FormatIdBanner { get; set; }
        [JsonProperty("formatIdToaster")]
        public int FormatIdToaster { get; set; }
        [JsonProperty("formatIdInterstitial")]
        public int FormatIdInterstitial { get; set; }
    }
}
