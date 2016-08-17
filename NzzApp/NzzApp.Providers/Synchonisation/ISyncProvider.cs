using System;
using System.Threading.Tasks;
using NzzApp.Model.Contracts.BreakingNews;
using NzzApp.Model.Contracts.Departments;
using NzzApp.Providers.Helpers;

namespace NzzApp.Providers.Synchonisation
{
    public interface ISyncProvider
    {
        event EventHandler<TaskResult> FetchDepartmentsCompleted;
        event EventHandler<TaskResult<IDepartment[]>> FetchArticlesForDepartmentCompleted;
        event EventHandler<TaskResult<string>> FetchFullArticleCompleted;
        event EventHandler<TaskResult<IBreakingNews>> BreakingNewsDownloadCompleted;

        void FetchDepartments();
        void FetchArticles(IDepartment[] departments);
        void FetchFullArticle(string articlePath);
        void DeleteExpiredData();
        void DownloadBreakingNews(int count);
        Task<IBreakingNews> DownloadBreakingNewsAsync(int count);
    }
}