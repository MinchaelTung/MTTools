<meta http-equiv=refresh content="1;url=.\index.asp">
<%@	Language=VBScript %>
<%
'On Error Resume Next
DBStr="DBQ=" & Server.mappath("des.mdb") & ";DefaultDir=;DRIVER={Microsoft Access Driver (*.mdb)};DriverId=25;FIL=MS Access;ImplicitCommitSync=Yes;MaxBuffersize=512;MaxScanRows=8;PageTimeout=5;SafeTransactions=0;Threads=3;UserCommitSync=Yes;"
Set Conn =Server.CreateObject("ADODB.Connection")
Conn.Open DBStr

dim id	'�ĵ�ID
id = Request.QueryString("id")


Sql="delete from doc where doc.id=" & id
Conn.Execute(sql)
'Response.Redirect("postil")
Conn.close
set Conn = Nothing
Response.Write "ɾ���ɹ�!ϵͳ�Զ�����<a href='index.asp'>��ҳ</a>"

%>
