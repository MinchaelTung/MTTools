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
		out.print("��������ַ�����������ֶ����� <A href='index.jsp'>��ҳ</A>");
		return;
	}
	//out.print(IPAddr);

%>
<title>�༭����</title>
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
<!-- --------------------=== ����Weboffice��ʼ������ ===--------------------- -->
<SCRIPT LANGUAGE=javascript FOR=WebOffice1 EVENT=NotifyCtrlReady>
<!--
	WebOffice1_NotifyCtrlReady()			// ��װ����Weboffice(ִ��<object>...</object>)�ؼ���ִ�� "WebOffice1_NotifyCtrlReady"����
//-->
</SCRIPT>
<SCRIPT LANGUAGE=javascript>
<!--
// ---------------------=== �ؼ���ʼ��WebOffice���� ===---------------------- //
function WebOffice1_NotifyCtrlReady() {
document.all.WebOffice1.OptionFlag |= 128;
<%	if(!id.equals("0")) {	%>		// װ���Ѵ��ڵ��ĵ�
		document.all.WebOffice1.LoadOriginalFile("<%=IPAddr%>/getdoc.jsp?id=<%=id%>", "<%=doctype%>");

<%	} else if(!modDocId.equals("0")){		%>	//װ��ģ��
		document.all.WebOffice1.LoadOriginalFile("<%=IPAddr%>/getModDoc.jsp?modId=<%=modDocId%>", "<%=doctype%>");
	
<%  }else{	%>	// �½��ĵ�
		document.all.WebOffice1.LoadOriginalFile("", "<%=doctype%>");
<%	}				%>
}
// ---------------------== �ر�ҳ��ʱ���ô˺������ر��ļ� ==---------------------- //
function window_onunload() {
	document.all.WebOffice1.Close();
}
// ---------------------=== �½��ĵ� ===---------------------- //
function newDoc() {
		var doctype=document.all.doctype.value;
		document.all.WebOffice1.LoadOriginalFile("", doctype);
}
// ---------------------=== ��ʾ��ӡ�Ի��� ===---------------------- //
function showPrintDialog(){
		document.all.WebOffice1.PrintDoc(1);
}
// ---------------------=== ֱ�Ӵ�ӡ ===---------------------- //
function zhiPrint(){
		document.all.WebOffice1.PrintDoc(0);
}
// ---------------------== �ر�ҳ��ʱ���ô˺������ر��ļ� ==---------------------- //
function window_onunload() {
	document.all.WebOffice1.Close();
}
 
// ---------------------------== ����ĵ����� ==---------------------------------- //
function UnProtect() {
	document.all.WebOffice1.ProtectDoc(0,1, document.all.docPwd.value);
}

// ---------------------------== �����ĵ����� ==---------------------------------- //
function ProtectFull() {
	document.all.WebOffice1.ProtectDoc(1,1, document.all.docPwd.value);
}
// ---------------------------== ��ֹ��ӡ ==---------------------------------- //
function notPrint() {
		document.all.WebOffice1.SetSecurity(0x01); 
}
// ---------------------------== �ָ������ӡ ==---------------------------------- //
function okPrint() {
		document.all.WebOffice1.SetSecurity(0x01 + 0x8000);

}
// ---------------------------== ��ֹ���� ==---------------------------------- //
function notSave() {
		document.all.WebOffice1.SetSecurity(0x02); 

}
// ---------------------------== �ָ������� ==---------------------------------- //
function okSave() {
		document.all.WebOffice1.SetSecurity(0x02 + 0x8000);

}
// ---------------------------== ��ֹ���� ==---------------------------------- //
function notCopy() {
		document.all.WebOffice1.SetSecurity(0x04); 
}
// ---------------------------== �ָ������� ==---------------------------------- //
function okCopy() {
		document.all.WebOffice1.SetSecurity(0x04 + 0x8000); 

}
// ---------------------------== ��ֹ�϶� ==---------------------------------- //
function notDrag() {
		document.all.WebOffice1.SetSecurity(0x08); 
}
// ---------------------------== �ָ��϶� ==---------------------------------- //
function okDrag() {
		document.all.WebOffice1.SetSecurity(0x08 + 0x8000); 

}
// -----------------------------== �޶��ĵ� ==------------------------------------ //
function ProtectRevision() {
	document.all.WebOffice1.SetTrackRevisions(1) 
}

