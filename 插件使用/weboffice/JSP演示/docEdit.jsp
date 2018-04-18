<%@ page contentType="text/html;charset=GB2312" %>
<%@ include file="conn.jsp" %>
<html>

<%
	String id = "0";
	String docID = "";
	String docTitle = "";
	String doctype = "doc";
	String modDocId="";
	String sql = "";
	if(request.getParameter("modId")!=null && !request.getParameter("modId").equals("")){
		modDocId=request.getParameter("modId");
	}
	if (request.getParameter("id") != null && !request.getParameter("id").equals("")) {
		id = request.getParameter("id")	;
	}
	doctype = request.getParameter("doctype");

	if(!id.equals("0")) {
		sql = "SELECT * from doc where id = " + id ; 
		rs = stmt.executeQuery(sql);
		if(rs.next()){
			doctype  = rs.getString("doctype");
			docID    = rs.getString("docID");
			docTitle = rs.getString("docTitle");
		}
	} 

	String IPAddr = "";
	IPAddr = (String)session.getAttribute("IPAddr");

	if(IPAddr == null && IPAddr.equals("")){
		out.print("服务器地址设置有误，请手动返回 <A href='index.jsp'>首页</A>");
		return;
	}
	//out.print(IPAddr);

%>
<title>编辑正文</title>
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
<!-- --------------------=== 调用Weboffice初始化方法 ===--------------------- -->
<SCRIPT LANGUAGE=javascript FOR=WebOffice1 EVENT=NotifyCtrlReady>
<!--
	WebOffice1_NotifyCtrlReady()			// 在装载完Weboffice(执行<object>...</object>)控件后执行 "WebOffice1_NotifyCtrlReady"方法
//-->
</SCRIPT>
<SCRIPT LANGUAGE=javascript>
<!--
// ---------------------=== 控件初始化WebOffice方法 ===---------------------- //
function WebOffice1_NotifyCtrlReady() {
document.all.WebOffice1.OptionFlag |= 128;
<%	if(!id.equals("0")) {	%>		// 装载已存在的文档
		document.all.WebOffice1.LoadOriginalFile("<%=IPAddr%>/getdoc.jsp?id=<%=id%>", "<%=doctype%>");

<%	} else if(!modDocId.equals("0")){		%>	//装载模板
		document.all.WebOffice1.LoadOriginalFile("<%=IPAddr%>/getModDoc.jsp?modId=<%=modDocId%>", "<%=doctype%>");
	
<%  }else{	%>	// 新建文档
		document.all.WebOffice1.LoadOriginalFile("", "<%=doctype%>");
<%	}				%>
}
// ---------------------== 关闭页面时调用此函数，关闭文件 ==---------------------- //
function window_onunload() {
	document.all.WebOffice1.Close();
}
// ---------------------=== 新建文档 ===---------------------- //
function newDoc() {
		var doctype=document.all.doctype.value;
		document.all.WebOffice1.LoadOriginalFile("", doctype);
}
// ---------------------=== 显示打印对话框 ===---------------------- //
function showPrintDialog(){
		document.all.WebOffice1.PrintDoc(1);
}
// ---------------------=== 直接打印 ===---------------------- //
function zhiPrint(){
		document.all.WebOffice1.PrintDoc(0);
}
// ---------------------== 关闭页面时调用此函数，关闭文件 ==---------------------- //
function window_onunload() {
	document.all.WebOffice1.Close();
}
 
// ---------------------------== 解除文档保护 ==---------------------------------- //
function UnProtect() {
	document.all.WebOffice1.ProtectDoc(0,1, document.all.docPwd.value);
}

