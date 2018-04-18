﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocEdit.aspx.cs" Inherits="DocEdit" %>
<%@ Import Namespace="System.Data.OleDb"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
 
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>编辑正文</title>

</head>
<body>
    <form name="myform" action="#" method="post">
      <% //获取服务器的地址
        string URL = this.Session["URL"].ToString();
        string ID;
        string DocType;
        string DocTitle = "";
        //起草文件,则ID为NULL,否则为记录的主键
        ID = Request.QueryString["ID"];
        if (ID == null || ID == "")
        {
            //获取新建文件的类型         
            DocType = Request.Form["DocType"];
        }
        else
        {
            //获取DocTitle
            DocTitle = Request.QueryString["DocTitle"];
            //获取文件的 类型       
            DocType = Request.QueryString["DocType"];
        }
 %>  
     <TABLE cellSpacing=0 cellPadding=0 width="100%" border=0>
          <TR> 
            <TD bgColor=#3366cc><IMG height=1 alt="" width=1></TD>
          </TR>
    </TABLE>

   <table style="width: 100%" border = "0" cellpadding="0" cellspacing="0">
       <tr>
           <td valign="top" bgColor="#e5ecf9" style="height: 21px;"><p align="left"><b><strong>WebOffice演示程序</strong></b>
    </p></td>
       </tr>
   </table>
   <br /> <br />
    <table width="100%"  border="0" cellpadding="0" cellspacing="1" bgcolor="#3366cc">

    <tr bgcolor="#FFFFFF">
        <td bgcolor="#ffffff" rowspan="2">
            <div align="center"><strong>标题</strong></div>
        </td>
        <td rowspan="2">
            <input name="DocTitle" value= "<%if(DocTitle=="" ) {Response.Write("text");} else {Response.Write(DocTitle);} %>" size="14" id="Text1"/></td>
		  <td  rowspan="2" >
              &nbsp; &nbsp;<input name="DocFilePath" type="file" size="34" />
	      <input type="button" value="打开本地文件" onclick="return docOpen()" id="Button1" style="width: 115px" /></td>
		  <td rowspan="2" bgcolor="#FFFFFF" style="width: 344px"><div align="center">
            <input type="button"  value="上传到服务器"  onclick="return SaveDoc()"/> 
              <input type="button" value="返回"  onclick="return return_onclick()"/></div></td>
				</tr>

    <tr bgcolor="#FFFFFF">
	</tr>

    <tr bgcolor="#FFFFFF">
	  <td valign="top" bgcolor="#FFFFFF" style="height: 560px">
		<!-- -------------------===  Start 嵌套Table ===------------------------------- -->
		<table width="100%"  border="0" cellpadding="0">

          <tr>
            <td  class="maintxt">用户名：<br />
              <input name="UserName" type="text" value="Test" style="width:74px;" maxlength="10" /><br />
                <input type="button" value="设置用户"  onclick="return SetUserName()" class="btn" /></td>
		  </tr>

          <tr><td class="maintxt">
			<hr  size="1" />保护密码：<br />
              <input name="docPwd" type="text" value="Password" style="width:74px;" maxlength="10" /><br />
			    <input type="button" class="btn" value="保护文档" onclick="return ProtectFull()" /></td>
		  </tr>

          <tr><td><input type="button" class="btn" value="解除保护" onclick="return UnProtect()" /></td></tr>

          <tr><td>
			<hr  size="1" />
			  <input  type="button" class="btn" value="修订文档" onclick="return ProtectRevision()" /></td>
		  </tr>

          <tr><td><input type="button" class="btn" value="显示修订" onclick="return ShowRevisions()" /></td></tr>
          <tr><td><input type="button" class="btn" value="隐藏修订" onclick="return UnShowRevisions()" /></td></tr>
          <tr><td><input type="button" class="btn" value="接受修订" onclick="return AcceptAllRevisions()" /></td></tr>

          <tr><td>
			<hr size="1" />
             <input  type="button" class="btn" value="设置书签" onclick="return addBookmark()" /></td>
	      </tr>

          <tr>
              <td rowspan="4">
                  <input  type="button" class="btn" value="套加红头" onclick="return addRedHead()" /></td>
          </tr>
        </table>
	    <!-- -------------------=== End 嵌套Table ===------------------------------- -->
	  </td>

	  <td colspan="3" valign="top" style="height: 560px">
		<!-- -----------------------------== 装载weboffice控件 ==----------------------------------->	  
				<script src="LoadWebOffice.js"></script>
        <!-- --------------------------------==  结束装载控件 ==------------------------------------->
	  </td>
    </tr>
  </table>
  <br />  <br />
          <table style="width: 100%" border = "0" cellpadding="0" cellspacing="0">
               <tr>
                   <td valign="top" bgColor="#e5ecf9" ><span style="color: red">服务器地址:<%=this.Session["URL"].ToString() %> </span>
                </td>
               </tr>
           </table>
          <TABLE cellSpacing=0 cellPadding=0 width="100%" border=0>
          <TR> 
            <TD bgColor=#3366cc><IMG height=1 alt="" width=1></TD>
          </TR>
    </TABLE>
