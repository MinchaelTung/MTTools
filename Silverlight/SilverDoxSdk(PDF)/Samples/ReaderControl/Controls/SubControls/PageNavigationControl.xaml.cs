using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PDFTron.SilverDox.Controls;

namespace PDFTron.SilverDox.Samples.SubControls
{
    /// <summary>
    /// Represents a set of button controls used to navigate though pages of the current document
    /// </summary>
	public partial class PageNavigationControl : UserControl
	{
        /// <summary>
        /// Creates a new instance of PageNavigationControl
        /// </summary>
		public PageNavigationControl()
		{
			// Required to initialize variables
			InitializeComponent();

		}
        /// <summary>
        /// Creates a new instance of PageNavigationControl
        /// </summary>
        /// <param name="showFirst"></param>
        /// <param name="showPrev"></param>
        /// <param name="showNext"></param>
        /// <param name="showLast"></param>
        public PageNavigationControl(bool showFirst, bool showPrev, bool showNext, bool showLast):this()
        {
            if (!showFirst)
                this.FirstPageButton.Visibility = System.Windows.Visibility.Collapsed;
            if (!showPrev)
                this.PreviousPageButton.Visibility = System.Windows.Visibility.Collapsed;
            if (!showNext)
                this.NextPageButton.Visibility = System.Windows.Visibility.Collapsed;
            if (!showLast)
                this.LastPageButton.Visibility = System.Windows.Visibility.Collapsed;
            // Required to initialize variables
            //InitializeComponent();
        }
		
		
        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            DocumentViewer fixedDocumentViewer = ((Button)sender).DataContext as DocumentViewer;
            fixedDocumentViewer.CurrentPageNumber++;
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            DocumentViewer fixedDocumentViewer = ((Button)sender).DataContext as DocumentViewer;
            fixedDocumentViewer.CurrentPageNumber--;
        }

        private void FirstPageButton_Click(object sender, RoutedEventArgs e)
        {
            DocumentViewer fixedDocumentViewer = ((Button)sender).DataContext as DocumentViewer;

            fixedDocumentViewer.DisplayFirstPage();

        }

        private void LastPageButton_Click(object sender, RoutedEventArgs e)
        {
            DocumentViewer fixedDocumentViewer = ((Button)sender).DataContext as DocumentViewer;
          
            fixedDocumentViewer.DisplayLastPage();

        }
	}
}