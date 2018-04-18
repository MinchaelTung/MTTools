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
using PDFTron.SilverDox.Samples;


namespace PDFTron.SilverDox.Samples.SubControls
{
    /// <summary>
    /// Represents a button control that prompts users to select a local document to be loaded
    /// </summary>
    public partial class OpenLocalFileButtonControl : UserControl
    {
        private ReaderControl _documentViewer;

        /// <summary>
        /// Creates a new instance of OpenLocalFileButtonControl
        /// </summary>
        /// <param name="documentViewer"></param>
        public OpenLocalFileButtonControl(ReaderControl documentViewer)
        {
            InitializeComponent();
            this._documentViewer = documentViewer;
        }

        private void OpenLocalFileButton_Click(object sender, RoutedEventArgs e)
        {
            _documentViewer.LoadLocalDocument();
        }

    }
}
