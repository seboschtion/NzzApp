using NzzApp.Model.Contracts.Articles;
using Sebastian.Toolkit.Util;

namespace NzzApp.Model.Implementation.Articles
{
    public class Author : PropertyChangedBase, IAuthor
    {
        private string _name;
        private string _abbreviation;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value; 
                OnPropertyChanged();
            }
        }

        public string Abbreviation
        {
            get { return _abbreviation; }
            set
            {
                _abbreviation = value;
                OnPropertyChanged();
            }
        }
    }
}
