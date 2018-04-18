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
    /// Represents a slider control that controls the zoom level of the current document
    /// </summary>
	public partial class ZoomSliderControl : UserControl
	{
        /// <summary>
        /// Creates a new instance of the ZoomSliderControl
        /// </summary>
		public ZoomSliderControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}
	}
}