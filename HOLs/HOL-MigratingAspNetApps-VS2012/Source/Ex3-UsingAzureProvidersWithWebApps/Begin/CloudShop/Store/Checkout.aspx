<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="CloudShop.Store.Checkout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Cloud Shop Check Out
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FeaturedContent" runat="server">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Your Order</h1>
            </hgroup>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <label for="cart">You have selected the following products:</label>
    <p>
    <asp:ListBox ID="cart" runat="server" EnableViewState="false" CssClass="product-list" />
    </p>
    <p>
    <asp:LinkButton ID="RemoveItem" runat="server" Text="Remove product from cart" onclick="RemoveItem_Click" />
    </p>
</asp:Content>
