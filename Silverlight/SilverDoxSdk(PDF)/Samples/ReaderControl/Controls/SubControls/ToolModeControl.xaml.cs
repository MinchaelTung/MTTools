using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace PDFTron.SilverDox.Samples.SubControls
{
    /// <summary>
    /// Represents a control that allows the selection of the hand and IBeam tool
    /// </summary>
    public partial class ToolModeControl : UserControl
    {
        /// <summary>
        /// Creates a new instance of ToolModeControl
        /// </summary>
        public ToolModeControl()
        {
            // Required to initialize variables
            InitializeComponent();

        }
    }

    /// <summary>
    /// Represents a ListBox with disabled left and right keys
    /// </summary>
    public class MyListBox : ListBox
    {
        /// <summary>
        /// Creates a new instance of MyListBox
        /// </summary>
        public MyListBox()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                case Key.PageDown:
                case Key.PageUp:
                case Key.Right:
                case Key.Home:
                case Key.End:
                    e.Handled = true;
                    break;
            }

            if (!e.Handled)
                base.OnKeyDown(e);
        }
    }


}