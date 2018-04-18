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
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;

public partial class upload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();
 		//ID为文档的主键，如果ID不为空，则更新数据，否则新建一条记录
        string ID = Request.Params["ID"];
        string DocID,DocTitle,DocType;
        DocID = "test";
        DocTitle = "test";
        if(ID != null && ID !=""){
            DocID = Request.Params["DocID"];
            DocTitle = Request.Params["DocTitle"]; 
        }
        DocType = Request.Params["DocType"];
        if(DocType == "")
            DocType =  "doc";

        DocType = DocType.Substring(0, 3);
        if (Request.Files.Count > 0)
        {
                OleDbConnection objConnection;
    
                HttpPostedFile upPhoto = Request.Files[0];
                int upPhotoLength = upPhoto.ContentLength;
                byte[] PhotoArray = new Byte[upPhotoLength];
                Stream PhotoStream = upPhoto.InputStream;
                PhotoStream.Read(PhotoArray, 0, upPhotoLength); //这些编码是把文件转换成二进制的文件

                string strConnection = "Provider=Microsoft.Jet.OleDb.4.0;";
                strConnection += @"Data Source=" + this.Server.MapPath("des.mdb");
                objConnection = new OleDbConnection(strConnection);
                objConnection.Open();

                string strSql;
                if (ID != null && ID != "")
                {
                    if (DocType == "aip")
                    {
                        strSql = "update Doc Set DocAip = @FImage ,DocState = 'aip' where id = " + ID;
                    }
                    else
                    {
                        strSql = "update Doc Set DocContent  = @FImage where id = " + ID;
                    }
                    OleDbCommand comd = new OleDbCommand(strSql, objConnection); //执行sql语句
                    comd.Parameters.Add("@FImage", OleDbType.Binary);
                    comd.Parameters["@FImage"].Value = PhotoArray;
                    comd.ExecuteNonQuery(); //执行查询    
                    
                }
                else
                {
                    strSql = "Insert into Doc(DocID,DocTitle,DocType,DocContent) values(@DocId,@DocTitle,@DocType,@FImage)";
                    OleDbCommand comd = new OleDbCommand(strSql, objConnection); //执行sql语句
                    if(DocID != "")
                        comd.Parameters.Add("@DocId", OleDbType.VarChar, 20).Value = DocID;  //定义参数同时给它赋值
                    if (DocTitle != "")
                        comd.Parameters.Add("@DocTitle", OleDbType.VarChar, 50).Value = DocTitle;
                    comd.Parameters.Add("@DocType", OleDbType.VarChar, 10).Value = DocType;
                    comd.Parameters.Add("@FImage", OleDbType.Binary);
                    comd.Parameters["@FImage"].Value = PhotoArray;
                    comd.ExecuteNonQuery(); //执行查询
                }
                objConnection.Close();  //关闭数据库
                Response.Write("succeed");
                Response.End();
            //-------------------------------------------
        }else{
            Response.Write("No File Upload!");
        }
    }
}
