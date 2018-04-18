<%@ page contentType="text/html;charset=GB2312" %>
<%@ page import="java.io.*,java.text.*,java.util.*,java.sql.*,javax.servlet.*,javax.servlet.http.*" %>
<%@ include file="conn.jsp" %>
<%@ include file="config.jsp" %>
<%
  String mDescript="";		// 模板描述
  String mFileName="";		// 模板名称

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


  //打开数据库

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
				//取得唯一值(RecordID)
        java.util.Date dt=new java.util.Date();
        long lg=dt.getTime();
        Long ld=new Long(lg);
				//初始化值
        RecordID=ld.toString();
        mFileName="公文模版."+FileType;
        mDescript="发文公文模版";
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
<title>模板管理</title>
<script language=javascript>
// 初始化控件
function WebOffice1_NotifyCtrlReady() {
	<%
		if("new".equals(newOrUpdate)) {	// 新建文档
	%>
			document.all.WebOffice1.LoadOriginalFile("", "<%=FileType%>");
	<% 
		} else {	// 打开文件 
	%>
			document.all.WebOffice1.LoadOriginalFile("<%=serverHome%>/getModDoc.jsp?modId=<%=TemplateId%>", "<%=FileType%>");
	<%
		}
	%>

	document.all.WebOffice1.SetToolBarButton2("Menu Bar",1,1);	// 屏蔽菜单栏的文件菜单(Word)
	// document.all.WebOffice1.SetToolBarButton2("Worksheet Menu Bar",1,1);  // 屏蔽菜单栏的文件菜单(Excel)
	document.all.WebOffice1.SetKeyCtrl(595,-1,0);				// 屏蔽 保存快捷键(Ctrl+S) 
	document.all.WebOffice1.SetKeyCtrl(592,-1,0);				// 屏蔽 打印快捷键(Ctrl+P) 
	document.all.WebOffice1.SetToolBarButton2("Standard",1,1);	// 屏蔽新建项
	document.all.WebOffice1.SetToolBarButton2("Standard",2,1);	// 屏蔽打开项
	document.all.WebOffice1.SetToolBarButton2("Standard",3,1);	// 屏蔽保存项

}

// 关闭文档
function unloadFile() {
	document.all.WebOffice1.SetToolBarButton2("Menu Bar",1,4);	// 恢复被屏蔽的菜单栏文件菜单(word)
	document.all.WebOffice1.SetToolBarButton2("Standard",1,3);	// 恢复被屏蔽的新建项
	document.all.WebOffice1.SetToolBarButton2("Standard",2,3);	// 恢复被屏蔽的打开项
	document.all.WebOffice1.SetToolBarButton2("Standard",3,3);	// 恢复被屏蔽的保存项
	document.all.WebOffice1.SetKeyCtrl(595,0,0);				// 恢复保存快捷键(Ctrl+S) 
    document.all.WebOffice1.SetKeyCtrl(592,0,0);				// 恢复打印快捷键(Ctrl+P)
	document.all.WebOffice1.Close();
}

