<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Postil.aspx.cs" Inherits="Postil" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title>�쵼�ױ�ǩ��</title>
<!-- --------------------=== ����Weboffice��ʼ������ ===--------------------- -->
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
<!-- --------------------=== ����Weboffice��ʼ������ ===--------------------- -->
<script language="javascript"  for="HWPostil" event="NotifyCtrlReady" type="text/javascript" >
<!--
 HWPostil_NotifyCtrlReady()
//-->
</script>

<script language="javascript" type="text/jscript" >
<!--

// --------------------------=== �ؼ���ʼ������ ===------------------------------ //
function HWPostil_NotifyCtrlReady() {
    // װ��word�ļ���ת��ΪAIP�ļ�
  //  if("<%=DocType%>" == "wps")
    //    document.all.HWPostil.LoadFileEx("<%=URL %>/GetDoc.aspx?ID=<%=ID%>&DocType=aip","<%=DocType%>",0,0);
   // else
        document.all.HWPostil.LoadFile("<%=URL %>/GetDoc.aspx?ID=<%=ID%>&DocType=aip");
 
 	document.all.HWPostil.Login("HWSEALDEMO",4,32767,"DEMO","");
	document.all.HWPostil.CurrAction = 264;
	document.all.HWPostil.SetPageMode(1,100);
}

// ---------------------------=== �ر��ĵ� ===-------------------------------- //
function window_onunload() {
	document.all.HWPostil.CloseDoc(0);

}

// -------------------------=== ȫ��ǩ���ĵ� ===------------------------------ //
function ShowFullWindows() {
	document.all.HWPostil.ShowFullScreen = true
	return false;
}

// --------------------------=== ����AIP�ĵ� ===------------------------------ //
function saveaip() {
	document.all.HWPostil.HttpInit();
	document.all.HWPostil.HttpAddPostString("ID","<%=ID%>");
	document.all.HWPostil.HttpAddPostString("DocType", "aip");
	document.all.HWPostil.HttpAddPostCurrFile("DocContent");
	var vtRet;
	vtRet = document.all.HWPostil.HttpPost("<%=URL %>/upload.aspx");
	if("succeed" == vtRet){
		alert("�ļ��ϴ��ɹ�");	
	}else{
		alert("�ļ��ϴ�ʧ��");
		return false;
	}
	document.all.HWPostil.CloseDoc(0);
    window.location.href  = "Default.aspx";
} 
// --------------------------=== ������ҳ ===------------------------------ //
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
       <td valign="top" bgColor="#e5ecf9" style="height: 21px;"><p align="left"><b><strong>WebOffice��ʾ����</strong></b>
</p></td>
   </tr>
</table>
<br /><br />
 

    <table width="100%"  border="0" cellpadding="0" cellspacing="1" bgcolor="#3366cc">

    <tr bgcolor="#FFFFFF">
        <td bgcolor="#ffffff" class="maintxt" rowspan="2" style="width: 49px; height: 24px">
            <div align="center"><strong>�ĺ�:</strong></div>
        </td>
        <td rowspan="2" style="width: 146px; height: 24px">
            <input name="DocTitle" value= "<%Response.Write(ID);%>" size="14" id="Text1"/></td>
        <td bgcolor="#ffffff" colspan="2" rowspan="2" style="height: 24px; text-align: right;">
            <a href="#" onClick="ShowFullWindows()">ȫ������</a> 
            <a href="#" onClick="saveaip()">����</a>  
            <a href="#" onClick="quit()">����</a> </td>
				</tr>

    <tr bgcolor="#FFFFFF">
	</tr>

    <tr bgcolor="#FFFFFF">
	  <td valign="top" bgcolor="#FFFFFF" style="height: 560px; width: 49px;">
		<!-- -------------------=== Start Ƕ��Table ===------------------------------- -->
	 
	    <!-- -------------------=== End Ƕ��Table ===------------------------------- -->
	  </td>

	  <td colspan="3" valign="top" style="height: 560px">
		<!-- -----------------------------== װ��AIP�ؼ� ==--------------------------------- -->
        <object id="HWPostil"  height="600" width="100%" classid="clsid:FF1FE7A0-0578-4FEE-A34E-FB21B277D561" codebase="HWPostil.ocx#V2,2,6,6">
        <param name="_Version" value="65536" />
        <param name="_ExtentX" value="19473" />
        <param name="_ExtentY" value="15875" />
        <param name="_StockProps" value="0" />
        </object>	    
        <!-- --------------------------------== ����װ�ؿؼ� ==----------------------------------- -->
	  </td>
    </tr>
  </table> 
 
 <br /><br /> 
 
<table style="width: 100%" border = "0" cellpadding="0" cellspacing="0">
   <tr>
       <td valign="top" bgColor="#e5ecf9" ><span style="color: red">������·��:<%=this.Session["URL"].ToString() %> </span>
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
