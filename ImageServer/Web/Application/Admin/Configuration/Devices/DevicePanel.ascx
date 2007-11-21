<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DevicePanel.ascx.cs" Inherits="ClearCanvas.ImageServer.Web.Application.Admin.Configuration.Devices.DevicePanel" %>
<%@ Register Src="~/Common/GridPager.ascx" TagName="GridPager" TagPrefix="uc8" %>
<%@ Register Src="~/Common/ConfirmDialog.ascx" TagName="ConfirmDialog" TagPrefix="uc5" %>

<%@ Register Src="DeviceToolBar.ascx" TagName="DeviceToolBar" TagPrefix="uc2" %>
<%@ Register Src="DeviceFilterPanel.ascx" TagName="DeviceFilterPanel" TagPrefix="uc3" %>
<%@ Register Src="DeviceGridView.ascx" TagName="DeviceGridView" TagPrefix="uc1" %>
<asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
<asp:Panel ID="Panel1" runat="server"  style="padding-right: 20px; padding-left: 20px; padding-bottom: 20px; padding-top: 10px" >

    <table width="100%">
        <tr class="toolBarPanel">
            <td colspan="1" style="width: 250px" >
                <uc2:DeviceToolBar ID="DeviceToolBarControl1" 
                        runat="server" />
            </td>
            <td align="right">
               <uc3:DeviceFilterPanel id="DeviceFilterPanel1" runat="server">
               </uc3:DeviceFilterPanel>
            </td>
        </tr>
        <tr>
            <td colspan="2" valign="top">
                <uc1:DeviceGridView ID="DeviceGridViewControl1" runat="server" />
            </td>
            
        </tr>
        <tr>
            <td colspan="2">
                <uc8:GridPager id="GridPager1" runat="server">
                </uc8:GridPager></td>
        </tr>
    </table>
   
</asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>


