<%@ page contentType="text/html;charset=GB2312" %>
<%@ page import="java.sql.*" %>
<%@ include file="conn.jsp" %>
<%@ include file="config.jsp" %>



<html>
<head>
<link href="style.css" rel="stylesheet" type="text/css">
<title>WebOffice������ʾ</title>
</head>

<SCRIPT LANGUAGE=javascript>
	function btn_word1(){
		form1.action="docEdit.jsp?doctype=doc&modId="+document.getElementById("wordMod").value;
		form1.submit();
	}
	function btn_excel(){
		form1.action="docEdit.jsp?doctype=xls&modId="+document.getElementById("excelMod").value;
		form1.submit();
	}
</script>

<body bgcolor=#ffffff topmargin=3> 
<center> 
  <table cellspacing=0 cellpadding=0 width=95% border=0> 
    <tr> 
      <td  width="1%"><img src="./images/logo.gif" alt="Dianju" width="64" height="62"    border="0"> 
      </td> 
      <td>&nbsp;&nbsp;</td> 
      <td  width="100%"><table border=0 cellspacing=0 width="100%" cellpadding=0> 
          <tr valign=top> 
            <td>&nbsp;</td> 
            <td><div align="right"><a href="http://www.dianju.com.cn/index.htm">�����ҳ</a></div></td> 
          </tr> 
          <tr> 
            <td  height="1" colspan="2" bgcolor="#25479D"><img height="2" src="" alt="" width="1"></td> 
          </tr> 
        </table></td> 
    <tr> 
      <td><img height=1 src="" width=1></td> 
    </tr> 
  </table>  
  <div align="center"><br>
    <input type="button" name="btn_mark" value="��ǩ����" onClick="javascript:location.href='BookMarkList.jsp'">
    <input type="button" name="btn_" value="ģ�����" onClick="javascript:location.href='TemplateList.jsp'">
  </div>
  <table width="90%" border=0 align="center" cellpadding=5 cellspacing=0>
    <tr valign="top" > 
      <td ><TABLE cellSpacing=0 cellPadding=0 width="100%" border=0>
          <TR> 
            <TD bgColor=#3366cc><IMG height=1 alt="" width=1></TD>
          </TR>
 			  </TABLE>
        <table width=100% border=0 cellspacing=0 cellpadding="5" >
          <tr> 
            <td valign="top" bgColor=#e5ecf9><p align="left"><b><strong>WebOffice��ʾ����</strong></b><br>
            </p></td>
          </tr>
		  <tr> 
		    <td valign="top" >
	<br><form name="form1" method="post" action="docEdit.jsp">
  <table width="90%" border="0" align="center" cellpadding="0" cellspacing="0">
    <tr  class="maintitle"> 
      <td   align="top">��ѡ��ģ��:
	  
				<select name="wordMod" size="1" id="wordMod">
					<option value="0" selected>---------����ģ��---------</option>
					<% 
					try{
					
					String docSql="select TemplateID,descript from template_file where filetype='doc'";
					ResultSet docRs=stmt.executeQuery(docSql);
				while(docRs.next()){
				%>
         <option value="<%=docRs.getInt(1)%>"><%=docRs.getString(2)%></option>
         	<%
         	}
         	}catch(Exception e){
         	
         	}
         
         	%>
        </select>
	  <!-- ---------------------------=== ����ע�� ===---------------------------------- -->
		</td>
      <td   align="top">
      <input name="btn_word" type="button" id="btn_word" value="���Word����" onclick="btn_word1()">
      </td>
      <td  align="top"></td>
    </tr>
        <tr  class="maintitle"> 
      <td align="top">��ѡ��ģ��:
	 <select name="excelMod" size="1" id="excelMod">
					<option value="0" selected>---------����ģ��---------</option>
					<% 
					try{
					
					String docSql="select TemplateID,descript from Template_File where filetype='xls'";
					ResultSet docRs=stmt.executeQuery(docSql);
				while(docRs.next()){
				%>
         <option value="<%=docRs.getInt(1)%>"><%=docRs.getString(2)%></option>
         	<%
         	}
         	}catch(Exception e){
         	
         	}
         
         	%>
        </select>
	  <!-- ---------------------------=== ����ע�� ===---------------------------------- -->
		</td>
      <td   align="top">
      <input name="CreateFile" type="submit" id="CreateFile" value="���Excel����" onclick="btn_excel()">
      </td>
      <td  align="top">��ʾ: ϵͳ���Զ����ذ�װ�ؼ�,���û�а�װ�ɹ�,<br>��ת��<a href="http://www.dianju.com.cn/weboffice.htm">WebOffice����ҳ��</a>�ֶ����ذ�װ</td>
    </tr>
  </table>
