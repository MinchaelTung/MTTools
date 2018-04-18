using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using UpdateLib;

namespace WPFUpdateApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //使用WPF的依赖属性的特性来构造委托
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        private UpdateProgressBarDelegate updatePbDelegate;
        private UpdateInfo updateInfo = null;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.txt_Status.Text = "正在处理更新文件";
            this.progressBar_Update.ValueChanged += progressBar_Update_ValueChanged;
        
            updatePbDelegate = new UpdateProgressBarDelegate(progressBar_Update.SetValue);
            //progressBar更变值方法
            //for (int i = 0; i < 100; i++)
            //{
            //    Dispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(i + 1) }); 
            //}
            
        }

        void progressBar_Update_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue == this.progressBar_Update.Maximum)
            {
                this.txt_Status.Text = "更新完成";
                if (updateInfo.ReStart == true)
                {
                    this.txt_Status.Text = "正在启动程序";
                    this.startProcess(updateInfo.AppName);
                }
                else
                {
                    this.Close();
                }
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            updateInfo = this.loadFile("data.dat");
            if (updateInfo == null)
            {
                this.Close();
            }
            if (updateInfo.ReStart == true)
            {
                this.txt_Status.Text = "正在关闭程序";
                killProcess(updateInfo.AppName);
            }
            this.progressBar_Update.Maximum = updateInfo.UpdateFileList.Count;
            this.txt_Status.Text = "正在更新程序";
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < updateInfo.UpdateFileList.Count; i++)
                {
                    Dispatcher.Invoke(() =>
                    {
                        this.txt_Status.Text = updateInfo.UpdateFileList[i].FileName;
                    });
                    string tmpfile = updateInfo.TmpDirectory + updateInfo.UpdateFileList[i].FileName;
                    string tagfile = updateInfo.UpdateFileList[i].ToString();
                    try
                    {
                        File.Copy(tmpfile, tagfile);
                    }
                    catch (Exception)
                    {

                    }
                    Dispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(i + 1) });

                }
            });

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
