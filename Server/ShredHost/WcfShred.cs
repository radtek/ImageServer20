using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace ClearCanvas.Server.ShredHost
{
    public abstract class WcfShred : ShredBase, IWcfShred
    {
        public WcfShred()
        {
            _serviceEndpointDescriptions = new Dictionary<string,ServiceEndpointDescription>();

        }

        public override object InitializeLifetimeService()
        {
            // I can't find any documentation yet, that says that returning null 
            // means that the lifetime of the object should not expire after a timeout
            // but the initial solution comes from this page: http://www.dotnet247.com/247reference/msgs/13/66416.aspx
            return null;
        }

        protected void StartHost<TServiceType, TServiceInterfaceType>(string name, string description)
        {
            ServiceEndpointDescription sed = WcfHelper.StartHost<TServiceType, TServiceInterfaceType>(this.ServicePort, name, description);
            _serviceEndpointDescriptions.Add(name, sed);
        }

        protected void StopHost(string name)
        {
            if (_serviceEndpointDescriptions.ContainsKey(name))
            {
                WcfHelper.StopHost(_serviceEndpointDescriptions[name]);
                _serviceEndpointDescriptions.Remove(name);
            }
            else
            {
                // TODO: throw an exception, since a name of a service endpoint that is
                // passed in here that doesn't exist should be considered a programming error
            }
        }

        #region Private Members
        private Dictionary<string, ServiceEndpointDescription> _serviceEndpointDescriptions;
        #endregion


        #region IWcfShred Members
        private int _servicePort;

        public int ServicePort
        {
            get
            {
                return _servicePort;
            }
            set
            {
                _servicePort = value;
            }
        }

        #endregion
    }
}
