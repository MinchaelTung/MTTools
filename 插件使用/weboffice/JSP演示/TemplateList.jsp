<%@ page contentType="text/html;charset=GB2312" %>
<%@ page import="java.util.*,java.sql.*" %>
<%@ include file="conn.jsp" %>
<%@ include file="config.jsp" %>
<html>
<head>
<title>ģ�����</title>
<link href="main.css" rel="stylesheet" type="text/css">
<script language="javascript">
function ConfirmDel(FileUrl){
	if (confirm('�Ƿ�ȷ��ɾ����ģ�壡')){
		location.href=FileUrl;
	}
}
</script>
</head>
<body bgcolor="#ffffff">
<div align="center"><font size=4 ><b>ģ�����</b></font></div>
<TABLE cellSpacing=0 cellPadding=0 width="100%" border=0>
        <TBODY>
        <TR>
          <TD bgColor=#332cc><IMG height=1 alt="" 
width=1></TD></TR></TBODY></TABLE>
<br>
 <input type=button name="AddDocTemplate" value="�½�Wordģ��"  onclick="javascript:location.href='TemplateEdit.jsp?FileType=doc';">
 <input type=button name="AddXslTemplate" value="�½�Excelģ��" onclick="javascript:location.href='TemplateEdit.jsp?FileType=xls';">
 <input type=button name="Return" value="�� ��"  onclick="javascript:location.href='index.jsp';">
<TABLE cellSpacing=0 cellPadding=0 width="100%" border=0>
        <TBODY>
        <TR>
          <TD bgColor=#332cc><IMG height=1 alt="" 
width=1></TD></TR></TBODY></TABLE>
<table id=alert_table border=0  cellspacing='0' cellpadding='4' width=100% align=center class=TBStyle>
	<tr>
		<td  bgColor=#e5ecf9 ><div align="center"><FONT size=-1><B>���</B></FONT></div></td>
		<td  bgColor=#e5ecf9 ><div align="center"><FONT size=-1><B>ģ������</B></FONT></div></td>
		<td  bgColor=#e5ecf9 ><div align="center"><FONT size=-1><B>ģ������</B></FONT></div></td>
		<td  bgColor=#e5ecf9 ><div align="center"><FONT size=-1><B>ģ��˵��</B></FONT></div></td>
		<td  bgColor=#e5ecf9 ><div align="center"><FONT size=-1><B>����</B></FONT></div></td>
	</tr>
<%
    try
    {
       rs=stmt.executeQuery("Select Templateid,RecordID,FileName,FileType,Descript From Template_File order by TemplateID desc") ;
      while ( rs.next() )
      {
      	String mTemplateId=rs.getString("Templateid");
        String mRecordID=rs.getString("RecordID");
        String mFileName=rs.getString("FileName");
        String mFileType=rs.getString("FileType");
        String mDescript=rs.getString("Descript");
%>
	<tr class=data_row>
		<td  class="maintxt"><div align="center"><FONT size=-1><%=mRecordID%>&nbsp;</FONT></div></td>
		<td  class="maintxt"><div align="center"><FONT size=-1><%=mFileName%>&nbsp;</FONT></div></td>
		<td  class="maintxt"><div align="center"><FONT size=-1><%=mFileType%>&nbsp;</FONT></div></td>
		<td  class="maintxt"><div align="center"><FONT size=-1><%=mDescript%>&nbsp;</FONT></div></td>
		<td  class="maintxt"><div align="center"><FONT size=-1>
			<input type=button onclick="javascript:location.href='TemplateEdit.jsp?TemplateID=<%=mTemplateId%>&FileType=<%=mFileType%>';" name="Edit" value=" �� �� ">
			<input type=button onclick="javascript:ConfirmDel('TemplateDel.jsp?RecordID=<%=mRecordID%>');" name="Del" value=" ɾ �� ">
		</FONT></div></td>
	</tr>
<%
      }
    }
    catch(Exception e)
    {
      System.out.println(e.toString());
    }
  
%>
<tr> 
 <td colspan="8"  bgColor=#e5ecf9 align="center" ><FONT size=-1>&nbsp; 
   </FONT></td>
  </tr>

<%@ include file="closeDB.jsp" %>
</table>
<TABLE cellSpacing=0 cellPadding=0 width="100%" border=0>
        <TBODY>
        <TR>
          <TD bgColor=#3366cc><IMG height=1 alt="" 
			width=1></TD></TR></TBODY></TABLE>
</body>
</html>
