using System;
using System.Collections.Generic;
using System.Linq;
using NzzApp.Data.Versions;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Contracts.Departments;
using NzzApp.Model.Contracts.Images;
using NzzApp.Model.Contracts.Settings;
using NzzApp.Model.Implementation.Articles;
using NzzApp.Model.Implementation.Departments;
using NzzApp.Model.Implementation.Images;
using NzzApp.Model.Implementation.RelatedContents;
using NzzApp.Model.Implementation.Settings;
using Sebastian.Toolkit.SQLite;
using SQLitePCL;

namespace NzzApp.Data.SQLite
{
    public class SQLiteDataProvider : SQLiteBase, IDataProvider
    {
        protected override string DatabaseFilename => "nzzapp.sqlite";
        protected override IEnumerable<DatabaseVersion> DatabaseVersions => new List<DatabaseVersion>
        {
            new Version0001(),
            new Version0002(),
            new Version0003(),
            new Version0004(),
            new Version0005(),
            new Version0006(),
            new Version0007()
        };

        public void UpdateDepartments(IList<IDepartment> departments)
        {
            const string deleteExpiredSql = "DELETE FROM Department WHERE Path NOT IN ({0})";
            const string addNewSql = "INSERT OR REPLACE INTO Department (Name, Path, DepartmentSerialisationType, ShowOn, ParentDepartment, UpdatedOn) " +
                                     "VALUES (@name, @path, @type, @showOn, @parent, @updatedOn)";

            using (var transaction = new SQLiteTransaction(SharedConnection))
            {
                string existingPaths = GetPropertiesCommaSeparated(departments.Select(d => d.Path));
                transaction.Execute(string.Format(deleteExpiredSql, existingPaths));

                foreach (var department in departments)
                {
                    using (var statement = transaction.Prepare(addNewSql))
                    {
                        statement.Binding("@name", department.Name);
                        statement.Binding("@path", department.Path);
                        statement.Binding("@type",
                            DepartmentSerialisationTypeConverter.GetString(department.DepartmentSerialisationType));
                        statement.Binding("@showOn", department.ShowOn);
                        statement.Binding("@parent", department.ParentDepartmentPath);
                        statement.Binding("@updatedOn", DateTime.UtcNow);

                        transaction.Execute(statement);
                    }
                }

                transaction.Commit();
            }
        }

