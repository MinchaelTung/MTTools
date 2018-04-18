using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PDFTron.SilverDox.Samples.SubControls;
using PDFTron.SilverDox.Controls;
using PDFTron.SilverDox.Samples.Controls.SubControls;

namespace PDFTron.SilverDox.Samples
{
    /// <summary>
    /// Represents a control that contains the outline treeview and thumbnail listbox controls
    /// </summary>
    public partial class SideWindowControl : UserControl
    {

        /// <summary>
        /// Creates a new instance of the SideWindowControl
        /// </summary>
        public SideWindowControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a side window based on customization options from ReaderControl
        /// </summary>
        /// <param name="readerControl">a ReaderControl that holds customization options </param>
        public void CreateSideWindow(ReaderControl readerControl)
        {
            if (readerControl.EnableOutlineTreeControl)
            {
                OutlineTreeControl outlineTree = new OutlineTreeControl();
                this.OutlineTabItem.Content = outlineTree;
                this.OutlineTabItem.Visibility = Visibility.Visible;
            }

            if (readerControl.EnableThumbnailListControl)
            {
                ThumbnailsControl thumbnailViewer = new ThumbnailsControl()
                {
                    Background = new SolidColorBrush(Colors.White),
                    Foreground = new SolidColorBrush(Colors.Black),
                    ScaleFactor = 1,
                };

                this.ThumbnailTabItem.Content = thumbnailViewer;
                this.ThumbnailTabItem.Visibility = Visibility.Visible;

                if (!readerControl.EnableOutlineTreeControl)
                    this.ThumbnailTabItem.IsSelected = true;
            }

            if (readerControl.EnableFullTextSearchControl)
            {
                SearchPanelControl searchPanel = new SearchPanelControl();
                this.SearchTabItem.Visibility = Visibility.Visible;
                this.SearchTabItem.Content = searchPanel;
                //FullTextSearchControl wholeDocumentViewerControl = new FullTextSearchControl();
                //this.SearchTabItem.Visibility = Visibility.Visible;
                //this.SearchTabItem.Content = wholeDocumentViewerControl;
            }

            if (readerControl.EnableAnnotationWindowControl)
            {
                var annotationControl = new AnnotationWindowControl();

                this.AnnotationTabItem.Visibility = Visibility.Visible;
                this.AnnotationTabItem.Content = annotationControl;
            }
        }

        //Show or hide AnnotationWindowControl tab in side bar
        public void SetAnnotationWindowControlTabVisibility(Visibility visibility)
        {
            this.AnnotationTabItem.Visibility = visibility;
        }

        public void SelectSearchTabItem(String searchTerm)
        {
            if (this.SearchTabItem.IsEnabled && this.SearchTabItem.Visibility == System.Windows.Visibility.Visible)
            {
                this.SideTabControl.SelectedItem = this.SearchTabItem;
                (this.SearchTabItem.Content as FullTextSearchControl).SearchTerm = searchTerm;
            }
        }
    }
}