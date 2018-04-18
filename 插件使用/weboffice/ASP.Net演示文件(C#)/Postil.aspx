<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Postil.aspx.cs" Inherits="Postil" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title>领导亲笔签批</title>
<!-- --------------------=== 调用Weboffice初始化方法 ===--------------------- -->
<%
    string URL = this.Session["URL"].ToString();
    string ID, DocType, DocOriType;
    ID = Request.QueryString["ID"];
    if(ID == null || ID==""){
        ID= "0";
    }
    DocType = Request.QueryString["DocType"];

    DocOriType = Request.QueryString["DocOriType"];
 %>
<!-- --------------------=== 调用Weboffice初始化方法 ===--------------------- -->
<script language="javascript"  for="HWPostil" event="NotifyCtrlReady" type="text/javascript" >
<!--
 HWPostil_NotifyCtrlReady()
//-->
</script>

<script language="javascript" type="text/jscript" >
<!--

// --------------------------=== 控件初始化方法 ===------------------------------ //
function HWPostil_NotifyCtrlReady() {
    // 装载word文件并转换为AIP文件
  //  if("<%=DocType%>" == "wps")
    //    document.all.HWPostil.LoadFileEx("<%=URL %>/GetDoc.aspx?ID=<%=ID%>&DocType=aip","<%=DocType%>",0,0);
   // else
        document.all.HWPostil.LoadFile("<%=URL %>/GetDoc.aspx?ID=<%=ID%>&DocType=aip");
 
 	document.all.HWPostil.Login("HWSEALDEMO",4,32767,"DEMO","");
	document.all.HWPostil.CurrAction = 264;
	document.all.HWPostil.SetPageMode(1,100);
}

// ---------------------------=== 关闭文档 ===-------------------------------- //
function window_onunload() {
	document.all.HWPostil.CloseDoc(0);

}

// -------------------------=== 全屏签批文档 ===------------------------------ //
function ShowFullWindows() {
	document.all.HWPostil.ShowFullScreen = true
	return false;
}

// --------------------------=== 保存AIP文档 ===------------------------------ //
function saveaip() {
	document.all.HWPostil.HttpInit();
	document.all.HWPostil.HttpAddPostString("ID","<%=ID%>");
	document.all.HWPostil.HttpAddPostString("DocType", "aip");
	document.all.HWPostil.HttpAddPostCurrFile("DocContent");
	var vtRet;
	vtRet = document.all.HWPostil.HttpPost("<%=URL %>/upload.aspx");
	if("succeed" == vtRet){
		alert("文件上传成功");	
	}else{
		alert("文件上传失败");
		return false;
	}
	document.all.HWPostil.CloseDoc(0);
    window.location.href  = "Default.aspx";
} 
// --------------------------=== 返回首页 ===------------------------------ //
function quit(){
	document.all.HWPostil.CloseDoc(0);
	window.location.href  = "Default.aspx"
}
//-->
</script>


</head>
<body>
<form id="form1" runat="server">
</form>
<TABLE cellSpacing=0 cellPadding=0 width="100%" border=0>
      <TR> 
        <TD bgColor=#3366cc><IMG height=1 alt="" width=1></TD>
      </TR>
</TABLE>
<table style="width: 100%" border = "0" cellpadding="0" cellspacing="0">
   <tr>
       <td valign="top" bgColor="#e5ecf9" style="height: 21px;"><p align="left"><b><strong>WebOffice演示程序</strong></b>
</p></td>
   </tr>
</table>
<br /><br />
 

    <table width="100%"  border="0" cellpadding="0" cellspacing="1" bgcolor="#3366cc">

    <tr bgcolor="#FFFFFF">
        <td bgcolor="#ffffff" class="maintxt" rowspan="2" style="width: 49px; height: 24px">
            <div align="center"><strong>文号:</strong></div>
        </td>
        <td rowspan="2" style="width: 146px; height: 24px">
            <input name="DocTitle" value= "<%Response.Write(ID);%>" size="14" id="Text1"/></td>
        <td bgcolor="#ffffff" colspan="2" rowspan="2" style="height: 24px; text-align: right;">
            <a href="#" onClick="ShowFullWindows()">全屏审批</a> 
            <a href="#" onClick="saveaip()">保存</a>  
            <a href="#" onClick="quit()">返回</a> </td>
				</tr>

    <tr bgcolor="#FFFFFF">
	</tr>

    <tr bgcolor="#FFFFFF">
	  <td valign="top" bgcolor="#FFFFFF" style="height: 560px; width: 49px;">
		<!-- -------------------=== Start 嵌套Table ===------------------------------- -->
	 
	    <!-- -------------------=== End 嵌套Table ===------------------------------- -->
	  </td>

	  <td colspan="3" valign="top" style="height: 560px">
		<!-- -----------------------------== 装载AIP控件 ==--------------------------------- -->
        <object id="HWPostil"  height="600" width="100%" classid="clsid:FF1FE7A0-0578-4FEE-A34E-FB21B277D561" codebase="HWPostil.ocx#V2,2,6,6">
        <param name="_Version" value="65536" />
        <param name="_ExtentX" value="19473" />
        <param name="_ExtentY" value="15875" />
        <param name="_StockProps" value="0" />
        </object>	    
        <!-- --------------------------------== 结束装载控件 ==----------------------------------- -->
	  </td>
    </tr>
  </table> 
 
 <br /><br /> 
 
<table style="width: 100%" border = "0" cellpadding="0" cellspacing="0">
   <tr>
       <td valign="top" bgColor="#e5ecf9" ><span style="color: red">服务器路径:<%=this.Session["URL"].ToString() %> </span>
    </td>
   </tr>
</table>
<TABLE cellSpacing=0 cellPadding=0 width="100%" border=0>
<TR> 
<TD bgColor=#3366cc><IMG height=1 alt="" width=1></TD>
</TR>
</TABLE>
</body>
</html>
