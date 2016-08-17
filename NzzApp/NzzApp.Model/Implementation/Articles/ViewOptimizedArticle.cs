using System;
using NzzApp.Model.Contracts.Articles;
using Sebastian.Toolkit.Util;

namespace NzzApp.Model.Implementation.Articles
{
    public class ViewOptimizedArticle : PropertyChangedBase, IComparable<ViewOptimizedArticle>
    {
        private IArticle _article;
        private int? _sort;
        private string _exactDepartmentName;
        private bool _isLeadArticle;

        public IArticle Article
        {
            get { return _article; }
            set
            {
                _article = value; 
                OnPropertyChanged();
            }
        }

        public int? Sort
        {
            get { return _sort; }
            set
            {
                _sort = value;
                OnPropertyChanged();
            }
        }

        public string ExactDepartmentName
        {
            get { return _exactDepartmentName; }
            set
            {
                _exactDepartmentName = value;
                OnPropertyChanged();
            }
        }

        public bool IsLeadArticle
        {
            get { return _isLeadArticle; }
            set
            {
                _isLeadArticle = value;
                OnPropertyChanged();
            }
        }

        public bool IsFreeSpace { get; set; }

        public int CompareTo(ViewOptimizedArticle other)
        {
            if (IsLeadArticle)
            {
                return int.MaxValue;
            }
            if (Sort.HasValue && other.Sort.HasValue)
            {
                if (Sort.Value - other.Sort.Value == 0)
                {
                    return Article.PublishedOn.CompareTo(other.Article.PublishedOn);
                }
                return other.Sort.Value - Sort.Value;
            }
            return Article.PublishedOn.CompareTo(other.Article.PublishedOn);
        }

        public override string ToString()
        {
            return Article?.Title ?? string.Empty;
        }

        public static ViewOptimizedArticle Create(IArticle article)
        {
            return new ViewOptimizedArticle()
            {
                Article = article
            };
        }
    }
}