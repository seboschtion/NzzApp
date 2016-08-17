namespace NzzApp.Services
{
    public class NzzRestServiceUrls
    {
        public const string BaseUrl = "http://headlines.nzz.ch";
        public const string DepartmentsAbsolute = BaseUrl + "/api/departments.json";
        public const string ArticleRelative = "/api/articles/";
        //public const string NewsTickerRelative = "/api/departments/newsticker.json";
        public const string NewsTickerAbsolute = "http://nzz.putine.ch/rest/v1/breaking?count={0}";
        public const string CopyrightAbsolute = "http://headlines.nzz.ch/mobile/copyright";
        public const string ImpressumAbsolute = "http://headlines.nzz.ch/mobile/impressum";
        public const string ContactAbsolute = "http://headlines.nzz.ch/static/kontakt";
    }
}