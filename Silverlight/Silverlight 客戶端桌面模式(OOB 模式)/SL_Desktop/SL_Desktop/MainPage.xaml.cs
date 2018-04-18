using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SL_Desktop
{
    /// <summary>
    /// Silverlight Demo
    /// </summary>
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            this.Btn_Install.Visibility = System.Windows.Visibility.Collapsed;
            if (Application.Current.IsRunningOutOfBrowser == true)
            {
                //使用桌面模式
                this.Txt_Msg.Text = "当前调用的是使用桌面模式";
            }
            else
            {
                //使用的游览器
                this.Txt_Msg.Text = "当前调用的是使用游览器模式";
            }
            this.checkNetworkStatus();
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            this.displayInstallState();
            Application.Current.CheckAndDownloadUpdateCompleted += Current_CheckAndDownloadUpdateCompleted;
            Application.Current.InstallStateChanged += Current_InstallStateChanged;
            
        }
        //网络连接改变事件
        void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
           this.checkNetworkStatus();
        }
        //檢查網絡狀態
        private void checkNetworkStatus()
        {
            if (NetworkInterface.GetIsNetworkAvailable() == true)
            {
                this.Txt_NetState.Text = "當前網絡處於連接狀態";
            }
            else
            {
                this.Txt_NetState.Text = "當前網絡處於離綫狀態";
            }
        }

        //更新事件
        void Current_CheckAndDownloadUpdateCompleted(object sender, CheckAndDownloadUpdateCompletedEventArgs e)
        {
            if (e.UpdateAvailable == true && e.Error == null)
            {
                MessageBox.Show("应用新版本已经下载成功，请重新启动程序。");
                Application.Current.MainWindow.Close();
            }
            else if (e.UpdateAvailable == false && e.Error == null)
            {
                MessageBox.Show("已经是最新版本了。");
            }
            else if (e.Error != null)
            {
                MessageBox.Show("在检测应用更新时, 出现以下错误信息:" + Environment.NewLine + Environment.NewLine + e.Error.Message);
            }
        }

        /// <summary>
        /// 检查安装状态
        /// </summary>
        private void displayInstallState()
        {
            switch (Application.Current.InstallState)
            {
                case InstallState.InstallFailed:
                    //不能将该应用程序安装为在浏览器外部运行
                    this.Txt_Status.Text = "不能将该应用程序安装为在浏览器外部运行";
                    break;
                case InstallState.Installed:
                    //已经将该应用程序安装为在浏览器外部运行
                    this.Txt_Status.Text = "已经将该应用程序安装为在浏览器外部运行";
                    break;
                case InstallState.Installing:
                    //正在将此应用程序安装为在浏览器外部运行
                    this.Txt_Status.Text = "正在将此应用程序安装为在浏览器外部运行";
                    break;
                case InstallState.NotInstalled:
                    //尚未将该应用程序安装为在浏览器外部运行
                    this.Txt_Status.Text = "尚未将该应用程序安装为在浏览器外部运行";
                    this.Btn_Install.Visibility = System.Windows.Visibility.Visible;
                    break;
                default:
                    break;
            }
        }
        //更新
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Application.Current.CheckAndDownloadUpdateAsync();
        }
        //安装状态
        void Current_InstallStateChanged(object sender, EventArgs e)
        {
            this.displayInstallState();
        }
        //关闭按钮
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
        //安装按钮
        private void Btn_Install_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Install();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("应用已经安装.");
            }
            catch (Exception)
            {
                this.Txt_Status.Text = "安装失败";
            }

        }


    }
}
