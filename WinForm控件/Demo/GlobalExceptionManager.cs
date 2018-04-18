using System;

namespace Demo
{
    internal static class GlobalExceptionManager
    {
        /// <summary>
        /// 显示全局异常提示信息
        /// </summary>
        /// <param name="globalException">捕获到的全局异常</param>
        public static void ShowGlobalExceptionInfo(Exception globalException)
        {
            string userName = "Test";
            string applicationName = "DemoForm";
            string developerName = "LeInfo";
            MTFramework.WinForm.Controls.GlobalExceptionForm form = new MTFramework.WinForm.Controls.GlobalExceptionForm(userName, globalException, applicationName, developerName);
            form.ShowDialog();

        }

    }
}
