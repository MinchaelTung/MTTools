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
		out.print("SQL�쳣�����ݿ����ӹر��쳣---" + e.getMessage());
	}
%>