</form>
</body>
</html>
<script language="javascript" type="text/javascript">
// ---------------------== 关闭页面时调用此函数，关闭文档--------------------- //
function window_onunload() {
	document.all.WebOffice1.Close();
}
 
// ---------------------------== 解除文档保护 ==---------------------------------- //
function UnProtect() {
	document.all.WebOffice1.ProtectDoc(0,1, myform.docPwd.value);
}

// ---------------------------== 设置文档保护 ==---------------------------------- //
function ProtectFull() {
	document.all.WebOffice1.ProtectDoc(1,1, myform.docPwd.value);
}
// -----------------------------== 修订文档 ==------------------------------------ //
function ProtectRevision() {
	document.all.WebOffice1.SetTrackRevisions(1) 
}

// -----------------------------== 隐藏修订 ==------------------------------------ //
function UnShowRevisions() {
	document.all.WebOffice1.ShowRevisions(0);
}

// --------------------------== 显示当前修订 ==---------------------------------- //
function ShowRevisions() {
	document.all.WebOffice1.ShowRevisions(1);

}

// -------------------------== 接受当前所有修订 ------------------------------ //
function AcceptAllRevisions() {
 	document.all.WebOffice1.SetTrackRevisions(4);
}

// ---------------------------== 设置当前操作用户 ==------------------------------- //
function SetUserName() {
	if(myform.UserName.value ==""){
		alert("用户名不能为空");
		myform.UserName.focus();
		return false;
	}
 	document.all.WebOffice1.SetCurrUserName(myform.UserName.value);
}

// -------------------------=== 设置书签套加红头 ===------------------------------ //
function addBookmark() {
	document.all.WebOffice1.SetFieldValue("mark_1", "北京点聚信息技术有限公司::ADDMARK::");			
}

// -------------------------=== 设置书签套加红头 ===------------------------------ //
function addRedHead() {
	document.all.WebOffice1.SetFieldValue("mark_1", "", "::ADDMARK::");			// 添加书签
	document.all.WebOffice1.SetFieldValue("mark_1", "<%=URL %>/template/tmp1.doc", "::FILE::");
}

// -----------------------------== 返回首页 ==------------------------------------ //
function return_onclick() {
	document.all.WebOffice1.Close();
	window.location.href  = "Default.aspx"
}
// 打开本地文件
function docOpen() {
    //alert(myform.DocFilePath.value)
	if(myform.DocFilePath.value == "") {
		alert("文件路径不可以为空");
		myform.DocFilePath.focus();
		return false;
	}
	var flag;
	//LoadOriginalFile接口装载文件

	flag = document.all.WebOffice1.LoadOriginalFile(myform.DocFilePath.value,"<%=DocType%>");
	if( 0 == flag){
		alert("文件打开失败，请检查路径是否正确");
		myform.DocFilePath.focus();
		return false;
	}	
}
 
