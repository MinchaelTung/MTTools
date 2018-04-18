using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MTFramework.WPF.Transitions;

namespace Demo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.initdate();
        }
        private void initdate()
        {
            List<object> list = new List<object>();

            list.Add(new UI());

            foreach (Picture item in GetPictures())
            {
                list.Add(item);
            }

            this.LbSe.ItemsSource = list;
            this.LbTName.SelectedIndex = 0;
        }

        private List<Picture> GetPictures()
        {
            List<Picture> list = new List<Picture>();
            //string path = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures) + "\\Sample Pictures";
            string path = AppDomain.CurrentDomain.BaseDirectory + "くらき まい";
            if (Directory.Exists(path) == false)
            {
                return list;
            }
            string[] filelist = Directory.GetFileSystemEntries(path);

            for (int i = 0; i < filelist.Length; i++)
            {
                if (filelist[i].LastIndexOf(".jpg") <= 0)
                {
                    continue;
                }
                list.Add(new Picture() { Url = filelist[i] });
            }

            return list;
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb == null)
            {
                return;
            }
            ListBoxItem lbi = lb.SelectedItem as ListBoxItem;
            if (lbi == null)
            {
                return;
            }
            this.tpContent.Transition = this.FindResource(lbi.Content) as Transition;
        }
    }


    public class UI
    {
    }

    public class Picture
    {
        public string Url { get; set; }
    }
}
