<%@ page contentType="text/html;charset=GB2312" %>
<html>
<head>
<title>��ǩ����</title>
<link href="main.css" rel="stylesheet" type="text/css">
<script language=javascript>
function Check(theForm){
	if (theForm.BookMarkName.value == ""){
		alert("�������ǩ��.");
		theForm.BookMarkName.focus();
		return (false);
	}
	var bookMarkText = theForm.BookMarkText.value;
	var tmpString = bookMarkText.toLowerCase();

	var reg = /^\d+/;
	if(reg.test(theForm.BookMarkName.value)) {
		alert("��ǩ�������������ֿ�ͷ");
	}
	if(reg.test(theForm.BookMarkDesc.value)) {
		alert("��ǩ˵���������������ֿ�ͷ");
	}

	if(reg.test(tmpString)) {
		alert("��ע�����������ֿ�ͷ");
	}

	if(tmpString.indexOf("http") == 0 || tmpString.indexOf("ftp") == 0 || tmpString.indexOf(" ")!= -1) {
		alert("��ǩ��ע�����г���http��ftp�ȷǷ��ַ�");
		return (false);
	}

	return (true);

}
function changeComment(){
	/*
	if(obj.options[obj.selectedIndex].value=="time"){
		search_1.style.display="none";
		search_2.style.display="block";
		document.all.searchType.value="time";
	} else{
		search_1.style.display="block";
		search_2.style.display="none";
		document.all.searchType.value="keyword";
	}
	*/
}
</Script>
</head>
<body bgcolor="#ffffff">
<div align="center"><font size=4 >��ǩ�������ӱ�ǩ��</font></div>
<hr size="1" color="#cccccc">
<br>
<center>
<div align="center" class="mainTxt">
<table width="100%"  border=1 align="center" cellPadding=0 cellSpacing=0 bordercolor="#cccccc">
	 <tr>
        <td align="center" valign="top" bordercolor="#ffffff" class="maintxt">
<form name="webform" method="post" action="BookMarkAdd.jsp" onSubmit="return Check(this)">
<table border=0  cellspacing='0' cellpadding='0' width=100% align=center >
<tr>
  <td nowrap class="TDTitleStyle" ><div align="center">��ǩ����:</div></td>
  <td class="maintxt"  ><input type="text" name="BookMarkName" size="50" maxlength="32" class="IptStyle" value=""></td>
</tr>
<tr>
  <td nowrap align=center class="TDTitleStyle" ><div align="center">��ǩ˵��:</div></td>
  <td class="maintxt"><input type="text" name="BookMarkDesc" size="50" maxlength="60" class="IptStyle"></td>
</tr>
<tr>
  <td nowrap align=center class="TDTitleStyle" ><div align="center">��ǩ��ע:<br>
  <!--
    [����
    <input name="radiobutton" type="radio" value="TextComment" checked> 
    ͼƬ
    <input type="radio" name="radiobutton" value="PicComment">
    ]
	-->
  </div></td>
  <td class="maintxt">
  	<div id='TextLayer'>
		<input type="text" name="BookMarkText" size="50" maxlength="150" class="IptStyle"></div>
	<div id='PicLayer' style="display:none">
		<input type="file" name="BookMarkPic" size="50" maxlength="150" class="IptStyle"></div>
	</td>
</tr>
<tr>
  <td colspan=2 class="TDTitleStyle" align="center">
    <input type=submit name="Save" value="�� ��">
    <input type=reset name="Reset" value="�� ��">
    <input type=button name="Return" value="�� ��"  onclick="javascript:history.back();">  </td>
</tr>
</table>
</form>
</td></tr>
</table>
</div>
</center>
</body>
</html>
