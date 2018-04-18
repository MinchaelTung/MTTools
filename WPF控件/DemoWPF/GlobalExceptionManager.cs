using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoWPF
{
    internal static class GlobalExceptionManager
    {
        public static void ShowGlobalExceptionInfo(Exception globalException)
        {
            string userName = "Test";
            string applicationName = "DemoForm";
            string developerName = "LeInfo";
            MTFramework.WPFControl.GlobalExceptionWPF wpf = new MTFramework.WPFControl.GlobalExceptionWPF(userName, globalException, applicationName, developerName);
            wpf.ShowDialog();
        }
    }
}
