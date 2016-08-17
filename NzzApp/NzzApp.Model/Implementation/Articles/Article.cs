using System;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Contracts.Images;
using Sebastian.Toolkit.Util;

namespace NzzApp.Model.Implementation.Articles
{
    public class Article : PropertyChangedBase, IArticle
    {
        private DateTime _publishedOn;
        private bool _isBreakingNews;
        private bool _isBriefing;
        private bool _isReportage;
        private bool _hasGallery;
        private string _teaser;
        private string _subTitle;
        private string _title;
        private bool _isOnStartPage;
        private IImage _leadImage;

        public Article(string path)
        {
            Path = path;
        }

        public string Path { get; }

        public DateTime PublishedOn
        {
            get { return _publishedOn; }
            set
            {
                _publishedOn = value;
                OnPropertyChanged();
            }
        }

        public bool IsBreakingNews
        {
            get { return _isBreakingNews; }
            set
            {
                _isBreakingNews = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string SubTitle
        {
            get { return _subTitle; }
            set
            {
                _subTitle = value;
                OnPropertyChanged();
            }
        }

        public string Teaser
        {
            get { return _teaser; }
            set
            {
                _teaser = value;
                OnPropertyChanged();
            }
        }

        public bool HasGallery
        {
            get { return _hasGallery; }
            set
            {
                _hasGallery = value;
                OnPropertyChanged();
            }
        }

        public bool IsReportage
        {
            get { return _isReportage; }
            set
            {
                _isReportage = value;
                OnPropertyChanged();
            }
        }

        public bool IsBriefing
        {
            get { return _isBriefing; }
            set
            {
                _isBriefing = value;
                OnPropertyChanged();
            }
        }

        public bool IsOnStartPage
        {
            get { return _isOnStartPage; }
            set
            {
                _isOnStartPage = value;
                OnPropertyChanged();
            }
        }

        public IImage LeadImage
        {
            get { return _leadImage; }
            set
            {
                _leadImage = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return Title;
        }
    }
}