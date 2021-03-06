<%@ Page Language="C#" MasterPageFile="~/GlobalMasterPage.master" AutoEventWireup="true"
    Codebehind="Default.aspx.cs" Inherits="ClearCanvas.ImageServer.Web.Application.Pages.Admin.Configure.ServerRules.Default"
    ValidateRequest="false" %>

<%@ Register Src="AddEditServerRuleDialog.ascx" TagName="AddEditServerRuleDialog" TagPrefix="localAsp" %>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="MainContentTitlePlaceHolder" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$Resources:Titles,ServerRules%>" /></asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:Panel runat="server" ID="PageContent">
        <asp:UpdatePanel ID="ServerRulePageUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <ccAsp:ServerPartitionTabs ID="ServerPartitionTabs" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    
    <ccAsp:MessageBox ID="ConfirmDialog" runat="server" />
    <localAsp:AddEditServerRuleDialog ID="AddEditServerRuleControl" runat="server" />
</asp:Content>
