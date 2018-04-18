<%@ page contentType="text/html;charset=GB2312" %>
<%@ page import="java.util.*,java.sql.*" %>
<%@ include file="conn.jsp" %>
<%@ include file="config.jsp" %>
<%
	String mRecordID=request.getParameter("RecordID");
  String mSql="Delete from Template_File where RecordID = '"+ mRecordID + "'";
 	stmt.executeUpdate(mSql);
%>
<%@ include file="closeDB.jsp" %>
<%
response.sendRedirect("TemplateList.jsp");
%>

