using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.OleDb;


public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
       
        string url="http://" + Request.ServerVariables["HTTP_HOST"].ToString() + Request.ServerVariables["PATH_INFO"].ToString();  //获得URL的值
        int i=url.LastIndexOf("/");
        url=url.Substring(0,i);  
        this.Session["URL"] = url; //定义Sesssion变量



    }
    protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        string strConnection = "Provider=Microsoft.Jet.OleDb.4.0;";
        strConnection += @"Data Source="+this.Server.MapPath("des.mdb");//动态连接Access数据库 
        OleDbConnection objConnection = new OleDbConnection(strConnection);
        objConnection.Open();  //打开连接
        string sql = "delete * from Doc where id=@id";
        OleDbCommand comd = new OleDbCommand(sql,objConnection);
        comd.Parameters.Add("@id", OleDbType.Integer, 8).Value = e.Keys[0].ToString();
        try
        {

            comd.ExecuteNonQuery();
            Label2.Text = "<font color='red'>" + "删除成功" + "</font>";


        }
        catch (Exception)
        {
           
            Label2.Text = "<font color='red'>" + "删除失败" + "</font>";
        
        }
        
        objConnection.Close();

    }
}
