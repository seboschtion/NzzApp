using NzzApp.Model.Contracts.Articles;
using NzzApp.Model.Implementation.Articles;

namespace NzzApp.Model.Implementation.RelatedContents
{
    public class Video : RelatedContent
    {
        private string _authorId;
        private string _customerId;
        private string _videoId;

        private const string BaseVideoUrl = "http://v.nzz.ch/content/{0}/{1}/{2}/simvid_1.mp4";

        public string AuthorId
        {
            get { return _authorId; }
            set
            {
                _authorId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(VideoUrl));
            }
        }

        public string CustomerId
        {
            get { return _customerId; }
            set
            {
                _customerId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(VideoUrl));
            }
        }

        public string VideoId
        {
            get { return _videoId; }
            set
            {
                _videoId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(VideoUrl));
            }
        }

        public string VideoUrl => string.Format(BaseVideoUrl, CustomerId, AuthorId, VideoId);
        public override RelatedContentType Type => RelatedContentType.Video;
    }
}
