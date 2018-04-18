using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PDFTron.SilverDox.Samples.SubControls
{
    /// <summary>
    /// Represents a button control that triggers full-screen mode on click
    /// </summary>
	public partial class FullScreenButtonControl : UserControl
	{
        /// <summary>
        /// Creates a new instance of FullScreenButtonControl
        /// </summary>
		public FullScreenButtonControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}

		private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen; 
        }
	}
}