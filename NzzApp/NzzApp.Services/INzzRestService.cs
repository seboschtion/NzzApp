using System.Threading.Tasks;
using NzzApp.Services.Responses.Articles;
using NzzApp.Services.Responses.BreakingNews;
using NzzApp.Services.Responses.Departments;

namespace NzzApp.Services
{
    public interface INzzRestService
    {
        Task<DepartmentsResponse> GetDepartments();
        Task<ArticlesResponse> GetArticlesForDepartment(string departmentPath);
        Task<FullArticleResponse> GetFullArticle(string articlePath);
        Task<BreakingNewsResponse> GetBreakingNews(int count);
    }
}