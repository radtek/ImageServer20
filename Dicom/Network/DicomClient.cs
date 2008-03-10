#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

#region mDCM License
// mDCM: A C# DICOM library
//
// Copyright (c) 2008  Colby Dillion
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Author:
//    Colby Dillion (colby.dillion@gmail.com)
#endregion

using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;

namespace ClearCanvas.Dicom.Network
{
    /// <summary>
    /// Class used by DICOM Clients for all network functionality.
    /// </summary>
    public sealed class DicomClient : NetworkBase
    {
        #region Private Members
		private IPEndPoint _remoteEndPoint;
		private int _timeout;
		private Socket _socket;
		private Stream _network;
		private ManualResetEvent _closedEvent;
		private bool _closedOnError;
        IDicomClientHandler _handler;
		#endregion

		#region Public Constructors
        private DicomClient(AssociationParameters assoc, IDicomClientHandler handler)
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
		public int Timeout {
			get { return _timeout; }
			set { _timeout = value; }
		}

		public Socket InternalSocket {
			get { return _socket; }
		}

        /// <summary>
        /// Flag telling if the connection was closed on an error.
        /// </summary>
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
            _socket.LingerState = new LingerOption(false, 0);
            // Nagle option
            _socket.NoDelay = false;
        }

        private void Connect(IPEndPoint ep)
        {
            _socket = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            SetSocketOptions(_assoc as ClientAssociationParameters);

            IAsyncResult result = _socket.BeginConnect(ep, null, null);

            bool success = result.AsyncWaitHandle.WaitOne(_assoc.ConnectTimeout, true);

            if (!success)
            {
                // NOTE, MUST CLOSE THE SOCKET
                _socket.Close();
                throw new NetworkException(String.Format("Timeout while attempting to connect to remote server {0}",ep));
            }


            _network = new NetworkStream(_socket);

            InitializeNetwork(_network, "Client handler to: " + ep);

            _closedEvent = new ManualResetEvent(false);

            _remoteEndPoint = ep;

            _assoc.RemoteEndPoint = ep;
            _assoc.LocalEndPoint = _socket.LocalEndPoint as IPEndPoint;

            OnClientConnected();
        }

        private void Connect()
        {
            _closedOnError = false;

            if (_assoc.RemoteEndPoint != null)
            {
                Connect(_assoc.RemoteEndPoint);
            }
            else
            {
                IPHostEntry entry = Dns.GetHostEntry(_assoc.RemoteHostname);
                IPAddress[] list = entry.AddressList;
                foreach (IPAddress dnsAddr in list)
                {
                    if (dnsAddr.AddressFamily == AddressFamily.InterNetwork)
                    {
                        try
                        {
                            Connect(new IPEndPoint(dnsAddr, _assoc.RemotePort));
                            return;
                        }
                        catch (Exception e)
                        {
                            DicomLogger.LogErrorException(e,
                                                          "Unable to connection to remote host, attempting other addresses: {0}",
                                                          dnsAddr.ToString());
                        }
                    }
                }
                foreach (IPAddress dnsAddr in list)
                {
                    if (dnsAddr.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        try
                        {
                            Connect(new IPEndPoint(dnsAddr, _assoc.RemotePort));
                            return;
                        }
                        catch (Exception e)
                        {
                            DicomLogger.LogErrorException(e,
                                                          "Unable to connection to remote host, attempting other addresses: {0}",
                                                          dnsAddr.ToString());
                        }
                    }
                }
                String message = String.Format("Unable to connect to: {0}:{1}, no valid addresses to connect to",_assoc.RemoteHostname,_assoc.RemotePort);

                DicomLogger.LogError(message);
                throw new DicomException(message);
            }
        }