// 重新装载文档
function LoadDocument(){
	unloadFile();					// 关闭
	WebOffice1_NotifyCtrlReady();	// 再打开
}
// 保存模板书签关联，和模板文件
function SaveDoc() {
	// 此处添加 SavaBookMarks()
	
	var returnValue;
	
	document.all.WebOffice1.HttpInit();			//初始化Http引擎
	// 添加相应的Post元素 
	document.all.WebOffice1.HttpAddPostString("TemplateId","<%=TemplateId%>");
	document.all.WebOffice1.HttpAddPostString("RecordID", "<%=RecordID%>");
	document.all.WebOffice1.HttpAddPostString("FileName",webform.FileName.value);
	document.all.WebOffice1.HttpAddPostString("Descript",webform.Descript.value);	
	document.all.WebOffice1.HttpAddPostString("FileType", "<%=FileType%>");
	document.all.WebOffice1.HttpAddPostCurrFile("FileBody","");		// 上传文件

	// 上传文件，并返回是否成功
	returnValue = document.all.WebOffice1.HttpPost("<%=serverHome%>/SaveTemplate.jsp");	

	if("succeed" == returnValue){
		alert("文件上传成功");	
		alert("000000"+returnValue);
	} else  {
		alert("文件上传失败");
			
				alert("000000"+returnValue);
	}
}
// 保存文档至本地
function SaveToLocal() {
	document.all.WebOffice1.ShowDialog(10000)
}
// 打印文档
function PrintDoc() {
	document.all.WebOffice1.ShowDialog(88);	
}
// 打开本地文档
function OpenLocalFile() {
	document.all.WebOffice1.LoadOriginalFile("open", "<%=FileType%>");	
}
// 定义当前文档书签
function DefineMarks() {
	document.all.WebOffice1.BookMarkOpt("<%=serverHome%>/ListBookMarks.jsp",1);
}
// 填充模板
function FillBookMarks(){
	document.all.WebOffice1.BookMarkOpt("<%=serverHome%>/FillBookMarks.jsp",2);
}
</script>

<SCRIPT LANGUAGE=javascript FOR=WebOffice1 EVENT=NotifyCtrlReady>
<!--
 WebOffice1_NotifyCtrlReady()			// 在装载完Weboffice(执行<object>...</object>)控件后执行 "WebOffice1_NotifyCtrlReady"方法
//-->
</SCRIPT>

</head>
<body bgcolor="#ffffff" onUnload="unloadFile()"> 

<form name="webform" method="post" action="TemplateSave.jsp" onSubmit="return SaveDoc();">
  <table border=0  cellspacing='0' cellpadding='0' width=100% height=100% align=center class=TBStyle>
<tr>
  <td align="right" class="TDTitleStyle" width=64>模版名</td>
  <td class="TDStyle"><input type="text" name="FileName" value="<%=mFileName%>" class="IptStyle" ></td>
  <td class="TDStyle"> 注意：只有选择《保存》后，所做的操作才有效！</td>
</tr>

<tr>
  <td align=right class="downSide" width=64>说  明</td>
  <td class="downSide"><input type="text" name="Descript" value="<%=mDescript%>" class="IptStyle" ></td>
  <td class="downSide"><input name="submit" type=submit value="上传文件" >
    <input name="reset" type=reset value="  清除  ">
    <input name="button" type=button onClick="javascript:window.location='TemplateList.jsp'" value="  返回  " ></td>
</tr>

<tr>
  <!--td align=right valign=top  class="TDTitleStyle" width=64>内容</td-->
  <td align=right valign=top  class="TDTitleStyle" width=64 height=90% >
                 <input type=button value="打印文档"  onclick="PrintDoc()">
                 <input type=button value="定义标签"  onclick="DefineMarks()">
                 <input type=button value="填充模版"  onclick="FillBookMarks()">
                 <input type=button value="重调文档"  onclick="LoadDocument()">
                 <input type=button value="打开文件"  onclick="OpenLocalFile()">
                 <input type=button value="保存文件"  onclick="SaveToLocal()">  </td>

  <td  height=90% colspan="2" class="TDStyle">
        <table border=0 cellspacing='0' cellpadding='0' width='100%' height='100%' >
        <tr>
          <td valign="top" bgcolor="menu">
         
            <OBJECT id="WebOffice" width="100%" height="100%" classid="clsid:23739A7E-5741-4D1C-88D5-D50B18F7C347" codebase="<%=mClientUrl%>" >
            </OBJECT>
-->
		
		<!-- ------------=== 装载Weboffice控件，如果没安装提示下载 ===-------------------- -->
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

<!-- ---------------------------=== 结束 ===----------------------------------- -->          </td>
        </tr>
        <tr>
          <td bgcolor=menu height='20'>
		<div id=StatusBar>状态栏</div>          </td>
        </tr>
        </table>  </td>
</tr>
</table>

</form>
<%@ include file="closeDB.jsp" %>
</body>
</html>
