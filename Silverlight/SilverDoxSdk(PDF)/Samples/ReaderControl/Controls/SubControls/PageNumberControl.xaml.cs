using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;
using PDFTron.SilverDox.Samples.Resources;

namespace PDFTron.SilverDox.Samples.SubControls
{
    /// <summary>
    /// Represents a textbox control that indicates and jumps to the current page of the document
    /// </summary>
	public partial class PageNumberControl : UserControl
	{
        /// <summary>
        /// Creates a new instance of PageNumberControl
        /// </summary>
		public PageNumberControl()
		{
			// Required to initialize variables
			InitializeComponent();
            Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_FullScreenChanged);
		}

        private void Content_FullScreenChanged(object sender, EventArgs e)
        {
            this.CurrentPageNumberTextBlock.IsReadOnly = Application.Current.Host.Content.IsFullScreen;
        }

        private void CurrentPageNumberTextBlock_KeyDown(object sender, KeyEventArgs e)
        {
            if( e.Key == Key.Enter )
                ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void CurrentPageNumberTextBlock_LostFocus(Object sender, EventArgs e)
        {
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void CurrentPageNumberTextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Application.Current.Host.Content.IsFullScreen)
            {
                PageNumToolTextBlock.Text = StringResource.SilverlightKeyboardRestriction;
            }
            else
            {
                PageNumToolTextBlock.Text = StringResource.CurrentPageToolTip;
            }
        }


	}
}