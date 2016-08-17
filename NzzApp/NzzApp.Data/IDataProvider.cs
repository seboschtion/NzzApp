using System;
using System.Collections.Generic;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Contracts.Departments;
using NzzApp.Model.Contracts.Settings;
using NzzApp.Model.Implementation.Articles;

namespace NzzApp.Data
{
    public interface IDataProvider
    {
        void UpdateDepartments(IList<IDepartment> departments);
        IList<IDepartment> GetDepartments();
        IList<IDepartment> GetDepartments(IArticle article);
        void UpdateArticles(IDictionary<IArticle, IList<IDepartment>> articles);
        IList<ViewOptimizedArticle> GetArticlesForDepartment(IDepartment department);
        void StoreFullArticle(Tuple<IFullArticle, IList<IDepartment>> tuple);
        IFullArticle GetFullArticle(IArticle article);
        void DeleteDataBehind(DateTime behind);
        bool IsFullArticleAvailable(string articlePath);
        void ReplaceStartPageArticles(IList<IArticle> allArticlesOnStartpage);
        IArticle GetArticleByPath(string articlePath);
        IAppSettings GetSettings();
        void SetSettings(IAppSettings settings);
    }
}