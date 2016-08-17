using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Microsoft.Xaml.Interactivity;
using NzzApp.Model.Contracts.Articles;
using NzzApp.UWP.Controls;
using NzzApp.UWP.ViewModels;

namespace NzzApp.UWP.Views
{
    public sealed partial class ArticleView : WaitingPage
    {
        public ArticleViewModel ArticleViewModel => (ArticleViewModel)this.DataContext;
        private bool _success;

        public ArticleView()
        {
            this.InitializeComponent();
        }

        public override void OnInitialized()
        {
            ArticleViewModel.ArticleLoaded += ArticleViewModelOnArticleLoaded;
            ArticleViewModel.ArticleLoadError += ArticleViewModelOnArticleLoadError;
            ArticleViewModel.ArticleRequested += ArticleViewModelOnArticleRequested;
        }

        private void ArticleViewModelOnArticleLoaded(object sender, EventArgs eventArgs)
        {
            FindName(nameof(ScrollViewer));
            if (ArticleViewModel.FullArticle.Article.LeadImage.HasImage)
            {
                FindName(nameof(MainImage));
                var behavior = new ParallaxBehavior();
                behavior.SetValue(ParallaxBehavior.ParallaxContentProperty, this.MainImage);
                behavior.SetValue(ParallaxBehavior.ParallaxMultiplierProperty, -0.4);
                Interaction.GetBehaviors(this.ScrollViewer).Add(behavior);
            }
            else
            {
                this.ImageRowDefinition.Height = GridLength.Auto;
            }
            if (ArticleViewModel.FullArticle.HasRelatedArticles)
            {
                FindName(nameof(RelatedArticlesListView));
                FindName(nameof(RelatedArticlesTextBlock));
                FindName(nameof(RelatedArticlesLine));
            }
            if (ArticleViewModel.FullArticle.HasRelatedContent)
            {
                FindName(nameof(RelatedContentGrid));
            }

            _success = true;
            GotoState(WaitStates.EndWait);
        }

        private void ArticleViewModelOnArticleLoadError(object sender, EventArgs eventArgs)
        {
            UpdateStatusText();
            _success = false;
            GotoState(WaitStates.EndWait);
        }

        private void ArticleViewModelOnArticleRequested(object sender, EventArgs eventArgs)
        {
            UpdateStatusText();
            GotoState(WaitStates.Wait);
        }

        private void ShowRawButton_OnClick(object sender, RoutedEventArgs e)
        {
            FindName(nameof(DebugContentPresenter));
            this.ListView.Visibility = Visibility.Collapsed;
            var rich = new RichTextBlock();
            foreach (var item in this.ListView.Items)
            {
                var articleParagraph = item as IParagraph;
                if (articleParagraph != null)
                {
                    var paragraph = new Paragraph();
                    paragraph.Inlines.Add(new Run() {Text = articleParagraph.Text});
                    rich.Blocks.Add(paragraph);
                }
            }
            this.DebugContentPresenter.Content = rich;
        }

        protected override ContentPresenter GetContentPresenter()
        {
            return this.WaitPlaceholderPresenter;
        }

        protected override string GetDisplayStatus()
        {
            return ArticleViewModel.LoadingStatus;
        }

        protected override void OnWaitEnded()
        {
            if (!_success)
            {
                FindName(nameof(NoDataStackPanel));
            }
        }
    }
}
