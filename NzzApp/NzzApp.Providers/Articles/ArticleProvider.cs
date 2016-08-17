using System.Collections.Generic;
using NzzApp.Data;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Contracts.Departments;
using NzzApp.Model.Implementation.Articles;

namespace NzzApp.Providers.Articles
{
    public class ArticleProvider : IArticleProvider
    {
        private readonly IDataProvider _dataProvider;

        public ArticleProvider(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public IFullArticle GetFullArticle(IArticle article)
        {
            return _dataProvider.GetFullArticle(article);
        }

        public IList<ViewOptimizedArticle> GetArticles(IDepartment department)
        {
            return _dataProvider.GetArticlesForDepartment(department);
        }

        public bool IsFullArticleReady(string articlePath)
        {
            return _dataProvider.IsFullArticleAvailable(articlePath);
        }

        public IArticle GetArticleByPath(string articlePath)
        {
            return _dataProvider.GetArticleByPath(articlePath);
        }
    }
}