// ---------------------------== 设置文档保护 ==---------------------------------- //
function ProtectFull() {
	document.all.WebOffice1.ProtectDoc(1,1, document.all.docPwd.value);
}
// ---------------------------== 禁止打印 ==---------------------------------- //
function notPrint() {
		document.all.WebOffice1.SetSecurity(0x01); 
}
// ---------------------------== 恢复允许打印 ==---------------------------------- //
function okPrint() {
		document.all.WebOffice1.SetSecurity(0x01 + 0x8000);

}
// ---------------------------== 禁止保存 ==---------------------------------- //
function notSave() {
		document.all.WebOffice1.SetSecurity(0x02); 

}
// ---------------------------== 恢复允许保存 ==---------------------------------- //
function okSave() {
		document.all.WebOffice1.SetSecurity(0x02 + 0x8000);

}
// ---------------------------== 禁止复制 ==---------------------------------- //
function notCopy() {
		document.all.WebOffice1.SetSecurity(0x04); 
}
// ---------------------------== 恢复允许复制 ==---------------------------------- //
function okCopy() {
		document.all.WebOffice1.SetSecurity(0x04 + 0x8000); 

}
// ---------------------------== 禁止拖动 ==---------------------------------- //
function notDrag() {
		document.all.WebOffice1.SetSecurity(0x08); 
}
// ---------------------------== 恢复拖动 ==---------------------------------- //
function okDrag() {
		document.all.WebOffice1.SetSecurity(0x08 + 0x8000); 

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
	if(document.all.UserName.value ==""){
		alert("用户名不可为空")
		document.all.UserName.focus();
		return false;
	}
 	document.all.WebOffice1.SetCurrUserName(document.all.UserName.value);
}
// -------------------------=== 设置书签 ===------------------------------ //
function addBookmark() {
	document.all.WebOffice1.BookMarkOpt("<%=IPAddr%>/ListBookMarks.jsp",1);
}
// 填充模板
function FillBookMarks(){
	document.all.WebOffice1.BookMarkOpt("<%=IPAddr%>/FillBookMarks.jsp",2);
}
// -------------------------=== 设置书签套加红头 ===------------------------------ //
function addRedHead() {
	document.all.WebOffice1.SetFieldValue("mark_1", "", "::ADDMARK::");			// 添加书签
	document.all.WebOffice1.SetFieldValue("mark_1", "http://www.dianju.cn/weboffice/html/image/tmp1.doc", "::FILE::");
}
// -------------------------=== 设置书签插入图片 ===------------------------------ //
function addImage() {
	document.all.WebOffice1.SetFieldValue("mark_1", "", "::ADDMARK::");			// 添加书签
	document.all.WebOffice1.SetFieldValue("mark_1", "http://www.dianju.cn/weboffice/html/image/logo.gif", "::JPG::");
}


