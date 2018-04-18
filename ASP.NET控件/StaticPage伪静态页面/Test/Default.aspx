<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Test._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
</head>
<body>
  <form id="form1" runat="server">
    <html:HtmlPanel ID="Pnl" runat="server">
      <asp:HyperLink ID="Hl" runat="server" NavigateUrl="~/Default.aspx?a=1&b=3" Text="Default" /><br />
      <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Admin/Default.aspx?a=1&b=3" Text="Admin" /><br />
      <asp:Panel runat="server">
        <a href="Default.aspx" runat="server">Hello</a><br />
        <a href="~/Admin/Default.aspx?ok=k&b=1" runat="server">World</a><br />
      </asp:Panel>
    </html:HtmlPanel>
    
    <html:HtmlPagger ID="pager" runat="server" NumberCount="10" />
  </form>
</body>
</html>
