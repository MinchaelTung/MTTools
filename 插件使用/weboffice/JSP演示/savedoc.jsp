<%@ page contentType="text/html;charset=GB2312" %>
<%@ include file="conn.jsp" %>
<%@ page language="java" import="com.jspsmart.upload.*"%>
<jsp:useBean id="mySmartUpload" scope="page" class="com.jspsmart.upload.SmartUpload" />
<%!
	public void SaveFileToRS(java.io.File file,ResultSet rs, String colname)  throws IOException
	{
		try	{
			java.io.InputStream inStream=new java.io.FileInputStream(file);
			byte[] bytes = new byte[(int)file.length()];
			inStream.read(bytes);
			inStream.close();
			rs.updateBytes(colname,bytes);	
		} catch(SQLException e)	{
			System.out.print(e.toString());
		}
	}
%>

<%
	// ��ʼ���ϴ����
 	mySmartUpload.initialize(pageContext);
	mySmartUpload.upload();
	
		
	String DocID = "";
	String DocTitle = "";
	String DocType = "";
	String Docdata = "";
	String ID = "";

	// ��ȡ��������¼
	ID = request.getParameter("id");   
	DocID = mySmartUpload.getRequest().getParameter("DocID");
	DocTitle = mySmartUpload.getRequest().getParameter("DocTitle");
	DocType = mySmartUpload.getRequest().getParameter("DocType");

	String FilePath;
	com.jspsmart.upload.File myFile = null;
	myFile = mySmartUpload.getFiles().getFile(0);
	FilePath = myFile.getFileName();
	if (!myFile.isMissing()){
		myFile.saveAs(FilePath,mySmartUpload.SAVE_PHYSICAL);	// �����ϴ��ļ����ڴ�
		java.io.File tfile = new java.io.File(FilePath);
		if ( (ID == null)|| (ID.equals("0")) ){					// ������¼�¼	                                                 	
			rs = stmt.executeQuery("select * from doc"); 			
			rs.moveToInsertRow();								// moves cursor to the insert row						
			rs.updateString("DocID",DocID);
			rs.updateString("DocTitle",DocTitle);
			rs.updateString("DocType",DocType);
			rs.updateTimestamp("DocDate",new Timestamp(System.currentTimeMillis()));
			SaveFileToRS(tfile, rs, "DocContent");
			rs.insertRow();
		}else{	// ���¼�¼
			rs = stmt.executeQuery("select * from doc where ID = " + ID); 
			if (rs.next()) {
				rs.updateString("DocID",DocID);
				rs.updateString("DocTitle",DocTitle);
				SaveFileToRS(tfile, rs, "DocContent");
				rs.updateRow();
			}
		}
		if(tfile.exists()) {
			tfile.delete();
		}
	}	
%>
<%@ include file="closeDB.jsp" %>
