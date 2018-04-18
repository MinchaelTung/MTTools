using System;
using System.Windows;
using System.Windows.Controls;
using PDFTron.SilverDox.Controls;
using PDFTron.SilverDox.IO;

namespace StreamingSample
{
    public partial class MainPage : UserControl
    {

        public MainPage()
        {
            InitializeComponent();
            LoadDocument();
        }



        public void LoadDocument()
        {
            Uri documentUri = new Uri("http://localhost:8080/StreamFile.aspx?file=newsletter.xod");
            var myHttpPartRetriever = new HttpStreamingPartRetriever(documentUri);
            this.MyDocumentViewer.LoadAsync(myHttpPartRetriever, OnLoadAsyncCallback);
        }

        public void OnLoadAsyncCallback(Exception ex)
        {
            if (ex != null)
            {
                //An error has occurred
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                MessageBox.Show("An error has occurred: " + ex.Message);
            }

            MyDocumentViewer.SetFitMode(DocumentViewer.FitModes.Panel, DocumentViewer.FitModes.None);
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            MyDocumentViewer.CurrentPageNumber -= 1;
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            MyDocumentViewer.CurrentPageNumber += 1;
        }

    }
}
