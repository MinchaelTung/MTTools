<%@ page contentType="text/html;charset=GB2312" %>
<%@ page import="java.io.*,java.text.*,java.util.*,java.sql.*" %>
<%@ include file="conn.jsp" %>
<%@ include file="config.jsp" %>
<html>
<head>
<title>��ǩ����</title>
</head>
<body bgcolor="#ffffff">
<div align="center"><font size=4 color=ff0000>��ǩ������ǩ�޸ġ�</font></div>
<hr size=1>
<br>
<%
String mBookMarkID=new String(request.getParameter("BookMarkID"));
String mBookMarkName=new String(request.getParameter("BookMarkName").getBytes("8859_1"));
String mBookMarkDesc=new String(request.getParameter("BookMarkDesc").getBytes("8859_1"));
String mBookMarkText=new String(request.getParameter("BookMarkText").getBytes("8859_1"));
boolean mResult=false;


  String mSql="select BookMarkName from BookMarks where BookMarkID<>"+ mBookMarkID +" and BookMarkName='" + mBookMarkName + "'";
  try
  {
     rs=stmt.executeQuery(mSql) ;
    if (rs.next())
    {
      out.write("����ʧ�ܣ����ݿ����Ѵ�����ͬ�ı�ǩ��"+mBookMarkName+"��<input type='button' value='�� ��' onclick='javascript:history.back();'");
      mResult=false;
    }
    else
    {
        java.sql.PreparedStatement prestmt=null;
        mSql="Update BookMarks Set BookMarkName = '"+ mBookMarkName +"',BookMarkDesc = '"+ mBookMarkDesc +"',BookMarkText='"+ mBookMarkText +"' where BookMarkID="+ mBookMarkID;
				stmt.executeUpdate(mSql);
        mResult=true;
    }
    rs.close();
  }
  catch(Exception e)
  {
      System.out.println(e.toString());
  }
if (mResult)
{
  response.sendRedirect("BookMarkList.jsp");
}
%>
<%@ include file="closeDB.jsp" %>
</body>
</html>
