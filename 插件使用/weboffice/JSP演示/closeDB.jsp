<%@ page contentType="text/html;charset=GB2312" %>
<%

	try {
		if(stmt!=null) {
			stmt.close();
		}

		if(conn!=null && !conn.isClosed()) {
			conn.close();
		}
	} catch (SQLException e) {
		out.print("SQL异常：数据库连接关闭异常---" + e.getMessage());
	}
%>

