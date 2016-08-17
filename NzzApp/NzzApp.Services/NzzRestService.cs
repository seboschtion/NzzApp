using System.Threading.Tasks;
using NzzApp.Services.Json;
using NzzApp.Services.Responses.Articles;
using NzzApp.Services.Responses.BreakingNews;
using NzzApp.Services.Responses.Departments;

namespace NzzApp.Services
{
    public class NzzRestService : RestClient, INzzRestService
    {
        public async Task<DepartmentsResponse> GetDepartments()
        {
            return await HttpClientGet<DepartmentsResponse>(NzzRestServiceUrls.DepartmentsAbsolute);
        }

        public async Task<ArticlesResponse> GetArticlesForDepartment(string departmentPath)
        {
            return await HttpClientGet<ArticlesResponse>(NzzRestServiceUrls.BaseUrl + departmentPath);
        }

        public async Task<FullArticleResponse> GetFullArticle(string articlePath)
        {
            return await HttpClientGet<FullArticleResponse>(NzzRestServiceUrls.BaseUrl + articlePath);
        }

        public async Task<BreakingNewsResponse> GetBreakingNews(int count)
        {
            return await HttpClientGet<BreakingNewsResponse>(string.Format(NzzRestServiceUrls.NewsTickerAbsolute, count));
        }
    }
}
