<%@ page import="java.sql.*, java.io.*" %>
<%@ page contentType="text/html;charset=GB2312" %>
<%
	Connection conn = null;          
	Statement stmt = null;     
	ResultSet rs = null;
	
	try{       
		String MdbPath = new java.io.File(application.getRealPath("DBDemo.mdb")).getParent() + "\\DBDemo.mdb";
		Class.forName("sun.jdbc.odbc.JdbcOdbcDriver"); 
		String url= "jdbc:odbc:driver={Microsoft Access Driver (*.mdb)};DBQ=" + MdbPath ; 
		conn = DriverManager.getConnection(url);          
		stmt = conn.createStatement(ResultSet.TYPE_SCROLL_INSENSITIVE,ResultSet.CONCUR_UPDATABLE);          
	} catch(SQLException e){      
		out.print("SQL异常：数据库连接关闭异常---" + e.getMessage());
	} 

%>