// -----------------------------== 保存文档 ==------------------------------------ //
function SaveDoc() {
	 if(myform.DocTitle.value ==""){
		alert("标题不可为空")
		myform.DocTitle.focus();
		return false;
	}
	//恢复被屏蔽的菜单项和快捷键
    document.all.WebOffice1.SetToolBarButton2("Standard",1,3);
    document.all.WebOffice1.SetToolBarButton2("Standard",2,3);
    document.all.WebOffice1.SetToolBarButton2("Standard",3,3);
    document.all.WebOffice1.SetToolBarButton2("Standard",6,3);           
    <%if (DocType   == "doc") {%>
        //恢复文件菜单项
        document.all.WebOffice1.SetToolBarButton2("Menu Bar",1,4);
        //恢复 保存快捷键(Ctrl+S) 
        document.all.WebOffice1.SetKeyCtrl(595,0,0);
        //恢复 打印快捷键(Ctrl+P) 
        document.all.WebOffice1.SetKeyCtrl(592,0,0);
    <%}else if(DocType   == "xls") {%>
        //恢复文件菜单项
        document.all.WebOffice1.SetToolBarButton2("Worksheet Menu Bar",1,4);         
    <%} %>
//初始化Http引擎
	document.all.WebOffice1.HttpInit();			
//添加相应的Post元素
	<%
	if(ID != ""){          
	%>
	document.all.WebOffice1.SetTrackRevisions(0);
	document.all.WebOffice1.ShowRevisions(0);
	document.all.WebOffice1.HttpAddPostString("ID","<%=ID%>");
	 <%
	 }
	 %>	
	document.all.WebOffice1.HttpAddPostString("DocTitle", myform.DocTitle.value);
	document.all.WebOffice1.HttpAddPostString("DocType","<%=DocType%>");
	//把当前文档添加到Post元素列表中，文件的标识符䶿DocContent
	document.all.WebOffice1.HttpAddPostCurrFile("DocContent","");		// 涓婁紶鏂囦欢
	var vtRet;
    //HttpPost执行上传的动仿WebOffice支持Http的直接上传，在upload.aspx的页面中,解析Post过去的数慿
    //拆分出Post元素和文件数据，可以有选择性的保存到数据库中，或保存在服务器的文件中⾿
	//HttpPost的返回值，根据upload.aspx中的设置，返回upload.aspx中Response.Write回来的数据
	vtRet = document.all.WebOffice1.HttpPost("<%=URL %>/upload.aspx");

 	//alert(vtRet);
 	if("succeed" == vtRet){
		alert("文件上传成功");	
	}else{
		alert("文件上传失败");
	}
	return_onclick(); 
}
//-->
</script>
<SCRIPT ID=clientEventHandlersJS LANGUAGE=javascript>
<!--

function WebOffice1_NotifyCtrlReady() {
	//LoadOriginalFile接口装载文件,
	//如果是编辑已有文件，则文件路径传给LoadOriginalFile的第一个参数
	<% 
     if (ID == null || ID==""){
    %>
        document.all.WebOffice1.LoadOriginalFile("","<%=DocType%>");
	<%} 
	else 
	{%>        
        document.all.WebOffice1.LoadOriginalFile("<%=URL %>/GetDoc.aspx?ID=<%=ID%>","<%=DocType%>");
 	    document.all.WebOffice1.SetTrackRevisions(1);
        document.all.WebOffice1.ShowRevisions(1);    
    <%}%>         
	

    //屏蔽标准工具栏的前几个按钮
    document.all.WebOffice1.SetToolBarButton2("Standard",1,1);
    document.all.WebOffice1.SetToolBarButton2("Standard",2,1);
    document.all.WebOffice1.SetToolBarButton2("Standard",3,1);
    document.all.WebOffice1.SetToolBarButton2("Standard",6,1);    
           
   <%if (DocType   == "doc") {%>
        //屏蔽文件菜单项
        document.all.WebOffice1.SetToolBarButton2("Menu Bar",1,1);
        //屏蔽 保存快捷键(Ctrl+S) 
        document.all.WebOffice1.SetKeyCtrl(595,-1,0);
        //屏蔽 打印快捷键(Ctrl+P) 
        document.all.WebOffice1.SetKeyCtrl(592,-1,0);
    <%}else if(DocType   == "xls") {%>
        //屏蔽文件菜单项
        document.all.WebOffice1.SetToolBarButton2("Worksheet Menu Bar",1,1);         
    <%} %>
}

//-->
</SCRIPT>
<!-- --------------------===  Weboffice初始化完成事件--------------------- -->

<SCRIPT LANGUAGE=javascript FOR=WebOffice1 EVENT=NotifyCtrlReady>
<!--
 WebOffice1_NotifyCtrlReady() // 在装载完Weboffice(执行<object>...</object>)控件后自动执行WebOffice1_NotifyCtrlReady方法
//-->
</SCRIPT>