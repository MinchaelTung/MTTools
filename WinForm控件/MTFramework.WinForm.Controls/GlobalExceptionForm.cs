using System;
using System.Threading;
using System.Windows.Forms;

namespace MTFramework.WinForm.Controls
{
    public partial class GlobalExceptionForm : Form
    {
        //当前异常
        private Exception ex = null;
        //当前用户
        private string userName = "";

        public GlobalExceptionForm(string userName, Exception globalException, string applicationName, string developerName)
        {
            InitializeComponent();
            ex = globalException;
            this.userName = userName;
            // 截取前13个字符作为应用程序标题显示
            applicationName =
                applicationName.Length > 13
                ? string.Format("{0}...", applicationName.Substring(0, 13))
                : applicationName;

            // 显示用户提示消息
            lb_Info.Text = string.Format(
                "{0} 遇到问题需要关闭。我们对此引起的不便表示抱歉。请将此问题报告给 {1}。",
                applicationName,
                developerName);

            environmentalInformation();
            initViewData();
            saveLog();
        }

        //显示运行环境
        private void environmentalInformation()
        {
            // ------ 环境信息 ------ //
            // 当前路径
            lbl_CurrentDirectory.Text = Environment.CurrentDirectory;
            // 机器名
            lbl_MachineName.Text = Environment.MachineName;
            // 操作系统
            lbl_OSVersion.Text = Environment.OSVersion.ToString();
            // 系统路径
            // Environment.SystemDirectory;
            // 用户名
            // Environment.UserName
            lbl_UserName.Text = userName;
            // .NET版本
            // Environment.Version;
        }
        //显示界面数据
        private void initViewData()
        {
            // ------ 异常信息 ------ //
            // 消息
            if (ex == null) return;
            txt_Info.Text += ex.Message;
            // 帮助链接
            // ex.HelpLink != null ? " " + ex.HelpLink : " " + "None";
            // 对象
            txt_Source.Text += ex.Source;
            // 堆栈
            txt_StackTrace.Text += ex.StackTrace;
            // 方法
            txt_TargeSite.Text += ex.TargetSite.ToString();

        }

        //记录错误日志
        private void saveLog()
        {

        }

        private void btn_Ignore_Click(object sender, EventArgs e)
        {
            //关闭窗体
            this.Close();
        }

        private void btn_Abort_Click(object sender, EventArgs e)
        {
            // 关闭异常提示
            this.Close();
            // 中止当前线程
            Thread.CurrentThread.Abort("Abort");
        }

        private void btn_Feedback_Click(object sender, EventArgs e)
        {
            //TODO：反馈信息
        }
    }
}
