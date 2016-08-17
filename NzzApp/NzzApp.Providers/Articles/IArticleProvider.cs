using System.Collections.Generic;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Contracts.Departments;
using NzzApp.Model.Implementation.Articles;

namespace NzzApp.Providers.Articles
{
    public interface IArticleProvider
    {
        IFullArticle GetFullArticle(IArticle article);
        IList<ViewOptimizedArticle> GetArticles(IDepartment department);
        bool IsFullArticleReady(string articlePath);
        IArticle GetArticleByPath(string articlePath);
    }
}