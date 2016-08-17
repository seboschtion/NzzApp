using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NzzApp.Data;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Contracts.BreakingNews;
using NzzApp.Model.Contracts.Departments;
using NzzApp.Model.Implementation.Articles;
using NzzApp.Model.Implementation.BreakingNews;
using NzzApp.Model.Implementation.Departments;
using NzzApp.Model.Implementation.Images;
using NzzApp.Model.Implementation.RelatedContents;
using NzzApp.Providers.Helpers;
using NzzApp.Services;
using NzzApp.Services.Responses.Articles;
using NzzApp.Services.Responses.BreakingNews;
using NzzApp.Services.Responses.Departments;
using Sebastian.Toolkit.Logging;
using Article = NzzApp.Model.Implementation.Articles.Article;
using Author = NzzApp.Model.Implementation.Articles.Author;
using Image = NzzApp.Model.Implementation.Images.Image;
using RelatedContent = NzzApp.Services.Responses.Articles.RelatedContent;
using Video = NzzApp.Model.Implementation.RelatedContents.Video;

namespace NzzApp.Providers.Synchonisation
{
    public class SyncProvider : ISyncProvider
    {
        public event EventHandler<TaskResult> FetchDepartmentsCompleted;
        public event EventHandler<TaskResult<IDepartment[]>> FetchArticlesForDepartmentCompleted;
        public event EventHandler<TaskResult<string>> FetchFullArticleCompleted;
        public event EventHandler<TaskResult<IBreakingNews>> BreakingNewsDownloadCompleted;

        private readonly IDataProvider _dataProvider;
        private readonly INzzRestService _nzzRestService;

        private IList<IDepartment> _departments = new List<IDepartment>();

        public SyncProvider(
            IDataProvider dataProvider,
            INzzRestService nzzRestService)
        {
            _dataProvider = dataProvider;
            _nzzRestService = nzzRestService;
        }

        public async void FetchDepartments()
        {
            this.Logger().Debug($"Start {nameof(FetchDepartments)}");
            try
            {
                var departments = await GetAllDepartments();
                if (departments == null)
                {
                    _departments = _dataProvider.GetDepartments();
                    OnFetchDepartmentsCompleted(TaskResult.CreateFail(ErrorTypes.NoInternet));
                    return;
                }
                _dataProvider.UpdateDepartments(departments);
                OnFetchDepartmentsCompleted(TaskResult.CreateSuccess());
                return;
            }
            catch (Exception e)
            {
                OnFetchDepartmentsCompleted(TaskResult.CreateFail(ErrorTypes.Unspecific));
                this.Logger().Error($"{nameof(FetchDepartments)}", e);
                return;
            }
            finally
            {
                this.Logger().Debug($"{nameof(FetchDepartments)} completed");
            }
        }

        public async void FetchArticles(IDepartment[] departments)
        {
            this.Logger().Debug($"Start {nameof(FetchArticles)}");
            try
            {
                foreach (var department in departments)
                {
                    if (department.DepartmentSerialisationType != DepartmentSerialisationType.Json)
                    {
                        break;
                    }

                    var articlesResponse = await GetArticlesForDepartmentPath(department.Path, department.IsStartPage);
                    if (articlesResponse == null)
                    {
                        OnFetchArticlesForDepartmentCompleted(TaskResult<IDepartment[]>.CreateFail(ErrorTypes.NoInternet, departments));
                        return;
                    }
                    _dataProvider.UpdateArticles(articlesResponse);
                    if (department.IsStartPage)
                    {
                        _dataProvider.ReplaceStartPageArticles(articlesResponse.Keys.ToList());
                    }
                }
            }
            catch (Exception e)
            {
                OnFetchArticlesForDepartmentCompleted(TaskResult<IDepartment[]>.CreateFail(ErrorTypes.Unspecific, departments));
                this.Logger().Error($"{nameof(FetchArticles)}", e);
                return;
            }
            OnFetchArticlesForDepartmentCompleted(TaskResult<IDepartment[]>.CreateSuccess(departments));
            this.Logger().Debug($"{nameof(FetchArticles)} completed");
        }

