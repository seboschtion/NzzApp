using NzzApp.UWP.ViewModels;
using Sebastian.Toolkit.Application;

namespace NzzApp.UWP.Views
{
    public sealed partial class BreakingNewsView : View
    {
        public BreakingNewsViewModel BreakingNewsViewModel => (BreakingNewsViewModel) DataContext;

        public BreakingNewsView()
        {
            this.InitializeComponent();
        }
    }
}
