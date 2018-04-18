<!--#include file="./config.asp"-->
<html>
<head>

<%
	Dim id  
	Dim docID
	Dim docTitle
	Dim doctype  

	' 获取文档ID
	id = request.QueryString("id")			
	if id = "" Then
		id = 0
	End if

	' 获取文档类型
	doctype = Request.Form("doctype")
	if doctype = "" Then
		doctype = "doc"
	End if


	if(id <>0) Then
		Dim DBStr
		Dim BbsDB
		Dim Sql
		Dim rs
		DBStr="DBQ=" & Server.mappath("Des.mdb") & ";DefaultDir=;DRIVER={Microsoft Access Driver (*.mdb)};DriverId=25;FIL=MS Access;ImplicitCommitSync=Yes;MaxBufferSize=512;MaxScanRows=8;PageTimeout=5;SafeTransactions=0;Threads=3;UserCommitSync=Yes;"
		Set BbsDB =Server.CreateObject("ADODB.Connection")
		BbsDB.Open DBStr
		Sql="SELECT * from doc where id = "&id
		Set rs=Server.CreateObject("ADODB.RecordSet")
		rs.Open Sql,BbsDB,3
		if rs.RecordCount = 0 Then
			Response.Write "未发现指定记录"
			Response.End 
		End if
		doctype  = rs("doctype")
		docID    = rs("docID")
		docTitle = rs("docTitle")
	End if
%>
<title>编辑正文</title>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312">

<SCRIPT LANGUAGE=javascript>
<!--
// ---------------------=== 控件初始化WebOffice方法 ===---------------------- //
function WebOffice1_NotifyCtrlReady() {
<%	if(id <> 0) Then	%>		// 装载已存在的文档
		document.all.WebOffice1.LoadOriginalFile("<%=Application("appPath")%>/getdoc.asp?id=<%=id%>", "<%=doctype%>");

<%	else				%>		// 新建文档
		document.all.WebOffice1.LoadOriginalFile("", "<%=doctype%>");
<%	End if				%>
}


// ---------------------== 关闭页面时调用此函数，关闭文件 ==---------------------- //
function window_onunload() {
	document.all.WebOffice1.Close();
}
 
// ---------------------------== 解除文档保护 ==---------------------------------- //
function UnProtect() {
	document.all.WebOffice1.ProtectDoc(0,1, myform.docPwd.value);
}

// ---------------------------== 设置文档保护 ==---------------------------------- //
function ProtectFull() {
	document.all.WebOffice1.ProtectDoc(1,1, myform.docPwd.value);
}
// -----------------------------== 修订文档 ==------------------------------------ //
function ProtectRevision() {
	document.all.WebOffice1.SetTrackRevisions(1) 
}

// -----------------------------== 隐藏修订 ==------------------------------------ //
function UnShowRevisions() {
	document.all.WebOffice1.ShowRevisions(0);
}

// --------------------------== 显示当前修订 ==---------------------------------- //
function ShowRevisions() {
	document.all.WebOffice1.ShowRevisions(1);

}

// -------------------------== 接受当前所有修订 ==------------------------------- //
function AcceptAllRevisions() {
 	document.all.WebOffice1.SetTrackRevisions(4);
}

// ---------------------------== 设置当前操作用户 ==------------------------------- //
function SetUserName() {
	if(myform.UserName.value ==""){
		alert("用户名不可为空")
		myform.UserName.focus();
		return false;
	}
 	document.all.WebOffice1.SetCurrUserName(myform.UserName.value);
}

// -------------------------=== 设置书签套加红头 ===------------------------------ //
function addBookmark() {
	document.all.WebOffice1.SetFieldValue("mark_1", "北京点聚信息技术有限公司", "::ADDMARK::");			
}

// -------------------------=== 设置书签套加红头 ===------------------------------ //
function addRedHead() {
	document.all.WebOffice1.SetFieldValue("mark_1", "", "::ADDMARK::");			// 添加书签
	document.all.WebOffice1.SetFieldValue("mark_1", "<%=Application("appPath")%>/template/tmp1.doc", "::FILE::");
}

