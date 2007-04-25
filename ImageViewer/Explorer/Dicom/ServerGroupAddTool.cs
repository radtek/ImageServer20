using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Desktop.Actions;

namespace ClearCanvas.ImageViewer.Explorer.Dicom
{
    [ButtonAction("activate", "dicomaenavigator-toolbar/ToolbarAddServerGroup")]
    [MenuAction("activate", "dicomaenavigator-contextmenu/MenuAddServerGroup")]
    [ClickHandler("activate", "AddNewServerGroup")]
    [EnabledStateObserver("activate", "Enabled", "EnabledChanged")]
	[Tooltip("activate", "TooltipAddServerGroup")]
    [IconSet("activate", IconScheme.Colour, "Icons.AddGroup.png", "Icons.AddGroup.png", "Icons.AddGroup.png")]
    [ExtensionOf(typeof(AENavigatorToolExtensionPoint))]
    public class ServerGroupAddTool : AENavigatorTool
    {
        public ServerGroupAddTool()
        {
        }

        private void AddNewServerGroup()
        {
            ServerTree _serverTree = this.Context.ServerTree;
            this.Context.UpdateType = (int)ServerUpdateType.Add;
            DicomServerGroupEditComponent editor = new DicomServerGroupEditComponent(_serverTree, ServerUpdateType.Add);
			ApplicationComponentExitCode exitCode = ApplicationComponent.LaunchAsDialog(this.Context.DesktopWindow, editor, SR.TitleAddServerGroup);
			this.Context.UpdateType = (int)ServerUpdateType.None; 
			return;
        }

        protected override void OnSelectedServerChanged(object sender, EventArgs e)
        {
            this.Enabled = !this.Context.ServerTree.CurrentNode.IsServer;
        }
    }
}
