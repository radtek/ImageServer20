<%@ Control Language="C#" AutoEventWireup="true" Inherits="ClearCanvas.ImageServer.Web.Application.Search.StudyListGridView"
    Codebehind="StudyListGridView.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="ClearCanvas.ImageServer.Web.Common" Namespace="ClearCanvas.ImageServer.Web.Common.WebControls.UI" TagPrefix="clearcanvas" %>


<asp:Panel ID="Panel1" runat="server" CssClass="CSSGridViewPanelContainer">
    <asp:Panel ID="Panel3" runat="server" CssClass="CSSGridViewPanelBorder">
        <asp:Panel ID="Panel4" runat="server" CssClass="CSSGridViewPanelContent">
            <clearcanvas:GridView ID="StudyListControl" runat="server" AutoGenerateColumns="False"  
                CssClass="CSSGridView"
                Width="100%" OnRowDataBound="StudyListControl_RowDataBound" 
                OnDataBound="StudyListControl_DataBound"                
                EmptyDataText="" 
                CellPadding="0"
                OnSelectedIndexChanged="StudyListControl_SelectedIndexChanged" 
                OnSelectedIndexChanging="StudyListControl_SelectedIndexChanging"
                OnPageIndexChanging="StudyListControl_PageIndexChanging"  
                SelectionMode="Multiple"
                PageSize="20" CellSpacing="0" AllowPaging="True" CaptionAlign="Top" BorderWidth="0px">
                <Columns>
                    <asp:TemplateField HeaderText="Patient Name">
                        <ItemTemplate>
                            <clearcanvas:PersonNameLabel ID="PatientName" runat="server" PersonName='<%# Eval("PatientsName") %>' PersonNameType="Dicom"></clearcanvas:PersonNameLabel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PatientID" HeaderText="Patient ID"></asp:BoundField>
                    <asp:BoundField DataField="AccessionNumber" HeaderText="Accession #"></asp:BoundField>
                    <asp:TemplateField HeaderText="Study Date">
                        <ItemTemplate>
                            <clearcanvas:DALabel ID="StudyDate" runat="server" Value='<%# Eval("StudyDate") %>'></clearcanvas:DALabel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="StudyDescription" HeaderText="Description"></asp:BoundField>
                    <asp:BoundField DataField="NumberOfRelatedSeries" HeaderText="Series"></asp:BoundField>
                    <asp:BoundField DataField="NumberOfRelatedInstances" HeaderText="Instances"></asp:BoundField>
                    
                </Columns>
                <EmptyDataTemplate>
                    <asp:Table ID="Table1" runat="server" Width="100%" CellPadding="0" CellSpacing="0" CssClass="CSSGridHeader">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell>Patient Name</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Patient ID</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Accession #</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Study Date</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Description</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Series</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Instances</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                   
                </EmptyDataTemplate> 
                <RowStyle CssClass="CSSGridRowStyle" />
                <AlternatingRowStyle CssClass="CSSGridAlternatingRowStyle" />
                <SelectedRowStyle CssClass="CSSGridSelectedRowStyle" />
                <HeaderStyle CssClass="CSSGridHeader" />
                <PagerTemplate>
                </PagerTemplate>
            </clearcanvas:GridView>
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
