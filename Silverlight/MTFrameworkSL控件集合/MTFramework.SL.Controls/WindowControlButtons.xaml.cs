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

namespace MTFramework.SL.Controls
{
    public partial class WindowControlButtons : UserControl
    {
        public WindowControlButtons()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 缩小窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Minimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 最大化窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Maximize_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Image image = btn.Content as Image;
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
          
                image.Source =  this.max1.Source;
                Application.Current.MainWindow.WindowState = WindowState.Normal;
              
            }
            else
            {
               
                image.Source = this.max2.Source;
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
               
            }
            

        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
    }
}
