using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace LinkingEAsy.ZipValidationApp
{


    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private const int WS_SHOWNORMAL = 1;
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// 获取当前是否具有相同进程。
        /// </summary>
        /// <returns></returns>
        public static Process GetRunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //遍历正在有相同名字运行的例程   
            foreach (Process process in processes)
            {
                //忽略现有的例程   
                if (process.Id != current.Id)
                    //确保例程从EXE文件运行 
                    if (System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                        return process;
            }
            return null;
        }
        /// <summary>
        /// 激活原有的进程。
        /// </summary>
        /// <param name="instance"></param>
        public static void HandleRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, WS_SHOWNORMAL);
            SetForegroundWindow(instance.MainWindowHandle);
        }

        private void Application_Startup_1(object sender, StartupEventArgs e)
        {
            Process instance = GetRunningInstance();
            if (instance == null)
            {
                this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            }
            else
            {
                HandleRunningInstance(instance);
            }

        }

    }
}
