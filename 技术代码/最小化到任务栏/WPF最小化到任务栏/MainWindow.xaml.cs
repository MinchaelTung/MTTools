using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF最小化到任务栏
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        WindowState ws;
        WindowState wsl;
        System.Windows.Forms.NotifyIcon notifyIcon;
        public MainWindow()
        {
            InitializeComponent();
            this.StateChanged += MainWindow_StateChanged;
            icon();
            wsl = WindowState;
        }

        void MainWindow_StateChanged(object sender, EventArgs e)
        {
            ws = WindowState;
            if (ws == WindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void icon()
        {
            this.notifyIcon = new System.Windows.Forms.NotifyIcon();
            // this.notifyIcon.BalloonTipText = "Hello, 文件监视器"; //设置程序启动时显示的标题文本
            // this.notifyIcon.ShowBalloonTip(1000);//显示标题文本事件
            this.notifyIcon.Text = "文件监视器";//最小化到托盘时，鼠标点击时显示的文本
            this.notifyIcon.Icon = new System.Drawing.Icon("cd1.ico");//程序图标
            this.notifyIcon.Visible = true;//显示图标
            notifyIcon.MouseDoubleClick += OnNotifyIconDoubleClick;

        }

        private void OnNotifyIconDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Show();
            WindowState = wsl;
        }


    }
}
