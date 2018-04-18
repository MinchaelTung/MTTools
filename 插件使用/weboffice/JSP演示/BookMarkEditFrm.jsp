<%@ page contentType="text/html;charset=GB2312" %>
<%@ page import="java.io.*,java.text.*,java.util.*,java.sql.*" %>
<%@ include file="conn.jsp" %>
<%@ include file="config.jsp" %>
<html>
<head>
<title>标签管理</title>
<link href="main.css" rel="stylesheet" type="text/css">
<script language=javascript>
function Check(theForm){
	if (theForm.BookMarkName.value == ""){
		alert("请输入标签名.");
		theForm.BookMarkName.focus();
		return (false);
	}
	return (true);
}

function ConfirmDel(FileUrl){
	if (confirm('是否确定删除该标签！')){
		location.href=FileUrl;
	}
}
</Script>
</head>
<body bgcolor="#ffffff">
<div align="center"><font size=4 >标签管理〖修改标签〗</font></div>
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
  <td align=center>标签名称:</td>
  <td ><input type="text" name="BookMarkName" size="50" maxlength="32" class="IptStyle" value="<%=rs.getString("BookMarkName")%>"></td>
</tr>
<tr>
  <td  align=center>标签说明:</td>
  <td ><input type="text" name="BookMarkDesc" size="50" maxlength="60" class="IptStyle" value="<%=rs.getString("BookMarkDesc")%>"></td>
</tr>
<tr>
  <td align=center>标签备注:</td>
  <td><input type="text" name="BookMarkText" size="50" maxlength="150" class="IptStyle" value="<%=rs.getString("BookMarkText")%>"></td>
</tr>
<tr>
  <td colspan=2 align="center">
    <input type="submit" name="Edit" value="修 改">
    <input type="button" name="Del" value="删 除" onclick="javascript:ConfirmDel('BookMarkDel.asp?BookMarkID=<%=mBookMarkID%>');">
    <input type="reset" name="Reset" value="重 填">
    <input type="button" name="Return" value="返 回"  onclick="javascript:history.back();">
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
       out.write("数据库中不存在该标签。<input type='button' value='返 回' onclick='javascript:history.back();'");
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