// -----------------------------== �����޶� ==------------------------------------ //
function UnShowRevisions() {
	document.all.WebOffice1.ShowRevisions(0);
}

// --------------------------== ��ʾ��ǰ�޶� ==---------------------------------- //
function ShowRevisions() {
	document.all.WebOffice1.ShowRevisions(1);

}

// -------------------------== ���ܵ�ǰ�����޶� ==------------------------------- //
function AcceptAllRevisions() {
 	document.all.WebOffice1.SetTrackRevisions(4);
}

// ---------------------------== ���õ�ǰ�����û� ==------------------------------- //
function SetUserName() {
	if(document.all.UserName.value ==""){
		alert("�û�������Ϊ��")
		document.all.UserName.focus();
		return false;
	}
 	document.all.WebOffice1.SetCurrUserName(document.all.UserName.value);
}
// -------------------------=== ������ǩ ===------------------------------ //
function addBookmark() {
	document.all.WebOffice1.BookMarkOpt("<%=IPAddr%>/ListBookMarks.jsp",1);
}
// ���ģ��
function FillBookMarks(){
	document.all.WebOffice1.BookMarkOpt("<%=IPAddr%>/FillBookMarks.jsp",2);
}
// -------------------------=== ������ǩ�׼Ӻ�ͷ ===------------------------------ //
function addRedHead() {
	document.all.WebOffice1.SetFieldValue("mark_1", "", "::ADDMARK::");			// �����ǩ
	document.all.WebOffice1.SetFieldValue("mark_1", "http://www.dianju.cn/weboffice/html/image/tmp1.doc", "::FILE::");
}
// -------------------------=== ������ǩ����ͼƬ ===------------------------------ //
function addImage() {
	document.all.WebOffice1.SetFieldValue("mark_1", "", "::ADDMARK::");			// �����ǩ
	document.all.WebOffice1.SetFieldValue("mark_1", "http://www.dianju.cn/weboffice/html/image/logo.gif", "::JPG::");
}


