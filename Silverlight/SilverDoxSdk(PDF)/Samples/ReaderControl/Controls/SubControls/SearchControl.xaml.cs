using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using PDFTron.SilverDox.Controls;
using Text = PDFTron.SilverDox.Documents.Text;
using System.Resources;
using PDFTron.SilverDox.Samples.Resources;

namespace PDFTron.SilverDox.Samples.SubControls
{
    /// <summary>
    /// Represents a control that searches for text within the DocumentViewer
    /// </summary>
    public partial class SearchControl : UserControl
    {
        /// <summary>
        /// Creates a new instance of SearchControl
        /// </summary>
        public SearchControl()
        {
            // Required to initialize variables
            InitializeComponent();
            Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_FullScreenChanged);




        }

        private void Content_FullScreenChanged(object sender, EventArgs e)
        {
            this.SearchTextBox.IsReadOnly = Application.Current.Host.Content.IsFullScreen;
        }

        private void SearchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DocumentViewer fixedDocumentViewer = ((Button)sender).DataContext as DocumentViewer;
            if (fixedDocumentViewer.Document != null)
                fixedDocumentViewer.SearchTextAsync(SearchTextBox.Text, Text.TextSearch.SearchModes.None, SearchTestCallback);
        }


        private void SearchTestCallback(Text.TextSearch.OnTextSearchAsyncCallbackArgs args)
        {
            if (args.ResultCode == Text.TextSearch.SearchResultCode.Done)
            {
                MessageBox.Show(StringResource.DocumentSearchedMessage,
                    StringResource.DocumentSearchedCaption, MessageBoxButton.OK);
            }
            else if (args.ResultCode == Text.TextSearch.SearchResultCode.NotFound)
            {
                MessageBox.Show(StringResource.TextNotFoundMessage,
                    StringResource.TextNotFoundCaption, MessageBoxButton.OK);
            }
            else if (args.ResultCode == Text.TextSearch.SearchResultCode.Error)
            {
                System.Diagnostics.Debug.WriteLine(args.Error.Message);
            }

        }
        private void SearchTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DocumentViewer fixedDocumentViewer = ((TextBox)sender).DataContext as DocumentViewer;
                if (fixedDocumentViewer.Document != null)
                    fixedDocumentViewer.SearchTextAsync(SearchTextBox.Text, Text.TextSearch.SearchModes.None, SearchTestCallback);

            }
        }

        private void SearchTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Application.Current.Host.Content.IsFullScreen)
            {
                TextSearchToolTipBlock.Text = StringResource.SilverlightKeyboardRestriction;
            }
            else
            {
                TextSearchToolTipBlock.Text = StringResource.SearchTextToolTip;
            }
        }
    }
}