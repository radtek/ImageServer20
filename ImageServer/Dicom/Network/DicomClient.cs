/*
 * Taken from code Copyright (c) Colby Dillion, 2007
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Threading;
using System.IO;

namespace ClearCanvas.ImageServer.Dicom.Network
{
    public class DicomClient : NetworkBase
    {
        #region Private Members
		private IPEndPoint _remoteEndPoint;
		private int _timeout;
		private Socket _socket;
		private Stream _network;
		private ManualResetEvent _closedEvent;
		private bool _closedOnError;
        IDicomClientHandler _handler;
        AssociationParameters _assoc;
		#endregion

		#region Public Constructors
        private DicomClient(AssociationParameters assoc, IDicomClientHandler handler) : base()
        {
            _remoteEndPoint = assoc.RemoteEndPoint;
            _socket = null;
            _network = null;
            _closedEvent = null;
            _timeout = 10;
            _handler = handler;
            _assoc = assoc;
        }
		#endregion

		#region Public Properties
		public string Host {
			get { return _remoteEndPoint.Address.ToString(); }
		}

		public int Port {
			get { return _remoteEndPoint.Port; }
		}

		public string CallingAE {
			get { return _assoc.CallingAE; }
			//set { _callingAe = value; }
		}

		public string CalledAE {
			get { return _assoc.CalledAE; }
			//set { _calledAe = value; }
		}

		public int Timeout {
			get { return _timeout; }
			set { _timeout = value; }
		}

		public Socket InternalSocket {
			get { return _socket; }
		}

		public bool ClosedOnError {
			get { return _closedOnError; }
		}
		#endregion

        #region Private Methods
        private void SetSocketOptions(ClientAssociationParameters parameters)
        {
            _socket.ReceiveBufferSize = parameters.ReceiveBufferSize;
            _socket.SendBufferSize = parameters.SendBufferSize;
            _socket.ReceiveTimeout = parameters.ReadTimeout;
            _socket.SendTimeout = parameters.WriteTimeout;
            _socket.LingerState = new LingerOption(false, 5);
            // Nagle option
            _socket.NoDelay = false;
        }

        private void Connect()
        {
            _closedOnError = false;

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            SetSocketOptions(this._assoc as ClientAssociationParameters);

            _socket.Connect(_remoteEndPoint);

            _network = new NetworkStream(_socket);

            InitializeNetwork(_network,"Client handler to: " + _remoteEndPoint.ToString());

            _closedEvent = new ManualResetEvent(false);

            OnClientConnected();
        }

        private void ConnectTLS()
        {
            _closedOnError = false;

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            SetSocketOptions(this._assoc as ClientAssociationParameters);

            _socket.Connect(_remoteEndPoint);

            _network = new SslStream(new NetworkStream(_socket));

            InitializeNetwork(_network, "TLS Client handler to: " + _remoteEndPoint.ToString());

            _closedEvent = new ManualResetEvent(false);

            OnClientConnected();
        }
        #endregion

        #region Public Members
        public static DicomClient Connect(AssociationParameters assoc, IDicomClientHandler handler)
        {
            DicomClient client = new DicomClient(assoc, handler);
            client.Connect();
            return client;
		}

        public static DicomClient ConnectTLS(AssociationParameters assoc, IDicomClientHandler handler)
        {
            DicomClient client = new DicomClient(assoc, handler);
            client.ConnectTLS();
            return client;
		}

        public void Close()
        {

            lock (this)
            {
                if (_network != null)
                {
                    _network.Close();
                    _network = null;
                }
                if (_socket != null)
                {
                    if (_socket.Connected)
                        _socket.Close();
                    _socket = null;
                }
                if (_closedEvent != null)
                {
                    _closedEvent.Set();
                    _closedEvent = null;
                }
            }
            ShutdownNetwork();

        }

		public bool Wait() {
			_closedEvent.WaitOne();
			return !_closedOnError;
		}
		#endregion

		#region NetworkBase Overrides
        protected void OnClientConnected()
        {
            DicomLogger.LogInfo("SCU -> Connect: {0}", InternalSocket.RemoteEndPoint);

            DicomLogger.LogInfo("C-Store SCU -> Association request");
            SendAssociateRequest(_assoc);

        }
		protected override bool NetworkHasData() {
			return _socket.Available > 0;
		}

		protected override void OnNetworkError(Exception e) {
            try
            {
                _handler.OnNetworkError(this, e);
            }
            catch (Exception) { }

			_closedOnError = true;
			Close();
		}

		protected override void OnDimseTimeout() {
            try
            {
                _handler.OnDimseTimeout(this);
            }
            catch (Exception e)
            {
                OnUserException(e, "Unexpected exception on OnDimseTimeout");
            }
		}

        protected override void OnReceiveAssociateAccept(AssociationParameters association)
        {
            try
            {
                _handler.OnReceiveAssociateAccept(this, association);
            }
            catch (Exception e)
            {
                OnUserException(e, "Unexpected exception on OnReceiveAssociateAccept");
            }

        }

		protected override void OnReceiveAssociateReject(DicomRejectResult result, DicomRejectSource source, DicomRejectReason reason) {
            
            _handler.OnReceiveAssociateReject(this,result, source, reason);

            _closedOnError = true;
			Close();
		}

		protected override void OnReceiveAbort(DicomAbortSource source, DicomAbortReason reason) {
            try
            {
                _handler.OnReceiveAbort(this, source, reason);
            }
            catch (Exception e)
            {
                OnUserException(e, "Unexpected exception on OnReceiveAbort");
            }
			_closedOnError = true;
			Close();
		}

		protected override void OnReceiveReleaseResponse() {
            try
            {
                _handler.OnReceiveReleaseResponse(this);
            }
            catch (Exception e)
            {
                OnUserException(e, "Unexpected exception on OnReceiveReleaseResponse");
            }
            _closedOnError = false;
            Close();
		}


        protected override void OnReceiveDimseRequest(byte pcid, DicomMessage msg)
        {
            try
            {
                _handler.OnReceiveRequestMessage(this, pcid, msg);
            }
            catch (Exception e)
            {
                OnUserException(e, "Unexpected exception on OnReceiveRequestMessage");
            }
            return ;
        }
        protected override void OnReceiveDimseResponse(byte pcid, DicomMessage msg)
        {

            try
            {
                _handler.OnReceiveResponseMessage(this, pcid, msg);
            }
            catch (Exception e)
            {
                OnUserException(e, "Unexpected exception on OnReceiveResponseMessage");
            }
            return;

        }
		#endregion
    }
}