        private void ConnectTLS()
        {
            _closedOnError = false;

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            SetSocketOptions(_assoc as ClientAssociationParameters);

            _socket.Connect(_remoteEndPoint);

            _network = new SslStream(new NetworkStream(_socket));

            InitializeNetwork(_network, "TLS Client handler to: " + _remoteEndPoint.ToString());

            _closedEvent = new ManualResetEvent(false);

            OnClientConnected();
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Connection to a remote DICOM application.
        /// </summary>
        /// <param name="assoc"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static DicomClient Connect(AssociationParameters assoc, IDicomClientHandler handler)
        {
            DicomClient client = new DicomClient(assoc, handler);
            client.Connect();
            return client;
		}

        /// <summary>
        /// Connection to a remote DICOM application via TLS.
        /// </summary>
        /// <param name="assoc"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public static DicomClient ConnectTLS(AssociationParameters assoc, IDicomClientHandler handler)
        {
            DicomClient client = new DicomClient(assoc, handler);
            client.ConnectTLS();
            return client;
		}


        /// <summary>
        /// Force a shutdown of the client DICOM connection.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This routine will force the network connection for the <see cref="DicomClient"/> to be closed
        /// and the background thread for processing the association to shutdown.  The routine will block 
        /// until the shutdown has completed.
        /// </para>
        /// <para>
        /// Note, for a graceful shutdown the <see cref="NetworkBase.SendAssociateAbort"/> or 
        /// <see cref="NetworkBase.SendReleaseRequest"/> methods should be called.  These routines
        /// will gracefully shutdown DICOM connections.
        /// </para>
        /// </remarks>
        public void Close()
        {
            ShutdownNetworkThread();
        }

        /// <summary>
        /// Wait for the background thread for the client to close.
        /// </summary>
		public void Join() {
			_closedEvent.WaitOne();
		}
		#endregion

		#region NetworkBase Overrides

        /// <summary>
        /// Close the DICOM connection.
        /// </summary>
        protected override void CloseNetwork()
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
                }
            }
            ShutdownNetworkThread();
        }

        private void OnClientConnected()
        {
            DicomLogger.LogInfo("{0} SCU -> Connect: {1}", _assoc.CallingAE, InternalSocket.RemoteEndPoint.ToString());

            SendAssociateRequest(_assoc);
        }

		protected override bool NetworkHasData() 
        {
            if (_socket == null)
                return false;

            // Tells the state of the connection as of the last activity on the socket
            if (!_socket.Connected)
            {
                CloseNetwork();
                return false;
            }

		    // This is the recommended way to determine if a socket is still active, make a
            // zero byte send call, and see if an exception is thrown.  See the Socket.Connected
            // MSDN documentation  Only do the check when we know there's no data available
            try
            {
                if (_socket.Poll(1000, SelectMode.SelectRead))
                {
                    if (_socket.Available > 0)
                        return true;
                    CloseNetwork();
                    return false;
                } 
                _socket.Send(new byte[1], 0, 0);
            }
            catch (SocketException e)
            {
                // 10035 == WSAEWOULDBLOCK
                if (!e.NativeErrorCode.Equals(10035))
                    CloseNetwork();
            }

            return false;
		}

        protected override void OnNetworkError(Exception e, bool closeConnection)
        {
            try
            {
                _handler.OnNetworkError(this, _assoc as ClientAssociationParameters, e);
            }
            catch (Exception x) 
            {
                DicomLogger.LogErrorException(x, "Unexpected exception when calling IDicomClientHandler.OnNetworkError");
            }

			_closedOnError = true;
            if (closeConnection)
                CloseNetwork();
		}

		protected override void OnDimseTimeout() {
            try
            {
                _handler.OnDimseTimeout(this, _assoc as ClientAssociationParameters);
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
                _handler.OnReceiveAssociateAccept(this, association as ClientAssociationParameters);
            }
            catch (Exception e)
            {
                OnUserException(e, "Unexpected exception on OnReceiveAssociateAccept");
            }

        }

		protected override void OnReceiveAssociateReject(DicomRejectResult result, DicomRejectSource source, DicomRejectReason reason) {

            _handler.OnReceiveAssociateReject(this, _assoc as ClientAssociationParameters, result, source, reason);

            _closedOnError = true;
			CloseNetwork();
		}

		protected override void OnReceiveAbort(DicomAbortSource source, DicomAbortReason reason) {
            try
            {
                _handler.OnReceiveAbort(this, _assoc as ClientAssociationParameters, source, reason);
            }
            catch (Exception e)
            {
                OnUserException(e, "Unexpected exception on OnReceiveAbort");
            }
			_closedOnError = true;
			CloseNetwork();
		}

		protected override void OnReceiveReleaseResponse() {
            try
            {
                _handler.OnReceiveReleaseResponse(this, _assoc as ClientAssociationParameters);
            }
            catch (Exception e)
            {
                OnUserException(e, "Unexpected exception on OnReceiveReleaseResponse");
            }
            _closedOnError = false;
            CloseNetwork();
		}


        protected override void OnReceiveDimseRequest(byte pcid, DicomMessage msg)
        {
            try
            {
                _handler.OnReceiveRequestMessage(this, _assoc as ClientAssociationParameters, pcid, msg);
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
                _handler.OnReceiveResponseMessage(this, _assoc as ClientAssociationParameters, pcid, msg);
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
