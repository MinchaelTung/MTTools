using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace FileBrowser
{
    public class DocumentInfo: INotifyPropertyChanged
    {
        public Uri Uri { get; set; }
        public String DisplayName{ get; set; }

        
        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get { return imageSource; }
            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    this.NotifyPropertyChanged("ImageSource");
                }
            }
        }

        public DocumentInfo(String uriString)
        {
            this.Uri = new Uri((string)Application.Current.Resources["PDFTronFileSourceString"]+uriString, UriKind.Absolute);
            this.DisplayName = uriString;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
        #endregion
    }
}
