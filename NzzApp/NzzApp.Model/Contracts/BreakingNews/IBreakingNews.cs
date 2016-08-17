using System.Collections.ObjectModel;
using NzzApp.Model.Contracts.Articles;

namespace NzzApp.Model.Contracts.BreakingNews
{
    public interface IBreakingNews
    {
        string Path { get; set; }
        string SpeakingName { get; set; }
        ObservableCollection<IArticle> Articles { get; } 
    }
}
