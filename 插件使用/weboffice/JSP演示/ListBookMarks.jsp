<%@ page contentType="text/html;charset=GB2312"%>
<%@ page import="java.io.*,java.text.*,java.util.*,java.sql.*,javax.servlet.*,javax.servlet.http.*" %>
<%@ include file="conn.jsp" %>
<%@ include file="config.jsp" %>
<%

String Sql= "SELECT * FROM Bookmarks ";
String Bookmark="";
String Description="";
String separator = "V*+";
boolean flag = true;		// ture�����÷��ر�ʾ��false������
try {
		try {
			ResultSet result = stmt.executeQuery(Sql);
			while (result.next()) {
				if(flag==true) { 
					out.print("::BMFIRST::");
					flag = false;
				} 
				try {
					Bookmark="";
					Description="";
					Bookmark=result.getString("BookMarkName");   //�û����б�
					Description=result.getString("BookMarkDesc");   //���˵����Ϣ���лس����򽫻س����>����

					out.print(Bookmark);
					out.print("\r\n");
					out.print(Description);
					out.print(separator);
				}
				catch (Exception ex) {
					System.out.println(ex.toString());
				}
			}
			result.close();
		}
		catch (SQLException e) {
			System.out.println(e.getMessage());
		}
}
finally {

}
%>
<%@ include file="closeDB.jsp" %>