// -----------------------------== 返回首页 ==------------------------------------ //
function return_onclick() {
	document.all.WebOffice1.Close();
	window.location.href  = "index.jsp"
}
// 打开本地文件
function docOpen() {
	document.all.WebOffice1.LoadOriginalFile("open", "doc");
}
// -----------------------------== 保存文档 ==------------------------------------ //
function newSave() {
	document.all.WebOffice1.Save();
}
// -----------------------------== 另存为文档 ==------------------------------------ //
function SaveAsTo() {
	document.all.WebOffice1.ShowDialog(84);
}
// -----------------------------== 隐藏菜单 ==------------------------------------ //
function notMenu() {
	document.all.WebOffice1.SetToolBarButton2("Menu Bar",1,8);
}
// -----------------------------== 显示菜单 ==------------------------------------ //
function okMenu() {
	document.all.WebOffice1.SetToolBarButton2("Menu Bar",1,11);
}
// -----------------------------== 隐藏常用工具栏 ==------------------------------------ //
function notOfter() {
	document.all.WebOffice1.SetToolBarButton2("Standard",1,8);
}
// -----------------------------== 显示常用工具栏 ==------------------------------------ //
function okOfter() {
	document.all.WebOffice1.SetToolBarButton2("Standard",1,11);
}
// -----------------------------== 隐藏格式工具栏 ==------------------------------------ //
function notFormat() {
	document.all.WebOffice1.SetToolBarButton2("Formatting",1,8);
}
// -----------------------------== 显示格式工具栏 ==------------------------------------ //
function okFormat() {
	document.all.WebOffice1.SetToolBarButton2("Formatting",1,11);
}    
// -----------------------------==套红及数据交互 ==------------------------------------ //
function linkRed() {
		window.open("http://www.dianju.cn/weboffice-demo/weboffice-cbd.htm");
} 
// -----------------------------== 上传文档 ==------------------------------------ //
function SaveDoc() {
	var returnValue;
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

	returnValue = document.all.WebOffice1.HttpPost("<%=IPAddr%>/savedoc.jsp?id=<%=id%>");	// 判断上传是否成功
	if("succeed" == returnValue){
		alert("文件上传成功");	
	}else if("failed" == returnValue)
		alert("文件上传失败")
	return_onclick(); 
}
function bToolBar_FullScreen_onclick() {
	try{
		document.all.WebOffice1.FullScreen = true;
	}catch(e){
		alert("异常\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
	}
}

function bToolBar_New_onclick() {
	try{
		var vCurItem = document.all.WebOffice1.HideMenuItem(0);
		//根据vCurItem判断当前按钮是否显示
		if(vCurItem & 0x01){
			document.all.WebOffice1.HideMenuItem(0x01); //Show it
		}else{
			document.all.WebOffice1.HideMenuItem(0x01 + 0x8000); //Hide it
		}
		
	}catch(e){
		alert("异常\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
	}
}

function bToolBar_Open_onclick() {
	try{
		var vCurItem = document.all.WebOffice1.HideMenuItem(0);
		//根据vCurItem判断当前按钮是否显示
		if(vCurItem & 0x02){
			document.all.WebOffice1.HideMenuItem(0x02); //Show it
		}else{
			document.all.WebOffice1.HideMenuItem(0x02 + 0x8000); //Hide it
		}
		
	}catch(e){
		alert("异常\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
	}
}

function bToolBar_Save_onclick() {
	try{
		var vCurItem = document.all.WebOffice1.HideMenuItem(0);
		//根据vCurItem判断当前按钮是否显示
		if(vCurItem & 0x04){
			document.all.WebOffice1.HideMenuItem(0x04); //Show it
		}else{
			document.all.WebOffice1.HideMenuItem(0x04 + 0x8000); //Hide it
		}
		
	}catch(e){
		alert("异常\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
	}
}

function bToolBar_onclick() {
	try{
		document.all.WebOffice1.ShowToolBar =  !document.all.WebOffice1.ShowToolBar;
	}catch(e){
		alert("异常\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
	}
}
function ReUserList_onclick()
{
	var vCount = document.all.WebOffice1.GetRevCount();
//1.Remove All
 	var selLen= document.all.UserList.length;
	for (i=0;i<selLen;i++){
		document.all.UserList.remove(0);
	}
//2.ReLoad All	 
	var vCount;
	vCount = document.all.WebOffice1.GetRevCount();
		var  el1   =   document.createElement("OPTION");   
		el1.text  ="--请选择用户--";   
		document.all.UserList.options.add(el1);	 
	
	for(var i=1;i<=vCount;i++){
		var strUserName=document.all.WebOffice1.GetRevInfo(i,0);
		var  el   =   document.createElement("OPTION");   
		el.text   =   strUserName;   
		el.value   =   strUserName;   
		document.all.UserList.options.add(el);	   
	}
}

/*************************************************
功能：在演示如何调用VBA接口
      WebOffice提供GetDocumentObject()的接口导出对象
      Word 导出的是：MSWord::_Document
      Excel导出的是: MSExcel::_Workbook
      WPS  导出的是: WPS::_Document
列子：
1.通过VBA获取当前用户的用户名
  document.all.WebOffice1.GetDocumentObject().Application.UserName;
2.获取文档的标题
	document.all.WebOffice1.GetDocumentObject().FullName;
**************************************************/

function TestVBA(){
	try{
		var vObj = document.all.WebOffice1.GetDocumentObject();
		if(!vObj){
			alert("获取对象失败，请核实您已经打开文档");
			return false;
		}
		var vUserName;
		var vFullName;
		var vDocType = document.all.WebOffice1.DocType;
		if(11==vDocType){ //对于WOrd文件
				vUserName = vObj.Application.UserName;
				vFullName = vObj.Name;
		}else if(12==vDocType){  //对于Excel文件
				vUserName = vObj.Application.UserName;
				vFullName = vObj.Name;
		}else{
			alert("不支持的文件格式");
			return false;
		}
		alert("VBA测试结果\r\n用户名:"+vUserName+"\r\n文档名:"+vFullName+"\r\n可以参照代码调用任意的VBA功能");
	}catch(e){
		alert("异常\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
	}
}

function AcceptRevision_onclick() {
	try{	
		var strUserName=document.all.UserList.value;
		document.all.WebOffice1.AcceptRevision(strUserName ,0)	
	}catch(e){
		alert("异常\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
	}


}
function unAcceptRevision_onclick() {
	try{	
		var strUserName=document.all.UserList.value;
		document.all.WebOffice1.AcceptRevision(strUserName ,1)	
	}catch(e){
		alert("异常\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
	}

}
//-->
</SCRIPT>
</head>
<body  onunload="return window_onunload()">
<form name="myform">
<table width="1123" border="0" cellpadding="0" cellspacing="0">
  <!--DWLayoutTable-->
  <tr>
    <td height="15" colspan="4" valign="top" bgcolor="#e5ecf9" class="topSide">&nbsp;&nbsp;
	文 号:<input name="DocID" value=<%if(!id.equals("0")) { %>  <%=docID%><% } else { %> "DJ-001"<% }%> size="14" ><span class="STYLE1"> | </span>
	标 题:<input name="DocTitle" value=<%if(!id.equals("0")) { %>  <%=docTitle%><% } else { %> "Test"<% } %> size="14">
	<span class="STYLE1"> |  </span><input type=button value=刷新 name="ReUserList" language=javascript onClick="return ReUserList_onclick()">
	<select name="UserList" size="1" id="UserList" language=javascript">
	<option>--请选择用户--</option>
    </select>
	<input type="button" value="接受修订" name="AcceptRevision" onClick="return AcceptRevision_onclick()" />
	<input type="button" value="拒绝修订" name="anAcceptRevision" onClick="return unAcceptRevision_onclick()" />
	<span class="STYLE1"> | </span>
	<input name="button9" type="button"  onClick="return SaveDoc()" value="上传到服务器" classs="rollout"> </td>
    <td></td>
  </tr>
  <tr>
    <td width="20" height="21">&nbsp;</td>
    <td width="44">&nbsp;</td>
    <td width="85">&nbsp;</td>
    <td width="972">&nbsp;</td>
    <td></td>
  </tr>
  <tr>
    <td height="23" colspan="4" valign="top" class="downSide">
	  <font size="-1" >&nbsp;&nbsp;&nbsp;文件格式: </font> 
      <!-- ---------------=== 该处文件格式的value不可以自定义 ===------------------------- -->
		<select name="doctype" size="1" id="doctype">
          <option value="doc">Word</option>
          <option value="xls">Excel</option>
	  <option value="wps" selected>wps</option>
      </select>
		<input name="CreateFile" type="button" id="CreateFile" value="新建文档" onClick="newDoc()">
            <span class="STYLE1">|</span>
      <input name="button" type="button" onClick="return docOpen()" value="打开本地文件" />
    <span class="STYLE1">|  </span><font size="-1">打印:</font>
	<input name="CreateFile" type="button" id="showPrint" value="显示对话框" onClick="showPrintDialog()">
	<input name="CreateFile" type="button" id="zhiPrint" value="直接打印" onClick="zhiPrint()">
	<span class="STYLE1">| </span><font size="-1" >保存:</font>
	<input name="CreateFile2" type="button" id="CreateFile2" value="保存" onClick="newSave()" />
	<input name="CreateFile3" type="button" id="CreateFile3" value="另存为" onClick="SaveAsTo()" />	<span class="STYLE1">| </span>
	<input name="CreateFile3" type="button" id="CreateFile3" value="套红及数据交互" onClick="linkRed()"  /><span class="STYLE1"> | </span>
	<input name="CreateFile3" type="button" id="CreateFile3" value="VBA调用" onClick="TestVBA()"  />
	<br /></td>
    <td></td>
  </tr>
  
  <tr>
    <td height="63">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide">
     <font size="-2" >用户名：</font>
	  <br />
        <input name="UserName" type="text" value="Test" style="width:74px;" maxlength="10" />
        <br />
        <input name="button2" type="button" class="btn"  onclick="return SetUserName()" value="设置用户" />    </td>
  <td colspan="2" rowspan="12" valign="top" class="leftBorder">
      <!-- -----------------------------== 装载weboffice控件 ==--------------------------------- -->
<script src="LoadWebOffice.js"></script></br>
	  <input name="bToolBar" type="button" class="btn" value="工具栏(隐藏/显示)"  LANGUAGE=javascript onClick="return bToolBar_onclick()">
     <input name="bToolBar_New" type="button" class="btn" value="新建文档(隐藏/显示)"   LANGUAGE=javascript onClick="return bToolBar_New_onclick()">
     <input name="bToolBar_Open" type="button" class="btn" value="打开文档(隐藏/显示)"  LANGUAGE=javascript onClick="return bToolBar_Open_onclick()">
     <input name="bToolBar_Save" type="button" class="btn" value="保存文档(隐藏/显示)" LANGUAGE=javascript onClick="return bToolBar_Save_onclick()">
     <input name="bToolBar_FullScreen" type="button" class="btn" value="全  屏"  LANGUAGE=javascript onClick="return bToolBar_FullScreen_onclick()">	    
    <!-- --------------------------------== 结束装载控件 ==----------------------------------- -->  </td>
  </tr>
  <tr>
    <td height="69">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide">
				<font size="-2">保护密码：</font><br>
              <input name="docPwd" type="text" value="Password" style="width:74px;" maxlength="10">
              <br>
              <input type="button" class="btn" value="保护文档" onClick="return ProtectFull()">
              <br />
    <input name="button3" type="button" class="btn" onClick="return UnProtect()" value="解除保护" /></td>
  </tr>
  <tr>
    <td height="44">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide"><input type="button" class="btn" value="禁用打印" onClick="return notPrint()">
      <br />
      <input name="button3" type="button" class="btn" onClick="return okPrint()" value="启用打印" /></td>
  </tr>
  <tr>
    <td height="44">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide"><input name="button10" type="button" class="btn" onClick="return notSave()" value="禁止保存" />
      <br />
      <input name="button32" type="button" class="btn" onClick="return okSave()" value="允许保存" /></td>
  </tr>
  <tr>
    <td height="44">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide"><input name="button11" type="button" class="btn" onClick="return notCopy()" value="禁止复制" />
      <br />
      <input name="button33" type="button" class="btn" onClick="return okCopy()" value="允许复制" /></td>
  </tr>
    <tr>
    <td height="44">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide"><input name="but11" type="button" class="btn" onClick="return notDrag()" value="禁止拖动" />
      <br />
      <input name="but33" type="button" class="btn" onClick="return okDrag()" value="允许拖动" /></td>
  </tr>
  <tr>
    <td height="86">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide"><input name="button4"  type="button" class="btn" onClick="return ProtectRevision()" value="修订文档" />
      <br />
      <input name="button5" type="button" class="btn" onClick="return ShowRevisions()" value="显示修订" />
      <br />
      <input name="button6" type="button" class="btn" onClick="return UnShowRevisions()" value="隐藏修订" />
      <br />
      <input name="button7" type="button" class="btn" onClick="return AcceptAllRevisions()" value="接受修订" /></td>
  </tr>
  <tr>
    <td height="23">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide"><input name="button8"  type="button" class="btn" onClick="return addBookmark()" value="设置书签" />
  <input type=button value="填充模版"  class="btn"  onclick="FillBookMarks()">    	</td>
  </tr>
  <!--<tr>
    <td height="23">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide"><span class="downSide">
      <input name="button9"  type="button" class="btn" onclick="return addRedHead()" value="套加红头" />
    </span></td>
  </tr>-->
  <tr>
    <td height="23">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide"><span class="downSide">
      <input name="button92"  type="button" class="btn" onClick="return addImage()" value="插入图片" />
    </span></td>
  </tr>
  <tr>
    <td height="126">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide">
      <input name="button12" type="button" class="btn" onClick="return notMenu()" value="隐藏菜单" />
	   <input name="button12" type="button" class="btn" onClick="return okMenu()" value="显示菜单" />
	    <input name="button12" type="button" class="btn" onClick="return notOfter()" value="隐藏常用" />
		 <input name="button12" type="button" class="btn" onClick="return okOfter()" value="显示常用" />
		  <input name="button12" type="button" class="btn" onClick="return notFormat()" value="隐藏格式" />
		   <input name="button12" type="button" class="btn" onClick="return okFormat()" value="显示格式" /></td>
  </tr>
  <tr>
    <td height="200">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  
  <tr>
    <td height="35">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td></td>
  </tr>
</table>
</form>
</body>
</html>
