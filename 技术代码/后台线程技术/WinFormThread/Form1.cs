using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormThread
{

    public partial class Form1 : Form
    {
        //方法耗时计算
        static System.Diagnostics.Stopwatch _watch = new System.Diagnostics.Stopwatch();

        //线程使用
        public Form1()
        {
            InitializeComponent();
        }
        //定义 BackgroundWorker 后台单独工作线程
        private BackgroundWorker backgroundWorker;
        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker = new BackgroundWorker();
            //正式做事情的地方,调用 RunWorkerAsync 时发生
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            //任务完称时要做的，比如提示等等,当后台操作已完成、被取消或引发异常时发生
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            //任务进行时，报告进度,调用 ReportProgress 时发生
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            //启用进度报告功能 和 ReportProgress，ProgressChanged一起使用
            backgroundWorker.WorkerReportsProgress = true;

            this.progressBar1.Minimum = 100;
            this.progressBar1.Minimum = 0;
            backgroundWorker.RunWorkerAsync();
            // 参数 会在 DoWorkEventArgs e 里面  e.Argument 参数获取
            // backgroundWorker.RunWorkerAsync(10);
            this.button1.Enabled = false;
        }

        //正式做事情的地方,调用 RunWorkerAsync 时发生
        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Action<string> action = delegate(string str)
            {
                this.label2.Text = str;
            };

            for (int i = 0; i < 100; i++)
            {
                //异步更改界面信息
                label2.BeginInvoke(action, "BackgroundWorker进行跑步：" + (i + 1));
                //报告进度
                backgroundWorker.ReportProgress(i + 1);
                System.Threading.Thread.Sleep(20);
            }

            //还可以这样
            //  e.Result = work();         

        }

        private int work()
        {
            Action<string> action = delegate(string str)
            {
                this.label2.Text = str;
            };

            for (int i = 0; i < 100; i++)
            {
                //异步更改界面信息
                label2.BeginInvoke(action, "BackgroundWorker进行跑步：" + (i + 1));
                //报告进度
                backgroundWorker.ReportProgress(i + 1);
                System.Threading.Thread.Sleep(20);
            }

            return -1;
        }

        //任务完称时要做的，比如提示等等,当后台操作已完成、被取消或引发异常时发生
        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.label3.Text = "BackgroundWorker 已经完成所有任务";
            this.button1.Enabled = true;
        }
        //任务进行时，报告进度,调用 ReportProgress 时发生
        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        private System.Threading.Thread thread;

        private void button2_Click(object sender, EventArgs e)
        {

            thread = new System.Threading.Thread(threadDoWork);
            thread.Start();

        }

        private void threadDoWork()
        {
            //返回UI线程设置 progressBar1属性
            progressBar1.BeginInvoke(new Action<int>((max) =>
            {
                progressBar1.Maximum = max;
                progressBar1.Minimum = 0;
            }), 100);

            for (int i = 0; i < 100; i++)
            {
                //返回UI线程设置 label2 变化属性
                label2.BeginInvoke(new Action<int>((value) =>
                {
                    label2.Text = "Thread进行跑步： " + value;
                }), i + 1);

                //返回UI线程设置 progressBar1 变化属性
                progressBar1.BeginInvoke(new Action<int>((vaule) =>
                {
                    progressBar1.Value = vaule;
                }), i + 1);
                System.Threading.Thread.Sleep(20);

            }

            //返回UI线程另一个方法 .Net 2.0 ~3.0只能使用这种模式
            invokeDisplay("Thread执行完毕");
        }

        delegate void UICallback(string msg);

        private void invokeDisplay(string msg)
        {
            if (this.InvokeRequired == true)
            {
                UICallback d = new UICallback(invokeDisplay);
                this.Invoke(d, msg);

            }
            else
            {
                this.label3.Text = msg;
            }

        }
        //async await 使用
        private async void button3_Click(object sender, EventArgs e)
        {
            bool b = await goWork();
            if (b == true)
            {
                label3.Text = "async await 线程完成任务";
            }
            else
            {
                label3.Text = "async await 线程未能完成任务";
            }
        }

        public Task<bool> goWork()
        {

            return Task.Run(() =>
            {
                progressBar1.BeginInvoke(new Action(() =>
                {
                    progressBar1.Maximum = 100;
                    progressBar1.Minimum = 0;
                }));

                for (int i = 0; i < 100; i++)
                {
                    if (i == 50)
                    {
                        return false;
                    }

                    label2.BeginInvoke(new Action<int>((value) =>
                    {
                        label2.Text = "async await 跑步： " + value;

                    }), i + 1);
                    progressBar1.BeginInvoke(new Action<int>((value) =>
                    {

                        progressBar1.Value = value;
                    }), i + 1);
                    System.Threading.Thread.Sleep(20);
                }


                return true;
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Console.WriteLine("------------------------------------------------------");
            _watch.Start();
            Task task1 = Task.Factory.StartNew(delegate() { taskWork1(); });
            Task task2 = Task.Factory.StartNew(delegate() { taskWork2(); });
            //一个一个的执行完毕 并等待完成
            //  task1.Wait();
            // task2.Wait();
            //同时执行 并等待全部完成
            Task.WaitAll(task1, task2);
            _watch.Stop();
            TimeSpan ts = _watch.Elapsed;
            Console.WriteLine(_watch.ElapsedMilliseconds);
        }


        private void taskWork1()
        {

            progressBar1.BeginInvoke(new Action(() =>
            {
                progressBar1.Maximum = 100;
                progressBar1.Minimum = 0;
            }));

            for (int i = 0; i < 100; i++)
            {
                label2.BeginInvoke(new Action<int>((value) =>
                {
                    label2.Text = "Task1任务模式" + value;
                }), i + 1);


                progressBar1.BeginInvoke(new Action<int>((value) =>
                {
                    progressBar1.Value = value;
                }), 1 + i);


                System.Threading.Thread.Sleep(20);
            }
            label2.BeginInvoke(new Action(() =>
            {
                label2.Text += "   任务Task1完成 ";
            }));
        }
        private void taskWork2()
        {
            for (int i = 0; i < 100; i++)
            {
                label3.BeginInvoke(new Action<int>((value) =>
                {

                    label3.Text = "Task2任务模式" + value;

                }), i + 1);


                System.Threading.Thread.Sleep(50);
            }
            label3.BeginInvoke(new Action(() =>
            {

                label3.Text += "   任务Task2完成";

            }));
        }

    }


}