        public IList<IDepartment> GetDepartments()
        {
            const string sql = "SELECT Name, Path, DepartmentSerialisationType, ShowOn, ParentDepartment " +
                               "FROM Department";

            var result = new List<IDepartment>();

            using (var statement = SharedConnection.Prepare(sql))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    var department = new Department(statement.GetValue<string>("Path"))
                    {
                        Name = statement.GetValue<string>("Name"),
                        DepartmentSerialisationType =
                            DepartmentSerialisationTypeConverter.GetType(statement.GetValue<string>("DepartmentSerialisationType")),
                        ShowOn = statement.GetValue<string>("ShowOn"),
                        ParentDepartmentPath = statement.GetValue<string>("ParentDepartment")
                    };
                    result.Add(department);
                }
            }
            return result;
        }

        public IList<IDepartment> GetDepartments(IArticle article)
        {
            const string sql = "SELECT d.Name, d.Path, d.DepartmentSerialisationType, d.ParentDepartment " +
                               "FROM DepartmentHasArticles da " +
                               "JOIN Department d ON (d.Path = da.DepartmentPath) " +
                               "WHERE da.ArticlePath = @articlePath";

            var result = new List<IDepartment>();

            using (var statement = SharedConnection.Prepare(sql))
            {
                statement.Binding("@articlePath", article.Path);
                while (statement.Step() == SQLiteResult.ROW)
                {
                    var department = new Department(statement.GetValue<string>("Path"))
                    {
                        Name = statement.GetValue<string>("Name"),
                        DepartmentSerialisationType =
                            DepartmentSerialisationTypeConverter.GetType(statement.GetValue<string>("DepartmentSerialisationType")),
                        ParentDepartmentPath = statement.GetValue<string>("ParentDepartment")
                    };
                    result.Add(department);
                }
            }
            return result;
        }

        public void UpdateArticles(IDictionary<IArticle, IList<IDepartment>> articles)
        {
            const string sql = "INSERT OR REPLACE INTO Article (Path, PublishedOn, IsBreakingNews, Title, Subtitle, Teaser, LeadImageId, HasGallery, IsReportage, IsBriefing, UpdatedOn) " +
                               "VALUES (@path, @publishedOn, @isBreakingNews, @title, @subtitle, @teaser, @leadImageId, @hasGallery, @isReportage, @isBriefing, @updatedOn)";
            const string addRelationSql = "INSERT OR REPLACE INTO DepartmentHasArticles (ArticlePath, DepartmentPath, UpdatedOn) " +
                                          "VALUES (@articlePath, @departmentPath, @updatedOn)";
            const string delRelationSql = "DELETE FROM DepartmentHasArticles WHERE ArticlePath = @articlePath AND DepartmentPath NOT IN ({0})";

            using (var transaction = new SQLiteTransaction(SharedConnection))
            {
                foreach (var article in articles.Keys)
                {
                    using (var statement = transaction.Prepare(sql))
                    {
                        statement.Binding("@path", article.Path);
                        statement.Binding("@publishedOn", article.PublishedOn);
                        statement.Binding("@title", article.Title);
                        statement.Binding("@subtitle", article.SubTitle);
                        statement.Binding("@teaser", article.Teaser);
                        statement.Binding("@hasGallery", article.HasGallery);
                        statement.Binding("@isReportage", article.IsReportage);
                        statement.Binding("@isBriefing", article.IsBriefing);
                        statement.Binding("@isBreakingNews", article.IsBreakingNews);
                        statement.Binding("@updatedOn", DateTime.UtcNow);
                        if (article.LeadImage != null)
                        {
                            statement.Binding("@leadImageId", article.LeadImage.Id);
                        }

                        transaction.Execute(statement);
                    }

                    IList<IDepartment> departments;
                    var success = articles.TryGetValue(article, out departments);
                    if (success)
                    {
                        foreach (var department in departments)
                        {
                            using (var statement = transaction.Prepare(addRelationSql))
                            {
                                statement.Binding("@articlePath", article.Path);
                                statement.Binding("@departmentPath", department.Path);
                                statement.Binding("@updatedOn", DateTime.UtcNow);
                                transaction.Execute(statement);
                            }
                        }

                        string departmentsString = GetPropertiesCommaSeparated(departments.Select(d => d.Path));
                        using (var statement = transaction.Prepare(string.Format(delRelationSql, departmentsString)))
                        {
                            statement.Binding("@articlePath", article.Path);
                            transaction.Execute(statement);
                        }
                    }

                    if (article.LeadImage != null)
                    {
                        ExecuteInsertImage(transaction, article.LeadImage);
                    }
                }
                transaction.Commit();
            }
        }

        private void ExecuteInsertImage(SQLiteTransaction transaction, IImage image)
        {
            const string sql = "INSERT OR REPLACE INTO Image (Id, Source, Caption, MimeType, Path, OriginalWidth, OriginalHeight, UpdatedOn) " +
                               "VALUES (@id, @source, @caption, @mimeType, @path, @originalWidth, @originalHeight, @updatedOn)";

            using (var statement = transaction.Prepare(sql))
            {
                statement.Binding("@id", image.Id);
                statement.Binding("@source", image.Source);
                statement.Binding("@caption", image.Caption);
                statement.Binding("@mimeType", image.MimeType);
                statement.Binding("@path", image.PathTemplate);
                statement.Binding("@originalWidth", image.OriginalWidth);
                statement.Binding("@originalHeight", image.OriginalHeight);
                statement.Binding("@updatedOn", DateTime.UtcNow);
                transaction.Execute(statement);
            }
        }

        public IList<ViewOptimizedArticle> GetArticlesForDepartment(IDepartment department)
        {
            string sql;

            if (department.IsStartPage)
            {
                sql =
                    "SELECT " +
                    "(SELECT d.Name FROM DepartmentHasArticles da JOIN department d ON(d.Path = da.DepartmentPath) WHERE da.articlepath = a.Path ORDER BY length(d.Path) desc LIMIT 1) AS ExactDepartmentName, " +
                    "a.Path, a.PublishedOn, a.IsBreakingNews, a.Title, a.Subtitle, a.Teaser, a.HasGallery, a.IsReportage, a.IsBriefing, i.Id, i.Path AS ImagePath, i.Source, i.Caption, i.MimeType, i.OriginalWidth, i.OriginalHeight, am.StartpageSort " +
                    "FROM Article a " +
                    "LEFT JOIN Image i ON (a.LeadImageId = i.Id) " +
                    "LEFT JOIN ArticleMeta am ON (am.ArticlePath = a.Path) " +
                    "WHERE am.IsOnStartpage = 1 " +
                    "ORDER BY am.StartpageSort ASC, a.PublishedOn DESC  " +
                    "LIMIT 33";
            }
            else
            {
                sql =
                    "SELECT " +
                    "(SELECT d.Name FROM DepartmentHasArticles da JOIN department d ON(d.Path = da.DepartmentPath) WHERE da.articlepath = a.Path ORDER BY length(d.Path) desc LIMIT 1) AS ExactDepartmentName, " +
                    "a.Path, a.PublishedOn, a.IsBreakingNews, a.Title, a.Subtitle, a.Teaser, a.HasGallery, a.IsReportage, a.IsBriefing, i.Id, i.Path AS ImagePath, i.Source, i.Caption, i.MimeType, i.OriginalWidth, i.OriginalHeight " +
                    "FROM DepartmentHasArticles da " +
                    "JOIN Article a ON (a.Path = da.ArticlePath) " +
                    "LEFT JOIN Image i ON (a.LeadImageId = i.Id)" +
                    "WHERE da.DepartmentPath = @departmentPath " +
                    "ORDER BY a.PublishedOn DESC  " +
                    "LIMIT 33";
            }

            var result = new List<ViewOptimizedArticle>();

            using (var statement = SharedConnection.Prepare(sql))
            {
                if (!department.IsStartPage)
                {
                    statement.Binding("@departmentPath", department.Path);
                }
                
                while (statement.Step() == SQLiteResult.ROW)
                {
                    var viewOptimizedArticle = new ViewOptimizedArticle();
                    viewOptimizedArticle.Article = new Article(statement.GetValue<string>("Path"))
                    {
                        PublishedOn = statement.GetValue<DateTime>("PublishedOn"),
                        Title = statement.GetValue<string>("Title"),
                        SubTitle = statement.GetValue<string>("Subtitle"),
                        Teaser = statement.GetValue<string>("Teaser"),
                        HasGallery = statement.GetValue<bool>("HasGallery"),
                        IsReportage = statement.GetValue<bool>("IsReportage"),
                        IsBriefing = statement.GetValue<bool>("IsBriefing"),
                        IsBreakingNews = statement.GetValue<bool>("IsBreakingNews"),
                        IsOnStartPage = department.IsStartPage,
                        LeadImage = new Image(statement.GetValue<string>("Id"))
                        {
                            Caption = statement.GetValue<string>("Caption"),
                            MimeType = statement.GetValue<string>("MimeType"),
                            PathTemplate = statement.GetValue<string>("ImagePath"),
                            Source = statement.GetValue<string>("Source"),
                            OriginalHeight = statement.GetValue<int>("OriginalHeight"),
                            OriginalWidth = statement.GetValue<int>("OriginalWidth")
                        }
                    };

                    if (department.IsStartPage)
                    {
                        viewOptimizedArticle.Sort = statement.GetValue<int>("StartpageSort");
                    }

                    viewOptimizedArticle.ExactDepartmentName = statement.GetValue<string>("ExactDepartmentName");

                    result.Add(viewOptimizedArticle);
                }
            }

            return result;
        }

        public void StoreFullArticle(Tuple<IFullArticle, IList<IDepartment>> tuple)
        {
            const string updateSql = "INSERT OR REPLACE INTO FullArticle (ArticlePath, LeadText, ShortWebUrl, WebUrl, Agency, UpdatedOn) " +
                                     "VALUES (@path, @leadText, @shortWebUrl, @webUrl, @agency, @updatedOn)";
            const string paragraphSql = "INSERT OR REPLACE INTO Paragraph (Id, ArticlePath, Text, Type, Sort, UpdatedOn) " +
                                        "VALUES (@id, @articlePath, @text, @type, @sort, @updatedOn)";
            const string authorSql = "INSERT OR REPLACE INTO Author (ArticlePath, Name, Abbreviation, UpdatedOn) " +
                                     "VALUES (@articlePath, @name, @abbreviation, @updatedOn)";
            const string relatedSql = "INSERT OR REPLACE INTO ArticleHasRelatedArticles (SourceArticlePath, TargetArticlePath, UpdatedOn) " +
                                      "VALUES (@sourceArticlePath, @targetArticlePath, @updatedOn)";
            const string itemSql = "INSERT OR REPLACE INTO Item (ParagraphId, Content, Sort, UpdatedOn) " +
                                   "VALUES (@paragraphId, @content, @sort, @updatedOn)";
            
            var fullArticle = tuple.Item1;

            var articlesToAdd = new Dictionary<IArticle, IList<IDepartment>> {{fullArticle.Article, tuple.Item2}};
            foreach (var relatedArticle in fullArticle.RelatedArticles)
            {
                articlesToAdd.Add(relatedArticle, new List<IDepartment>());
            }
            UpdateArticles(articlesToAdd);

            using (var transaction = new SQLiteTransaction(SharedConnection))
            {
                using (var statement = transaction.Prepare(updateSql))
                {
                    statement.Binding("@path", fullArticle.Article.Path);
                    statement.Binding("@leadText", fullArticle.LeadText);
                    statement.Binding("@shortWebUrl", fullArticle.ShortWebUrl);
                    statement.Binding("@webUrl", fullArticle.WebUrl);
                    statement.Binding("@agency", fullArticle.Agency);
                    statement.Binding("@updatedOn", DateTime.UtcNow);
                    transaction.Execute(statement);
                }

                int paragraphCounter = 1;
                foreach (var paragraph in fullArticle.Paragraphs)
                {
                    string paragraphId = paragraphCounter + fullArticle.Article.Path;
                    using (var statement = transaction.Prepare(paragraphSql))
                    {
                        statement.Binding("@id", paragraphId);
                        statement.Binding("@articlePath", fullArticle.Article.Path);
                        statement.Binding("@text", paragraph.Text);
                        statement.Binding("@type", ParagraphTypeConverter.ToText(paragraph.ParagraphType));
                        statement.Binding("@sort", paragraphCounter);
                        statement.Binding("@updatedOn", DateTime.UtcNow);
                        transaction.Execute(statement);
                    }

                    ExecuteInsertRelatedContent(transaction, true, paragraph.Boxes, fullArticle.Article.Path, paragraphId);

                    int itemCounter = 0;
                    foreach (var item in paragraph.Items)
                    {
                        itemCounter++;
                        using (var statement = transaction.Prepare(itemSql))
                        {
                            statement.Binding("@paragraphId", paragraphId);
                            statement.Binding("@content", item);
                            statement.Binding("@sort", itemCounter);
                            statement.Binding("@updatedOn", DateTime.UtcNow);
                            transaction.Execute(statement);
                        }
                    }

                    paragraphCounter++;
                }

                foreach (var author in fullArticle.Authors)
                {
                    using (var statement = transaction.Prepare(authorSql))
                    {
                        statement.Binding("@articlePath", fullArticle.Article.Path);
                        statement.Binding("@name", author.Name);
                        statement.Binding("@abbreviation", author.Abbreviation);
                        statement.Binding("@updatedOn", DateTime.UtcNow);
                        transaction.Execute(statement);
                    }
                }

                foreach (var article in fullArticle.RelatedArticles)
                {
                    using (var statement = transaction.Prepare(relatedSql))
                    {
                        statement.Binding("@sourceArticlePath", fullArticle.Article.Path);
                        statement.Binding("@targetArticlePath", article.Path);
                        statement.Binding("@updatedOn", DateTime.UtcNow);
                        transaction.Execute(statement);
                    }
                }

                ExecuteInsertRelatedContent(transaction, false, fullArticle.RelatedContents, fullArticle.Article.Path, string.Empty);

                transaction.Commit();
            }
        }

        private void ExecuteInsertRelatedContent(SQLiteTransaction transaction, bool isBox, IList<IRelatedContent> relatedContents, string articlePath, string paragraphId)
        {
            const string sql = "INSERT OR REPLACE INTO RelatedContent (RelatedContentType, ArticlePath, ParagraphId, Type, VideoAuthorId, VideoCustomerId, VideoId, ImageId, ImagePath, ImageSource, ImageCaption, ImageMimeType, ImageOriginalWidth, ImageOriginalHeight, HtmlBody, InfoTitle, InfoBody, GalleryId, GalleryTitle, Sort, UpdatedOn) " +
                               "VALUES(@relatedContentType, @articlePath, @paragraphId, @type, @videoAuthorId, @videoCustomerId, @videoId, @imageId, @imagePath, @imageSource, @imageCaption, @imageMimeType, @imageOriginalWidth, @imageOriginalHeight, @htmlBody, @infoTitle, @infoBody, @galleryId, @galleryTitle, @sort, @updatedOn)";

            int counter = 0;
            foreach (var relatedContent in relatedContents)
            {
                Gallery gallery;
                counter++;
                using (var statement = transaction.Prepare(sql))
                {
                    var video = relatedContent as Video;
                    if (video != null)
                    {
                        statement.Binding("@videoAuthorId", video.AuthorId);
                        statement.Binding("@videoCustomerId", video.CustomerId);
                        statement.Binding("@videoId", video.VideoId);
                    }
                    var image = relatedContent as Image;
                    if (image != null)
                    {
                        statement.Binding("@imageId", image.Id);
                        statement.Binding("@imagePath", image.PathTemplate);
                        statement.Binding("@imageSource", image.Source);
                        statement.Binding("@imageCaption", image.Caption);
                        statement.Binding("@imageMimeType", image.MimeType);
                        statement.Binding("@imageOriginalWidth", image.OriginalWidth);
                        statement.Binding("@imageOriginalHeight", image.OriginalHeight);
                    }
                    var html = relatedContent as HtmlRelatedContent;
                    if (html != null)
                    {
                        statement.Binding("@htmlBody", html.Body);
                    }
                    var info = relatedContent as InfoRelatedContent;
                    if (info != null)
                    {
                        statement.Binding("@infoTitle", info.Title);
                        statement.Binding("@infoBody", info.Body);
                    }
                    gallery = relatedContent as Gallery;
                    if (gallery != null)
                    {
                        statement.Binding("@galleryId", gallery.Id);
                        statement.Binding("@galleryTitle", gallery.Title);
                    }
                    statement.Binding("@relatedContentType", isBox ? "box" : "related");
                    statement.Binding("@articlePath", articlePath);
                    statement.Binding("@paragraphId", paragraphId);
                    statement.Binding("@type", RelatedContentConverter.GetString(relatedContent.Type));
                    statement.Binding("@sort", counter);
                    statement.Binding("@updatedOn", DateTime.UtcNow);
                    transaction.Execute(statement);
                }

                if (gallery != null)
                {
                    const string relationSql =
                        "INSERT OR REPLACE INTO GalleryHasImages (GalleryId, ImageId, Sort, UpdatedOn) VALUES (@galleryId, @imageId, @sort, @updatedOn)";
                    int imageCounter = 1;
                    foreach (var galleryImage in gallery.Images)
                    {
                        ExecuteInsertImage(transaction, galleryImage);

                        using (var statement = transaction.Prepare(relationSql))
                        {
                            statement.Binding("@galleryId", gallery.Id);
                            statement.Binding("@imageId", galleryImage.Id);
                            statement.Binding("@sort", imageCounter++);
                            statement.Binding("@updatedOn", DateTime.UtcNow);
                            transaction.Execute(statement);
                        }
                    }

                }
            }
        }

        public IFullArticle GetFullArticle(IArticle article)
        {
            const string additionalArticleInfoSql = "SELECT LeadText, WebUrl, ShortWebUrl, Agency FROM FullArticle WHERE ArticlePath = @path";
            const string authorsSql = "SELECT Name, Abbreviation FROM Author WHERE ArticlePath = @path";
            const string relatedSql = "SELECT a.Path, a.PublishedOn, a.IsBreakingNews, a.Title, a.Subtitle, a.Teaser, a.HasGallery, a.IsReportage, a.IsBriefing, i.Id, i.Path AS ImagePath, i.Source, i.Caption, i.MimeType, i.OriginalWidth, i.OriginalHeight " +
                                      "FROM ArticleHasRelatedArticles ahra " +
                                      "JOIN Article a ON (a.Path = ahra.TargetArticlePath) " +
                                      "LEFT JOIN Image i ON (a.LeadImageId = i.Id) " +
                                      "WHERE ahra.SourceArticlePath = @sourceArticlePath";
            const string paragraphSql = "SELECT Id, Text, Type FROM Paragraph WHERE ArticlePath = @path ORDER BY Sort ASC";
            const string itemSql = "SELECT Content " +
                                   "FROM Item " +
                                   "WHERE ParagraphId = @paragraphId ORDER BY Sort ASC";

            var result = new FullArticle(article);
            using (var statement = SharedConnection.Prepare(additionalArticleInfoSql))
            {
                statement.Binding("@path", article.Path);
                while (statement.Step() == SQLiteResult.ROW)
                {
                    result.LeadText = statement.GetValue<string>("LeadText");
                    result.WebUrl = statement.GetValue<string>("WebUrl");
                    result.ShortWebUrl = statement.GetValue<string>("ShortWebUrl");
                    result.Agency = statement.GetValue<string>("Agency");
                }
            }

            using (var statement = SharedConnection.Prepare(authorsSql))
            {
                statement.Binding("@path", article.Path);
                while (statement.Step() == SQLiteResult.ROW)
                {
                    result.Authors.Add(new Author()
                    {
                        Name = statement.GetValue<string>("Name"),
                        Abbreviation = statement.GetValue<string>("Abbreviation")
                    });
                }
            }

            using (var statement = SharedConnection.Prepare(relatedSql))
            {
                statement.Binding("@sourceArticlePath", article.Path);
                while (statement.Step() == SQLiteResult.ROW)
                {
                    result.RelatedArticles.Add(new Article(statement.GetValue<string>("Path"))
                    {
                        PublishedOn = statement.GetValue<DateTime>("PublishedOn"),
                        IsBreakingNews = statement.GetValue<bool>("IsBreakingNews"),
                        Title = statement.GetValue<string>("Title"),
                        SubTitle = statement.GetValue<string>("Subtitle"),
                        Teaser = statement.GetValue<string>("Teaser"),
                        HasGallery = statement.GetValue<bool>("HasGallery"),
                        IsReportage = statement.GetValue<bool>("IsReportage"),
                        IsBriefing = statement.GetValue<bool>("IsBriefing"),
                        LeadImage = new Image(statement.GetValue<string>("Id"))
                        {
                            Caption = statement.GetValue<string>("Caption"),
                            MimeType = statement.GetValue<string>("MimeType"),
                            PathTemplate = statement.GetValue<string>("ImagePath"),
                            Source = statement.GetValue<string>("Source"),
                            OriginalWidth = statement.GetValue<int>("OriginalWidth"),
                            OriginalHeight = statement.GetValue<int>("OriginalHeight")
                        }
                    });
                }
            }

            var paragraphIds = new List<string>();
            using (var statement = SharedConnection.Prepare(paragraphSql))
            {
                statement.Binding("@path", article.Path);
                while (statement.Step() == SQLiteResult.ROW)
                {
                    paragraphIds.Add(statement.GetValue<string>("Id"));
                    var paragraph = new Paragraph()
                    {
                        Text = statement.GetValue<string>("Text"),
                        ParagraphType = ParagraphTypeConverter.ToType(statement.GetValue<string>("Type"))
                    };
                    result.Paragraphs.Add(paragraph);
                }
            }

            for(int i = 0; i < paragraphIds.Count; i++)
            {
                var boxes = GetRelatedContent(true, null, paragraphIds[i]);
                foreach (var box in boxes)
                {
                    result.Paragraphs[i].Boxes.Add(box);
                }

                using (var statement = SharedConnection.Prepare(itemSql))
                {
                    statement.Binding("@paragraphId", paragraphIds[i]);
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        result.Paragraphs[i].Items.Add(statement.GetValue<string>("Content"));
                    }
                }
            }

            var relatedContent = GetRelatedContent(false, article.Path, null);
            foreach (var content in relatedContent)
            {
                result.RelatedContents.Add(content);
            }

            return result;
        }

        private IList<IRelatedContent> GetRelatedContent(bool isBox, string articlePath, string paragraphId)
        {
            const string select = "SELECT Type, VideoAuthorId, VideoCustomerId, VideoId, ImageId, ImagePath, ImageSource, ImageCaption, ImageMimeType, ImageOriginalHeight, ImageOriginalWidth, HtmlBody, InfoTitle, InfoBody, GalleryId, GalleryTitle ";
            const string relatedSql = @"FROM RelatedContent WHERE ArticlePath = @where AND RelatedContentType = ""related"" ORDER BY Sort ASC";
            const string boxSql = @"FROM RelatedContent WHERE ParagraphId = @where AND RelatedContentType = ""box"" ORDER BY Sort ASC";
            string sql = select + (isBox ? boxSql : relatedSql);
            IList<IRelatedContent> result = new List<IRelatedContent>();
            using (var statement = SharedConnection.Prepare(sql))
            {
                statement.Binding("@where", isBox ? paragraphId : articlePath);

                while (statement.Step() == SQLiteResult.ROW)
                {
                    var type = RelatedContentConverter.GetType(statement.GetValue<string>("Type"));
                    switch (type)
                    {
                        case RelatedContentType.Video:
                            result.Add(new Video()
                            {
                                AuthorId = statement.GetValue<string>("VideoAuthorId"),
                                CustomerId = statement.GetValue<string>("VideoCustomerId"),
                                VideoId = statement.GetValue<string>("VideoId")
                            });
                            break;
                        case RelatedContentType.Image:
                            result.Add(new Image(statement.GetValue<string>("ImageId"))
                            {
                                Caption = statement.GetValue<string>("ImageCaption"),
                                MimeType = statement.GetValue<string>("ImageMimeType"),
                                PathTemplate = statement.GetValue<string>("ImagePath"),
                                Source = statement.GetValue<string>("ImageSource"),
                                OriginalHeight = statement.GetValue<int>("ImageOriginalHeight"),
                                OriginalWidth = statement.GetValue<int>("ImageOriginalWidth")
                            });
                            break;
                        case RelatedContentType.Html:
                            result.Add(new HtmlRelatedContent()
                            {
                                Body = statement.GetValue<string>("HtmlBody")
                            });
                            break;
                        case RelatedContentType.Infobox:
                            result.Add(new InfoRelatedContent()
                            {
                                Title = statement.GetValue<string>("InfoTitle"),
                                Body = statement.GetValue<string>("InfoBody")
                            });
                            break;
                        case RelatedContentType.Gallery:
                            result.Add(new Gallery()
                            {
                                Id = statement.GetValue<string>("GalleryId"),
                                Title = statement.GetValue<string>("GalleryTitle")
                            });
                            break;
                        default:
                            result.Add(new DefaultRelatedContent());
                            break;
                    }
                }
            }

            const string galleryImageSql = "SELECT i.Id, i.Path, i.Source, i.Caption, i.MimeType, i.OriginalHeight, i.OriginalWidth " +
                                           "FROM GalleryHasImages g " +
                                           "LEFT JOIN Image i ON (i.Id = g.ImageId) " +
                                           "WHERE g.GalleryId = @galleryId " +
                                           "ORDER BY Sort ASC";

            foreach (var gallery in result.Where(r => r.Type == RelatedContentType.Gallery).OfType<Gallery>())
            {
                using (var statement = SharedConnection.Prepare(galleryImageSql))
                {
                    statement.Binding("@galleryId", gallery.Id);
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        gallery.Images.Add(new Image(statement.GetValue<string>("Id"))
                        {
                            PathTemplate = statement.GetValue<string>("Path"),
                            Source = statement.GetValue<string>("Source"),
                            Caption = statement.GetValue<string>("Caption"),
                            MimeType = statement.GetValue<string>("MimeType"),
                            OriginalHeight = statement.GetValue<int>("OriginalHeight"),
                            OriginalWidth = statement.GetValue<int>("OriginalWidth")
                        });
                    }
                }
            }

            return result;
        } 

        public void DeleteDataBehind(DateTime behind)
        {
            string[] tables =
            {
                "Article", "Department", "ArticleMeta", "FullArticle", "Paragraph",
                "ArticleHasRelatedArticles", "Image", "Author", "DepartmentHasArticles", "RelatedContent", "Item",
                "GalleryHasImages"
            };
            const string sql = "DELETE FROM {0} WHERE UpdatedOn < @behind";

            using (var transaction = new SQLiteTransaction(SharedConnection))
            {
                foreach (var table in tables)
                {
                    using (var statement = transaction.Prepare(string.Format(sql, table)))
                    {
                        statement.Binding("@behind", behind);
                        transaction.Execute(statement);
                    }
                }
                transaction.Commit();
            }
        }

        public bool IsFullArticleAvailable(string articlePath)
        {
            const string sql = "SELECT count(*) AS Num FROM FullArticle WHERE ArticlePath = @articlePath";

            using (var statement = SharedConnection.Prepare(sql))
            {
                statement.Binding("@articlePath", articlePath);

                while (statement.Step() == SQLiteResult.ROW)
                {
                    return statement.GetInteger("Num") > 0;
                }
            }
            return false;
        }

        public void ReplaceStartPageArticles(IList<IArticle> allArticlesOnStartpage)
        {
            const string removeSql = "UPDATE ArticleMeta SET IsOnStartpage = 0, StartpageSort = NULL";
            const string addSql =
                "INSERT OR REPLACE INTO ArticleMeta (ArticlePath, IsOnStartpage, StartpageSort, UpdatedOn) " +
                "VALUES (@articlePath, 1, @startpageSort, @updatedOn)";

            using (var transaction = new SQLiteTransaction(SharedConnection))
            {
                transaction.Execute(removeSql);

                int counter = 1;
                foreach (var article in allArticlesOnStartpage)
                {
                    using (var statement = transaction.Prepare(addSql))
                    {
                        statement.Binding("@articlePath", article.Path);
                        statement.Binding("@startpageSort", counter);
                        statement.Binding("@updatedOn", DateTime.UtcNow);
                        transaction.Execute(statement);
                    }

                    counter++;
                }

                transaction.Commit();
            }
        }

        public IArticle GetArticleByPath(string articlePath)
        {
            const string sql =
                "SELECT " +
                "a.Path, a.PublishedOn, a.IsBreakingNews, a.Title, a.Subtitle, a.Teaser, a.HasGallery, a.IsReportage, a.IsBriefing, i.Id, i.Path AS ImagePath, i.Source, i.Caption, i.MimeType, i.OriginalHeight, i.OriginalWidth, am.IsOnStartpage " +
                "FROM Article a " +
                "LEFT JOIN Image i ON (a.LeadImageId = i.Id) " +
                "LEFT JOIN ArticleMeta am ON (am.ArticlePath = a.Path) " +
                "WHERE a.Path = @articlePath " +
                "LIMIT 1";

            using (var statement = SharedConnection.Prepare(sql))
            {
                statement.Binding("@articlePath", articlePath);
                while (statement.Step() == SQLiteResult.ROW)
                {
                    var article = new Article(statement.GetValue<string>("Path"))
                    {
                        PublishedOn = statement.GetValue<DateTime>("PublishedOn"),
                        Title = statement.GetValue<string>("Title"),
                        SubTitle = statement.GetValue<string>("Subtitle"),
                        Teaser = statement.GetValue<string>("Teaser"),
                        HasGallery = statement.GetValue<bool>("HasGallery"),
                        IsReportage = statement.GetValue<bool>("IsReportage"),
                        IsBriefing = statement.GetValue<bool>("IsBriefing"),
                        IsBreakingNews = statement.GetValue<bool>("IsBreakingNews"),
                        IsOnStartPage = statement.GetValue<bool>("IsOnStartpage"),
                        LeadImage = new Image(statement.GetValue<string>("Id"))
                        {
                            Caption = statement.GetValue<string>("Caption"),
                            MimeType = statement.GetValue<string>("MimeType"),
                            PathTemplate = statement.GetValue<string>("ImagePath"),
                            Source = statement.GetValue<string>("Source"),
                            OriginalHeight = statement.GetValue<int>("OriginalHeight"),
                            OriginalWidth = statement.GetValue<int>("OriginalWidth")
                        }
                    };

                    return article;
                }
            }

            return null;
        }

        public IAppSettings GetSettings()
        {
            const string sql = "SELECT FirstAppStart, BreakingLiveTileEnabled, SuccessfullInitialization, ArticleFontSize, LastLiveTileTaskExecutionDate, DisableLiveTileTask, ArticleFontFamily FROM AppSettings WHERE Id = 1 LIMIT 1";
            using (var statement = SharedConnection.Prepare(sql))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    return new AppSettings
                    {
                        IsFirstAppStart = statement.GetValue<bool>("FirstAppStart"),
                        SuccessfullInitialization = statement.GetValue<bool>("SuccessfullInitialization"),
                        BreakingLiveTileEnabled = statement.GetValue<bool>("BreakingLiveTileEnabled"),
                        ArticleFontSize = statement.GetValue<int>("ArticleFontSize"),
                        LastLiveTileTaskExecutionDate = statement.GetValue<DateTime>("LastLiveTileTaskExecutionDate"),
                        DisableLiveTileTask = statement.GetValue<bool>("DisableLiveTileTask"),
                        ArticleFontFamily = statement.GetValue<string>("ArticleFontFamily")
                    };
                }
            }
            return null;
        }

        public void SetSettings(IAppSettings settings)
        {
            const string sql = "UPDATE AppSettings SET FirstAppStart = @firstAppStart, SuccessfullInitialization = @successfullInitialization, BreakingLiveTileEnabled = @breakingLiveTileEnabled, ArticleFontSize = @articleFontSize, LastLiveTileTaskExecutionDate = @lastLiveTileTaskExecutionDate, DisableLiveTileTask = @disableLiveTileTask, ArticleFontFamily = @articleFontFamily WHERE Id = 1";
            using (var transaction = new SQLiteTransaction(SharedConnection))
            {
                using (var statement = transaction.Prepare(sql))
                {
                    statement.Binding("@firstAppStart", settings.IsFirstAppStart);
                    statement.Binding("@successfullInitialization", settings.SuccessfullInitialization);
                    statement.Binding("@breakingLiveTileEnabled", settings.BreakingLiveTileEnabled);
                    statement.Binding("@articleFontSize", settings.ArticleFontSize);
                    statement.Binding("@lastLiveTileTaskExecutionDate", settings.LastLiveTileTaskExecutionDate);
                    statement.Binding("@disableLiveTileTask", settings.DisableLiveTileTask);
                    statement.Binding("@articleFontFamily", settings.ArticleFontFamily);
                    transaction.Execute(statement);
                }
                transaction.Commit();
            }
        }

        private string GetPropertiesCommaSeparated(IEnumerable<string> strings)
        {
            var results = strings.Select(s => $"'{s}'").ToList();
            return string.Join(", ", results);
        }
    }
}