        public async void FetchFullArticle(string articlePath)
        {
            this.Logger().Debug($"Start {nameof(FetchFullArticle)}");
            try
            {
                var fullArticleResponse = await _nzzRestService.GetFullArticle(articlePath);
                if (fullArticleResponse == null)
                {
                    OnFetchFullArticleCompleted(TaskResult<string>.CreateFail(ErrorTypes.NoInternet, articlePath));
                    return;
                }
                var fullArticle = GetFullArticleFromResponse(fullArticleResponse);
                _dataProvider.StoreFullArticle(fullArticle);
                OnFetchFullArticleCompleted(TaskResult<string>.CreateSuccess(fullArticle.Item1.Article.Path));
                return;
            }
            catch (Exception e)
            {
                OnFetchFullArticleCompleted(TaskResult<string>.CreateFail(ErrorTypes.Unspecific, articlePath));
                this.Logger().Error($"{nameof(FetchFullArticle)}", e);
            }
            finally
            {
                this.Logger().Debug($"{nameof(FetchFullArticle)} completed");
            }
        }

        public void DeleteExpiredData()
        {
            var fullWeek = DateTime.UtcNow.AddDays(-7);
            _dataProvider.DeleteDataBehind(fullWeek);
        }

        public async void DownloadBreakingNews(int count)
        {
            this.Logger().Debug($"Start {nameof(DownloadBreakingNews)}");
            try
            {
                var breakingNewsResponse = await _nzzRestService.GetBreakingNews(count);
                if (breakingNewsResponse == null)
                {
                    OnBreakingNewsDownloadCompleted(TaskResult<IBreakingNews>.CreateFail(ErrorTypes.NoInternet, null));
                    return;
                }
                var breakingNews = GetBreakingNewsFromResponse(breakingNewsResponse, count);
                OnBreakingNewsDownloadCompleted(TaskResult<IBreakingNews>.CreateSuccess(breakingNews));
            }
            catch (Exception e)
            {
                OnBreakingNewsDownloadCompleted(TaskResult<IBreakingNews>.CreateFail(ErrorTypes.Unspecific, null));
                this.Logger().Error($"{nameof(DownloadBreakingNews)}", e);
            }
            finally
            {
                this.Logger().Debug($"{nameof(DownloadBreakingNews)} completed");
            }
        }

        public async Task<IBreakingNews> DownloadBreakingNewsAsync(int count)
        {
            var breakingNewsResponse = await _nzzRestService.GetBreakingNews(count);
            var breakingNews = GetBreakingNewsFromResponse(breakingNewsResponse, count);
            return breakingNews;
        }

        private IBreakingNews GetBreakingNewsFromResponse(BreakingNewsResponse breakingNewsResponse, int count)
        {
            var breakingNews = new BreakingNews
            {
                Path = breakingNewsResponse.Path,
                SpeakingName = breakingNewsResponse.SpeakingName
            };

            foreach (var articleResponse in breakingNewsResponse.Articles.Take(count))
            {
                var optimizedArticleResponse = new Services.Responses.Articles.Article
                {
                    Path = articleResponse.Path,
                    PublicationDateTime = articleResponse.PublicationDateTime,
                    IsBreakingNews = articleResponse.IsBreakingNews,
                    Title = articleResponse.Title,
                    SubTitle = articleResponse.SubTitle,
                    HasGallery = articleResponse.HasGallery,
                    IsReportage = articleResponse.IsReportage,
                    IsBriefing = articleResponse.IsBriefing,
                    LeadImage = articleResponse.LeadImage,
                    Departments = articleResponse.Departments,
                    Teaser = string.Join(" ", articleResponse.Teaser.Select(t => t.Text))
                };
                var article = GetArticleFromArticleResponse(optimizedArticleResponse);
                breakingNews.Articles.Add(article.Item1);
            }

            return breakingNews;
        }

