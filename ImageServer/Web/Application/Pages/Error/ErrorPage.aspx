<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" MasterPageFile="ErrorPageMaster.Master" Inherits="ClearCanvas.ImageServer.Web.Application.Pages.Error.ErrorPage" %>

<asp:Content runat="server" ContentPlaceHolderID="ErrorMessagePlaceHolder">
	    <asp:label ID="ErrorMessageLabel" Text="Something happened that the ImageServer was unprepared for." runat="server" />
</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="DescriptionPlaceHolder">
    <asp:Label ID = "DescriptionLabel" runat="server">
        This message is being displayed because the ClearCanvas ImageServer encountered a 
        situation that was unexpected. The resulting error has been recorded for future analysis.
         <br/><br/>
         If you would like to report the error, please post to one of the forums listed below and 
         provide any information you think will be helpful in handling this situation in the future. 
         <asp:panel ID="StackTraceMessage" runat="server" Visible="false">To include the ImageServer's error message, <a class="ErrorLink" href="javascript:toggleLayer('StackTrace');">click here</a>.</asp:panel>
    </asp:Label>
    <div id="StackTrace" style="margin-top: 15px" visible="false"><asp:TextBox runat="server" ID="StackTraceTextBox" Visible="false" Rows="5" Columns="57" TextMode="MultiLine" ReadOnly="true"></asp:TextBox></div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="UserEscapePlaceHolder">
    <div class="UserEscapeContainer" ><a href="../../Default.aspx" class="UserEscapeLink" style="">Return to Study Search</a> <a href="javascript:window.close()" class="UserEscapeLink">Close Window</a></div>
</asp:Content>