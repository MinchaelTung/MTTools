<!--#include file="./config.asp"-->
<html>
<head>

<%
	Dim id  
	Dim docID
	Dim docTitle
	Dim doctype  

	' ��ȡ�ĵ�ID
	id = request.QueryString("id")			
	if id = "" Then
		id = 0
	End if

	' ��ȡ�ĵ�����
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
			Response.Write "δ����ָ����¼"
			Response.End 
		End if
		doctype  = rs("doctype")
		docID    = rs("docID")
		docTitle = rs("docTitle")
	End if
%>
<title>�༭����</title>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312">

<SCRIPT LANGUAGE=javascript>
<!--
// ---------------------=== �ؼ���ʼ��WebOffice���� ===---------------------- //
function WebOffice1_NotifyCtrlReady() {
<%	if(id <> 0) Then	%>		// װ���Ѵ��ڵ��ĵ�
		document.all.WebOffice1.LoadOriginalFile("<%=Application("appPath")%>/getdoc.asp?id=<%=id%>", "<%=doctype%>");

<%	else				%>		// �½��ĵ�
		document.all.WebOffice1.LoadOriginalFile("", "<%=doctype%>");
<%	End if				%>
}


// ---------------------== �ر�ҳ��ʱ���ô˺������ر��ļ� ==---------------------- //
function window_onunload() {
	document.all.WebOffice1.Close();
}
 
// ---------------------------== ����ĵ����� ==---------------------------------- //
function UnProtect() {
	document.all.WebOffice1.ProtectDoc(0,1, myform.docPwd.value);
}

// ---------------------------== �����ĵ����� ==---------------------------------- //
function ProtectFull() {
	document.all.WebOffice1.ProtectDoc(1,1, myform.docPwd.value);
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
	if(myform.UserName.value ==""){
		alert("�û�������Ϊ��")
		myform.UserName.focus();
		return false;
	}
 	document.all.WebOffice1.SetCurrUserName(myform.UserName.value);
}

// -------------------------=== ������ǩ�׼Ӻ�ͷ ===------------------------------ //
function addBookmark() {
	document.all.WebOffice1.SetFieldValue("mark_1", "���������Ϣ�������޹�˾", "::ADDMARK::");			
}

// -------------------------=== ������ǩ�׼Ӻ�ͷ ===------------------------------ //
function addRedHead() {
	document.all.WebOffice1.SetFieldValue("mark_1", "", "::ADDMARK::");			// �����ǩ
	document.all.WebOffice1.SetFieldValue("mark_1", "<%=Application("appPath")%>/template/tmp1.doc", "::FILE::");
}

// -----------------------------== ������ҳ ==------------------------------------ //
function return_onclick() {
	document.all.WebOffice1.Close();
	window.location.href  = "index.asp"
}
// �򿪱����ļ�
function docOpen() {
	if(myform.DocFilePath.value == "") {
		alert("�ļ�·��������Ϊ��");
		myform.DocFilePath.focus();
		return false;
	}
	if( 0 == document.all.WebOffice1.LoadOriginalFile(myform.DocFilePath.value,"doc")){
		alert("�ļ���ʧ�ܣ�����·���Ƿ�Ϸ�");
		myform.DocFilePath.focus();
		return false;
	}	
}
// -----------------------------== �����ĵ� ==------------------------------------ //
function SaveDoc() {
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

	if("OK" == document.all.WebOffice1.HttpPost("<%=Application("appPath")%>/savedoc.asp?id=<%=id%>")){
		alert("�ļ��ϴ��ɹ�");	
	}else
		alert("�ļ��ϴ�ʧ��")
	return_onclick(); 
}
//-->
</SCRIPT>
<!-- --------------------=== ����Weboffice��ʼ������ ===--------------------- -->
<SCRIPT LANGUAGE=javascript FOR=WebOffice1 EVENT=NotifyCtrlReady>
<!--
 WebOffice1_NotifyCtrlReady()			// ��װ����Weboffice(ִ��<object>...</object>)�ؼ���ִ�� "WebOffice1_NotifyCtrlReady"����
//-->
</SCRIPT>

<link href="style.css" rel="stylesheet" type="text/css">
</head>
<body leftMargin=0 topMargin=0 marginheight="0" marginwidth="0"  onunload="return window_onunload()">
<form name="myform">
  <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#3366cc">

    <tr bgcolor="#FFFFFF">
      <td width="78"  bgcolor="#FFFFFF" class="maintxt"><div align="center"><strong>�� �ţ�</strong></div></td>
        <td width="138"><input name="DocID" value=<%if(id <>0) Then%>  <%=DocID%><%else %> "DJ-001"<%End if%> size="14" ></td>
		  <td width="395" rowspan="2" nowrap><input name="DocFilePath" type="file" size="14">
	      <input type="button" value="�򿪱����ļ�" onClick="return docOpen()"></td>
		  <td width="377" rowspan="2" bgcolor="#FFFFFF"><div align="center">
            <input type="button" classs="rollout" value="�ϴ���������"  onClick="return SaveDoc()">����
              <input type="button" value="  ��  ��  "  onClick="return return_onclick()"></div></td>
				</tr>

    <tr bgcolor="#FFFFFF">
      <td height="13" bgcolor="#FFFFFF" class="maintxt"><div align="center"><strong>�� �⣺</strong></div></td>
        <td><input name="DocTitle" value=<%if(id <>0) Then%>  <%=DocTitle%><%else %> "Test"<%End if%> size="14"></td>
	</tr>

    <tr bgcolor="#FFFFFF">
	  <td valign="top" bgcolor="#FFFFFF">
		<!-- -------------------=== Start Ƕ��Table ===------------------------------- -->
		<table width="100%" height="289" border="0" cellpadding="0">

          <tr>
            <td nowrap class="maintxt">�û�����<br>
              <input name="UserName" type="text" value="Test" style="width:74px;" maxlength="10"><br>
                <input type="button" value="�����û�"  onclick="return SetUserName()" class="btn"></td>
		  </tr>

          <tr><td class="maintxt">
			<Hr color="#3366cc" size=1 >�������룺<br>
              <input name="docPwd" type="text" value="Password" style="width:74px;" maxlength="10"><br>
			    <input type="button" class="btn" value="�����ĵ�" onClick="return ProtectFull()"></td>
		  </tr>

          <tr><td><input type="button" class="btn" value="�������" onclick="return UnProtect()"></td></tr>

          <tr><td>
			<hr color="#3366cc" size=1>
			  <input  type="button" class="btn" value="�޶��ĵ�" onclick="return ProtectRevision()"></td>
		  </tr>

          <tr><td><input type="button" class="btn" value="��ʾ�޶�" onclick="return ShowRevisions()"></td></tr>
          <tr><td><input type="button" class="btn" value="�����޶�" onclick="return UnShowRevisions()"></td></tr>
          <tr><td><input type="button" class="btn" value="�����޶�" onclick="return AcceptAllRevisions()"></td></tr>

          <tr><td>
			<hr color="#3366cc" size=1>
             <input  type="button" class="btn" value="������ǩ" onclick="return addBookmark()"></td>
	      </tr>

          <tr><td><input  type="button" class="btn" value="�׼Ӻ�ͷ" onClick="return addRedHead()"></td></tr>

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
	    <!-- -------------------=== End Ƕ��Table ===------------------------------- -->
	  </td>

	  <td colspan="3" valign="top">
		<!-- -----------------------------== װ��weboffice�ؼ� ==--------------------------------- -->
			<script src="LoadWebOffice.js"></script>  
        <!-- --------------------------------== ����װ�ؿؼ� ==----------------------------------- -->
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
