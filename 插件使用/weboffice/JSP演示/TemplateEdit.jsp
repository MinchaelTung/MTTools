<%@ page contentType="text/html;charset=GB2312" %>
<%@ page import="java.io.*,java.text.*,java.util.*,java.sql.*,javax.servlet.*,javax.servlet.http.*" %>
<%@ include file="conn.jsp" %>
<%@ include file="config.jsp" %>
<%
  String mDescript="";		// ģ������
  String mFileName="";		// ģ������

  String mHttpUrlName=request.getRequestURI();
  String mScriptName=request.getServletPath();
  String mServerName="OfficeServer.jsp";
  String mServerUrl="http://"+request.getServerName()+":"+request.getServerPort()+mHttpUrlName.substring(0,mHttpUrlName.lastIndexOf(mScriptName))+"/"+mServerName;
  String mClientUrl="http://"+request.getServerName()+":"+request.getServerPort()+mHttpUrlName.substring(0,mHttpUrlName.lastIndexOf(mScriptName))+"/"+mClientName;

  String mEditType="1";
  String mUserName="Administrator";


  // ========= DJ ============
	String serverHome = "http://"+request.getServerName()+":"+request.getServerPort() +
							mHttpUrlName.substring(0,mHttpUrlName.lastIndexOf(mScriptName));
	String TemplateId;
	if(request.getParameter("TemplateID")==null){
		 TemplateId="0";
	}else{
		TemplateId=request.getParameter("TemplateID");
	}
	String RecordID="" ;
	String newOrUpdate = "update";
	String FileType = request.getParameter("FileType");
  // ========== END ============


  //�����ݿ�

    String mSql="Select * From Template_File Where TemplateID="+TemplateId;
    try
    {
      rs=stmt.executeQuery(mSql);
      if (rs.next()) {
				newOrUpdate = "update";
        mFileName=rs.getString("FileName");
        mDescript=rs.getString("Descript");
        RecordID=rs.getString("recordid");
      } else {
				 newOrUpdate = "new";
				//ȡ��Ψһֵ(RecordID)
        java.util.Date dt=new java.util.Date();
        long lg=dt.getTime();
        Long ld=new Long(lg);
				//��ʼ��ֵ
        RecordID=ld.toString();
        mFileName="����ģ��."+FileType;
        mDescript="���Ĺ���ģ��";
      }
    }catch(Exception e)
    {
      System.out.println(e.toString());
    }
  
%>

