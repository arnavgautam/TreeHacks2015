<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="CloudShop.Store.Products" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Cloud Shop Products
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%= User.IsInRole("Home") ? "Home" : "Enterprise" %> Products</h1>
            </hgroup>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<label for="items">Select a product from the list:</label>
    <p>
    <asp:ListBox ID="products" runat="server" EnableViewState="false" CssClass="product-list" />
    </p>
    <p>
       <asp:LinkButton ID="addItem" runat="server" Text="Add item to cart" OnClick="AddItem_Click"/>
    </p>
</asp:Content>
