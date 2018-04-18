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
using System.Windows.Navigation;

namespace FileBrowser
{
    public partial class DocumentReaderPage : Page
    {
        public DocumentReaderPage()
        {
            InitializeComponent();
            
        }
        protected override void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            base.OnFragmentNavigation(e);
            if (!string.IsNullOrEmpty(e.Fragment))
            {
                string urlPrefix = (string)Application.Current.Resources["PDFTronFileSourceString"];
                this.DocumentReader.LoadDocument(urlPrefix + e.Fragment);
            }
        }

    }
}
