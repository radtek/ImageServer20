<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ServiceLockPanel.ascx.cs"
    Inherits="ClearCanvas.ImageServer.Web.Application.Pages.Configure.ServiceLocks.ServiceLockPanel" %>
<%@ Register Src="ServiceLockGridView.ascx" TagName="ServiceLockGridView" TagPrefix="localAsp" %>
<%@ Register Src="EditServiceLockDialog.ascx" TagName="EditServiceLockDialog" TagPrefix="localAsp" %>

<asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    
    <div>
        <b class="roundedCorners"><b class="roundedCorners1"><b></b></b><b class="roundedCorners2">
            <b></b></b><b class="roundedCorners3"></b><b class="roundedCorners4"></b><b class="roundedCorners5">
            </b></b>
        <div class="roundedCornersfg">          
        
            <asp:Table ID="Table" runat="server" Width="100%" CellPadding="0">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="right" VerticalAlign="Bottom" Wrap="false">
                                <asp:Panel ID="Panel6" runat="server" CssClass="SearchPanelContent" DefaultButton="SearchToolbarButton">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="Label1" runat="server" Text="Type" CssClass="SearchTextBoxLabel"
                                                    EnableViewState="False"></asp:Label><br />
                                                <asp:DropDownList ID="TypeDropDownList" runat="server" CssClass="SearchDropDownList">
                                                </asp:DropDownList>
                                                </td>
                                            <td align="left" valign="bottom">
                                                <asp:Label ID="Label3" runat="server" Text="Status" CssClass="SearchTextBoxLabel"></asp:Label><br />
                                                <asp:DropDownList ID="StatusFilter" runat="server" CssClass="SearchDropDownList">
                                                </asp:DropDownList></td>
                                            <td align="left" valign="bottom">
                                                <asp:Panel ID="SearchButtonContainer" runat="server" CssClass="SearchButtonContainer">                                                        
                                                    <ccUI:ToolbarButton
                                                        ID="SearchToolbarButton" runat="server" 
                                                        EnabledImageURL="~/images/icons/QueryEnabled.png" 
                                                        DisabledImageURL="~/images/icons/QueryDisabled.png"
                                                        OnClick="SearchButton_Click" Tooltip="Search Service Schedules"
                                                        />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <table width="100%" cellpadding="2" cellspacing="0" class="ToolbarButtonPanel">
                            <tr><td >
                            <asp:UpdatePanel ID="ToolbarUpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="ToolbarButtons" runat="server" CssClass="ToolbarButtons"><asp:Button runat="server" ID="EditServiceScheduleButton" Text="Edit" CssClass="ButtonStyle" /></asp:Panel>
                             </ContentTemplate>
                          </asp:UpdatePanel>                  
                        </td></tr>
                        <tr><td>

                         <asp:Panel ID="Panel2" runat="server" style="border: solid 1px #3d98d1; ">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                 <tr><td style="border-bottom: solid 1px #3d98d1"><ccAsp:GridPager ID="GridPagerTop" runat="server" /></td></tr>                        
                                <tr><td style="background-color: white;"><localAsp:ServiceLockGridView  ID="ServiceLockGridViewControl" Height="500px" runat="server" /></td></tr>
                                <tr><td style="border-top: solid 1px #3d98d1"><ccAsp:GridPager ID="GridPagerBottom" runat="server" /></td></tr>                    
                            </table>                        
                        </asp:Panel>
                        </td>
                        </tr>
                        </table>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

       </div>
        <b class="roundedCorners"><b class="roundedCorners5"></b><b class="roundedCorners4">
        </b><b class="roundedCorners3"></b><b class="roundedCorners2"><b></b></b><b class="roundedCorners1">
            <b></b></b></b>
    </div>
        
        <ccAsp:ConfirmationDialog ID="ConfirmEditDialog" runat="server" />
        <localAsp:EditServiceLockDialog ID="EditServiceLockDialog" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
