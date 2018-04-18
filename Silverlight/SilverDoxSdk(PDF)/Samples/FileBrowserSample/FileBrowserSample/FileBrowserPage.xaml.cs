using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Windows.Threading;


namespace FileBrowser
{
    public partial class FileBrowserPage : Page
    {
        private DispatcherTimer timer;
        private string clickedItem;
        private bool clickedOnce;

        public FileBrowserPage()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += new EventHandler(timer_Tick);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            clickedOnce = false; // expires
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            timer.Start();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            timer.Stop();
        }

        private void ThumbnailPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string item = (sender as FrameworkElement).Tag as string;
            System.Diagnostics.Debug.WriteLine("Tag: " + item);
            if (!clickedOnce)
            {
                timer.Start();
                clickedOnce = true;
                clickedItem = item;
            }
            else if (!clickedItem.Equals(item))
            {
                //a different element was clicked
                clickedOnce = false;
            }
            else
            {
                clickedOnce = false;
                System.Diagnostics.Debug.WriteLine("Double-Click");
                
                DocumentInfo docInfo = (sender as FrameworkElement).DataContext as DocumentInfo;
                this.NavigationService.Navigate(new Uri(
                    String.Format("Document#{0}", docInfo.DisplayName),
                    UriKind.Relative));
            }
        }
    }
}
