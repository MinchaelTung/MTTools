<%@ page contentType="text/html;charset=GB2312" %>
<%@ page import="java.util.*,java.sql.*" %>
<%@ include file="conn.jsp" %>
<%@ include file="config.jsp" %>
<%
String mRecordID=request.getParameter("RecordID");
String mFileName=new String(request.getParameter("FileName").getBytes("8859_1"));
String mDescript=new String(request.getParameter("Descript").getBytes("8859_1"));

  String mSql="Update Template_File Set FileName = '"+ mFileName +"',Descript = '"+ mDescript +"' Where RecordID='"+ mRecordID +"'";

 stmt.executeUpdate(mSql);
%>
<%@ include file="closeDB.jsp" %>
<%response.sendRedirect("TemplateList.jsp");
%>

