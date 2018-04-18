<%@ Language=VBScript %>
<%
On Error Resume Next
dim id 
Dim title

id = Request.QueryString("id")
if id = "" then 
   id = 0
end if

if id<>0 then 
    Sql="SELECT * from doc where doc.id = "&id
else
    Sql="SELECT * from doc"
end if


	
DBStr="DBQ=" & Server.mappath("des.mdb") & ";DefaultDir=;DRIVER={Microsoft Access Driver (*.mdb)};DriverId=25;FIL=MS Access;ImplicitCommitSync=Yes;MaxBufferSize=512;MaxScanRows=8;PageTimeout=5;SafeTransactions=0;Threads=3;UserCommitSync=Yes;"
Set BbsDB =Server.CreateObject("ADODB.Connection")
BbsDB.Open DBStr
Sql="SELECT * from doc where doc.id = "&id
Set rs=Server.CreateObject("ADODB.RecordSet")
rs.Open Sql,BbsDB,3

if rs.RecordCount = 0 then
	Response.Write "未发现指定记录"
	RS.Close
	Set RS=Nothing
	BbsDB.close
	set BbsDB = Nothing
	Set	Uploader = Nothing
	Response.End 
end if

If Not RS.EOF THEN
	Response.BinaryWrite(RS("DocContent"))
ELSE

END IF

RS.Close
Set RS=Nothing
BbsDB.close
set BbsDB = Nothing
Set	Uploader = Nothing
%>