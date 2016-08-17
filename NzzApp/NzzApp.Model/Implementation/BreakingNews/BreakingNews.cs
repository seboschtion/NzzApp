using System.Collections.ObjectModel;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Contracts.BreakingNews;
using Sebastian.Toolkit.Util;

namespace NzzApp.Model.Implementation.BreakingNews
{
    public class BreakingNews : PropertyChangedBase, IBreakingNews
    {
        private string _path;
        private string _speakingName;
        private ObservableCollection<IArticle> _articles = new ObservableCollection<IArticle>();

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value; 
                OnPropertyChanged();
            }
        }

        public string SpeakingName
        {
            get { return _speakingName; }
            set
            {
                _speakingName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IArticle> Articles
        {
            get { return _articles; }
            private set
            {
                _articles = value; 
                OnPropertyChanged();
            }
        }
    }
}
