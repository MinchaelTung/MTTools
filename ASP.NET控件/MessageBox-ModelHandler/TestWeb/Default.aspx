<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestWeb._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SM">
    </asp:ScriptManager>
    <table id="table1" width="500">
        <thead>
            <tr>
                <td style="width: 30%">
                    姓名：
                </td>
                <td style="width: 50%">
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                </td>
                <td style="width: 20%">
                </td>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>
                    密码：
                </td>
                <td>
                    <asp:TextBox ID="txtPaw" runat="server"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <center>
                        <asp:Button ID="btnOK" runat="server" Text="提交" Width="100" OnClick="btnOK_Click" />
                    </center>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <center>
                        <asp:Button ID="Button1" runat="server" Text="提交1" Width="100" OnClick="Button1_Click" />
                    </center>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <center>
                        <asp:Button ID="Button2" runat="server" Text="提交2" Width="100" OnClick="Button2_Click" />
                    </center>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <center>
                        <asp:Button ID="Button3" runat="server" Text="提交3" Width="100" OnClick="Button3_Click" />
                    </center>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <center>
                        <asp:Button ID="Button4" runat="server" Text="提交4" Width="100" OnClick="Button4_Click" />
                    </center>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <center>
                        <asp:Button ID="Button5" runat="server" Text="提交5" Width="100" OnClick="Button5_Click" />
                    </center>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <center>
                        <asp:Button ID="Button6" runat="server" Text="提交6" Width="100" OnClick="Button6_Click" />
                    </center>
                </td>
            </tr>
        </tbody>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    </asp:UpdatePanel>
    </form>
</body>
</html>
