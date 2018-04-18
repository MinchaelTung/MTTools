using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Postil : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = "http://" + Request.ServerVariables["HTTP_HOST"].ToString() + Request.ServerVariables["PATH_INFO"].ToString();  //获得URL的值
        int i = url.LastIndexOf("/");
        url = url.Substring(0, i);
        this.Session["URL"] = url; //定义Sesssion变量
    }
}
