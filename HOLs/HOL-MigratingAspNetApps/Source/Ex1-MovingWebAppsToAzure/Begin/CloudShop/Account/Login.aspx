<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="CloudShop.Account.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>
        Welcome to the Cloud Shop</h1>
    <asp:Login ID="Login1" runat="server" CssClass="login" DisplayRememberMe="false"
        LoginButtonType="Link" TitleText="Please login to continue" DestinationPageUrl="~/Store/Products.aspx" />
</asp:Content>