// -----------------------------== ������ҳ ==------------------------------------ //
function return_onclick() {
	document.all.WebOffice1.Close();
	window.location.href  = "index.jsp"
}
// �򿪱����ļ�
function docOpen() {
	document.all.WebOffice1.LoadOriginalFile("open", "doc");
}
// -----------------------------== �����ĵ� ==------------------------------------ //
function newSave() {
	document.all.WebOffice1.Save();
}
// -----------------------------== ���Ϊ�ĵ� ==------------------------------------ //
function SaveAsTo() {
	document.all.WebOffice1.ShowDialog(84);
}
// -----------------------------== ���ز˵� ==------------------------------------ //
function notMenu() {
	document.all.WebOffice1.SetToolBarButton2("Menu Bar",1,8);
}
// -----------------------------== ��ʾ�˵� ==------------------------------------ //
function okMenu() {
	document.all.WebOffice1.SetToolBarButton2("Menu Bar",1,11);
}
// -----------------------------== ���س��ù����� ==------------------------------------ //
function notOfter() {
	document.all.WebOffice1.SetToolBarButton2("Standard",1,8);
}
// -----------------------------== ��ʾ���ù����� ==------------------------------------ //
function okOfter() {
	document.all.WebOffice1.SetToolBarButton2("Standard",1,11);
}
// -----------------------------== ���ظ�ʽ������ ==------------------------------------ //
function notFormat() {
	document.all.WebOffice1.SetToolBarButton2("Formatting",1,8);
}
// -----------------------------== ��ʾ��ʽ������ ==------------------------------------ //
function okFormat() {
	document.all.WebOffice1.SetToolBarButton2("Formatting",1,11);
}    
// -----------------------------==�׺켰���ݽ��� ==------------------------------------ //
function linkRed() {
		window.open("http://www.dianju.cn/weboffice-demo/weboffice-cbd.htm");
} 
// -----------------------------== �ϴ��ĵ� ==------------------------------------ //
function SaveDoc() {
	var returnValue;
	 if(myform.DocTitle.value ==""){
		alert("���ⲻ��Ϊ��")
		myform.DocTitle.focus();
		return false;
	}
	if(myform.DocID.value ==""){
		alert("�ĺŲ���Ϊ��")
		myform.DocID.focus();
		return false;
	}
	
	document.all.WebOffice1.HttpInit();			//��ʼ��Http����
	// �����Ӧ��PostԪ�� 
	document.all.WebOffice1.HttpAddPostString("DocTitle", myform.DocTitle.value);
	document.all.WebOffice1.HttpAddPostString("DocID", myform.DocID.value);
	document.all.WebOffice1.HttpAddPostString("DocType","<%=doctype%>");
	document.all.WebOffice1.HttpAddPostCurrFile("DocContent","");		// �ϴ��ļ�

	returnValue = document.all.WebOffice1.HttpPost("<%=IPAddr%>/savedoc.jsp?id=<%=id%>");	// �ж��ϴ��Ƿ�ɹ�
	if("succeed" == returnValue){
		alert("�ļ��ϴ��ɹ�");	
	}else if("failed" == returnValue)
		alert("�ļ��ϴ�ʧ��")
	return_onclick(); 
}
function bToolBar_FullScreen_onclick() {
	try{
		document.all.WebOffice1.FullScreen = true;
	}catch(e){
		alert("�쳣\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
	}
}

function bToolBar_New_onclick() {
	try{
		var vCurItem = document.all.WebOffice1.HideMenuItem(0);
		//����vCurItem�жϵ�ǰ��ť�Ƿ���ʾ
		if(vCurItem & 0x01){
			document.all.WebOffice1.HideMenuItem(0x01); //Show it
		}else{
			document.all.WebOffice1.HideMenuItem(0x01 + 0x8000); //Hide it
		}
		
	}catch(e){
		alert("�쳣\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
	}
}

function bToolBar_Open_onclick() {
	try{
		var vCurItem = document.all.WebOffice1.HideMenuItem(0);
		//����vCurItem�жϵ�ǰ��ť�Ƿ���ʾ
		if(vCurItem & 0x02){
			document.all.WebOffice1.HideMenuItem(0x02); //Show it
		}else{
			document.all.WebOffice1.HideMenuItem(0x02 + 0x8000); //Hide it
		}
		
	}catch(e){
		alert("�쳣\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
	}
}

function bToolBar_Save_onclick() {
	try{
		var vCurItem = document.all.WebOffice1.HideMenuItem(0);
		//����vCurItem�жϵ�ǰ��ť�Ƿ���ʾ
		if(vCurItem & 0x04){
			document.all.WebOffice1.HideMenuItem(0x04); //Show it
		}else{
			document.all.WebOffice1.HideMenuItem(0x04 + 0x8000); //Hide it
		}
		
	}catch(e){
		alert("�쳣\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
	}
}

function bToolBar_onclick() {
	try{
		document.all.WebOffice1.ShowToolBar =  !document.all.WebOffice1.ShowToolBar;
	}catch(e){
		alert("�쳣\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
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
		el1.text  ="--��ѡ���û�--";   
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
���ܣ�����ʾ��ε���VBA�ӿ�
      WebOffice�ṩGetDocumentObject()�Ľӿڵ�������
      Word �������ǣ�MSWord::_Document
      Excel��������: MSExcel::_Workbook
      WPS  ��������: WPS::_Document
���ӣ�
1.ͨ��VBA��ȡ��ǰ�û����û���
  document.all.WebOffice1.GetDocumentObject().Application.UserName;
2.��ȡ�ĵ��ı���
	document.all.WebOffice1.GetDocumentObject().FullName;
**************************************************/

function TestVBA(){
	try{
		var vObj = document.all.WebOffice1.GetDocumentObject();
		if(!vObj){
			alert("��ȡ����ʧ�ܣ����ʵ���Ѿ����ĵ�");
			return false;
		}
		var vUserName;
		var vFullName;
		var vDocType = document.all.WebOffice1.DocType;
		if(11==vDocType){ //����WOrd�ļ�
				vUserName = vObj.Application.UserName;
				vFullName = vObj.Name;
		}else if(12==vDocType){  //����Excel�ļ�
				vUserName = vObj.Application.UserName;
				vFullName = vObj.Name;
		}else{
			alert("��֧�ֵ��ļ���ʽ");
			return false;
		}
		alert("VBA���Խ��\r\n�û���:"+vUserName+"\r\n�ĵ���:"+vFullName+"\r\n���Բ��մ�����������VBA����");
	}catch(e){
		alert("�쳣\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
	}
}

function AcceptRevision_onclick() {
	try{	
		var strUserName=document.all.UserList.value;
		document.all.WebOffice1.AcceptRevision(strUserName ,0)	
	}catch(e){
		alert("�쳣\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
	}


}
function unAcceptRevision_onclick() {
	try{	
		var strUserName=document.all.UserList.value;
		document.all.WebOffice1.AcceptRevision(strUserName ,1)	
	}catch(e){
		alert("�쳣\r\nError:"+e+"\r\nError Code:"+e.number+"\r\nError Des:"+e.description);
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
	�� ��:<input name="DocID" value=<%if(!id.equals("0")) { %>  <%=docID%><% } else { %> "DJ-001"<% }%> size="14" ><span class="STYLE1"> | </span>
	�� ��:<input name="DocTitle" value=<%if(!id.equals("0")) { %>  <%=docTitle%><% } else { %> "Test"<% } %> size="14">
	<span class="STYLE1"> |  </span><input type=button value=ˢ�� name="ReUserList" language=javascript onClick="return ReUserList_onclick()">
	<select name="UserList" size="1" id="UserList" language=javascript">
	<option>--��ѡ���û�--</option>
    </select>
	<input type="button" value="�����޶�" name="AcceptRevision" onClick="return AcceptRevision_onclick()" />
	<input type="button" value="�ܾ��޶�" name="anAcceptRevision" onClick="return unAcceptRevision_onclick()" />
	<span class="STYLE1"> | </span>
	<input name="button9" type="button"  onClick="return SaveDoc()" value="�ϴ���������" classs="rollout"> </td>
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
	  <font size="-1" >&nbsp;&nbsp;&nbsp;�ļ���ʽ: </font> 
      <!-- ---------------=== �ô��ļ���ʽ��value�������Զ��� ===------------------------- -->
		<select name="doctype" size="1" id="doctype">
          <option value="doc">Word</option>
          <option value="xls">Excel</option>
	  <option value="wps" selected>wps</option>
      </select>
		<input name="CreateFile" type="button" id="CreateFile" value="�½��ĵ�" onClick="newDoc()">
            <span class="STYLE1">|</span>
      <input name="button" type="button" onClick="return docOpen()" value="�򿪱����ļ�" />
    <span class="STYLE1">|  </span><font size="-1">��ӡ:</font>
	<input name="CreateFile" type="button" id="showPrint" value="��ʾ�Ի���" onClick="showPrintDialog()">
	<input name="CreateFile" type="button" id="zhiPrint" value="ֱ�Ӵ�ӡ" onClick="zhiPrint()">
	<span class="STYLE1">| </span><font size="-1" >����:</font>
	<input name="CreateFile2" type="button" id="CreateFile2" value="����" onClick="newSave()" />
	<input name="CreateFile3" type="button" id="CreateFile3" value="���Ϊ" onClick="SaveAsTo()" />	<span class="STYLE1">| </span>
	<input name="CreateFile3" type="button" id="CreateFile3" value="�׺켰���ݽ���" onClick="linkRed()"  /><span class="STYLE1"> | </span>
	<input name="CreateFile3" type="button" id="CreateFile3" value="VBA����" onClick="TestVBA()"  />
	<br /></td>
    <td></td>
  </tr>
  
  <tr>
    <td height="63">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide">
     <font size="-2" >�û�����</font>
	  <br />
        <input name="UserName" type="text" value="Test" style="width:74px;" maxlength="10" />
        <br />
        <input name="button2" type="button" class="btn"  onclick="return SetUserName()" value="�����û�" />    </td>
  <td colspan="2" rowspan="12" valign="top" class="leftBorder">
      <!-- -----------------------------== װ��weboffice�ؼ� ==--------------------------------- -->
<script src="LoadWebOffice.js"></script></br>
	  <input name="bToolBar" type="button" class="btn" value="������(����/��ʾ)"  LANGUAGE=javascript onClick="return bToolBar_onclick()">
     <input name="bToolBar_New" type="button" class="btn" value="�½��ĵ�(����/��ʾ)"   LANGUAGE=javascript onClick="return bToolBar_New_onclick()">
     <input name="bToolBar_Open" type="button" class="btn" value="���ĵ�(����/��ʾ)"  LANGUAGE=javascript onClick="return bToolBar_Open_onclick()">
     <input name="bToolBar_Save" type="button" class="btn" value="�����ĵ�(����/��ʾ)" LANGUAGE=javascript onClick="return bToolBar_Save_onclick()">
     <input name="bToolBar_FullScreen" type="button" class="btn" value="ȫ  ��"  LANGUAGE=javascript onClick="return bToolBar_FullScreen_onclick()">	    
    <!-- --------------------------------== ����װ�ؿؼ� ==----------------------------------- -->  </td>
  </tr>
  <tr>
    <td height="69">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide">
				<font size="-2">�������룺</font><br>
              <input name="docPwd" type="text" value="Password" style="width:74px;" maxlength="10">
              <br>
              <input type="button" class="btn" value="�����ĵ�" onClick="return ProtectFull()">
              <br />
    <input name="button3" type="button" class="btn" onClick="return UnProtect()" value="�������" /></td>
  </tr>
  <tr>
    <td height="44">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide"><input type="button" class="btn" value="���ô�ӡ" onClick="return notPrint()">
      <br />
      <input name="button3" type="button" class="btn" onClick="return okPrint()" value="���ô�ӡ" /></td>
  </tr>
  <tr>
    <td height="44">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide"><input name="button10" type="button" class="btn" onClick="return notSave()" value="��ֹ����" />
      <br />
      <input name="button32" type="button" class="btn" onClick="return okSave()" value="������" /></td>
  </tr>
  <tr>
    <td height="44">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide"><input name="button11" type="button" class="btn" onClick="return notCopy()" value="��ֹ����" />
      <br />
      <input name="button33" type="button" class="btn" onClick="return okCopy()" value="������" /></td>
  </tr>
    <tr>
    <td height="44">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide"><input name="but11" type="button" class="btn" onClick="return notDrag()" value="��ֹ�϶�" />
      <br />
      <input name="but33" type="button" class="btn" onClick="return okDrag()" value="�����϶�" /></td>
  </tr>
  <tr>
    <td height="86">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide"><input name="button4"  type="button" class="btn" onClick="return ProtectRevision()" value="�޶��ĵ�" />
      <br />
      <input name="button5" type="button" class="btn" onClick="return ShowRevisions()" value="��ʾ�޶�" />
      <br />
      <input name="button6" type="button" class="btn" onClick="return UnShowRevisions()" value="�����޶�" />
      <br />
      <input name="button7" type="button" class="btn" onClick="return AcceptAllRevisions()" value="�����޶�" /></td>
  </tr>
  <tr>
    <td height="23">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide"><input name="button8"  type="button" class="btn" onClick="return addBookmark()" value="������ǩ" />
  <input type=button value="���ģ��"  class="btn"  onclick="FillBookMarks()">    	</td>
  </tr>
  <!--<tr>
    <td height="23">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide"><span class="downSide">
      <input name="button9"  type="button" class="btn" onclick="return addRedHead()" value="�׼Ӻ�ͷ" />
    </span></td>
  </tr>-->
  <tr>
    <td height="23">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide"><span class="downSide">
      <input name="button92"  type="button" class="btn" onClick="return addImage()" value="����ͼƬ" />
    </span></td>
  </tr>
  <tr>
    <td height="126">&nbsp;</td>
    <td colspan="2" align="center" valign="top" class="downSide">
      <input name="button12" type="button" class="btn" onClick="return notMenu()" value="���ز˵�" />
	   <input name="button12" type="button" class="btn" onClick="return okMenu()" value="��ʾ�˵�" />
	    <input name="button12" type="button" class="btn" onClick="return notOfter()" value="���س���" />
		 <input name="button12" type="button" class="btn" onClick="return okOfter()" value="��ʾ����" />
		  <input name="button12" type="button" class="btn" onClick="return notFormat()" value="���ظ�ʽ" />
		   <input name="button12" type="button" class="btn" onClick="return okFormat()" value="��ʾ��ʽ" /></td>
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
