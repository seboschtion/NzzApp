using System.Collections.Generic;

namespace NzzApp.Model.Implementation.Articles
{
    public class ArticleEqualityComparer : IEqualityComparer<ViewOptimizedArticle>
    {
        public bool Equals(ViewOptimizedArticle x, ViewOptimizedArticle y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            if (x.Sort.HasValue && y.Sort.HasValue)
            {
                return x.Article.Path.Equals(y.Article.Path) && x.Sort.Value == y.Sort.Value;
            }
            return x.Article.Path.Equals(y.Article.Path);
        }

        public int GetHashCode(ViewOptimizedArticle obj)
        {
            return obj.GetHashCode();
        }
    }
}