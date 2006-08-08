using System;
using System.Collections.Generic;
using Gtk;
using ClearCanvas.Common;
//using ClearCanvas.Workstation.Model;
using ClearCanvas.Desktop;

//namespace ClearCanvas.ImageViewer.View.GTK
namespace ClearCanvas.Desktop.View.GTK
{
	[GuiToolkit(GuiToolkitID.GTK)]
	[ExtensionOf(typeof(ClearCanvas.Common.MessageBoxExtensionPoint))]
	public class MessageBox : IMessageBox
	{

		private static Dictionary<int, ButtonsType> _buttonMap;
		private static Dictionary<ResponseType, int> _resultMap;

		static MessageBox()
		{
			_buttonMap = new Dictionary<int, ButtonsType>();
            		_buttonMap.Add((int)MessageBoxActions.Ok, ButtonsType.Ok);
            		_buttonMap.Add((int)MessageBoxActions.OkCancel, ButtonsType.OkCancel);
            		_buttonMap.Add((int)MessageBoxActions.YesNo, ButtonsType.YesNo);

			// Not suppported by Gtk
            		//_buttonMap.Add((int)MessageBoxActions.YesNoCancel, ButtonsType.YesNoCancel);
			
			_resultMap = new Dictionary<ResponseType, int>();
			_resultMap.Add(ResponseType.Ok, (int)DialogBoxAction.Ok);
			_resultMap.Add(ResponseType.Cancel, (int)DialogBoxAction.Cancel);
			_resultMap.Add(ResponseType.Yes, (int)DialogBoxAction.Yes);
			_resultMap.Add(ResponseType.No, (int)DialogBoxAction.No);
		}

		public MessageBox()
		{
			
		}
		
		public void Show(string msg)
		{
			Window parent = null;
			// check if there is a main view and if it is *the* view implemented by this plugin
			//IWorkstationView view = WorkstationModel.View;
			IDesktopView view = DesktopApplication.View;
			//if(view != null && view is WorkstationView)
			if(view != null && view is DesktopView)
			{
				//parent = ((WorkstationView)view).MainWindow;
				parent = ((DesktopView)view).MainWindow;
			}
			
			MessageDialog mb = new MessageDialog(parent, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, msg);
			mb.Run();
			mb.Destroy();
		}

		public DialogBoxAction Show(string msg, MessageBoxActions buttons)
		{
			
			Window parent = null;
			IDesktopView view = DesktopApplication.View;
			ResponseType messageDialogResponse = 0; 


			if (buttons == MessageBoxActions.YesNoCancel)
			{
				throw new Exception("Button YesNoCancel not supported by GTK.ButtonsType");
			}

			if(view != null && view is DesktopView)
			{
				parent = ((DesktopView)view).MainWindow;
			}
			
			MessageDialog mb = new MessageDialog(parent, DialogFlags.DestroyWithParent, MessageType.Info, _buttonMap[(int)buttons], msg);

			messageDialogResponse = (ResponseType)mb.Run();
			mb.Destroy();

			return (DialogBoxAction)_resultMap[messageDialogResponse];
		}

	}
}
