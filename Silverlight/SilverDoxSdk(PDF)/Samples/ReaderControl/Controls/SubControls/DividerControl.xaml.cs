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
    /// Represents a control on the toolbar that visually divide groups of controls
    /// </summary>
	public partial class DividerControl : UserControl
	{
        /// <summary>
        /// Creates a new instance of DividerControl
        /// </summary>
		public DividerControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}
		
        /// <summary>
        /// Creates a new instance of DividerControl with a margin
        /// </summary>
        /// <param name="margin">Thickness of the margin</param>
		public DividerControl(Thickness margin)
		{
			// Required to initialize variables
			InitializeComponent();
			this.Margin = margin;
		}
	}
}