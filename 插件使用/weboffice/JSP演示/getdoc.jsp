<%@ page contentType="text/html;charset=GB2312" %>
<%@ include file="conn.jsp" %>

<%
	String ID = request.getParameter("id");

	try	{ 
		rs = stmt.executeQuery("select * from doc where ID="+ID); 
	} 
	catch(SQLException ex) { 
		System.err.println("SQLÒì³£: " + ex.getMessage()); 
	}

	while(rs.next()) {
		try	{
			java.io.InputStream in = rs.getBinaryStream("DocContent");
			java.io.OutputStream outStream = response.getOutputStream();
			byte[] buf = new byte[1024];
			int bytes = 0;
			while((bytes = in.read(buf)) != -1)
				outStream.write(buf, 0, bytes);
				in.close();	
				outStream.close();	
		}
		catch(Throwable e) {
			out.println(e.toString());
		}
	}

%>
<%@ include file="closeDB.jsp" %>
