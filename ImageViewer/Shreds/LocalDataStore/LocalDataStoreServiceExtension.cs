using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Common;
using ClearCanvas.Server.ShredHost;
using ClearCanvas.ImageViewer.Services.LocalDataStore;

namespace ClearCanvas.ImageViewer.Shreds.LocalDataStore
{
	[ExtensionOf(typeof(ShredExtensionPoint))]
	public class LocalDataStoreServiceExtension : WcfShred
	{
		private readonly string _className;
		private readonly string _localDataStoreEndpointName;
		private readonly string _localDataStoreActivityMonitorEndpointName;

		public LocalDataStoreServiceExtension()
		{
			_className = this.GetType().ToString();
			_localDataStoreEndpointName = "LocalDataStore";
			_localDataStoreActivityMonitorEndpointName = "LocalDataStoreActivityMonitor";

			System.Diagnostics.Trace.WriteLine(_className + ": constructed");
		}

		public override void Start()
		{
			Platform.Log(_className + "[" + AppDomain.CurrentDomain.FriendlyName + "]: Start invoked");

			LocalDataStoreService.Instance.Start();

			StartNetPipeHost<LocalDataStoreServiceType, ILocalDataStoreService>(_localDataStoreEndpointName, SR.LocalDataStoreService);
			StartNetPipeHost<LocalDataStoreActivityMonitorServiceType, ILocalDataStoreActivityMonitorService>(_localDataStoreActivityMonitorEndpointName, SR.LocalDataStoreActivityMonitorService);
		}

		public override void Stop()
		{
			StopHost(_localDataStoreEndpointName);
			StopHost(_localDataStoreActivityMonitorEndpointName);

			LocalDataStoreService.Instance.Stop();

			Platform.Log(_className + "[" + AppDomain.CurrentDomain.FriendlyName + "]: Stop invoked");
		}

		public override string GetDisplayName()
		{
			return SR.LocalDataStoreServices;
		}

		public override string GetDescription()
		{
			return SR.LocalDataStoreServiceDescription;
		}
	}
}