        private async Task<IList<IDepartment>> GetAllDepartments()
        {
            var departmentsResponse = await _nzzRestService.GetDepartments();
            if (departmentsResponse?.Menus == null)
            {
                return null;
            }

            _departments.Clear();
            foreach (var department in departmentsResponse.Menus)
            {
                var departments = GetDepartmentFromMenu(department);
                foreach (var d in departments)
                {
                    _departments.Add(d);
                }
            }
            return _departments;
        }

        private IEnumerable<IDepartment> GetDepartmentFromMenu(Menu menu)
        {
            var department = new Department(menu.Path)
            {
                Name = menu.Name,
                DepartmentSerialisationType = DepartmentSerialisationTypeConverter.GetType(menu.Type),
                ShowOn = menu.ShowOn
            };
            if (menu.SubMenus?.Length > 0)
            {
                foreach (var subMenu in menu.SubMenus)
                {
                    var subDepartment = new Department(subMenu.Path)
                    {
                        Name = subMenu.Name,
                        ParentDepartmentPath = department.Path,
                        DepartmentSerialisationType = DepartmentSerialisationTypeConverter.GetType(menu.Type),
                        ShowOn = menu.ShowOn
                    };
                    department.SubDepartments.Add(subDepartment);
                    yield return subDepartment;
                }
            }
            yield return department;
        }

        private async Task<IDictionary<IArticle, IList<IDepartment>>> GetArticlesForDepartmentPath(string path, bool onStartPage)
        {
            var articlesResponse = await _nzzRestService.GetArticlesForDepartment(path);
            if (articlesResponse == null)
            {
                return null;
            }

            var result = new Dictionary<IArticle, IList<IDepartment>>();
            foreach (var article in articlesResponse.Articles)
            {
                var tuple = GetArticleFromArticleResponse(article);
                tuple.Item1.IsOnStartPage = onStartPage;
                result.Add(tuple.Item1, tuple.Item2);
            }
            return result;
        }

        private Tuple<IArticle, IList<IDepartment>> GetArticleFromArticleResponse(Services.Responses.Articles.Article articleResponse)
        {
            var article = new Article(articleResponse.Path)
            {
                PublishedOn = GetDateTimeFromString(articleResponse.PublicationDateTime),
                IsBreakingNews = articleResponse.IsBreakingNews,
                Title = articleResponse.Title,
                SubTitle = articleResponse.SubTitle,
                Teaser = articleResponse.Teaser,
                HasGallery = articleResponse.HasGallery,
                IsReportage = articleResponse.IsReportage,
                IsBriefing = articleResponse.IsBriefing
            };

            if (articleResponse.LeadImage != null)
            {
                article.LeadImage = new Image(articleResponse.LeadImage.Guid)
                {
                    Caption = articleResponse.LeadImage.Caption,
                    PathTemplate = articleResponse.LeadImage.Path,
                    Source = articleResponse.LeadImage.Source,
                    MimeType = articleResponse.LeadImage.MimeType,
                    OriginalWidth = articleResponse.LeadImage.OriginalWidth ?? 0,
                    OriginalHeight = articleResponse.LeadImage.OriginalHeight ?? 0
                };
            }

            var departments = new List<IDepartment>();
            foreach (var department in articleResponse.Departments)
            {
                var result = _departments.FirstOrDefault(d => d.Name == department);
                if (result != null)
                {
                    departments.Add(result);
                }
            }

            return new Tuple<IArticle, IList<IDepartment>>(article, departments);
        }

