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
	// 初始化上传组件
 	mySmartUpload.initialize(pageContext);
	mySmartUpload.upload();
	
		
	String DocID = "";
	String DocTitle = "";
	String DocType = "";
	String Docdata = "";
	String Descript="";
	String TemplateId="";
	// 获取传到表单记录
	TemplateId=mySmartUpload.getRequest().getParameter("TemplateId");
 	DocID = mySmartUpload.getRequest().getParameter("RecordID");
	DocTitle = mySmartUpload.getRequest().getParameter("FileName");
	out.print(DocID);
	DocType = mySmartUpload.getRequest().getParameter("FileType");
	Descript= mySmartUpload.getRequest().getParameter("Descript");

	String FilePath;
	com.jspsmart.upload.File myFile = null;
	myFile = mySmartUpload.getFiles().getFile(0);
	FilePath = myFile.getFileName();
	if (!myFile.isMissing()){
		myFile.saveAs(FilePath,mySmartUpload.SAVE_PHYSICAL);	// 保存上传文件到内存
		java.io.File tfile = new java.io.File(FilePath);
		if ( (TemplateId == null)|| (TemplateId.equals("0")) ){					// 如果是新记录	                                                 	
			rs = stmt.executeQuery("select RecordID,FileName,FileType,Descript,FileDate,FileBody from template_file"); 			
			rs.moveToInsertRow();								// moves cursor to the insert row		
			rs.updateString("RecordID",DocID);
			rs.updateString("FileName",DocTitle);
			rs.updateString("FileType",DocType);
			rs.updateString("Descript",Descript);
			rs.updateTimestamp("FileDate",new Timestamp(System.currentTimeMillis()));
			SaveFileToRS(tfile, rs, "FileBody");
			rs.insertRow();
		}else{	// 更新记录
			rs = stmt.executeQuery("select * from template_file where TemplateId = " + TemplateId ); 
			if (rs.next()) {
				rs.updateString("RecordID",DocID);
				rs.updateString("FileName",DocTitle);
				rs.updateString("Descript",Descript);
				SaveFileToRS(tfile, rs, "FileBody");
				rs.updateRow();
			}
		}
		if(tfile.exists()) {
			tfile.delete();
		}
	}	
%>
<%@ include file="closeDB.jsp" %>
