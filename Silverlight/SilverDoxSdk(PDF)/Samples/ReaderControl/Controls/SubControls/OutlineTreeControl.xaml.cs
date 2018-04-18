using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PDFTron.SilverDox.Documents;
using PDFTron.SilverDox.Controls;

namespace PDFTron.SilverDox.Samples.SubControls
{
    /// <summary>
    /// Represents a tree control that contains bookmarks of the current document
    /// </summary>
    public partial class OutlineTreeControl : UserControl
	{        
        /// <summary>
        /// Creates a new instance of the OutlineTreeControl
        /// </summary>
		public OutlineTreeControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        private void OutlineTreeControl_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var bookmark = e.NewValue as Bookmark;
            DocumentViewer docViewer = ((TreeView)sender).DataContext as DocumentViewer;
            docViewer.DisplayBookmark(bookmark);
        }
	}
}