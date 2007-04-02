using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Desktop;

namespace ClearCanvas.ImageViewer.Services.Tools
{
	public class LocalDataStoreActivityMonitorComponentManager
	{
		private static IWorkspace _importComponentWorkspace;
		private static IWorkspace _reindexComponentWorkspace;
		private static IWorkspace _dicomSendReceiveActivityComponentWorkspace;

		private LocalDataStoreActivityMonitorComponentManager()
		{
		}

		public static void ShowSendReceiveActivityComponent(IDesktopWindow desktopWindow)
		{
			if (_dicomSendReceiveActivityComponentWorkspace != null)
			{
				_dicomSendReceiveActivityComponentWorkspace.Activate();
			}
			else
			{
				try
				{
					ReceiveQueueApplicationComponent receiveComponent = new ReceiveQueueApplicationComponent();
					SendQueueApplicationComponent sendComponent = new SendQueueApplicationComponent();

					SplitPane topPane = new SplitPane(SR.TitleReceive, receiveComponent, 0.5F);
					SplitPane bottomPane = new SplitPane(SR.TitleSend, sendComponent, 0.5F);

					SplitComponentContainer container = new SplitComponentContainer(topPane, bottomPane, SplitOrientation.Horizontal);

					_dicomSendReceiveActivityComponentWorkspace = ApplicationComponent.LaunchAsWorkspace(desktopWindow, container, SR.DicomSendReceiveActivity,
						delegate(IApplicationComponent closingComponent)
						{
							_dicomSendReceiveActivityComponentWorkspace = null;
						});
				}
				catch
				{
					_dicomSendReceiveActivityComponentWorkspace = null;
					throw;
				}
			}
		}

		public static void ShowImportComponent(IDesktopWindow desktopWindow)
		{
			if (_importComponentWorkspace != null)
			{
				_importComponentWorkspace.Activate();
				return;
			}

			try
			{
				DicomFileImportApplicationComponent component = new DicomFileImportApplicationComponent();
				_importComponentWorkspace = ApplicationComponent.LaunchAsWorkspace(desktopWindow, component, SR.TitleImportActivity,
					delegate(IApplicationComponent closingComponent)
					{
						_importComponentWorkspace = null;
					});
			}
			catch
			{
				_importComponentWorkspace = null;
				throw;
			}
		}

		public static void ShowReindexComponent(IDesktopWindow desktopWindow)
		{
			if (_reindexComponentWorkspace != null)
			{
				_reindexComponentWorkspace.Activate();
				return;
			}

			try
			{
				//DicomFileImportApplicationComponent component = new DicomFileImportApplicationComponent();
				//_reindexComponentWorkspace = ApplicationComponent.LaunchAsWorkspace(desktopWindow, component, "Reindex Activity",
				//    delegate(IApplicationComponent closingComponent)
				//    {
				//        _reindexComponentWorkspace = null;
				//    });
			}
			catch 
			{
				_reindexComponentWorkspace = null;
				throw;
			}
		}
	}
}
