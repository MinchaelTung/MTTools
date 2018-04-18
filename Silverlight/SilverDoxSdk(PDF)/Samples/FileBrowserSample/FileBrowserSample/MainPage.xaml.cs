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

using System.Collections.ObjectModel;
using System.IO;
using PDFTron.SilverDox.Documents;
using PDFTron.SilverDox.IO;
namespace FileBrowser
{
    public partial class MainPage : UserControl
    {
        public ObservableCollection<DocumentInfo> ThumbnailCollection { get; set; }

        public MainPage()
        {
            InitializeComponent();
            ThumbnailCollection = new ObservableCollection<DocumentInfo>();
            WebClient webClient = new WebClient();

            string fileSourceListFile = (string)Application.Current.Resources["PDFTronFileSourceString"] + "FileSource.txt";

            webClient.OpenReadAsync(new Uri(fileSourceListFile, UriKind.RelativeOrAbsolute));
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
        }


        private void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            StreamReader streamReader = new StreamReader(e.Result);
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    DocumentInfo docInfo = new DocumentInfo(line);
                    ThumbnailCollection.Add(docInfo);
                    this.GetThumbnailImage(docInfo);
                }
            }
        }

        private void GetThumbnailImage(DocumentInfo docInfo)
        {
            Document doc = new Document();
            HttpPartRetriever myRetriever = new HttpPartRetriever(docInfo.Uri, false);

            doc.LoadAsync(myRetriever, error =>
            {
                if (error == null)
                {
                    doc.LoadThumbnailAsync(0, e =>
                    {
                        if (e.Error == null)
                        {
                            docInfo.ImageSource = e.BitmapImage;
                        }
                        else
                        {
                            MessageBox.Show(string.Format("Error loading bitmap for {0}.", docInfo.DisplayName),
                                            "Error", MessageBoxButton.OK);
                        }
                    });

                    
                }
                else
                {
                    MessageBox.Show(string.Format("Error loading document for {0}.", docInfo.DisplayName),
                                    "Error", MessageBoxButton.OK);
                }
            });
        }

        private void MainFrame_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Uri.OriginalString))
            {
                e.Cancel = true;
            }
        }

    }
}
