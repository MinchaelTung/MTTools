<%@ page contentType="text/html;charset=GB2312" %>
<%@ page import="java.util.*,java.sql.*" %>
<%@ include file="conn.jsp" %>
<%@ include file="config.jsp" %>
<html>
<head>
<title>��ǩ����</title>
<link href="main.css" rel="stylesheet" type="text/css">
<script language="javascript">
function ConfirmDel(FileUrl){
        if (confirm('�Ƿ�ȷ��ɾ���ñ�ǩ��')){
                location.href=FileUrl;
        }
}
</script>
</head>
<body bgcolor="#ffffff">
<div align="center"><font size=4 ><b>��ǩ����</b></font></div>
<TABLE cellSpacing=0 cellPadding=0 width="100%" border=0>
        <TBODY>
        <TR>
          <TD bgColor=#3366cc><IMG height=1 alt="" 
width=1></TD></TR></TBODY></TABLE>

<br>
<input type=button name="Add" value="���ӱ�ǩ"  onclick="javascript:location.href='BookMarkAddFrm.jsp'">
<input type=button name="Return" value="�� ��"  onclick="javascript:location.href='index.jsp';">
<TABLE  cellSpacing=0 cellPadding=0 width="100%" border=0>
        <TBODY>
        <TR>
          <TD bgColor=#3366cc><IMG height=1 alt="" 
width=1></TD></TR></TBODY></TABLE>
<table id=alert_table border=0  cellspacing="0" cellpadding="4" width=100% align=center>
        <tr>
                <td bgColor=#e5ecf9 ><div align="center"><FONT size=-1><B>���</B></font></div></td>
                <td bgColor=#e5ecf9 ><div align="center"><FONT size=-1><B>��ǩ����</B></font></div></td>
                <td bgColor=#e5ecf9 ><div align="center"><FONT size=-1><B>��ǩ˵��</B></font></div></td>
                <td bgColor=#e5ecf9 ><div align="center"><FONT size=-1><B>��ǩ��ע</B></font></div></td>
								<td bgColor=#e5ecf9 ><div align="center"><FONT size=-1><B>����</B></font></div></td>
        </tr>
<%

    try
    {
      rs=stmt.executeQuery("select * from BookMarks order by BookMarkID desc") ;
      while ( rs.next() )
      {
        String mBookMarkID=rs.getString("BookMarkID");
%>
      <tr  class=data_row>
          <td  class="maintxt"><div align="center"><FONT size=-1><%=mBookMarkID%>&nbsp;</FONT></div></td>
          <td  class="maintxt"><div align="center"><FONT size=-1><%=rs.getString("BookMarkName")%>&nbsp;</FONT></div></td>
          <td  class="maintxt"><div align="center"><FONT size=-1><%=rs.getString("BookMarkDesc")%>&nbsp;</FONT></div></td>
          <td  class="maintxt"><div align="center"><FONT size=-1><%=rs.getString("BookMarkText")%>&nbsp;</FONT></div></td>
          <td  class="maintxt"><div align="center"><FONT size=-1>
                 <input type="button" name="Edit" value=" �� �� " onClick="javascript:location.href='BookMarkEditFrm.jsp?BookMarkID=<%=mBookMarkID%>';">
                 <input type="button" name="Del" value=" ɾ �� " onClick="javascript:ConfirmDel('BookMarkDel.jsp?BookMarkID=<%=mBookMarkID%>');">
         </FONT></div></td>
	</tr>
<%
      }
    }
    catch(SQLException e)
    {
      System.out.println(e.toString());
    }

%>

   <tr> 
    <td colspan="5"  bgColor=#e5ecf9 align="center" ><FONT size=-1>&nbsp; </FONT></td>
   </tr>
</table>
<TABLE cellSpacing=0 cellPadding=0 width="100%" border=0>
        <TBODY>
        <TR>
          <TD bgColor=#3366cc><IMG height=1 alt="" 
			width=1></TD></TR></TBODY></TABLE>
<%@ include file="closeDB.jsp" %>
</body>
</html>
