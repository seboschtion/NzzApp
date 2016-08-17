using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Implementation.Articles;

namespace NzzApp.Model.Implementation.RelatedContents
{
    public class HtmlRelatedContent : RelatedContent
    {
        private string _body;

        public string Body
        {
            get { return _body; }
            set
            {
                _body = value;
                OnPropertyChanged();
            }
        }

        public override RelatedContentType Type => RelatedContentType.Html;
    }
}