<%@ page contentType="text/html;charset=GB2312" %>
<%@ include file="conn.jsp" %>
<% 
	String id  = request.getParameter("id");
	String sql = "delete from Doc where id=" + id;
	try {
		stmt.executeUpdate(sql); 
	} catch(SQLException e) {
		out.print(e.toString());
	} finally {
		stmt.close();
		conn.close();
	}
	out.print("¼ÇÂ¼É¾³ý³É¹¦");
%>
<meta http-equiv=refresh content="1;url=.\index.jsp">
<%@ include file="closeDB.jsp" %>
