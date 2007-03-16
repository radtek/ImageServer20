using System;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Network;
using ClearCanvas.ImageViewer.StudyManagement;
using ClearCanvas.ImageViewer.Shreds.DicomServer;

namespace ClearCanvas.ImageViewer.Explorer.Dicom
{
    [ButtonAction("activate", "dicomstudybrowser-toolbar/Send")]
    [MenuAction("activate", "dicomstudybrowser-contextmenu/Send")]
    [ClickHandler("activate", "SendStudy")]
    [EnabledStateObserver("activate", "Enabled", "EnabledChanged")]
    [Tooltip("activate", "Send Study")]
    [IconSet("activate", IconScheme.Colour, "Icons.SendStudySmall.png", "Icons.SendStudySmall.png", "Icons.SendStudySmall.png")]
    [ExtensionOf(typeof(StudyBrowserToolExtensionPoint))]
    public class SendStudyTool : StudyBrowserTool
    {
        public SendStudyTool()
        {

        }

        private void SendStudy()
        {
            if (this.Context.SelectedStudy == null)
                return;

            AENavigatorComponent aeNavigator = new AENavigatorComponent();
            DialogContent content = new DialogContent(aeNavigator);
            DialogComponentContainer dialogContainer = new DialogComponentContainer(content);

            ApplicationComponentExitCode code =
                ApplicationComponent.LaunchAsDialog(
                    this.Context.DesktopWindow,
                    dialogContainer,
					SR.TitleSendStudy);

            if (code == ApplicationComponentExitCode.Cancelled)
                return;

            ApplicationEntity destinationAE = aeNavigator.SelectedServers.Servers[0].DicomAE;

            if (destinationAE == null)
            {
				Platform.ShowMessageBox(SR.MessageSelectDestination);
                return;
            }

			DicomSendRequest request = new DicomSendRequest();
			request.DestinationAETitle = destinationAE.AE;
			request.DestinationHostName = destinationAE.Host;
			request.Port = destinationAE.Port;
			
			List<string> studyUids = new List<string>();
			foreach (StudyItem item in this.Context.SelectedStudies)
				studyUids.Add(item.StudyInstanceUID);
			
			request.Uids = studyUids.ToArray();

			DicomMoveRequestServiceClient client = new DicomMoveRequestServiceClient();

			try
			{
				client.Open();
				client.Send(request);
				client.Close();
			}
			catch (Exception e)
			{
				ExceptionHandler.Report(e, this.Context.DesktopWindow);
			}
        }

        protected override void OnSelectedStudyChanged(object sender, EventArgs e)
        {
            // If the results aren't from the local machine, then we don't
            // even care whether a study has been selected or not
            if (!this.Context.SelectedServerGroup.IsLocalDatastore)
                return;

            base.OnSelectedStudyChanged(sender, e);
        }

        protected override void OnSelectedServerChanged(object sender, EventArgs e)
        {
            // If no study is selected then we don't even care whether
            // the last searched server has changed.
            if (this.Context.SelectedStudy == null)
                return;

            if (this.Context.SelectedServerGroup.IsLocalDatastore)
                this.Enabled = true;
            else
                this.Enabled = false;
        }
    }
}
