using System.Collections.Generic;
using NzzApp.Model.Contracts.Articles;
using Sebastian.Toolkit.Util;

namespace NzzApp.Model.Implementation.Articles
{

    public class Paragraph : PropertyChangedBase, IParagraph
    {
        private ParagraphType _paragraphType;
        private string _text = string.Empty;
        private IList<IRelatedContent> _boxes = new List<IRelatedContent>();
        private IList<string> _items = new List<string>();

        public ParagraphType ParagraphType
        {
            get { return _paragraphType; }
            set
            {
                _paragraphType = value;
                OnPropertyChanged();
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public IList<IRelatedContent> Boxes
        {
            get { return _boxes; }
            set
            {
                _boxes = value;
                OnPropertyChanged();
            }
        }

        public IList<string> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }
    }
}
