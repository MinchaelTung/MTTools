using System.Web;
using System.Web.UI;

namespace MTFramework.Web.Controls
{
    /// <summary>
    /// 显示提示对话框（WEB）
    /// </summary>
    public class MessageBox
    {
        /// <summary>
        /// 显示提示对话框（跳转到指定的 URL ）
        /// </summary>
        /// <param name="alertStrt">提示字符</param>
        /// <param name="url">跳转URL</param>
        public static void Show(HttpContext currentPage, string alertStrt, string url)
        {
            Page p = (Page)currentPage.Handler;

            p.Response.Write(string.Format("<script type=\"text/javascript\">alert('{0}');window.location.href='{1}';</script>", alertStrt, url));
            p.Response.End();
        }

        /// <summary>
        /// 显示提示对话框
        /// </summary>
        /// <param name="alertStrt">提示字符</param>
        public static void Show(HttpContext currentPage, string alertStr)
        {
            Page p = (Page)currentPage.Handler;

            p.ClientScript.RegisterStartupScript(p.GetType(), "MessageBox", string.Format("alert('{0}');", alertStr), true);
        }
    }

}
