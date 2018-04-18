/// 
/// Copyright (c) 2008 Corey Schuman | http://coreyschuman.net | cschuman@gmail.com
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation 
/// files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, 
/// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the 
/// Software is furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
/// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
/// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
/// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VideoSliderControl
{
    [TemplatePart(Name = "LeftTrack", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "RightTrack", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "DownloadProgressTrack", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "MainTrack", Type = typeof(FrameworkElement))]
    public class VideoSlider : Slider
    {

        private Thumb HorizontalThumb;
        private FrameworkElement LeftTrack;
        private FrameworkElement RightTrack;
        private FrameworkElement DownloadProgressTrack;
        private FrameworkElement MainTrack;
        //public Brush DownloadProgressFill;

        #region Properties
        public double DownloadProgress
        {
            get { return (double)GetValue(DownloadProgressProperty); }
            set { SetValue(DownloadProgressProperty, value); }
        }

        public static readonly DependencyProperty DownloadProgressProperty = 
            DependencyProperty.Register("DownloadProgress", typeof(double), typeof(VideoSlider), new PropertyMetadata(0.5, 
            new PropertyChangedCallback(DownloadProgressChanged)));

        private static void DownloadProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (VideoSlider) d;
            if(control == null) return;
            if (control.DownloadProgressTrack == null) return;
            ((ScaleTransform) control.DownloadProgressTrack.RenderTransform).ScaleX = (double) e.NewValue;
        }
        #endregion

        #region Default constructor
        /// <summary>
        /// 
        /// </summary>
        public VideoSlider()
        {
            DefaultStyleKey = typeof(VideoSlider);
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.HorizontalThumb = GetTemplateChild("HorizontalThumb") as Thumb;
            this.LeftTrack = GetTemplateChild("LeftTrack") as FrameworkElement;
            this.RightTrack = GetTemplateChild("RightTrack") as FrameworkElement;
            this.DownloadProgressTrack = GetTemplateChild("DownloadProgressTrack") as FrameworkElement;
            this.MainTrack = GetTemplateChild("MainTrack") as FrameworkElement;

            if (this.LeftTrack != null) this.LeftTrack.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e) { OnMoveThumbToMouse(e); };//new MouseButtonEventHandler(OnMoveThumbToMouse);
            if (this.RightTrack != null) this.RightTrack.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e) { OnMoveThumbToMouse(e); };// new MouseButtonEventHandler(OnMoveThumbToMouse);

            if (HorizontalThumb != null)
            {
                HorizontalThumb.DragStarted += new DragStartedEventHandler(thumb_DragStarted);
                HorizontalThumb.DragCompleted += new DragCompletedEventHandler(thumb_DragCompleted);
            }

            OnDownloadProgressFillChanged(DownloadProgressFill);
        }

        #region Event handlers
        private void OnMoveThumbToMouse(MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this);
            Value = (p.X - (this.HorizontalThumb.ActualWidth / 2)) / (ActualWidth - this.HorizontalThumb.ActualWidth) * Maximum;
            OnTrackClicked();
        }

        void thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            OnThumbDragCompleted();
        }

        void thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            OnThumbDragStarted();
        }
        #endregion

        #region Events
        public event EventHandler<EventArgs> ThumbDragStarted;
        public virtual void OnThumbDragStarted()
        {
            if (ThumbDragStarted != null)
                ThumbDragStarted(this, new EventArgs());
        }

        public event EventHandler<EventArgs> ThumbDragCompleted;
        protected virtual void OnThumbDragCompleted()
        {
            if (ThumbDragCompleted != null)
                ThumbDragCompleted(this, new EventArgs());
        }

        public event EventHandler<EventArgs> TrackClicked;
        protected virtual void OnTrackClicked()
        {
            if (TrackClicked != null)
                TrackClicked(this, new EventArgs());
        }
        #endregion

        #region DownloadProgressFill

        /// <summary>
        /// Gets or sets the DownloadProgressFill possible Value of the Brush object.
        /// </summary>
        public Brush DownloadProgressFill
        {
            get { return (Brush)GetValue(DownloadProgressFillProperty); }
            set { SetValue(DownloadProgressFillProperty, value); }
        }

        /// <summary>
        /// Identifies the DownloadProgressFill dependency property.
        /// </summary>
        public static readonly DependencyProperty DownloadProgressFillProperty =
                    DependencyProperty.Register(
                          "DownloadProgressFill",
                          typeof(Brush),
                          typeof(VideoSlider),
                          new PropertyMetadata(OnDownloadProgressFillPropertyChanged));

        /// <summary>
        /// DownloadProgressFillProperty property changed handler.
        /// </summary>
        /// <param name="d">VideoSlider that changed its DownloadProgressFill.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param>
        private static void OnDownloadProgressFillPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VideoSlider v = d as VideoSlider;
            if (v != null)
            {
                v.OnDownloadProgressFillChanged((Brush)e.NewValue);
            }
        }

        protected virtual void OnDownloadProgressFillChanged(Brush brush)
        {
            if (this.DownloadProgressTrack != null)
            {
                (this.DownloadProgressTrack as Rectangle).Fill = brush;
            }
        }
        #endregion DownloadProgressFill

        #region PlaybackFill

        /// <summary>
        /// Gets or sets the PlaybackFill possible Value of the Brush object.
        /// </summary>
        public Brush PlaybackFill
        {
            get { return (Brush)GetValue(PlaybackFillProperty); }
            set { SetValue(PlaybackFillProperty, value); }
        }

        /// <summary>
        /// Identifies the PlaybackFill dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaybackFillProperty =
                    DependencyProperty.Register(
                          "PlaybackFill",
                          typeof(Brush),
                          typeof(VideoSlider),
                          new PropertyMetadata(OnPlaybackFillPropertyChanged));

        /// <summary>
        /// PlaybackFillProperty property changed handler.
        /// </summary>
        /// <param name="d">VideoSlider that changed its PlaybackFill.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param>
        private static void OnPlaybackFillPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VideoSlider v = d as VideoSlider;
            if (v != null)
            {
                v.OnPlaybackFillChanged((Brush)e.NewValue);
            }
        }

        protected virtual void OnPlaybackFillChanged(Brush brush)
        {
            if (this.LeftTrack != null)
            {
                (this.LeftTrack as Rectangle).Fill = brush;
            }
        }
        #endregion PlaybackFill

        #region TrackFill

        /// <summary>
        /// Gets or sets the TrackFill possible Value of the Brush object.
        /// </summary>
        public Brush TrackFill
        {
            get { return (Brush)GetValue(TrackFillProperty); }
            set { SetValue(TrackFillProperty, value); }
        }

        /// <summary>
        /// Identifies the TrackFill dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackFillProperty =
                    DependencyProperty.Register(
                          "TrackFill",
                          typeof(Brush),
                          typeof(VideoSlider),
                          new PropertyMetadata(OnTrackFillPropertyChanged));

        /// <summary>
        /// TrackFillProperty property changed handler.
        /// </summary>
        /// <param name="d">VideoSlider that changed its TrackFill.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param>
        private static void OnTrackFillPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VideoSlider v = d as VideoSlider;
            if (v != null)
            {
                v.OnTrackFillChanged((Brush)e.NewValue);
            }
        }

        protected virtual void OnTrackFillChanged(Brush brush)
        {
            if (this.MainTrack != null)
            {
                (this.MainTrack as Rectangle).Fill = brush;
            }
        }
        #endregion TrackFill
    }
}
