<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Register.aspx.cs" Inherits="CloudShop.Account.Register" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   <h1>Registration</h1>
   <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" CssClass="login" CreateUserButtonType="Link"
        CancelButtonType="Link" ContinueButtonType="Link" DisplayCancelButton="true"
        StartNextButtonType="Link" StepNextButtonType="Link" StepPreviousButtonType="Link"
        CancelDestinationPageUrl="~/Account/Login.aspx" ContinueDestinationPageUrl="~/Store/Products.aspx">
        <WizardSteps>
            <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
            </asp:CreateUserWizardStep>
            <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
            </asp:CompleteWizardStep>
        </WizardSteps>
   </asp:CreateUserWizard>
</asp:Content>
