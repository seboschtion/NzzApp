using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Sebastian.Toolkit.Application;
using Sebastian.Toolkit.Util.Extensions;

namespace NzzApp.UWP.Controls
{
    public enum WaitStates
    {
        Wait,
        EndWait
    }

    public abstract class WaitingPage : View, INotifyPropertyChanged
    {
        public static DependencyProperty DefaultPlaceholderProperty = DependencyProperty.Register("DefaultPlaceholder",
            typeof (DataTemplate), typeof (WaitingPage), new PropertyMetadata(null, DefaultPlaceholderPropertyChangedCallback));

        protected abstract ContentPresenter GetContentPresenter();
        protected abstract string GetDisplayStatus();
        protected abstract void OnWaitEnded();

        private TextBlock _statusTextBlock;
        private ProgressRing _progressRing;
        private DependencyObject _defaultPlaceholderContent;
        private bool _waitingViewVisible = true;
        private bool _waiting;

        public DataTemplate DefaultPlaceholder
        {
            get { return (DataTemplate) GetValue(DefaultPlaceholderProperty); }
            set { SetValue(DefaultPlaceholderProperty, value); }
        }

        public bool Waiting
        {
            get { return _waiting; }
            private set
            {
                _waiting = value;
                OnPropertyChanged();
            }
        }

        private void Wait()
        {
            ShowContentPresenter();
            Waiting = true;
            SetProgressRingIsActive(true);
            UpdateStatusText();
            GetContentPresenter().Content = _defaultPlaceholderContent;
        }

        private void EndWait()
        {
            Waiting = false;
            SetProgressRingIsActive(false);
            HideContentPresenter();
            OnWaitEnded();
        }

        protected void GotoState(WaitStates state)
        {
            switch (state)
            {
                case WaitStates.Wait:
                    Wait();
                    break;
                case WaitStates.EndWait:
                    EndWait();
                    break;
            }
        }

        protected void UpdateStatusText()
        {
            if (_statusTextBlock != null)
            {
                _statusTextBlock.Text = GetDisplayStatus();
            }
        }

        private static void DefaultPlaceholderPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var page = sender as WaitingPage;
            var dataTemplate = e.NewValue as DataTemplate;
            if (page != null && dataTemplate != null)
            {
                page._defaultPlaceholderContent = dataTemplate.LoadContent();
                page._statusTextBlock = page._defaultPlaceholderContent.GetChildOfType<TextBlock>();
                page._progressRing = page._defaultPlaceholderContent.GetChildOfType<ProgressRing>();
            }
        }

        private void SetProgressRingIsActive(bool isActive)
        {
            if (_progressRing != null)
            {
                _progressRing.IsActive = isActive;
            }
        }

        private void ShowContentPresenter()
        {
            if (!_waitingViewVisible)
            {
                var visual = ElementCompositionPreview.GetElementVisual(GetContentPresenter());
                var compositor = visual.Compositor;
                var animation = compositor.CreateScalarKeyFrameAnimation();
                animation.InsertKeyFrame(0.00f, 0.00f);
                animation.InsertKeyFrame(1.00f, 1.00f);
                animation.Duration = TimeSpan.FromMilliseconds(500);
                visual.StartAnimation("Opacity", animation);
                _waitingViewVisible = true;
            }
        }

        private void HideContentPresenter()
        {
            if (_waitingViewVisible)
            {
                var visual = ElementCompositionPreview.GetElementVisual(GetContentPresenter());
                var compositor = visual.Compositor;
                var batch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
                batch.Completed += (s, e) =>
                {
                    GetContentPresenter().Content = null;
                    _waitingViewVisible = false;
                };
                var animation = compositor.CreateScalarKeyFrameAnimation();
                animation.InsertKeyFrame(1.00f, 0.00f);
                animation.InsertKeyFrame(0.00f, 1.00f);
                animation.Duration = TimeSpan.FromMilliseconds(1000);
                visual.StartAnimation("Opacity", animation);
                batch.End();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}