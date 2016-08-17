using System;
using Windows.UI.Xaml.Controls;

namespace NzzApp.UWP.ContentDialogs
{
    public sealed partial class OpenArticleByUrl : ContentDialog
    {
        public event EventHandler<string> RequestOpenArticle; 

        public OpenArticleByUrl()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            OnRequestOpenArticle(this.TextBox.Text);
        }

        private void OnRequestOpenArticle(string e)
        {
            RequestOpenArticle?.Invoke(this, e);
        }
    }
}