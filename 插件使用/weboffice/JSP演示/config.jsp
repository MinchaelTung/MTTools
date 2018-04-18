<%@ page contentType="text/html;charset=GB2312" %>

<%
	String IPAddr;
	IPAddr = "http://" + request.getServerName() + ":" + request.getServerPort() + request.getRequestURI();
	if(IPAddr.lastIndexOf("/") != -1) {
		int off = 0;
		off = IPAddr.lastIndexOf("/");
		IPAddr = IPAddr.substring(0,off);
	}
	session.setAttribute("IPAddr", IPAddr);
%>