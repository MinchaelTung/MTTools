using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Demo.Properties;
using MTFramework.WinForm.Controls;
using MTFramework.WinForm.Controls.Events;

namespace Demo
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.notifyIcon1.Visible = true;
            //this.notifyIcon1.ShowBalloonTip(1000);//(1000, "当前时间：", DateTime.Now.ToLocalTime().ToString(), ToolTipIcon.Info);

            //return;

            // 创建对象
            TaskbarMessage taskbarMessage1 = new TaskbarMessage();
            //定义背景


            taskbarMessage1.SetBackgroundBitmap(Resources.skin, Color.FromArgb(255, 0, 255));
            //定义关闭按钮
            taskbarMessage1.SetCloseBitmap(Resources.close, Color.FromArgb(255, 0, 255), new Point(200, 20));
            //定义标题绘制矩形位置
            taskbarMessage1.TitleRectangle = new Rectangle(40, 9, 70, 25);
            //定义信息内容绘制矩形位置
            taskbarMessage1.ContentRectangle = new Rectangle(8, 41, 133, 68);
            //定义点击事件
            taskbarMessage1.TitleClick += new EventHandler(taskbarMessage1_TitleClick);
            taskbarMessage1.ContentClick += new EventHandler(taskbarMessage1_ContentClick);
            taskbarMessage1.CloseClick += new EventHandler(taskbarMessage1_CloseClick);


            //触发显示及可以动态更变属性
            //是否使用点击关闭
            taskbarMessage1.CloseClickable = true;
            //是否使用点击标题
            taskbarMessage1.TitleClickable = true;
            //是否使用点击内容
            taskbarMessage1.ContentClickable = true;
            //是否使用选择绘制矩形
            taskbarMessage1.EnableSelectionRectangle = true;
            //鼠标在任务栏消息上是否维持显示状态
            taskbarMessage1.KeepVisibleOnMouseOver = true;
            //再次出现的举动老鼠它当它的消失
            taskbarMessage1.ReShowOnMouseOver = true;
            //显示任务栏消息框 毫秒
            taskbarMessage1.Show("标题文本", "内容文本", (int)2000, (int)2000, (int)2000);
        }
        void taskbarMessage1_CloseClick(object sender, EventArgs e)
        {
            MessageBox.Show("点击关闭");
        }

        void taskbarMessage1_ContentClick(object sender, EventArgs e)
        {
            MessageBox.Show("点击内容");
        }

        void taskbarMessage1_TitleClick(object sender, EventArgs e)
        {
            MessageBox.Show("点击标题");
        }

        private void notifyIcon1_MouseMove(object sender, MouseEventArgs e)
        {
            this.notifyIcon1.ShowBalloonTip(1000);// (1000, "当前时间：", DateTime.Now.ToLocalTime().ToString(), ToolTipIcon.Info);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //实例登陆窗体  //标题  背景图
            LoginForm f = new LoginForm("ss", null);

            //登陆验证事件
            //f.LoginValidation += delegate(object o, LoginValidationEventArgs lve)
            f.LoginValidation += (s, lve) =>
            {
                string logName = lve.LoginName;
                string paw = lve.LoginPassword;
                //验证登陆是否成功
                //标记给LoginForm是否成功 
                lve.IsValidation = LoginForm.LoginStatus.Error;
            };

            f.ShowDialog();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            //查看GlobalExceptionManager类
            //和Program启动类的事件注册
            int[] nums = new int[2];

            int num = nums[10];
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            var screenPoint = PointToScreen(btn.Location);
            //在屏幕上显示一条浮动的帮助信息
            System.Windows.Forms.Help.ShowPopup(this, "这里是帮助信息", new Point(screenPoint.X + 53, screenPoint.Y + 10));

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //            打开帮助文件
            //"file://c:\\charmap.chm"
            System.Windows.Forms.Help.ShowHelp(this, @"file://c:/windows/help/mspaint.chm");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //            打开帮助文件，并跳转到指定的主题
            //"file://c:\\charmap.chm"
            System.Windows.Forms.Help.ShowHelp(this, @"file://c:/windows/help/mspaint.chm", "paint_lines.htm");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //            打开帮助文件，并转到“索引”选项卡
            //url 参数的形式可以是 C:\path\sample.chm 或 /folder/file.htm。
            System.Windows.Forms.Help.ShowHelpIndex(this, @"\folder\paint_lines.htm");
        }
    }
}
