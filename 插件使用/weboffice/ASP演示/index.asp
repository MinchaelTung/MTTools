<%@LANGUAGE="vbSCRIPT" CODEPAGE="65001"%>
<!--#include file="./config.asp"-->
<%
	Dim iPageNum
	If Request.QueryString("iPageNum") = "" Then
	     iPageNum = 1
	Else
	     iPageNum = Request.QueryString("iPageNum")
	     iPageNum = CInt(iPageNum)
	End If

	iRowsPerPage = 12
 	DBStr="DBQ=" & Server.mappath("des.mdb") & ";DefaultDir=;DRIVER={Microsoft Access Driver (*.mdb)};DriverId=25;FIL=MS Access;ImplicitCommitSync=Yes;MaxBufferSize=512;MaxScanRows=8;PageTimeout=5;SafeTransactions=0;Threads=3;UserCommitSync=Yes;"

  	Set BbsDB =Server.CreateObject("ADODB.Connection")
  	BbsDB.Open DBStr
  	Sql="SELECT * from doc ORDER BY doc.id DESC"
	Set rs=Server.CreateObject("ADODB.RecordSet")
	rs.Open Sql,BbsDB,3
	if rs.RecordCount <> 0 then
		rs.PageSize  = iRowsPerPage
		if rs.PageCount < iPageNum then
			rs.AbsolutePage = rs.PageCount
		else
			rs.AbsolutePage = iPageNum
		end if
	end if	
%>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<link href="style.css" rel="stylesheet" type="text/css">
<title>WebOffice在线演示</title>
</head>
<body bgcolor=#ffffff topmargin=3> 
<center> 
  <table cellspacing=0 cellpadding=0 width=95% border=0> 
    <tr> 
      <td  width="1%"><img src="./images/logo.GIF" alt="Dianju" width="64" height="62"    border="0"> 
      </td> 
      <td>&nbsp;&nbsp;</td> 
      <td  width="100%"><table border=0 cellspacing=0 width="100%" cellpadding=0> 
          <tr valign=top> 
            <td>&nbsp;</td> 
            <td><div align="right"><a href="http://www.dianju.com.cn/index.htm">点聚首页</a></div></td> 
          </tr> 
          <tr> 
            <td  height="1" colspan="2" bgcolor="#25479D"><img height="2" src="" alt="" width="1"></td> 
          </tr> 
        </table></td> 
    <tr> 
      <td><img height=1 src="" width=1></td> 
    </tr> 
  </table>  <br>
  <table width="90%" border=0 align="center" cellpadding=5 cellspacing=0>
    <tr valign="top" > 
      <td > <TABLE cellSpacing=0 cellPadding=0 width="100%" border=0>
          <TR> 
            <TD bgColor=#3366cc><IMG height=1 alt="" width=1></TD>
          </TR>
        </TABLE>
        <table width=100% border=0 cellspacing=0 cellpadding="5" >
          <tr> 
            <td valign="top" bgColor=#e5ecf9><p align="left"><b><strong>WebOffice演示程序</strong></b><br>
            </p></td>
          </tr>
          <tr> 
            <td valign="top" >
	<br><form name="form1" method="post" action="DocEdit.asp">
  <table width="90%" border="0" align="center" cellpadding="0" cellspacing="0">
    <tr  class="maintitle"> 
      <td width="91"  align="top">文件格式:</td>
      <td width="91"  align="top">
	  <!-- ---------------=== 该处文件格式的value不可以自定义 ===------------------------- -->
		<select name="DocType" size="1" id="DocType">
          <option value="doc">Word</option>
          <option value="xls">Excel</option>
        </select>
	  <!-- ---------------------------=== 结束注释 ===---------------------------------- -->
		</td>
      <td   align="top"><input name="CreateFile" type="submit" id="CreateFile" value="起草正文"></td>
      <td  align="top">提示: 系统会自动下载安装控件,如果没有安装成功,<br>请转到<a href="http://www.dianju.com.cn/weboffice.htm">WebOffice下载页面</a>手动下载安装</td>
    </tr>
  </table>
</form>
    <div align="center"><br>
        <br>
    </div>
    <table width="100%" border="1" align="center" cellpadding="0" cellspacing="0" bordercolor="#3366CC">
  <tr bgcolor="#e5ecf9"> 
	<td width="10%" bordercolor="#e5ecf9" bgcolor="#e5ecf9" class="maintitle"><div align="center">记录号</div></td>
	<td width="14%" bordercolor="#e5ecf9" bgcolor="#e5ecf9" class="maintitle"><div align="center">文档号</div></td>
	<td width="26%" bordercolor="#e5ecf9" bgcolor="#e5ecf9" class="maintitle"><div align="center">公文标题</div></td>
	<td width="12%" bordercolor="#e5ecf9" bgcolor="#e5ecf9" class="maintitle"><div align="center">公文格式</div></td>
	<td width="22%" bordercolor="#e5ecf9" bgcolor="#e5ecf9" class="maintitle"><div align="center">时间</div></td>
	<td width="16%"  bordercolor="#e5ecf9" bgcolor="#e5ecf9" class="maintitle"><div align="center">操作</div></td>
  </tr>
<%
	iLoop = 1
	Do While (Not rs.EOF) and (iLoop <= iRowsPerPage)
