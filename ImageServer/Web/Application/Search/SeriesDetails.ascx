<%@ Control Language="C#" AutoEventWireup="true" Codebehind="SeriesDetails.ascx.cs"
    Inherits="ClearCanvas.ImageServer.Web.Application.Search.SeriesDetails" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Panel runat="server" ID="DetailsPanel" CssClass="toolBarPanel">
<asp:Label runat="server" ID="SeriesLabel" CssClass="sectionLabel" Text="Series:"></asp:Label>
    <hr class="sectionDivLine" />
    <asp:Table ID="DetailsTable" runat="server">
    </asp:Table>
</asp:Panel>
