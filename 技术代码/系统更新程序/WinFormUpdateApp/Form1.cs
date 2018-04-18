 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UpdateLib;

namespace WinFormUpdateApp
{
    public partial class Form1 : Form
    {
        private UpdateInfo updateInfo;

        private BackgroundWorker backgroundWorker;


        //BackgroundWorker
        public Form1()
        {
            InitializeComponent();

            this.progressBar1.Minimum = 0;
            this.lbl_Status.Text = "正在处理更新文件";
            this.Load += Form1_Load;
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            //正式做事情的地方,调用 RunWorkerAsync 时发生
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            //任务完称时要做的，比如提示等等,当后台操作已完成、被取消或引发异常时发生
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            //任务进行时，报告进度,调用 ReportProgress 时发生
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
        }

        //任务进行时，报告进度,调用 ReportProgress 时发生
        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        //任务完称时要做的，比如提示等等,当后台操作已完成、被取消或引发异常时发生
        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Cancelled == true)
            {
                this.Close();
                return;
            }

            this.lbl_Status.Text = "更新完成";
            if (updateInfo.ReStart == true)
            {
                this.lbl_Status.Text = "正在启动程序";
                this.startProcess(updateInfo.AppName);
            }
            else
            {
                this.Close();
            }

        }

        //正式做事情的地方,调用 RunWorkerAsync 时发生
        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = CopyFile(sender, e);

        }

        void Form1_Load(object sender, EventArgs e)
        {

            updateInfo = this.loadFile("data.dat");
            if (updateInfo == null)
            {
                this.Close();
            }
            if (updateInfo.ReStart == true)
            {
                this.lbl_Status.Text = "正在关闭程序";
                killProcess(updateInfo.AppName);
            }
            this.progressBar1.Maximum = updateInfo.UpdateFileList.Count;
            this.lbl_Status.Text = "正在更新程序";

            backgroundWorker.RunWorkerAsync();
        }

        private int CopyFile(object sender, DoWorkEventArgs e)
        {
            Action<string> action = new Action<string>((str) =>
            {
                this.lbl_Status.Text = str;
            });


            for (int i = 0; i < updateInfo.UpdateFileList.Count; i++)
            {
                lbl_Status.BeginInvoke(action, updateInfo.UpdateFileList[i].FileName);

                string tmpfile = updateInfo.TmpDirectory + updateInfo.UpdateFileList[i].FileName;
                string tagfile = updateInfo.UpdateFileList[i].ToString();
                try
                {
                    File.Copy(tmpfile, tagfile);
                }
                catch (Exception)
                {

                }
                backgroundWorker.ReportProgress(i + 1);

            }
            return -1;
        }

        /// <summary>
        /// 获取配置文件信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private UpdateInfo loadFile(string filePath)
        {
            //并非一定要使用这种方式获取数据 可以通过xml也可以实现的
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open))
                {
                    IFormatter formatter = new BinaryFormatter();
                    object obj = formatter.Deserialize(file);
                    if (obj is UpdateInfo)
                    {
                        return obj as UpdateInfo;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 关闭程序
        /// </summary>
        /// <param name="appName">程序名称如:app.exe</param>
        private void killProcess(string appName)
        {
            System.Diagnostics.Process[] proc = System.Diagnostics.Process.GetProcessesByName(appName);
            foreach (System.Diagnostics.Process pro in proc)
            {
                try
                {
                    pro.Kill();
                }
                catch (Exception)
                {
                }

            }
        }
        /// <summary>
        /// 启动程序
        /// </summary>
        /// <param name="appName"></param>
        private void startProcess(string appName)
        {
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + appName);
        }
    }
}
