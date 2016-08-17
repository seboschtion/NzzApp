namespace NzzApp.Model.Contracts.Departments
{
    public enum DepartmentSerialisationType
    {
        Json,
        Html,
        Article
    }

    public static class DepartmentSerialisationTypeConverter
    {
        public static DepartmentSerialisationType GetType(string type)
        {
            switch (type)
            {
                case "json":
                    return DepartmentSerialisationType.Json;
                case "html":
                    return DepartmentSerialisationType.Html;
                case "article":
                    return DepartmentSerialisationType.Article;
                default:
                    return DepartmentSerialisationType.Json;
            }
        }

        public static string GetString(DepartmentSerialisationType type)
        {
            switch (type)
            {
                case DepartmentSerialisationType.Json:
                    return "json";
                case DepartmentSerialisationType.Html:
                    return "html";
                case DepartmentSerialisationType.Article:
                    return "article";
                default:
                    return string.Empty;
            }
        }
    }
}