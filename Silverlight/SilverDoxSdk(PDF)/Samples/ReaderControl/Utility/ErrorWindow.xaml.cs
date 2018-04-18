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

namespace PDFTron.SilverDox.Samples.Utility
{
    /// <summary>
    /// Represents a ChildWindow that displays an error message
    /// </summary>
    public partial class ErrorWindow : ChildWindow
    {
        /// <summary>
        /// Creates a new instance of the ErrorWindow
        /// </summary>
        public ErrorWindow(String errorString)
        {
            InitializeComponent();
            this.ErrorTextBlock.Text = errorString;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

