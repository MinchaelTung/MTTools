<%@ page contentType="text/html;charset=GB2312" %>
<%@ page import="java.io.*,java.text.*,java.util.*,java.sql.*" %>
<%@ include file="conn.jsp" %>
<%@ include file="config.jsp" %>
<%
String mBookMarkID = new String(request.getParameter("BookMarkID"));
boolean mResult=false;
  String mSql = "Delete from BookMarks where BookMarkID = "+ mBookMarkID;
  int i=stmt.executeUpdate(mSql);
  if(i<1){
  mResult=false;
}else{
  mResult=true;
}

if (mResult)
{
  response.sendRedirect("BookMarkList.jsp");
}

%>
<%@ include file="closeDB.jsp" %>