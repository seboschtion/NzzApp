using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using NzzApp.Model.Contracts.BreakingNews;

namespace NzzApp.UWP.Controls
{
    public sealed partial class BreakingNewsControl : UserControl
    {
        public DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof (IBreakingNews),
            typeof (BreakingNewsControl), new PropertyMetadata(null, PropertyChangedCallback));

        public DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof (ICommand),
            typeof (BreakingNewsControl), null);

        private DispatcherTimer _timer;

        public BreakingNewsControl()
        {
            this.InitializeComponent();
            FlipView.DataContext = this;
        }

        public IBreakingNews Source
        {
            get { return (IBreakingNews) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var breakingNews = e.NewValue as IBreakingNews;
            if (breakingNews?.Articles.Count > 0)
            {
                var control = (BreakingNewsControl) d;
                control.Activate();
            }
        }

        private void Activate()
        {
            if (_timer == null)
            {
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromSeconds(4);
                _timer.Tick += TimerOnTick;
                _timer.Start();
            }
        }

        private void TimerOnTick(object sender, object o)
        {
            if (this.FlipView?.Items != null)
            {
                this.FlipView.SelectedIndex = this.FlipView.SelectedIndex + 1 >= this.FlipView.Items.Count
                    ? 0
                    : this.FlipView.SelectedIndex + 1;
            }
        }

        private void FlipView_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
            Command?.Execute(Source);
        }
    }
}
