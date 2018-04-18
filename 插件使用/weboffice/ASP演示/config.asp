<%
	dim appPath
	Dim tmpPath
	Dim n

	appPath = "http://"  & Request.ServerVariables("HTTP_HOST") & Request.ServerVariables("PATH_INFO")

	For n = Len(appPath) To 0 Step -1
		If Mid(appPath,n,1) = "/" then 
			tmpPath = left(appPath,n-1)
			Exit For
		End If
	Next
	Application("appPath") = tmpPath
	'Response.write(Application("appPath") & "<br>")
%>