        private Tuple<IFullArticle, IList<IDepartment>> GetFullArticleFromResponse(FullArticleResponse response)
        {
            var tuple = GetArticleFromArticleResponse(response);
            var fullArticle = new FullArticle(tuple.Item1)
            {
                LeadText = response.LeadText,
                WebUrl = response.WebUrl,
                ShortWebUrl = response.ShortWebUrl,
                Agency = response.Agency
            };
            foreach (var author in response.Authors)
            {
                fullArticle.Authors.Add(new Author()
                {
                    Name = author.Name,
                    Abbreviation = author.Abbreviation
                });
            }
            foreach (var relatedArticle in response.RelatedArticles)
            {
                fullArticle.RelatedArticles.Add(GetArticleFromArticleResponse(relatedArticle).Item1);
            }
            foreach (var relatedContent in response.RelatedContent)
            {
                fullArticle.RelatedContents.Add(ParseRelatedContent(relatedContent));
            }
            foreach (var body in response.Body)
            {
                var paragraph = new Paragraph()
                {
                    Text = body.Text,
                    ParagraphType = ParagraphTypeConverter.ToType(body.Style)
                };
                foreach (var box in body.Boxes)
                {
                    paragraph.Boxes.Add(ParseRelatedContent(box));
                }
                foreach (var item in body.Items)
                {
                    paragraph.Items.Add(item);
                }
                fullArticle.Paragraphs.Add(paragraph);
            }
            return new Tuple<IFullArticle, IList<IDepartment>>(fullArticle, tuple.Item2);
        }

        private IRelatedContent ParseRelatedContent(RelatedContent relatedContent)
        {
            switch (relatedContent.Type)
            {
                case "video":
                    return new Video()
                    {
                        AuthorId = relatedContent.AuthorId,
                        CustomerId = relatedContent.CustomerId,
                        VideoId = relatedContent.VideoId
                    };
                case "image":
                    return new Image(relatedContent.Guid)
                    {
                        Caption = relatedContent.Caption,
                        MimeType = relatedContent.MimeType,
                        PathTemplate = relatedContent.Path,
                        Source = relatedContent.Source,
                        OriginalHeight = relatedContent.OriginalHeight ?? 0,
                        OriginalWidth = relatedContent.OriginalWidth ?? 0
                    };
                case "html":
                    return new HtmlRelatedContent
                    {
                        Body = relatedContent.Body
                    };
                case "infobox":
                    return new InfoRelatedContent
                    {
                        Body = relatedContent.Body,
                        Title = relatedContent.Title
                    };
                case "gallery":
                    return new DefaultRelatedContent();
                    //return GetGalleryFromResponse(relatedContent);
            }
            return new DefaultRelatedContent();
        }

        //private Gallery GetGalleryFromResponse(RelatedContent relatedContent)
        //{
        //    var gallery = new Gallery
        //    {
        //        Id = relatedContent.Guid,
        //        Title = relatedContent.Title
        //    };
        //    foreach (var image in relatedContent.Images)
        //    {
        //        gallery.Images.Add(new Image(image.Guid)
        //        {
        //            Caption = image.Caption,
        //            PathTemplate = image.Path,
        //            MimeType = image.MimeType,
        //            Source = image.Source,
        //            OriginalWidth = image.OriginalWidth ?? 0,
        //            OriginalHeight = image.OriginalHeight ?? 0
        //        });
        //    }

        //    return gallery;
        //}

        protected virtual void OnFetchDepartmentsCompleted(TaskResult e)
        {
            FetchDepartmentsCompleted?.Invoke(this, e);
        }

        protected virtual void OnFetchArticlesForDepartmentCompleted(TaskResult<IDepartment[]> e)
        {
            FetchArticlesForDepartmentCompleted?.Invoke(this, e);
        }

        protected virtual void OnFetchFullArticleCompleted(TaskResult<string> e)
        {
            FetchFullArticleCompleted?.Invoke(this, e);
        }

        protected virtual void OnBreakingNewsDownloadCompleted(TaskResult<IBreakingNews> e)
        {
            BreakingNewsDownloadCompleted?.Invoke(this, e);
        }

        private DateTime GetDateTimeFromString(string dateString)
        {
            DateTime result;
            if(DateTime.TryParse(dateString, out result))
            {
                return result;
            }
            return new DateTime(DateTime.Now.Year, 1, 1);
        }
    }
}