// -----------------------------== 返回首页 ==------------------------------------ //
function return_onclick() {
	document.all.WebOffice1.Close();
	window.location.href  = "index.asp"
}
// 打开本地文件
function docOpen() {
	if(myform.DocFilePath.value == "") {
		alert("文件路径不可以为空");
		myform.DocFilePath.focus();
		return false;
	}
	if( 0 == document.all.WebOffice1.LoadOriginalFile(myform.DocFilePath.value,"doc")){
		alert("文件打开失败，请检查路径是否合法");
		myform.DocFilePath.focus();
		return false;
	}	
}
// -----------------------------== 保存文档 ==------------------------------------ //
function SaveDoc() {
	 if(myform.DocTitle.value ==""){
		alert("标题不可为空")
		myform.DocTitle.focus();
		return false;
	}
	if(myform.DocID.value ==""){
		alert("文号不可为空")
		myform.DocID.focus();
		return false;
	}
	
	document.all.WebOffice1.HttpInit();			//初始化Http引擎
	// 添加相应的Post元素 
	document.all.WebOffice1.HttpAddPostString("DocTitle", myform.DocTitle.value);
	document.all.WebOffice1.HttpAddPostString("DocID", myform.DocID.value);
	document.all.WebOffice1.HttpAddPostString("DocType","<%=doctype%>");
	document.all.WebOffice1.HttpAddPostCurrFile("DocContent","");		// 上传文件

	if("OK" == document.all.WebOffice1.HttpPost("<%=Application("appPath")%>/savedoc.asp?id=<%=id%>")){
		alert("文件上传成功");	
	}else
		alert("文件上传失败")
	return_onclick(); 
}
//-->
</SCRIPT>
<!-- --------------------=== 调用Weboffice初始化方法 ===--------------------- -->
<SCRIPT LANGUAGE=javascript FOR=WebOffice1 EVENT=NotifyCtrlReady>
<!--
 WebOffice1_NotifyCtrlReady()			// 在装载完Weboffice(执行<object>...</object>)控件后执行 "WebOffice1_NotifyCtrlReady"方法
//-->
</SCRIPT>

<link href="style.css" rel="stylesheet" type="text/css">
</head>
<body leftMargin=0 topMargin=0 marginheight="0" marginwidth="0"  onunload="return window_onunload()">
<form name="myform">
  <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#3366cc">

    <tr bgcolor="#FFFFFF">
      <td width="78"  bgcolor="#FFFFFF" class="maintxt"><div align="center"><strong>文 号：</strong></div></td>
        <td width="138"><input name="DocID" value=<%if(id <>0) Then%>  <%=DocID%><%else %> "DJ-001"<%End if%> size="14" ></td>
		  <td width="395" rowspan="2" nowrap><input name="DocFilePath" type="file" size="14">
	      <input type="button" value="打开本地文件" onClick="return docOpen()"></td>
		  <td width="377" rowspan="2" bgcolor="#FFFFFF"><div align="center">
            <input type="button" classs="rollout" value="上传到服务器"  onClick="return SaveDoc()">　　
              <input type="button" value="  返  回  "  onClick="return return_onclick()"></div></td>
				</tr>

    <tr bgcolor="#FFFFFF">
      <td height="13" bgcolor="#FFFFFF" class="maintxt"><div align="center"><strong>标 题：</strong></div></td>
        <td><input name="DocTitle" value=<%if(id <>0) Then%>  <%=DocTitle%><%else %> "Test"<%End if%> size="14"></td>
	</tr>

    <tr bgcolor="#FFFFFF">
	  <td valign="top" bgcolor="#FFFFFF">
		<!-- -------------------=== Start 嵌套Table ===------------------------------- -->
		<table width="100%" height="289" border="0" cellpadding="0">

          <tr>
            <td nowrap class="maintxt">用户名：<br>
              <input name="UserName" type="text" value="Test" style="width:74px;" maxlength="10"><br>
                <input type="button" value="设置用户"  onclick="return SetUserName()" class="btn"></td>
		  </tr>

          <tr><td class="maintxt">
			<Hr color="#3366cc" size=1 >保护密码：<br>
              <input name="docPwd" type="text" value="Password" style="width:74px;" maxlength="10"><br>
			    <input type="button" class="btn" value="保护文档" onClick="return ProtectFull()"></td>
		  </tr>

          <tr><td><input type="button" class="btn" value="解除保护" onclick="return UnProtect()"></td></tr>

          <tr><td>
			<hr color="#3366cc" size=1>
			  <input  type="button" class="btn" value="修订文档" onclick="return ProtectRevision()"></td>
		  </tr>

          <tr><td><input type="button" class="btn" value="显示修订" onclick="return ShowRevisions()"></td></tr>
          <tr><td><input type="button" class="btn" value="隐藏修订" onclick="return UnShowRevisions()"></td></tr>
          <tr><td><input type="button" class="btn" value="接受修订" onclick="return AcceptAllRevisions()"></td></tr>

          <tr><td>
			<hr color="#3366cc" size=1>
             <input  type="button" class="btn" value="设置书签" onclick="return addBookmark()"></td>
	      </tr>

          <tr><td><input  type="button" class="btn" value="套加红头" onClick="return addRedHead()"></td></tr>

		  <tr><td>&nbsp;
              </td>
		  </tr>

		  <tr>
		    <td>&nbsp;</td>
		  </tr>
		  <tr>
		    <td>&nbsp;</td>
	      </tr>

        </table>
	    <!-- -------------------=== End 嵌套Table ===------------------------------- -->
	  </td>

	  <td colspan="3" valign="top">
		<!-- -----------------------------== 装载weboffice控件 ==--------------------------------- -->
			<script src="LoadWebOffice.js"></script>  
        <!-- --------------------------------== 结束装载控件 ==----------------------------------- -->
	  </td>
    </tr>
  </table>
</form>
<p>&nbsp;</p>
<p>

<%
	if(id <>0) Then
		rs.Close 
		BbsDB.Close 
	End if
%>
</p>
</body>
</html>
