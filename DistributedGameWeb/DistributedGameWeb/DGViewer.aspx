<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeFile="DGViewer.aspx.cs" Inherits="_Default" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server"> <title>DisributedGame Web Viewer</title></head>
    <body>
        <form id="frmMain" runat="server">
            <div>
                <asp:Menu runat="server">
                    <Items>
                        <asp:MenuItem Text="Login"></asp:MenuItem>
                        <asp:MenuItem Text="HeroSelect"></asp:MenuItem>
                        <asp:MenuItem Text="Friends"></asp:MenuItem>
                    </Items>
                </asp:Menu> 
            </div>
            <div>
                <table id="friendTable" runat="server">

                </table>
            </div>
            <div>
                
            </div>
            <div>
                <table id="ServerTable"></table>
            </div>
        </form>
    </body>
</html>

