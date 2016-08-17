using System.Windows.Input;
using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Contracts.BreakingNews;
using Sebastian.Toolkit.Application;
using Sebastian.Toolkit.MVVM.Navigation;
using Sebastian.Toolkit.Util;

namespace NzzApp.UWP.ViewModels
{
    public class BreakingNewsViewModel : ViewModel
    {
        private readonly INavigator _navigator;

        private IBreakingNews _breakingNews;

        public BreakingNewsViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }

        public IBreakingNews BreakingNews
        {
            get { return _breakingNews; }
            set
            {
                _breakingNews = value;
                OnPropertyChanged();
            }
        }

        public override void OnActivated(object parameter)
        {
            base.OnActivated(parameter);
            BreakingNews = (IBreakingNews) parameter;
        }

        public ICommand OpenArticle => new RelayCommand<IArticle>(article =>
        {
            _navigator.Navigate<ArticleViewModel>(article.Path);
        });
    }
}
