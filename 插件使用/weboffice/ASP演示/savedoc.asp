<%@	Language=VBScript %>
<!-- #include file="upload.inc"	-->
<%
Set	Uploader = New UpFile_Class
Uploader.NoAllowExt="cs;vb;js;exe"				' ���ò������ϴ����ļ�����
Uploader.GetData (Request.TotalBytes)			' �������Ƶ��ϴ��ļ���С��Request.TotalBytes��ʾ������

if Uploader.isErr then  '�������
  select case Uploader.isErr
	case 1
	Response.Write "ϵͳû��ȡ�����ϴ������ݣ�������"
	case 2
	Response.Write "���ϴ����ļ��������ǵ�����"
	End select
	Response.End
End if

DBStr="DBQ=" & Server.mappath("Des.mdb") & ";DefaultDir=;DRIVER={Microsoft Access Driver (*.mdb)};DriverId=25;FIL=MS Access;ImplicitCommitSync=Yes;MaxBuffersize=512;MaxScanRows=8;PageTimeout=5;SafeTransactions=0;Threads=3;UserCommitSync=Yes;"
Set BbsDB =Server.CreateObject("ADODB.Connection")
BbsDB.Open DBStr
Set rs=Server.CreateObject("ADODB.RecordSet")

dim id 
id = Request.QueryString("id")
if id = "" then 
   id = 0
End if

if id<>0 then 
    Sql="SELECT * from doc where doc.id = "&id
else
    Sql="SELECT * from doc"
End if

rs.Open Sql,BbsDB,1, 3

for each formName in Uploader.file					' �г������ϴ��˵��ļ�
	set file=Uploader.file(formName)				' ����һ���ļ�����
	If id<>0 then  
		rs("DocID")	      =	Uploader.Form("DocID")
		rs("DocTitle")	  =	Uploader.Form("DocTitle")
		rs("DocContent")  = Uploader.FileData(formname)	
		rs("DocType")	  = Uploader.Form("DocType")
	else
		rs.AddNew
		rs("DocID")	     = Uploader.Form("DocID")
		rs("DocTitle")	 = Uploader.Form("DocTitle")
		rs("DocContent") = Uploader.FileData(formname)    ''�����ļ�
		rs("Docdata")	 = Now() 
		rs("DocType")	 = Uploader.Form("DocType")
	End If
 Response.Write "OK"
 set file=nothing
 rs.Update
exit for 
next

rs.Close
Set rs=Nothing
BbsDB.close
set BbsDB = Nothing
Set	Uploader = Nothing

%>
