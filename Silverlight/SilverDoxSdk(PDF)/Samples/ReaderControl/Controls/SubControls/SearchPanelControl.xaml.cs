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
using PDFTron.SilverDox.Controls;
using PDFTron.SilverDox.Samples.Resources;

namespace PDFTron.SilverDox.Samples.Controls.SubControls
{
    public partial class SearchPanelControl : UserControl
    {
        /// <summary>
        /// Represents a control that searches text in the whole document, displaying the results in a ListBox.
        /// </summary>
        public SearchPanelControl()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DocumentViewer docViewer = this.DataContext as DocumentViewer;
            if (docViewer != null &&
                docViewer.PageCount > 0)
            {
                SearchControl.SearchWithOptions();
            }

        }

        private void SearchBoxKeyDown(object sender, KeyEventArgs e)
        {
            DocumentViewer docViewer = this.DataContext as DocumentViewer;

            if (docViewer != null &&
                docViewer.PageCount > 0 &&
                e.Key == Key.Enter)
            {
                SearchControl.SearchWithOptions();
            }

        }

        private void SearchControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.SearchControl.PageStringFormat = StringResource.PageStringFormat;
        }

    }
}
