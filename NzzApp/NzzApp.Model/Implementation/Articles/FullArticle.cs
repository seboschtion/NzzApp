using System.Collections.Generic;
using System.Linq;
using NzzApp.Model.Contracts.Articles;
using Sebastian.Toolkit.Util;

namespace NzzApp.Model.Implementation.Articles
{
    public class FullArticle : PropertyChangedBase, IFullArticle
    {
        private IList<IParagraph> _paragraphs = new List<IParagraph>();
        private string _leadText;
        private IList<IAuthor> _authors = new List<IAuthor>();
        private IList<IArticle> _relatedArticles = new List<IArticle>();
        private string _webUrl;
        private string _shortWebUrl;
        private string _agency;
        private IList<IRelatedContent> _relatedContents = new List<IRelatedContent>();

        public FullArticle(IArticle articleInfo)
        {
            Article = articleInfo;
        }

        public IArticle Article { get; }

        public IList<IParagraph> Paragraphs
        {
            get { return _paragraphs; }
            set
            {
                _paragraphs = value;
                OnPropertyChanged();
            }
        }

        public IList<IRelatedContent> RelatedContents
        {
            get { return _relatedContents; }
            private set
            {
                _relatedContents = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasRelatedContent));
            }
        }

        public string LeadText
        {
            get { return _leadText; }
            set
            {
                _leadText = value;
                OnPropertyChanged();
            }
        }

        public IList<IAuthor> Authors
        {
            get { return _authors; }
            set
            {
                _authors = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAuthorAvailable));
                OnPropertyChanged(nameof(AuthorsText));
            }
        }

        public IList<IArticle> RelatedArticles
        {
            get { return _relatedArticles; }
            set
            {
                _relatedArticles = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasRelatedArticles));
            }
        }

        public string WebUrl
        {
            get { return _webUrl; }
            set
            {
                _webUrl = value;
                OnPropertyChanged();
            }
        }

        public string ShortWebUrl
        {
            get { return _shortWebUrl; }
            set
            {
                _shortWebUrl = value;
                OnPropertyChanged();
            }
        }

        public string Agency
        {
            get { return _agency; }
            set
            {
                _agency = value;
                OnPropertyChanged();
            }
        }

        public string AuthorsText => string.Join(", ", Authors.Select(a => a.Name));
        public bool IsAuthorAvailable => Authors.Count > 0;
        public bool HasRelatedArticles => RelatedArticles.Count > 0;
        public bool HasRelatedContent => RelatedContents.Count > 0;
    }
}