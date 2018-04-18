using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms最小化到任务栏
{
    public partial class Form1 : Form
    {
        FormWindowState ws;
        FormWindowState wsl;
        System.Windows.Forms.NotifyIcon notifyIcon;
        public Form1()
        {
            InitializeComponent();
            this.SizeChanged += Form1_SizeChanged;
            this.iconset();
            wsl = this.WindowState;

        }

        void Form1_SizeChanged(object sender, EventArgs e)
        {
            ws = WindowState;
            if (ws == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void iconset()
        {
            this.notifyIcon = new System.Windows.Forms.NotifyIcon();
            // this.notifyIcon.BalloonTipText = "Hello, 文件监视器"; //设置程序启动时显示的标题文本
            // this.notifyIcon.ShowBalloonTip(1000);//显示标题文本事件
            this.notifyIcon.Text = "文件监视器";//最小化到托盘时，鼠标点击时显示的文本
            this.notifyIcon.Icon = new System.Drawing.Icon("cd1.ico");//程序图标
            this.notifyIcon.Visible = true;//显示图标
            notifyIcon.MouseDoubleClick += OnNotifyIconDoubleClick;
        }

        private void OnNotifyIconDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            WindowState = wsl;
        }
    }
}
