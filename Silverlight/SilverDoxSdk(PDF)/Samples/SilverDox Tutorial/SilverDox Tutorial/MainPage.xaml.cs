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
using PDFTron.SilverDox.IO;
using System.IO;
using PDFTron.SilverDox.Controls;

namespace SilverDox_Tutorial
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            this.LoadLocalDocument();
            //LoadDocument();
        }

        public void LoadDocument()
        {
            Uri documentUri = new Uri("http://www.pdftron.com/silverdox/samples/ClientBin/PDFTron_PDF2XPS_User_Manual.xod");
            HttpPartRetriever myHttpPartRetriever = new HttpPartRetriever(documentUri);
            this.MyDocumentViewer.LoadAsync(myHttpPartRetriever, OnLoadAsyncCallback);
        }

        private LocalPartRetriever MyRetriever;
        public void LoadLocalDocument()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            dlg.Filter = "XOD Files (*.xod)|*.xod";

            // open dialog
            bool ok = (bool)dlg.ShowDialog();

            if (ok)
            {
                if (MyRetriever as LocalPartRetriever != null)
                    ((LocalPartRetriever)MyRetriever).Dispose();
                FileStream fileStream = dlg.File.OpenRead();
                MyRetriever = new LocalPartRetriever(fileStream);
                MyDocumentViewer.LoadAsync(MyRetriever, OnLoadAsyncCallback);
            }
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