</form>
    <div align="center"><br>
        <br>
    </div>
    <table width="100%" border="1" align="center" cellpadding="0" cellspacing="0" bordercolor="#3366CC">
  <tr bgcolor="#e5ecf9"> 
	<td width="10%" bordercolor="#e5ecf9" bgcolor="#e5ecf9" class="maintitle"><div align="center">��¼��</div></td>
	<td width="14%" bordercolor="#e5ecf9" bgcolor="#e5ecf9" class="maintitle"><div align="center">�ĵ���</div></td>
	<td width="26%" bordercolor="#e5ecf9" bgcolor="#e5ecf9" class="maintitle"><div align="center">���ı���</div></td>
	<td width="12%" bordercolor="#e5ecf9" bgcolor="#e5ecf9" class="maintitle"><div align="center">���ĸ�ʽ</div></td>
	<td width="22%" bordercolor="#e5ecf9" bgcolor="#e5ecf9" class="maintitle"><div align="center">ʱ��</div></td>
	<td width="16%"  bordercolor="#e5ecf9" bgcolor="#e5ecf9" class="maintitle"><div align="center">����</div></td>
  </tr>

<%
  String sql="SELECT * from doc ORDER BY doc.id DESC";
	rs = stmt.executeQuery(sql);
%>
<%
	int iLoop = 1;
	String docID="";
	while(rs.next()) {
		docID = rs.getString(1);
%>
	<tr <% if ( 0 == iLoop % 2) { %>bgcolor="#CCCCCC"<% } else {%> bgcolor="#FFFFFF"<% } %>> 
	<td <% if ( 0 == iLoop % 2) { %>bordercolor="#CCCCCC"<% } else { %> bordercolor="#FFFFFF"<% } %>  class="maintxt"><div align="center"><%= docID%></div></td>
	<td <% if ( 0 == iLoop % 2) { %>bordercolor="#CCCCCC"<% } else { %> bordercolor="#FFFFFF"<% } %> class="maintxt"><div align="center">
	<%=rs.getString(2)%>
	  </div></td>
	<td <% if ( 0 == iLoop % 2) { %>bordercolor="#CCCCCC"<% } else { %> bordercolor="#FFFFFF"<% } %> class="maintxt"><div align="center">
	<%=rs.getString(3)%>
	  </div></td>
	<td <% if ( 0 == iLoop % 2) { %>bordercolor="#CCCCCC"<% }else { %> bordercolor="#FFFFFF"<% } %> class="maintxt"><div align="center">
	<%=rs.getString(4)%>
	  </div></td>
	<td <% if ( 0 == iLoop % 2) { %>bordercolor="#CCCCCC"<% } else { %> bordercolor="#FFFFFF"<% } %> class="maintxt"><div align="center">
	<%=rs.getString(5)%>

	  </div></td>
   <td <% if ( 0 == iLoop % 2) { %>bordercolor="#CCCCCC"<% } else { %> bordercolor="#FFFFFF"<% } %> class="maintxt"><div align="center"><a href= docEdit.jsp?id=<%= docID%>>�༭</a> | <a href="deldoc.jsp?id=<%= docID%>">ɾ��</a> </div></td>

  </tr>
<%
		iLoop = iLoop + 1;
	}
%><tr> 
 <td colspan="6" bordercolor="#e5ecf9" bgcolor="#e5ecf9" class="maintitle"><div align="center">&nbsp; 
   </div></FONT></td>
  </tr>
</table>
<table width="90%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr> 
    <td class="maintitle" align="right">

</td>
  </tr>
</table><br>		
<!-- ------------=== װ��Weboffice�ؼ������û��װ��ʾ���� ===-------------------- -->

	<OBJECT width="0" height="0" id=WebOffice1 style="LEFT: 0px; TOP: 0px; " 
		classid="clsid:E77E049B-23FC-4DB8-B756-60529A35FAD5" codebase=WebOffice.ocx#V3,0,0,0 >
		<PARAM NAME="_Version" VALUE="65536">
		<PARAM NAME="_ExtentX" VALUE="2646">
		<PARAM NAME="_ExtentY" VALUE="1323">
		<PARAM NAME="_StockProps" VALUE="0">
	</OBJECT>

<!-- ---------------------------=== ���� ===----------------------------------- -->
			
			</td>
          </tr>
        </table>
        <TABLE cellSpacing=0 cellPadding=0 width="100%" border=0>
          <TR><Strong><font color="red">������·����<%=session.getAttribute("IPAddr")%>
</font></Strong>
            <TD bgColor=#3366cc><IMG height=1 alt="" width=1></TD>
          </TR>
        </table>
	  </td>
    </tr>
  </table>
<table width="90%" border="0" cellspacing="0" cellpadding="0">
  <tr> 
  <tr  class="maintxt" align="center"> 
    <td colspan="4">��Ȩ���� �����ܹ�˾<br>
      Copyright &copy; 2006  All Rights Reserved</tr>
</table></center> 
<%@ include file="closeDB.jsp" %>

</body>
</html>
