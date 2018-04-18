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

using SLMitsuControls;
using PDFTron.SilverDox.Documents;
using PDFTron.SilverDox.IO;
using System.Windows.Media.Effects;
namespace FlipBook
{
    public partial class MainPage : UserControl, IDataProvider
    {
        Document document;
        List<object> canvasList;
        bool isActionReady;
        private const string DOCUMENTURI = "http://www.pdftron.com/silverdox/samples/ClientBin/mech.xod";
        /// <summary>
        /// Creates an new instance of MainPage
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            canvasList = new List<object>();
            isActionReady = false;
            LoadDocument(DOCUMENTURI);
        }

        /// <summary>
        /// Callback triggered when a document has been loaded
        /// </summary>
        private void OnLoadAsyncCallback(Exception error)
        {
            if (error != null)
            {
                System.Diagnostics.Debug.WriteLine(error.StackTrace);
                string errorMessage = "Failed to load document from " + DOCUMENTURI;
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK);
                this.Cursor = Cursors.Arrow;
            }

            for(int i = 0; i < document.Pages.Count; i++)
            {
                canvasList.Add(new Canvas());
                if (i == 0)
                    this.document.LoadCanvasAsync(i, onLoadCanvasAsyncCallback, onLoadCanvasAndResourcesCallback);
                else
                    this.document.LoadCanvasAsync(i, onLoadCanvasAsyncCallback);
            }
            this.TotalPagesText.Text = canvasList.Count.ToString();
        }

        /// <summary>
        /// Loads a remote document through url
        /// </summary>
        /// <param name="path">url path in string</param>
        public void LoadDocument(String path)
        {
            // Create a new empty document to be loaded later
            this.document = new Document();
           
            Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);
            IPartRetriever myRetriever = new HttpPartRetriever(uri);
            this.document.LoadAsync(myRetriever, OnLoadAsyncCallback);
            this.Cursor = Cursors.Wait;
        }

        private void onLoadCanvasAndResourcesCallback(Document.OnLoadCanvasAsyncCallbackArgs e)
        {
            this.ucbook.SetData(this);
            this.isActionReady = true;
            this.Cursor = Cursors.Arrow;
        }

        private void onLoadCanvasAsyncCallback(Document.OnLoadCanvasAsyncCallbackArgs e)
        {
            e.PageCanvas.Background = new SolidColorBrush(Colors.White);
            e.PageCanvas.Children.Add(new Rectangle() { 
                                            Stroke = new SolidColorBrush(Colors.Black), 
                                            StrokeThickness = 1, 
                                            Width = e.PageCanvas.ActualWidth, 
                                            Height = e.PageCanvas.ActualHeight });
            
            canvasList[e.PageNumber] = e.PageCanvas;
            System.Diagnostics.Debug.WriteLine("Canvas {0} callback", e.PageNumber);
        }

        public object GetItem(int index)
        {
            return this.canvasList[index];            
        }

        public int GetCount()
        {
            return this.canvasList.Count();            
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.document.Pages.Count > 0 && this.isActionReady)
                ucbook.AnimateToNextPage(250);   
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.document.Pages.Count > 0 && this.isActionReady)
                ucbook.AnimateToPreviousPage(250);            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ucbook.OnPageTurned += new PageTurnedEventHandler(ucbook_OnPageTurned);
        }

        private void ucbook_OnPageTurned(int leftPageIndex, int rightPageIndex)
        {
            if (leftPageIndex == -1)
            {
                this.LeftPageNumberText.Text = "";
                this.RightPageNumberText.Text = rightPageIndex.ToString();
                this.AmpersandText.Visibility = System.Windows.Visibility.Collapsed;
            }
            else if (rightPageIndex == -1)
            {
                this.LeftPageNumberText.Text = leftPageIndex.ToString();
                this.RightPageNumberText.Text = "";                
                this.AmpersandText.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                this.LeftPageNumberText.Text = leftPageIndex.ToString();
                this.RightPageNumberText.Text = rightPageIndex.ToString();
                this.AmpersandText.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