<html>
<head>
<style type="text/css">
<!--
.topSide {
	border-top-width: 1px;
	border-right-width: 1px;
	border-bottom-width: 1px;
	border-left-width: 1px;
	border-top-style: solid;
	border-top-color: #3366CC;
	border-right-color: #3366CC;
	border-bottom-color: #3366CC;
	border-left-color: #3366CC;
}
.leftBorder {
	border-top-width: 1px;
	border-right-width: 1px;
	border-bottom-width: 1px;
	border-left-width: 1px;
	border-left-style: dashed;
	border-left-color: #0066FF;
}
.downSide {
	border-top-width: 1px;
	border-right-width: 1px;
	border-bottom-width: 1px;
	border-left-width: 1px;
	border-bottom-style: dashed;
	border-bottom-color: #0066FF;
}
.STYLE1 {color: #FF0000}
-->
</style>
<title>ģ�����</title>
<script language=javascript>
// ��ʼ���ؼ�
function WebOffice1_NotifyCtrlReady() {
	<%
		if("new".equals(newOrUpdate)) {	// �½��ĵ�
	%>
			document.all.WebOffice1.LoadOriginalFile("", "<%=FileType%>");
	<% 
		} else {	// ���ļ� 
	%>
			document.all.WebOffice1.LoadOriginalFile("<%=serverHome%>/getModDoc.jsp?modId=<%=TemplateId%>", "<%=FileType%>");
	<%
		}
	%>

	document.all.WebOffice1.SetToolBarButton2("Menu Bar",1,1);	// ���β˵������ļ��˵�(Word)
	// document.all.WebOffice1.SetToolBarButton2("Worksheet Menu Bar",1,1);  // ���β˵������ļ��˵�(Excel)
	document.all.WebOffice1.SetKeyCtrl(595,-1,0);				// ���� �����ݼ�(Ctrl+S) 
	document.all.WebOffice1.SetKeyCtrl(592,-1,0);				// ���� ��ӡ��ݼ�(Ctrl+P) 
	document.all.WebOffice1.SetToolBarButton2("Standard",1,1);	// �����½���
	document.all.WebOffice1.SetToolBarButton2("Standard",2,1);	// ���δ���
	document.all.WebOffice1.SetToolBarButton2("Standard",3,1);	// ���α�����

}

// �ر��ĵ�
function unloadFile() {
	document.all.WebOffice1.SetToolBarButton2("Menu Bar",1,4);	// �ָ������εĲ˵����ļ��˵�(word)
	document.all.WebOffice1.SetToolBarButton2("Standard",1,3);	// �ָ������ε��½���
	document.all.WebOffice1.SetToolBarButton2("Standard",2,3);	// �ָ������εĴ���
	document.all.WebOffice1.SetToolBarButton2("Standard",3,3);	// �ָ������εı�����
	document.all.WebOffice1.SetKeyCtrl(595,0,0);				// �ָ������ݼ�(Ctrl+S) 
    document.all.WebOffice1.SetKeyCtrl(592,0,0);				// �ָ���ӡ��ݼ�(Ctrl+P)
	document.all.WebOffice1.Close();
}

// ����װ���ĵ�
function LoadDocument(){
	unloadFile();					// �ر�
	WebOffice1_NotifyCtrlReady();	// �ٴ�
}
// ����ģ����ǩ��������ģ���ļ�
function SaveDoc() {
	// �˴���� SavaBookMarks()
	
	var returnValue;
	
	document.all.WebOffice1.HttpInit();			//��ʼ��Http����
	// �����Ӧ��PostԪ�� 
	document.all.WebOffice1.HttpAddPostString("TemplateId","<%=TemplateId%>");
	document.all.WebOffice1.HttpAddPostString("RecordID", "<%=RecordID%>");
	document.all.WebOffice1.HttpAddPostString("FileName",webform.FileName.value);
	document.all.WebOffice1.HttpAddPostString("Descript",webform.Descript.value);	
	document.all.WebOffice1.HttpAddPostString("FileType", "<%=FileType%>");
	document.all.WebOffice1.HttpAddPostCurrFile("FileBody","");		// �ϴ��ļ�

	// �ϴ��ļ����������Ƿ�ɹ�
	returnValue = document.all.WebOffice1.HttpPost("<%=serverHome%>/SaveTemplate.jsp");	

	if("succeed" == returnValue){
		alert("�ļ��ϴ��ɹ�");	
		alert("000000"+returnValue);
	} else  {
		alert("�ļ��ϴ�ʧ��");
			
				alert("000000"+returnValue);
	}
}
// �����ĵ�������
function SaveToLocal() {
	document.all.WebOffice1.ShowDialog(10000)
}
// ��ӡ�ĵ�
function PrintDoc() {
	document.all.WebOffice1.ShowDialog(88);	
}
// �򿪱����ĵ�
function OpenLocalFile() {
	document.all.WebOffice1.LoadOriginalFile("open", "<%=FileType%>");	
}
// ���嵱ǰ�ĵ���ǩ
function DefineMarks() {
	document.all.WebOffice1.BookMarkOpt("<%=serverHome%>/ListBookMarks.jsp",1);
}
// ���ģ��
function FillBookMarks(){
	document.all.WebOffice1.BookMarkOpt("<%=serverHome%>/FillBookMarks.jsp",2);
}
</script>

<SCRIPT LANGUAGE=javascript FOR=WebOffice1 EVENT=NotifyCtrlReady>
<!--
 WebOffice1_NotifyCtrlReady()			// ��װ����Weboffice(ִ��<object>...</object>)�ؼ���ִ�� "WebOffice1_NotifyCtrlReady"����
//-->
</SCRIPT>

</head>
<body bgcolor="#ffffff" onUnload="unloadFile()"> 

<form name="webform" method="post" action="TemplateSave.jsp" onSubmit="return SaveDoc();">
  <table border=0  cellspacing='0' cellpadding='0' width=100% height=100% align=center class=TBStyle>
<tr>
  <td align="right" class="TDTitleStyle" width=64>ģ����</td>
  <td class="TDStyle"><input type="text" name="FileName" value="<%=mFileName%>" class="IptStyle" ></td>
  <td class="TDStyle"> ע�⣺ֻ��ѡ�񡶱��桷�������Ĳ�������Ч��</td>
</tr>

<tr>
  <td align=right class="downSide" width=64>˵  ��</td>
  <td class="downSide"><input type="text" name="Descript" value="<%=mDescript%>" class="IptStyle" ></td>
  <td class="downSide"><input name="submit" type=submit value="�ϴ��ļ�" >
    <input name="reset" type=reset value="  ���  ">
    <input name="button" type=button onClick="javascript:window.location='TemplateList.jsp'" value="  ����  " ></td>
</tr>

<tr>
  <!--td align=right valign=top  class="TDTitleStyle" width=64>����</td-->
  <td align=right valign=top  class="TDTitleStyle" width=64 height=90% >
                 <input type=button value="��ӡ�ĵ�"  onclick="PrintDoc()">
                 <input type=button value="�����ǩ"  onclick="DefineMarks()">
                 <input type=button value="���ģ��"  onclick="FillBookMarks()">
                 <input type=button value="�ص��ĵ�"  onclick="LoadDocument()">
                 <input type=button value="���ļ�"  onclick="OpenLocalFile()">
                 <input type=button value="�����ļ�"  onclick="SaveToLocal()">  </td>

  <td  height=90% colspan="2" class="TDStyle">
        <table border=0 cellspacing='0' cellpadding='0' width='100%' height='100%' >
        <tr>
          <td valign="top" bgcolor="menu">
         
            <OBJECT id="WebOffice" width="100%" height="100%" classid="clsid:23739A7E-5741-4D1C-88D5-D50B18F7C347" codebase="<%=mClientUrl%>" >
            </OBJECT>
-->
		
		<!-- ------------=== װ��Weboffice�ؼ������û��װ��ʾ���� ===-------------------- -->
        <object id=WebOffice1 height=560 width="100%" style="LEFT: 0px; TOP: 0px" 
			  classid="clsid:E77E049B-23FC-4DB8-B756-60529A35FAD5" codebase=WebOffice.ocx#V3,0,0,0>
          <param name="_ExtentX" value="6350">
          <param name="_ExtentY" value="6350">
          <param name="BorderColor" value="-2147483632">
          <param name="BackColor" value="-2147483643">
          <param name="ForeColor" value="-2147483640">
          <param name="TitlebarColor" value="-2147483635">
          <param name="TitlebarTextColor" value="-2147483634">
          <param name="BorderStyle" value="1">
          <param name="Titlebar" value="1">
          <param name="Toolbars" value="1">
          <param name="Menubar" value="1">
        </object>	    

<!-- ---------------------------=== ���� ===----------------------------------- -->          </td>
        </tr>
        <tr>
          <td bgcolor=menu height='20'>
		<div id=StatusBar>״̬��</div>          </td>
        </tr>
        </table>  </td>
</tr>
</table>

</form>
<%@ include file="closeDB.jsp" %>
</body>
</html>
