namespace NzzApp.Model.Contracts.Articles
{
    public enum     ParagraphType
    {
        P,
        H1,
        H2,
        H3,
        H4,
        H5,
        H6,
        UL
    }

    public static class ParagraphTypeConverter
    {
        public static ParagraphType ToType(string type)
        {
            switch (type)
            {
                case "p":
                    return ParagraphType.P;
                case "h1":
                    return ParagraphType.H1;
                case "h2":
                    return ParagraphType.H2;
                case "h3":
                    return ParagraphType.H3;
                case "h4":
                    return ParagraphType.H4;
                case "h5":
                    return ParagraphType.H5;
                case "h6":
                    return ParagraphType.H6;
                case "ul":
                    return ParagraphType.UL;
                default:
                    return ParagraphType.P;
            }
        }

        public static string ToText(ParagraphType type)
        {
            switch (type)
            {
                case ParagraphType.P:
                    return "p";
                case ParagraphType.H1:
                    return "h1";
                case ParagraphType.H2:
                    return "h2";
                case ParagraphType.H3:
                    return "h3";
                case ParagraphType.H4:
                    return "h4";
                case ParagraphType.H5:
                    return "h5";
                case ParagraphType.H6:
                    return "h6";
                case ParagraphType.UL:
                    return "ul";
                default:
                    return string.Empty;
            }
        }
    }
}
