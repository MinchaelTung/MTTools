using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace VideoSliderControl.Test
{
    public partial class MainPage : UserControl
    {
        private DispatcherTimer timer;
        private bool isSliderDragging;

        public MainPage()
        {
            InitializeComponent();

            // Set timer to update the slider
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += (s, e) =>
            {
                if (isSliderDragging) return;
                if (this.MyVideo.NaturalDuration.TimeSpan.TotalMilliseconds <= 0) return;

                this.MySlider.Value = this.MyVideo.Position.TotalMilliseconds / this.MyVideo.NaturalDuration.TimeSpan.TotalMilliseconds;
            };

            // Set the timer
            this.MyVideo.CurrentStateChanged += (s, e) =>
                                                    {
                                                        if (this.MyVideo.CurrentState == MediaElementState.Playing)
                                                            timer.Start();
                                                        else
                                                            timer.Stop();
                                                    };
            
            // Update the download progress
            this.MyVideo.DownloadProgressChanged += (s, e) =>
            {
                this.MySlider.DownloadProgress = this.MyVideo.DownloadProgress;
            };

            // Register the Slider events
            this.MySlider.TrackClicked += new EventHandler<EventArgs>(MySlider_TrackClicked);
            this.MySlider.ThumbDragStarted += new EventHandler<EventArgs>(MySlider_ThumbDragStarted);
            this.MySlider.ThumbDragCompleted += new EventHandler<EventArgs>(MySlider_ThumbDragCompleted);

        }

        void MySlider_ThumbDragCompleted(object sender, EventArgs e)
        {
            isSliderDragging = false;
            this.MyVideo.Position = TimeSpan.FromMilliseconds(this.MyVideo.NaturalDuration.TimeSpan.TotalMilliseconds * this.MySlider.Value);
        }

        void MySlider_ThumbDragStarted(object sender, EventArgs e)
        {
            isSliderDragging = true;
        }

        void MySlider_TrackClicked(object sender, EventArgs e)
        {
            this.MyVideo.Position = TimeSpan.FromMilliseconds(this.MyVideo.NaturalDuration.TimeSpan.TotalMilliseconds * this.MySlider.Value);
        }
    }
}
