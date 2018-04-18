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
using System.IO;
using System.Data.OleDb;

public partial class GetDoc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //---------------------------------------------------
            Response.Clear();
            string DocType;
            DocType = Request.QueryString["DocType"]; 
            string ID = Request.QueryString["ID"];
            if(ID == null || ID == "")
            {
                Response.End();
                return ;
            }


            string strConnection = "Provider=Microsoft.Jet.OleDb.4.0;";
            strConnection += @"Data Source="+this.Server.MapPath("des.mdb");
            OleDbConnection objConnection=new OleDbConnection(strConnection);
            string sql = "select DocState ,DocContent,DocAip from Doc where id=@id";
            OleDbCommand comd = new OleDbCommand(sql,objConnection);
            comd.Parameters.Add("@id", OleDbType.SmallInt, 10).Value = Int32.Parse(ID);
            objConnection.Open();
            OleDbDataReader dr = comd.ExecuteReader();

           if (dr.Read())
            {
                string DocState = dr.GetValue(0).ToString();

                if(DocType == "aip" && DocState == "aip")
                {
                    Response.BinaryWrite((Byte[])dr["DocAip"]);   //读取二进制的文件
                }else{
                    Response.BinaryWrite((Byte[])dr["DocContent"]);   //读取二进制的文件
                }
            }
            dr.Close();
            objConnection.Close();            
            Response.End();

     } 
}
