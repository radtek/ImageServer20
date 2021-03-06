<%@ Control Language="C#" AutoEventWireup="true" Codebehind="SearchPanel.ascx.cs"
    Inherits="ClearCanvas.ImageServer.Web.Application.Pages.Queues.StudyIntegrityQueue.SearchPanel" %>

<%@ Register Src="StudyIntegrityQueueItemList.ascx" TagName="StudyIntegrityQueueItemList" TagPrefix="localAsp" %>

<asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="conditional">
    <ContentTemplate>
    
<script type="text/Javascript">

Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(MultiSelect);
Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(InputHover);

function MultiSelect() {
       
        $("#<%=ReasonListBox.ClientID %>").multiSelect({
            noneSelected: '',
            oneOrMoreSelected: '*',
            style: 'width: 150px;'
        });   

}
    </script>    

            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="right" VerticalAlign="Bottom" >
                    
                       <table cellpadding="0" cellspacing="0"  width="100%">
                            <!-- need this table so that the filter panel container is fit to the content -->
                            <tr>
                                <td align="left">
                                <asp:Panel ID="Panel6" runat="server" CssClass="SearchPanelContent" DefaultButton="SearchButton">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="bottom">
                                                <asp:Label ID="Label1" runat="server" Text="Patient Name" CssClass="SearchTextBoxLabel"
                                                    EnableViewState="False" /><br />
                                                <asp:TextBox ID="PatientName" runat="server" CssClass="SearchTextBox" />
                                            </td>
                                             <td align="left" valign="bottom">
                                                <asp:Label ID="Label4" runat="server" Text="Patient ID" CssClass="SearchTextBoxLabel"
                                                    EnableViewState="False" /><br />
                                                <asp:TextBox ID="PatientId" runat="server" CssClass="SearchTextBox" ToolTip="Search the list by Patient Id" />
                                            </td>
                                            <td align="left" valign="bottom">
                                                <asp:Label ID="Label3" runat="server" Text="Accession #" CssClass="SearchTextBoxLabel"
                                                    EnableViewState="False" /><br />
                                                <asp:TextBox ID="AccessionNumber" runat="server" CssClass="SearchTextBox" />
                                            </td>
                                            <td align="left" valign="bottom">
                                                <asp:Label ID="Label2" runat="server" Text="From Date " CssClass="SearchTextBoxLabel"
                                                    EnableViewState="False" />
                                                <asp:LinkButton ID="ClearFromDateButton" runat="server" Text="X" CssClass="SmallLink" style="margin-left: 10px;"/><br />
                                                <asp:TextBox ID="FromDate" runat="server" CssClass="SearchDateBox" />
                                            </td>
                                            <td align="left" valign="bottom">
                                                <asp:Label ID="Label6" runat="server" Text="To Date " CssClass="SearchTextBoxLabel"
                                                    EnableViewState="False" />
                                                <asp:LinkButton ID="ClearToDateButton" runat="server" Text="X" CssClass="SmallLink" style="margin-left: 2px;"/><br />
                                                <asp:TextBox ID="ToDate" runat="server" CssClass="SearchDateBox" />
                                            </td>                                            
                                            <td align="left" valign="bottom">
                                                <asp:Label ID="Label5" runat="server" Text="Reason" CssClass="SearchTextBoxLabel"
                                                    EnableViewState="False" /><br />
                                                <asp:ListBox runat="server" id="ReasonListBox" SelectionMode="Multiple">                                             
                                                </asp:ListBox>
                                            </td>
                                            <td valign="bottom">
                                                <asp:Panel ID="Panel1" runat="server" CssClass="SearchButtonPanel"><ccUI:ToolbarButton ID="SearchButton" runat="server" SkinID="SearchIcon" OnClick="SearchButton_Click" /></asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                    <ccUI:CalendarExtender ID="FromDateCalendarExtender" runat="server" TargetControlID="FromDate"
                                        CssClass="Calendar">
                                    </ccUI:CalendarExtender>
                                    <ccUI:CalendarExtender ID="ToDateCalendarExtender" runat="server" TargetControlID="ToDate"
                                        CssClass="Calendar">
                                    </ccUI:CalendarExtender>                                    
                                </td>
                            </tr>
                        </table>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <table width="100%" cellpadding="2" cellspacing="0" class="ToolbarButtonPanel">
                            <tr><td >
                            <asp:UpdatePanel ID="ToolBarUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="ToolbarButtons" runat="server" CssClass="ToolbarButtons">
                                        <ccUI:ToolbarButton ID="ReconcileButton" runat="server" SkinID="ReconcileButton" OnClick="ReconcileButton_Click" />
                                    </asp:Panel>
                             </ContentTemplate>
                          </asp:UpdatePanel>                  
                        </td></tr>
                        <tr><td>

                         <asp:Panel ID="Panel2" runat="server" style="border: solid 1px #3d98d1; ">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr><td style="border-bottom: solid 1px #3d98d1"><ccAsp:GridPager ID="GridPagerTop" runat="server" /></td></tr>                        
                                <tr><td style="background-color: white;"><localAsp:StudyIntegrityQueueItemList id="StudyIntegrityQueueItemList" runat="server" Height="500px"></localAsp:StudyIntegrityQueueItemList></td></tr>
                            </table>                        
                        </asp:Panel>
                        </td>
                        </tr>
                        </table>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

    </ContentTemplate>
</asp:UpdatePanel>
