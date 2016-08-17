using System;
using Sebastian.Toolkit.SQLite;

namespace NzzApp.Data.Versions
{
    internal class Version0002 : DatabaseVersion
    {
        public override Version DbVersion => new Version(0, 0, 0, 2);

        protected override void Upgrade(SQLiteTransaction transaction)
        {
            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""Department"" (
                                    ""Name"" TEXT NOT NULL,
                                    ""Path"" TEXT PRIMARY KEY,
                                    ""DepartmentSerialisationType"" TEXT NOT NULL,
                                    ""ParentDepartment"" TEXT NULL,
                                    ""ShowOn"" TEXT NULL,
                                    ""UpdatedOn"" INTEGER NOT NULL)");

            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""Article"" (
                                    ""Path"" TEXT PRIMARY KEY,
                                    ""PublishedOn"" INTEGER NULL,
                                    ""IsBreakingNews"" INTEGER NOT NULL DEFAULT 0,
                                    ""Title"" TEXT NULL,
                                    ""Subtitle"" TEXT NULL,
                                    ""Teaser"" TEXT NULL,
                                    ""LeadImageId"" TEXT NULL,
                                    ""HasGallery"" INTEGER NOT NULL DEFAULT 0,
                                    ""IsReportage"" INTEGER NOT NULL DEFAULT 0,
                                    ""IsBriefing"" INTEGER NOT NULL DEFAULT 0,
                                    ""UpdatedOn"" INTEGER NOT NULL)");

            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""FullArticle"" (
                                    ""ArticlePath"" TEXT PRIMARY KEY,
                                    ""LeadText"" TEXT NULL,
                                    ""WebUrl"" TEXT NULL,
                                    ""ShortWebUrl"" TEXT NULL,
                                    ""Agency"" TEXT NULL,
                                    ""UpdatedOn"" INTEGER NOT NULL)");

            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""DepartmentHasArticles"" (
                                    ""ArticlePath"" TEXT,
                                    ""DepartmentPath"" TEXT,
                                    ""UpdatedOn"" INTEGER NOT NULL,
                                    PRIMARY KEY (ArticlePath, DepartmentPath))");

            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""Paragraph"" (
                                    ""Id"" TEXT PRIMARY KEY,
                                    ""ArticlePath"" TEXT NOT NULL,
                                    ""Text"" TEXT NULL,
                                    ""Type"" TEXT NOT NULL DEFAULT ""p"",
                                    ""Sort"" INTEGER NOT NULL,
                                    ""UpdatedOn"" INTEGER NOT NULL)");

            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""ArticleHasRelatedArticles"" (
                                    ""SourceArticlePath"" TEXT NOT NULL,
                                    ""TargetArticlePath"" TEXT NOT NULL,
                                    ""UpdatedOn"" INTEGER NOT NULL,
                                    PRIMARY KEY (SourceArticlePath, TargetArticlePath))");

            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""Author"" (
                                    ""ArticlePath"" TEXT NOT NULL,
                                    ""Name"" TEXT NOT NULL,
                                    ""Abbreviation"" TEXT NULL,
                                    ""UpdatedOn"" INTEGER NOT NULL,
                                    PRIMARY KEY(ArticlePath, Name))");

            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""ArticleMeta"" (
                                    ""ArticlePath"" TEXT PRIMARY KEY,
                                    ""IsOnStartpage"" INTEGER NULL DEFAULT 0,
                                    ""StartpageSort"" INTEGER NULL,
                                    ""UpdatedOn"" INTEGER NOT NULL)");

            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""Image"" (
                                    ""Id"" TEXT PRIMARY KEY,
                                    ""Source"" TEXT NULL,
                                    ""Caption"" TEXT NULL,
                                    ""MimeType"" TEXT NULL,
                                    ""Path"" TEXT NULL,
                                    ""OriginalWidth"" INTEGER NOT NULL DEFAULT 0,
                                    ""OriginalHeight"" INTEGER NOT NULL DEFAULT 0,
                                    ""UpdatedOn"" INTEGER NOT NULL)");

            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""Item"" (
                                    ""ParagraphId"" TEXT NOT NULL,
                                    ""Content"" TEXT NULL,
                                    ""Sort"" INTEGER NOT NULL,
                                    ""UpdatedOn"" INTEGER NOT NULL,
                                    PRIMARY KEY (ParagraphId, Sort))");

            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""GalleryHasImages"" (
                                    ""GalleryId"" TEXT NOT NULL,
                                    ""ImageId"" TEXT NOT NULL,
                                    ""Sort"" INTEGER NOT NULL,
                                    ""UpdatedOn"" INTEGER NOT NULL,
                                    PRIMARY KEY (GalleryId, ImageId))");

            transaction.Execute(@"CREATE TABLE IF NOT EXISTS ""RelatedContent"" (
                                    ""ParagraphId"" TEXT NOT NULL,
                                    ""ArticlePath"" TEXT NOT NULL,
                                    ""Sort"" INTEGER NOT NULL,
                                    ""RelatedContentType"" TEXT NOT NULL,
                                    ""Type"" TEXT NULL,
                                    ""VideoAuthorId"" TEXT NULL,
                                    ""VideoCustomerId"" TEXT NULL,
                                    ""VideoId"" TEXT NULL,
                                    ""ImageId"" TEXT NULL,
                                    ""ImageSource"" TEXT NULL,
                                    ""ImageCaption"" TEXT NULL,
                                    ""ImageMimeType"" TEXT NULL,
                                    ""ImagePath"" TEXT NULL,
                                    ""ImageOriginalWidth"" INTEGER NOT NULL DEFAULT 0,
                                    ""ImageOriginalHeight"" INTEGER NOT NULL DEFAULT 0,
                                    ""HtmlBody"" TEXT NULL,
                                    ""InfoTitle"" TEXT NULL,
                                    ""InfoBody"" TEXT NULL,
                                    ""GalleryId"" TEXT NULL,
                                    ""GalleryTitle"" TEXT NULL,
                                    ""UpdatedOn"" INTEGER NOT NULL,
                                    PRIMARY KEY (ParagraphId, ArticlePath, Sort, RelatedContentType))");
        }
    }
}