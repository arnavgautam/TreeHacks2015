<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="SampleWebApp._Default" EnableViewStateMac="false" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <label>Name:</label>
    <asp:TextBox ID="Username" runat="server" />
    <asp:Button ID="ShowGreeting" runat="server" Text="Greet me!" onclick="ShowGreeting_Click" />
    <br />
    <h2>Legacy COM component says:</h2>
    <asp:Label ID="Message" runat="server" Font-Size="X-Large" ForeColor="Red" />
</asp:Content>
