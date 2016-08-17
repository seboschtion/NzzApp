using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NzzApp.Model.Contracts.Articles;

namespace NzzApp.UWP.Selectors
{
    public class ArticleTextTemplateSelector : DataTemplateSelector
    {
        public DataTemplate P { get; set; }
        public DataTemplate H4 { get; set; }
        public DataTemplate Boxes { get; set; }
        public DataTemplate Items { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var paragraph = item as IParagraph;
            if (paragraph == null)
            {
                return null;
            }

            switch (paragraph.ParagraphType)
            {
                case ParagraphType.H1:
                case ParagraphType.H2:
                case ParagraphType.H3:
                case ParagraphType.H4:
                case ParagraphType.H5:
                case ParagraphType.H6:
                    return H4;
                case ParagraphType.UL:
                    return Items;
                case ParagraphType.P:
                    if (paragraph.Boxes.Count > 0)
                    {
                        return Boxes;
                    }
                    if (paragraph.Items.Count > 0)
                    {
                        return Items;
                    }
                    return P;
            }

            return null;
        }
    }
}