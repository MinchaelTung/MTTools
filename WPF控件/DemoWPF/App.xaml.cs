using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace DemoWPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //注册全部线程异常捕获事件
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            this.StartupUri = new Uri("MainWindow.xaml", UriKind.RelativeOrAbsolute);
        }
        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //   e.Exception
            //通知程序异常已被处理
            //e.Exception
            GlobalExceptionManager.ShowGlobalExceptionInfo(e.Exception);

            e.Handled = true;

        }
    }
}
