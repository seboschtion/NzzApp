using System;
using Windows.UI.Xaml.Documents;

namespace NzzApp.UWP.Controls
{
    public class InternalLink : Span
    {
        public event EventHandler<string> Click;

        public string NavigateTo { get; set; }

        public void AddHyperlink(Hyperlink hyperlink)
        {
            hyperlink.Click += HyperlinkOnClick;
            Inlines.Add(hyperlink);
        }

        private void HyperlinkOnClick(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            OnClick(sender);
        }

        protected virtual void OnClick(Hyperlink sender)
        {
            Click?.Invoke(sender, NavigateTo);
        }
    }
}