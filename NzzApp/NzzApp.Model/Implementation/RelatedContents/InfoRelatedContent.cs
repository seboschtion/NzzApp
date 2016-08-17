using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Implementation.Articles;

namespace NzzApp.Model.Implementation.RelatedContents
{
    public class InfoRelatedContent : RelatedContent
    {
        private string _title;
        private string _body;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Body
        {
            get { return _body; }
            set
            {
                _body = value; 
                OnPropertyChanged();
            }
        }

        public override RelatedContentType Type => RelatedContentType.Infobox;
    }
}
