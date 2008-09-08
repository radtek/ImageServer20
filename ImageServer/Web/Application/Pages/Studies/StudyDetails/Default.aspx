<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Pages/Common/MainContentSection.Master" Codebehind="Default.aspx.cs" 
    Inherits="ClearCanvas.ImageServer.Web.Application.Pages.Studies.StudyDetails.Default" %>

<%@ Register Src="Controls/PatientSummaryPanel.ascx" TagName="PatientSummaryPanel" TagPrefix="localAsp" %>
<%@ Register Src="Controls/StudyDetailsPanel.ascx" TagName="StudyDetailsPanel" TagPrefix="localAsp" %>
<%@ Register Src="Controls/EditStudyDetailsDialog.ascx" TagName="EditStudyDetailsDialog" TagPrefix="localAsp" %>
<%@ Register Src="Controls/StudyDetailsTabs.ascx" TagName="StudyDetailsTabs" TagPrefix="localAsp" %>

<asp:Content runat="server" ID="MainMenuContent" contentplaceholderID="MainMenuPlaceHolder">
    <asp:Table ID="Table1" runat="server"><asp:TableRow><asp:TableCell HorizontalAlign="right" style="padding-top: 12px;"><asp:LinkButton ID="LinkButton1" runat="server" SkinId="CloseButton" OnClientClick="javascript: window.close(); return false;" /></asp:TableCell></asp:TableRow></asp:Table>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentSectionPlaceHolder" runat="server">
            <asp:UpdatePanel runat="server" ID="updatepanel" UpdateMode="Conditional">
                <ContentTemplate>
                    <localAsp:StudyDetailsPanel ID="StudyDetailsPanel" runat="server" />
                    <localAsp:EditStudyDetailsDialog ID="EditStudyDialog" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>