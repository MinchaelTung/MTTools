<%@ page contentType="text/html;charset=GB2312" %>
<%@ page import="java.io.*,java.text.*,java.util.*,java.sql.*" %>
<%@ include file="conn.jsp" %>
<%@ include file="config.jsp" %>
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
	return (true);
}

function ConfirmDel(FileUrl){
	if (confirm('�Ƿ�ȷ��ɾ���ñ�ǩ��')){
		location.href=FileUrl;
	}
}
</Script>
</head>
<body bgcolor="#ffffff">
<div align="center"><font size=4 >��ǩ�����޸ı�ǩ��</font></div>
<hr size="1" color="#cccccc">
<br>
<%
String mBookMarkID=request.getParameter("BookMarkID");
boolean mResult=false;

  String mSql="select * from BookMarks where BookMarkID = " + mBookMarkID;
  try
  {
    rs=stmt.executeQuery(mSql) ;
    if (rs.next())
    {

%>
<center>
<div align="center" class="mainTxt">
<table width="100%"  border=1 align="center" cellPadding=0 cellSpacing=0 bordercolor="#cccccc">
	 <tr>
        <td align="center" valign="top" bordercolor="#ffffff" class="maintxt">
<form name="webform" method="post" action="BookMarkEdit.jsp" onsubmit="return Check(this)">
<input type="hidden" name="BookMarkID" value="<%=mBookMarkID%>">
<table border=0  cellspacing='0' cellpadding='0' width=100% align=center class=TBStyle>
<tr>
  <td align=center>��ǩ����:</td>
  <td ><input type="text" name="BookMarkName" size="50" maxlength="32" class="IptStyle" value="<%=rs.getString("BookMarkName")%>"></td>
</tr>
<tr>
  <td  align=center>��ǩ˵��:</td>
  <td ><input type="text" name="BookMarkDesc" size="50" maxlength="60" class="IptStyle" value="<%=rs.getString("BookMarkDesc")%>"></td>
</tr>
<tr>
  <td align=center>��ǩ��ע:</td>
  <td><input type="text" name="BookMarkText" size="50" maxlength="150" class="IptStyle" value="<%=rs.getString("BookMarkText")%>"></td>
</tr>
<tr>
  <td colspan=2 align="center">
    <input type="submit" name="Edit" value="�� ��">
    <input type="button" name="Del" value="ɾ ��" onclick="javascript:ConfirmDel('BookMarkDel.asp?BookMarkID=<%=mBookMarkID%>');">
    <input type="reset" name="Reset" value="�� ��">
    <input type="button" name="Return" value="�� ��"  onclick="javascript:history.back();">
  </td>
</tr>
</table>
</form>
</td></tr>
</table>
</div>
</center>
<%
    }
    else
    {
       out.write("���ݿ��в����ڸñ�ǩ��<input type='button' value='�� ��' onclick='javascript:history.back();'");
    }
    rs.close();
  }
  catch(Exception e)
  {
      System.out.println(e.toString());
  }

%>
<%@ include file="closeDB.jsp" %>
</body>
</html>
