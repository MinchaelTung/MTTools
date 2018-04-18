using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;



namespace TestWeb
{
    public class MessageBox
    {
        System.Web.UI.Page p;
        public MessageBox(Page page)
        {
            p = page;
        }

        /// <summary>
        ///  Open
        /// </summary>
        /// <param name="key">标识</param>
        /// <param name="alert">提示文本</param>
        /// <param name="url">打开路径</param>
        /// <param name="up">UpdatePanel容器</param>
        public void Show(string key, string alert, string url, UpdatePanel up)
        {
            if (up == null)
            {
                p.ClientScript.RegisterStartupScript(p.GetType(),key, "<script>alert('" + alert + "');window.open('" + url + "');</script>");
                //p.RegisterStartupScript(key, "<script>alert('" + alert + "');window.open('" + url + "');</script>");
                return;

            }
            else
            {
                ScriptManager.RegisterStartupScript(up, this.GetType(), key, "alert('" + alert + "');window.open('" + url + "');", true);
                return;
            }
        }


        /// <summary>
        /// 提示后打开一个超链接
        /// </summary>
        /// <param name="key">标识</param>
        /// <param name="alert">提示文本</param>
        /// <param name="url">跳转路径.若为空直接alert</param>
        /// <param name="up">UpdatePanel容器</param>
        /// <param name="parent">false：window.location.href ; true：window.parent.location.href</param>
        public void Show(string key, string alert, string url, UpdatePanel up, bool parent)
        {

            if (!parent)
            {
                if (url != string.Empty && url != null)
                {
                    ScriptManager.RegisterStartupScript(up, this.GetType(), key, "alert('" + alert + "');window.location.href='" + url + "';", true);
                    return;
                }
            }
            else
            {
                if (url != string.Empty && url != null)
                {
                    ScriptManager.RegisterStartupScript(up, this.GetType(), key, "alert('" + alert + "');window.parent.location.href='" + url + "';", true);
                    return;
                }
            }
            ScriptManager.RegisterStartupScript(up, this.GetType(), key, "alert('" + alert + "');", true);
        }

        /// <summary>
        /// 提示后打开一个超链接
        /// </summary>
        /// <param name="key">标识</param>
        /// <param name="alert">提示文本</param>
        /// <param name="url">跳转路径.若为空直接alert</param>
        /// <param name="parent">false：window.location.href ; true：window.parent.location.href</param>
        public void Show(string key, string alert, string url, bool parent)
        {

            if (!parent)
            {
                if (url != string.Empty && url != null)
                {
                    p.ClientScript.RegisterStartupScript(p.GetType(), key, "<script>alert('" + alert + "');window.location.href='" + url + "';</script>");
                    //p.RegisterStartupScript(key, "<script>alert('" + alert + "');window.location.href='" + url + "';</script>");
                    return;
                }
            }
            else
            {
                if (url != string.Empty && url != null)
                {
                    p.ClientScript.RegisterStartupScript(p.GetType(),key, "<script>alert('" + alert + "');window.parent.location.href='" + url + "';</script>");
                    //p.RegisterStartupScript(key, "<script>alert('" + alert + "');window.parent.location.href='" + url + "';</script>");
                    return;
                }
            }
            p.ClientScript.RegisterStartupScript(p.GetType(), key, "<script>alert('" + alert + "');</script>");
            //p.RegisterStartupScript(key, "<script>alert('" + alert + "');</script>");
        }

        /// <summary>
        /// 自定义javascript语句
        /// </summary>
        /// <param name="key">标识</param>
        /// <param name="js">javascript语句</param>
        /// <param name="up">UpdatePanel 容器</param>
        public void Show(string key, string js, UpdatePanel up)
        {
            if (up == null)
            {
                p.ClientScript.RegisterStartupScript(p.GetType(),key, "<script>" + js + "</script>");
                //p.RegisterStartupScript(key, "<script>" + js + "</script>");
                return;

            }
            else
            {
                ScriptManager.RegisterStartupScript(up, this.GetType(), key, js, true);
                return;
            }
        }



    }
}