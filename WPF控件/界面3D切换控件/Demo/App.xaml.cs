using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Demo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            //this.StartupUri = new Uri("Will\\Will_FlipTile3D.xaml", UriKind.Relative);
            this.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            //this.StartupUri = new Uri("MapPathfindingEngineWindow.xaml", UriKind.Relative);
        }
    }
}
