using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using PDFTron.SilverDox.Samples;

namespace PDFTron.SilverDox.Samples.SubControls
{
    /// <summary>
    /// Represents a toggle button that shows or hides the side window containing bookmarks and thumbnails
    /// </summary>
	public partial class OutlineToggleButtonControl : UserControl
	{                
        private ImageSource expandedImage;
        private ImageSource collapsedImage;

        
        /// <summary>
        /// Creates a new instance of the OutlineToggleButtonControl
        /// </summary>
        /// <param name="reader">The ReaderControl that is the target of the outline toggling</param>
        public OutlineToggleButtonControl(ReaderControl reader)
        {
            InitializeComponent();
            
            this.DataContext = reader;
            expandedImage = new BitmapImage(new Uri("/ReaderControl;component/Resources/section_expanded.png", UriKind.Relative));
            collapsedImage = new BitmapImage(new Uri("/ReaderControl;component/Resources/section_collapsed.png", UriKind.Relative));

            if (reader.ShowSideWindow)
            {
                this.NavigationToggleImage.Source = expandedImage;
                this.ToolTipTextBlock.Text = "Hide outline navigation";
            }
            else
            {
                this.NavigationToggleImage.Source = collapsedImage;
                this.ToolTipTextBlock.Text = "Show outline navigation";
            }
        }

        private void OutlineNavigationButton_Checked(object sender, RoutedEventArgs e)
        {
            this.NavigationToggleImage.Source = expandedImage;
            this.ToolTipTextBlock.Text = "Hide outline navigation";
        }

        private void OutlineNavigationButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.NavigationToggleImage.Source = collapsedImage;
            this.ToolTipTextBlock.Text = "Show outline navigation";
        }
	}
}