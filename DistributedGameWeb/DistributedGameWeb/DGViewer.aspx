<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeFile="DGViewer.aspx.cs" Inherits="_Default" %>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server"> <title>DistributedGame Web Viewer</title></head>
    <body>
        <form id="frmMain" runat="server">
            <asp:menu runat="server" OnMenuItemClick="Unnamed1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Login"></asp:MenuItem>
                    <asp:MenuItem Text="Friends"></asp:MenuItem>
                    <asp:MenuItem Text="HeroSelect"></asp:MenuItem>
                </Items>
            </asp:menu>
        </form>
    </body>
</html>
        
    