%>
	<tr <% if ( 0 = iLoop mod 2) then %>bgcolor="#CCCCCC"<%else%> bgcolor="#FFFFFF"<%end if%>> 
	<td <% if ( 0 = iLoop mod 2) then %>bordercolor="#CCCCCC"<%else%> bordercolor="#FFFFFF"<%end if%>  class="maintxt"><div align="center"><%= rs.Fields(0)%></div></td>
	<td <% if ( 0 = iLoop mod 2) then %>bordercolor="#CCCCCC"<%else%> bordercolor="#FFFFFF"<%end if%> class="maintxt"><div align="center">
	      <% if (rs.Fields(1).value <>"") then%>
	      <%= Mid(CStr(rs.Fields(1).value),1,20)%>
	      <%if Len(CStr(rs.Fields(1).value)) >20 then%>
	      ...
	      <%end if%>
	      <%end if%>
	  </div></td>
	<td <% if ( 0 = iLoop mod 2) then %>bordercolor="#CCCCCC"<%else%> bordercolor="#FFFFFF"<%end if%> class="maintxt"><div align="center">
	      <% if (rs.Fields(2).value <>"") then%>
	      <%= Mid(CStr(rs.Fields(2).value),1,20)%>
	      <%if Len(CStr(rs.Fields(2).value)) >20 then%>
	      ...
	      <%end if%>
	      <%end if%>
	  </div></td>
	<td <% if ( 0 = iLoop mod 2) then %>bordercolor="#CCCCCC"<%else%> bordercolor="#FFFFFF"<%end if%> class="maintxt"><div align="center">
	      <% if (rs.Fields(3).value <>"") then%>
	      <%= Mid(CStr(rs.Fields(3).value),1,20)%>
	      <%end if%>
	  </div></td>
	<td <% if ( 0 = iLoop mod 2) then %>bordercolor="#CCCCCC"<%else%> bordercolor="#FFFFFF"<%end if%> class="maintxt"><div align="center">
	      <% if (rs.Fields(4).value <>"") then%>
	      <%= Mid(CStr(rs.Fields(4).value),1,20)%>
	      <%end if%>
	  </div></td>
   <td <% if ( 0 = iLoop mod 2) then %>bordercolor="#CCCCCC"<%else%> bordercolor="#FFFFFF"<%end if%> class="maintxt"><div align="center"><a href= DocEdit.asp?id=<%= rs.Fields(0).value%>>编辑</a> | <a href="deldoc.asp?id=<%= rs.Fields(0).value%>">删除</a> </div></td>
  </tr>
<%
		iLoop = iLoop + 1
		rs.MoveNext 
		Loop
		%></table>
<table width="90%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr> 
    <td class="maintitle" align="right">
	<%		
		ShowNavBar(rs)
		rs.Close 
		BbsDB.Close 
	%> 
</td>
  </tr>
</table><br>		
<!-- ------------=== 装载Weboffice控件，如果没安装提示下载 ===-------------------- -->

	<OBJECT width="0" height="0" id=WebOffice1 style="LEFT: 0px; TOP: 0px; " 
		classid="clsid:E77E049B-23FC-4DB8-B756-60529A35FAD5" codebase=WebOffice.ocx#V3,0,0,0 >
		<PARAM NAME="_Version" VALUE="65536">
		<PARAM NAME="_ExtentX" VALUE="2646">
		<PARAM NAME="_ExtentY" VALUE="1323">
		<PARAM NAME="_StockProps" VALUE="0">
	</OBJECT>

<!-- ---------------------------=== 结束 ===----------------------------------- -->
			
			</td>
          </tr>
        </table>
        <TABLE cellSpacing=0 cellPadding=0 width="100%" border=0>
          <TR><Strong><font color="red">服务器路径：<%=Application("appPath")%></font></Strong>
            <TD bgColor=#3366cc><IMG height=1 alt="" width=1></TD>
          </TR>
        </table>
	  </td>
    </tr>
  </table>
<table width="90%" border="0" cellspacing="0" cellpadding="0">
  <tr> 
  <tr  class="maintxt" align="center"> 
    <td colspan="4">版权所有 北京点聚信息技术有限公司<br>
      Copyright &copy; 2006  All Rights Reserved</tr>
</table></center> 


<SCRIPT LANGUAGE=vbscript RUNAT=Server>
' 分页过程
Sub ShowNavBar(rs)
	Dim iPageCount
	Dim iLoop
	Dim sScriptName
    sScriptName = Request.ServerVariables("SCRIPT_NAME")
    If iPageNum > 1 Then
      Response.Write " <a href=" & sScriptName & "?iPageNum="
      Response.Write (iPageNum -1) & "><< 上一页</a>"
    End If
    iPageCount = rs.PageCount
    Do Until iLoop > iPageCount
    if iLoop = iPageNum Then
       Response.Write " <B>" & CStr(iLoop) & "</B>"
      Else
       Response.Write " <a href=" & sScriptName & "?iPageNum=" & _
       Cstr(iLoop) & ">" & iLoop & "</a>"
       End If
       iLoop = iLoop + 1
    Loop

    If Not rs.EOF Then
     Response.Write " <a href=" & sScriptName & "?iPageNum="
     Response.Write (iPageNum +1) & "> 下一页 >></a><BR>"
    Else
       Response.Write "<BR > "
    End If
    Response.Write "第" & iPageNum & "页 共" & iPageCount & "页　共"
    Response.Write rs.RecordCount & " 条记录" 
End Sub

</SCRIPT>
</body>
</html>
