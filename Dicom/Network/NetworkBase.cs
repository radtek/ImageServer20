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

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using ClearCanvas.Dicom.IO;

namespace ClearCanvas.Dicom.Network
{
    /// <summary>
    /// Internal enumerated value used to represent the DICOM Upper Layer State Machine (Part PS 3.8, Section 9.2.1
    /// </summary>
    internal enum DicomAssociationState
    {
        Sta1_Idle,
        Sta2_TransportConnectionOpen,
        Sta3_AwaitingLocalAAssociationResponsePrimative,
        Sta4_AwaitingTransportConnectionOpeningToComplete,
        Sta5_AwaitingAAssociationACOrReject,
        Sta6_AssociationEstablished,
        Sta7_AwaitingAReleaseRP,
        Sta8_AwaitingAReleaseRPLocalUser,
        Sta9_ReleaseCollisionRequestorSide,
        Sta10_ReleaseCollisionAcceptorSide,
        Sta11_ReleaseCollisionRequestorSide,
        Sta12_ReleaseCollisionAcceptorSide,
        Sta13_AwaitingTransportConnectionClose
    }

    /// <summary>
    /// Query/Retrieve levels defined by DICOM
    /// </summary>
    public enum DicomQueryRetrieveLevel
    {
        Patient,
        Study,
        Series,
        Instance,
        Worklist
    }

    /// <summary>
    /// An enumerated value representing the priority values encoded in the tag <see cref="DicomTags.Priority"/>.
    /// </summary>
    public enum DicomPriority : ushort
    {
        Low = 0x0002,
        Medium = 0x0000,
        High = 0x0001
    }

    /// <summary>
    /// An enumerated value represneting the values for the tag <see cref="DicomTags.CommandField"/>.
    /// </summary>
    public enum DicomCommandField : ushort
    {
        CStoreRequest = 0x0001,
        CStoreResponse = 0x8001,
        CGetRequest = 0x0010,
        CGetResponse = 0x8010,
        CFindRequest = 0x0020,
        CFindResponse = 0x8020,
        CMoveRequest = 0x0021,
        CMoveResponse = 0x8021,
        CEchoRequest = 0x0030,
        CEchoResponse = 0x8030,
        NEventReportRequest = 0x0100,
        NEventReportResponse = 0x8100,
        NGetRequest = 0x0110,
        NGetResponse = 0x8110,
        NSetRequest = 0x0120,
        NSetResponse = 0x8120,
        NActionRequest = 0x0130,
        NActionResponse = 0x8130,
        NCreateRequest = 0x0140,
        NCreateResponse = 0x8140,
        NDeleteRequest = 0x0150,
        NDeleteResponse = 0x8150,
        CCancelRequest = 0x0FFF
    }

    internal class DcmDimseInfo
    {
        public DicomAttributeCollection Command;
        public DicomAttributeCollection Dataset;
        public ChunkStream CommandData;
        public ChunkStream DatasetData;
        public DicomStreamReader CommandReader;
        public DicomStreamReader DatasetReader;
        public TransferMonitor Stats;
        public bool IsNewDimse;

        public DcmDimseInfo()
        {
            Stats = new TransferMonitor();

            
            IsNewDimse = true;
        }
    }



    /// <summary>
    /// Class used for DICOM network communications.
    /// </summary>
    /// <remarks>
    /// The classes <see cref="DicomClient"/>"/> and <see cref="DicomServer"/> inherit from this class, to implement network functionality.
    /// </remarks>
    public abstract class NetworkBase
    {
        #region Protected Members
        private ushort _messageId;
        private Stream _network;
        protected AssociationParameters _assoc;
        private DcmDimseInfo _dimse;
        private Thread _thread;
        private bool _stop;
        private DicomAssociationState _state = DicomAssociationState.Sta1_Idle;
        private int _timeout = 30;
        
        internal Queue<RawPDU> _pduQueue = new Queue<RawPDU>();
        #endregion

        #region Public members
        public AssociationParameters AssociationParams
        {
            get { return _assoc; }
        }

        
        #endregion


        #region Public Constructors
        public NetworkBase()
        {
            _messageId = 1;
        }

        #endregion

        #region Protected Methods
        protected void InitializeNetwork(Stream network, String name)
        {
            _network = network;
            _stop = false;
            _thread = new Thread(Process);
            _thread.Name = name;
            
            _thread.Start();
        }

        /// <summary>
        /// Method for shutting down the network thread.  Should only be called from the CloseNetwork() routine.
        /// </summary>
        protected void ShutdownNetworkThread()
        {
            _stop = true;
            if (_thread != null)
            {
                if (!Thread.CurrentThread.Equals(_thread))
                {
                    _thread.Join();
                    _thread = null;
                }
            }
        }

        /// <summary>
        /// Method for closing the network connection.
        /// </summary>
        protected abstract void CloseNetwork();

        /// <summary>
        /// Internal routine for enqueueing a PDU for transfer.
        /// </summary>
        /// <param name="pdu"></param>
        internal void EnqueuePDU(RawPDU pdu)
        {
            lock (_pduQueue)
            {
                SendRawPDU(pdu);
                //_pduQueue.Enqueue(pdu);
            }
        }

        /// <summary>
        /// Internal routine for dequeueing a PDU that needs to be transfered.
        /// </summary>
        /// <returns></returns>
        internal RawPDU DequeuePDU()
        {
            lock (_pduQueue)
            {
                return null;
                //return _pduQueue.Dequeue();
            }
        }

        protected abstract bool NetworkHasData();

        protected virtual void OnUserException(Exception e, String description)
        {
            DicomLogger.LogErrorException(e,"Unexpected User exception, description: " + description);
            switch (_state)
            {
                case DicomAssociationState.Sta2_TransportConnectionOpen:
                    OnNetworkError(e, true);
                    break;
                case DicomAssociationState.Sta3_AwaitingLocalAAssociationResponsePrimative:
                    OnNetworkError(e, true);
                    break;
                case DicomAssociationState.Sta4_AwaitingTransportConnectionOpeningToComplete:
                    OnNetworkError(e, true);
                    break;
                case DicomAssociationState.Sta5_AwaitingAAssociationACOrReject:
                    OnNetworkError(e, true);
                    break;
                case DicomAssociationState.Sta6_AssociationEstablished:
                    DicomLogger.LogError("Aborting association from {0} to {1}", _assoc.CallingAE, _assoc.CalledAE);
                    SendAssociateAbort(DicomAbortSource.ServiceProvider, DicomAbortReason.NotSpecified);
                    OnNetworkError(e, false);
                    break;
                case DicomAssociationState.Sta7_AwaitingAReleaseRP:
                    OnNetworkError(e, true);
                    break;
                case DicomAssociationState.Sta8_AwaitingAReleaseRPLocalUser:
                    OnNetworkError(e, true);
                    break;
                default:
                    OnNetworkError(e, true);
                    break;
            }
            
        }

        /// <summary>
        /// Callback called on a network error.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="closeConnection"></param>
        protected virtual void OnNetworkError(Exception e, bool closeConnection)
        {            
        }

        /// <summary>
        /// Callback called on a timeout.
        /// </summary>
        protected virtual void OnDimseTimeout()
        {
        }

        protected virtual void OnReceiveAbort(DicomAbortSource source, DicomAbortReason reason)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected virtual void OnReceiveAssociateRequest(ServerAssociationParameters association)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected virtual void OnReceiveAssociateAccept(AssociationParameters association)
        {
        }

        protected virtual void OnReceiveAssociateReject(DicomRejectResult result, DicomRejectSource source, DicomRejectReason reason)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected virtual void OnReceiveReleaseRequest()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected virtual void OnReceiveReleaseResponse()
        {
            throw new Exception("The method or operation is not implemented.");
        }


        protected virtual void OnReceiveDimseBegin(byte pcid, DicomAttributeCollection command, DicomAttributeCollection dataset, TransferMonitor stats)
        {
        }

        protected virtual void OnReceiveDimseProgress(byte pcid, DicomAttributeCollection command, DicomAttributeCollection dataset, TransferMonitor stats)
        {
        }

        protected virtual void OnReceiveDimseRequest(byte pcid, DicomMessage msg)
        {
        }

        protected virtual void OnReceiveDimseResponse(byte pcid, DicomMessage msg)
        {
        }

        protected virtual void OnDimseRequestSent(byte pcid, DicomMessage msg)
        {
        }

        protected virtual void OnDimseResponseSent(byte pcid, DicomMessage msg)
        {
        }

        private bool OnReceiveDimse(byte pcid, DicomAttributeCollection command, DicomAttributeCollection dataset)
        {
            DicomMessage msg = new DicomMessage(command, dataset);
            DicomCommandField commandField = msg.CommandField;

            if ((commandField == DicomCommandField.CStoreRequest)
                || (commandField == DicomCommandField.CCancelRequest)
                || (commandField == DicomCommandField.CEchoRequest)
                || (commandField == DicomCommandField.CFindRequest)
                || (commandField == DicomCommandField.CGetRequest)
                || (commandField == DicomCommandField.CMoveRequest)
                || (commandField == DicomCommandField.NActionRequest)
                || (commandField == DicomCommandField.NCreateRequest)
                || (commandField == DicomCommandField.NDeleteRequest)
                || (commandField == DicomCommandField.NEventReportRequest)
                || (commandField == DicomCommandField.NGetRequest)
                || (commandField == DicomCommandField.NSetRequest))
            {
                OnReceiveDimseRequest(pcid, msg);

                if (MessageReceived != null)
                    MessageReceived(_assoc, msg);

                return true;
            }

            if ((commandField == DicomCommandField.CStoreResponse)
             || (commandField == DicomCommandField.CEchoResponse)
             || (commandField == DicomCommandField.CFindResponse)
             || (commandField == DicomCommandField.CGetResponse)
             || (commandField == DicomCommandField.CMoveResponse)
             || (commandField == DicomCommandField.NActionResponse)
             || (commandField == DicomCommandField.NCreateResponse)
             || (commandField == DicomCommandField.NDeleteResponse)
             || (commandField == DicomCommandField.NEventReportResponse)
             || (commandField == DicomCommandField.NGetResponse)
             || (commandField == DicomCommandField.NSetResponse))
            {
                OnReceiveDimseResponse(pcid, msg);
                
                if (MessageReceived != null)
                    MessageReceived(_assoc, msg);
                return true;
            }
            return false;
        }

        protected virtual void OnSendDimseBegin(byte pcid, DicomAttributeCollection command, DicomAttributeCollection dataset, TransferMonitor monitor)
        {
        }

        protected virtual void OnSendDimseProgress(byte pcid, DicomAttributeCollection command, DicomAttributeCollection dataset, TransferMonitor monitor)
        {
        }

        protected virtual void OnDimseSent(byte pcid, DicomAttributeCollection command, DicomAttributeCollection dataset)
        {
            DicomMessage msg = new DicomMessage(command, dataset);
            DicomCommandField commandField = msg.CommandField;

            if ((commandField == DicomCommandField.CStoreRequest)
                || (commandField == DicomCommandField.CCancelRequest)
                || (commandField == DicomCommandField.CEchoRequest)
                || (commandField == DicomCommandField.CFindRequest)
                || (commandField == DicomCommandField.CGetRequest)
                || (commandField == DicomCommandField.CMoveRequest)
                || (commandField == DicomCommandField.NActionRequest)
                || (commandField == DicomCommandField.NCreateRequest)
                || (commandField == DicomCommandField.NDeleteRequest)
                || (commandField == DicomCommandField.NEventReportRequest)
                || (commandField == DicomCommandField.NGetRequest)
                || (commandField == DicomCommandField.NSetRequest))
            {
                OnDimseRequestSent(pcid, msg);

                if (MessageSent != null)
                    MessageSent(_assoc, msg);
            }

            if ((commandField == DicomCommandField.CStoreResponse)
             || (commandField == DicomCommandField.CEchoResponse)
             || (commandField == DicomCommandField.CFindResponse)
             || (commandField == DicomCommandField.CGetResponse)
             || (commandField == DicomCommandField.CMoveResponse)
             || (commandField == DicomCommandField.NActionResponse)
             || (commandField == DicomCommandField.NCreateResponse)
             || (commandField == DicomCommandField.NDeleteResponse)
             || (commandField == DicomCommandField.NEventReportResponse)
             || (commandField == DicomCommandField.NGetResponse)
             || (commandField == DicomCommandField.NSetResponse))
            {
                OnDimseResponseSent(pcid, msg);
                if (MessageSent != null)
                    MessageSent(_assoc, msg);
            }
        }

        
        #endregion

        #region Events

        /// <summary>
        /// Defines an event handler  when an association has been established.
        /// </summary>
        /// <param name="assoc"></param>
        public delegate void AssociationEstablishedEventHandler(AssociationParameters assoc);
        /// <summary>
        /// Defines an event handler  when an association has been rejected.
        /// </summary>
        /// <param name="assoc"></param>
        public delegate void AssociationRejectedEventHandler(DicomRejectSource source, DicomRejectReason reason);
        /// <summary>
        /// Defines an event handler  when an association is being released.
        /// </summary>
        /// <param name="assoc"></param>
        public delegate void AssociationReleasingEventHandler(AssociationParameters assoc);
        /// <summary>
        /// Defines an event handler  when an association has been released.
        /// </summary>
        /// <param name="assoc"></param>
        public delegate void AssociationReleasedEventHandler(AssociationParameters assoc);
        /// <summary>
        /// Defines an event handler  when an association has been aborted.
        /// </summary>
        /// <param name="assoc"></param>
        /// <param name="reason></param>
        public delegate void AssociationAbortedEventHandler(AssociationParameters assoc, DicomAbortReason reason);
        /// <summary>
        /// Defines an event handler  when a Dimse message is being sent.
        /// </summary>
        /// <param name="assoc>"</param>
        /// <param name="presentationContextID>"</param>
        /// <param name="command>"</param>
        /// <param name="dataset>"</param>
        /// <param name="tranferStats>"</param>
        public delegate void DimseMessageSendingEventHandler(AssociationParameters assoc, byte presentationContextID, DicomAttributeCollection command, DicomAttributeCollection dataset, TransferMonitor tranferStats);
        /// <summary>
        /// Defines an event handler  when a Dimse message is being received.
        /// </summary>
        /// <param name="assoc>"</param>
        /// <param name="presentationContextID>"</param>
        /// <param name="tranferStats>"</param>
        public delegate void DimseMessageReceivingEventHandler(AssociationParameters assoc, byte presentationContextID, TransferMonitor tranferStats);
        /// <summary>
        /// Defines an event handler  when a Dicom message has been sent.
        /// </summary>
        /// <param name="assoc>"</param>
        /// <param name="msg"/>
        public delegate void MessageSentEventHandler(AssociationParameters assoc, DicomMessage msg);
        /// <summary>
        /// Defines an event handler  when a Dicom message has been received.
        /// </summary>
        /// <param name="assoc>"</param>
        /// <param name="msg"/>
        public delegate void MessageReceivedEventHandler(AssociationParameters assoc, DicomMessage msg);
        /// <summary>
        /// Defines an event handler  when the network stream has been closed.
        /// </summary>
        /// <param name="assoc>"</param>
        /// <param name="data"></param>
        public delegate void NetworkClosedEventHandler(object data);
        /// <summary>
        /// Defines an event handler  when a network error occurs
        /// </summary>
        /// <param name="assoc>"</param>
        /// <param name="data"/>
        public delegate void NetworkErrorEventHandler(object data);
        

        /// <summary>
        /// Occurs when an association has been established between the called AE and calling AE.
        /// </summary>
        public event AssociationEstablishedEventHandler AssociationEstablished;
        /// <summary>
        /// Occurs when an association has been rejected.
        /// </summary>
        public event AssociationRejectedEventHandler AssociationRejected;
        /// <summary>
        /// Occurs when an association is being released.
        /// </summary>
        public event AssociationReleasingEventHandler AssociationReleasing;
         /// <summary>
        /// Occurs when an association has been released.
        /// </summary>
        public event AssociationReleasedEventHandler AssociationReleased;
        /// <summary>
        /// Occurs when an association is being aborted.
        /// </summary>
        public event AssociationAbortedEventHandler AssociationAborted;
        /// <summary>
        /// Occurs when a dimse message is being sent.
        /// </summary>
        public event DimseMessageSendingEventHandler DimseMessageSending;
        /// <summary>
        /// Occurs when a dimse message is being received.
        /// </summary>
        public event DimseMessageReceivingEventHandler DimseMessageReceiving;
        /// <summary>
        /// Occurs when a dicom message has been sent.
        /// </summary>
        public event MessageSentEventHandler MessageSent;

        /// <summary>
        /// Occurs when a dicom message has been received.
        /// </summary>
        public event MessageReceivedEventHandler MessageReceived;

        /// <summary>
        /// Occurs when a network stream has been closed.
        /// </summary>
        public event NetworkClosedEventHandler NetworkClosed;

        /// <summary>
        /// Occurs when a network error occured.
        /// </summary>
        public event NetworkErrorEventHandler NetworkError;


        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the next message Id to be used over the association.
        /// </summary>
        /// <returns></returns>
        public ushort NextMessageID()
        {
            return _messageId++;
        }

        /// <summary>
        /// Method used to send an association request.
        /// </summary>
        /// <param name="associate">The parameters used in the association request.</param>
        public void SendAssociateRequest(AssociationParameters associate)
        {
            _assoc = associate;
            AAssociateRQ pdu = new AAssociateRQ(_assoc);

            EnqueuePDU(pdu.Write());
        }

        /// <summary>
        /// Method to send an association abort PDU.
        /// </summary>
        /// <param name="source">The source of the abort.</param>
        /// <param name="reason">The reason for the abort.</param>
        public void SendAssociateAbort(DicomAbortSource source, DicomAbortReason reason)
        {
            if (_state != DicomAssociationState.Sta13_AwaitingTransportConnectionClose)
            {
                AAbort pdu = new AAbort(source, reason);
                
                EnqueuePDU(pdu.Write());
                _state = DicomAssociationState.Sta13_AwaitingTransportConnectionClose;


                if (AssociationAborted != null)
                    AssociationAborted(_assoc, reason);

            }
            else
            {
                DicomLogger.LogError("Unexpected state for association abort, closing connection from {0} to {1}", _assoc.CallingAE, _assoc.CalledAE);
                CloseNetwork();

                if (NetworkClosed!=null)
                    NetworkClosed("Unexpected state for association abort");
            }
        }

        /// <summary>
        /// Method to send an association accept.
        /// </summary>
        /// <param name="associate">The parameters to use for the association accept.</param>
        public void SendAssociateAccept(AssociationParameters associate)
        {
            if (_state != DicomAssociationState.Sta3_AwaitingLocalAAssociationResponsePrimative)
            {
                DicomLogger.LogError("Error attempting to send association accept at invalid time in association.");
                SendAssociateAbort(DicomAbortSource.ServiceProvider, DicomAbortReason.NotSpecified);
                throw new NetworkException("Attempting to send association accept at invalid time in association, aborting");
            }
            AAssociateAC pdu = new AAssociateAC(_assoc);
            
            EnqueuePDU(pdu.Write());

            _state = DicomAssociationState.Sta6_AssociationEstablished;


            if (AssociationEstablished != null)
                AssociationEstablished(_assoc);
        }

        /// <summary>
        /// Method to send an association rejection.
        /// </summary>
        /// <param name="result">The </param>
        /// <param name="source"></param>
        /// <param name="reason"></param>
        public void SendAssociateReject(DicomRejectResult result, DicomRejectSource source, DicomRejectReason reason)
        {
            if (_state != DicomAssociationState.Sta3_AwaitingLocalAAssociationResponsePrimative)
            {
                DicomLogger.LogError("Error attempting to send associaiton reject at invalid time in association.");
                SendAssociateAbort(DicomAbortSource.ServiceProvider, DicomAbortReason.NotSpecified);
                throw new NetworkException("Attempting to send association reject at invalid time in association, aborting");
            }
            AAssociateRJ pdu = new AAssociateRJ(result, source, reason);
            
            EnqueuePDU(pdu.Write());

            _state = DicomAssociationState.Sta13_AwaitingTransportConnectionClose;

            if (AssociationRejected != null)
                AssociationRejected(source, reason);
        }

        /// <summary>
        /// Method to send an association release request.  this method can only be used by clients.
        /// </summary>
        public void SendReleaseRequest()
        {
            if (_state != DicomAssociationState.Sta6_AssociationEstablished)
            {
                DicomLogger.LogError("Unexpected attempt to send Release Request when in invalid state.");
                return;
            }

            AReleaseRQ pdu = new AReleaseRQ();
            
            EnqueuePDU(pdu.Write());

            _state = DicomAssociationState.Sta7_AwaitingAReleaseRP;

            // still waiting for remote AE to send release response
            if (AssociationReleasing != null)
                AssociationReleasing(_assoc);
        }

        /// <summary>
        /// Method to send an association release response.
        /// </summary>
        protected void SendReleaseResponse()
        {
            if (_state != DicomAssociationState.Sta8_AwaitingAReleaseRPLocalUser)
            {
            }

            AReleaseRP pdu = new AReleaseRP();
            
            EnqueuePDU(pdu.Write());
            _state = DicomAssociationState.Sta13_AwaitingTransportConnectionClose;

            if (AssociationReleased != null)
                AssociationReleased(_assoc);

        }

        /// <summary>
        /// Method to send a DICOM C-ECHO-RQ message.
        /// </summary>
        /// <param name="presentationID">The presentation context to send the request on.</param>
        /// <param name="messageID">The messageID to use.</param>
        public void SendCEchoRequest(byte presentationID, ushort messageID)
        {
            DicomLogger.LogInfo("Sending C Echo request, pres ID: {0}, messageID = {1}", presentationID, messageID);
            DicomMessage msg = new DicomMessage();
            msg.MessageId = messageID;
            msg.CommandField = DicomCommandField.CEchoRequest;
            msg.AffectedSopClassUid = _assoc.GetAbstractSyntax(presentationID).UID;
            msg.DataSetType = 0x0101;

            SendDimse(presentationID, msg.CommandSet, null);
        }

        /// <summary>
        /// Method to send a DICOM C-ECHO-RSP message.
        /// </summary>
        /// <param name="presentationID"></param>
        /// <param name="messageID"></param>
        /// <param name="status"></param>
        public void SendCEchoResponse(byte presentationID, ushort messageID, DicomStatus status)
        {
            DicomUid affectedClass = _assoc.GetAbstractSyntax(presentationID);
            DicomMessage msg = new DicomMessage();
            msg.MessageIdBeingRespondedTo = messageID;
            msg.CommandField = DicomCommandField.CEchoResponse;
            msg.AffectedSopClassUid = affectedClass.UID;
            msg.DataSetType = 0x0101;
            msg.Status = status;

            SendDimse(presentationID, msg.CommandSet, null);
        }

        /// <summary>
        /// Method to send a DICOM C-STORE-RQ message.
        /// </summary>
        /// <param name="presentationID"></param>
        /// <param name="messageID"></param>
        /// <param name="priority"></param>
        /// <param name="message"></param>
        public void SendCStoreRequest(byte presentationID, ushort messageID,
            DicomPriority priority, DicomMessage message)
        {
            SendCStoreRequest(presentationID, messageID, priority, null, 0, message);
        }

        /// <summary>
        /// Method to send a DICOM C-STORE-RQ message.
        /// </summary>
        /// <param name="presentationID"></param>
        /// <param name="messageID"></param>
        /// <param name="priority"></param>
        /// <param name="moveAE"></param>
        /// <param name="moveMessageID"></param>
        /// <param name="message"></param>
        public void SendCStoreRequest(byte presentationID, ushort messageID,
            DicomPriority priority, string moveAE, ushort moveMessageID, DicomMessage message)
        {
            DicomUid affectedClass = _assoc.GetAbstractSyntax(presentationID);

            if (!affectedClass.UID.Equals(message.SopClass.Uid))
                throw new DicomException(String.Format("SOP Class Uid in the message {0} does not match SOP Class UID for presentation context {1}",
                    message.SopClass.Uid,affectedClass.UID));

            DicomAttributeCollection command = message.MetaInfo;

            message.MessageId = messageID;
            message.CommandField = DicomCommandField.CStoreRequest;
            message.AffectedSopClassUid = message.SopClass.Uid;
            message.DataSetType = 0x0202;
            message.Priority = priority;

            String sopInstanceUid;
            bool ok = message.DataSet[DicomTags.SopInstanceUid].TryGetString(0, out sopInstanceUid);
            if (!ok)
                throw new DicomException("SOP Instance UID unexpectedly not set in CStore Message being sent.");

            message.AffectedSopInstanceUid = sopInstanceUid;
            
            
            if (moveAE != null && moveAE != String.Empty)
            {
                message.MoveOriginatorApplicationEntityTitle = moveAE;
                message.MoveOriginatorMessageId = moveMessageID;
            }

            SendDimse(presentationID, command, message.DataSet);
        }

        /// <summary>
        /// Method to send a DICOM C-STORE-RSP message.
        /// </summary>
        /// <param name="presentationID"></param>
        /// <param name="messageID"></param>
        /// <param name="affectedInstance"></param>
        /// <param name="status"></param>
        public void SendCStoreResponse(byte presentationID, ushort messageID, string affectedInstance, DicomStatus status)
        {
            DicomMessage msg = new DicomMessage();
            msg.MessageIdBeingRespondedTo = messageID;
            msg.CommandField = DicomCommandField.CStoreResponse;
            msg.AffectedSopClassUid = _assoc.GetAbstractSyntax(presentationID).UID;
            msg.AffectedSopInstanceUid = affectedInstance;
            msg.DataSetType = 0x0101;
            msg.Status = status;

            
            SendDimse(presentationID, msg.CommandSet, null);
        }

        /// <summary>
        /// Method to send a DICOM C-FIND-RQ message.
        /// </summary>
        /// <param name="presentationID"></param>
        /// <param name="messageID"></param>
        /// <param name="message"></param>
        public void SendCFindRequest(byte presentationID, ushort messageID, DicomMessage message)
        {
            if (message.DataSet.IsEmpty())
                throw new DicomException("Unexpected empty DataSet when sending C-FIND-RQ.");

            DicomUid affectedClass = _assoc.GetAbstractSyntax(presentationID);

            message.AffectedSopClassUid = affectedClass.UID;
            message.MessageId = messageID;
            message.CommandField = DicomCommandField.CFindRequest;
            if (!message.CommandSet.Contains(DicomTags.Priority))
                message.Priority = DicomPriority.Medium;
            message.DataSetType = 0x0202;

            SendDimse(presentationID, message.CommandSet, message.DataSet);
        }

        /// <summary>
        /// Method to send a DICOM C-CANCEL-FIND-RQ message.
        /// </summary>
        /// <param name="messageID">The message ID of the original C-FIND-RQ that is being canceled</param>
        /// <param name="presentationID"></param>
        public void SendCFindCancelRequest(byte presentationID, ushort messageID)
        {
            DicomMessage message = new DicomMessage();
            message.CommandField = DicomCommandField.CCancelRequest;
            message.DataSetType = 0x0101;
            message.MessageIdBeingRespondedTo = messageID;

            SendDimse(presentationID, message.CommandSet, null);
        }

        /// <summary>
        /// Method to send a DICOM C-FIND-RSP message.
        /// </summary>
        /// <param name="presentationID"></param>
        /// <param name="messageID"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        public void SendCFindResponse(byte presentationID, ushort messageID, DicomMessage message, DicomStatus status)
        {
            DicomUid affectedClass = _assoc.GetAbstractSyntax(presentationID);
            message.CommandField = DicomCommandField.CFindResponse;
            message.Status = status;
            message.MessageIdBeingRespondedTo = messageID;
            message.AffectedSopClassUid = affectedClass.UID;
            message.DataSetType = message.DataSet.IsEmpty() ? (ushort)0x0101 : (ushort)0x0202;

            SendDimse(presentationID, message.CommandSet, message.DataSet);
        }


        /// <summary>
        /// Method to send a DICOM C-MOVE-RQ message.
        /// </summary>
        /// <param name="presentationID"></param>
        /// <param name="messageID"></param>
        /// <param name="destinationAE"></param>
        /// <param name="message"></param>
        public void SendCMoveRequest(byte presentationID, ushort messageID, string destinationAE, DicomMessage message)
        {
            if (message.DataSet.IsEmpty())
                throw new DicomException("Unexpected empty DataSet when sending C-MOVE-RQ.");

            DicomUid affectedClass = _assoc.GetAbstractSyntax(presentationID);
            message.CommandField = DicomCommandField.CMoveRequest;
            message.AffectedSopClassUid = affectedClass.UID;
            if (!message.CommandSet.Contains(DicomTags.Priority))
                message.Priority = DicomPriority.Medium;
            message.DataSetType = 0x0202;
            message.MoveDestination = destinationAE;
            SendDimse(presentationID, message.CommandSet, message.DataSet);
        }

        /// <summary>
        /// Method to send a DICOM C-CANCEL-MOVE-RQ message.
        /// </summary>
        /// <param name="messageID">The message ID of the original C-MOVE-RQ that is being canceled</param>
        /// <param name="presentationID"></param>
        public void SendCMoveCancelRequest(byte presentationID, ushort messageID)
        {
            DicomMessage message = new DicomMessage();
            message.CommandField = DicomCommandField.CCancelRequest;
            message.DataSetType = 0x0101;
            message.MessageIdBeingRespondedTo = messageID;
            SendDimse(presentationID, message.CommandSet, null);
        }

        /// <summary>
        /// Method to send a DICOM C-MOVE-RSP message.
        /// </summary>
        /// <param name="presentationID"></param>
        /// <param name="messageID"></param>
        /// <param name="message"></param>
        /// <param name="status"></param>
        public void SendCMoveResponse(byte presentationID, ushort messageID, DicomMessage message, DicomStatus status)
        {
            DicomUid affectedClass = _assoc.GetAbstractSyntax(presentationID);
            message.CommandField = DicomCommandField.CMoveResponse;
            message.Status = status;
            message.MessageIdBeingRespondedTo = messageID;
            message.AffectedSopClassUid = affectedClass.UID;
            message.DataSetType = message.DataSet.IsEmpty() ? (ushort)0x0101 : (ushort)0x0202;

            SendDimse(presentationID, message.CommandSet, message.DataSet);
        }

        /// <summary>
        /// Method to send a DICOM C-MOVE-RSP message.
        /// </summary>
        /// <param name="presentationID"></param>
        /// <param name="messageID"></param>
        /// <param name="message"></param>
        /// <param name="status"></param>
        /// <param name="NumberOfCompletedSubOperations"></param>
        /// <param name="NumberOfRemainingSubOperations"></param>
        /// <param name="NumberOfFailedSubOperations"></param>
        /// <param name="NumberOfWarningSubOperations"></param>
        public void SendCMoveResponse(byte presentationID, ushort messageID, DicomMessage message, DicomStatus status,
            ushort NumberOfCompletedSubOperations, ushort NumberOfRemainingSubOperations,
            ushort NumberOfFailedSubOperations, ushort NumberOfWarningSubOperations)
        {
            message.CommandField = DicomCommandField.CMoveResponse;
            message.Status = status;
            message.MessageIdBeingRespondedTo = messageID;
            message.AffectedSopClassUid = _assoc.GetAbstractSyntax(presentationID).UID;
            message.NumberOfCompletedSubOperations = NumberOfCompletedSubOperations;
            message.NumberOfRemainingSubOperations = NumberOfRemainingSubOperations;
            message.NumberOfFailedSubOperations = NumberOfFailedSubOperations;
            message.NumberOfWarningSubOperations = NumberOfWarningSubOperations;
            message.DataSetType = message.DataSet.IsEmpty() ? (ushort) 0x0101 : (ushort) 0x0202;

            SendDimse(presentationID, message.CommandSet, message.DataSet);
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Main processing routine for processing a network connection.
        /// </summary>
        private void Process()
        {
            try
            {
                DateTime timeout = DateTime.Now.AddSeconds(_timeout);
                while (!_stop)
                {
                    if (NetworkHasData())
                    {
                        if (_assoc != null)
                            timeout = DateTime.Now.AddSeconds(_assoc.ReadTimeout / 1000);
                        else
                            timeout = DateTime.Now.AddSeconds(_timeout);

                        bool success = ProcessNextPDU();
                        if (!success)
                        {
                            // Start the Abort process, not much else we can do
                            DicomLogger.LogError("Unexpected error processing PDU.  Aborting Association from {0} to {1}", _assoc.CallingAE, _assoc.CalledAE);
                            SendAssociateAbort(DicomAbortSource.ServiceProvider, DicomAbortReason.InvalidPDUParameter);
                        }
                    }
                    else if (_pduQueue.Count > 0)
                    {
                        //SendRawPDU(DequeuePDU());
                    }
                    else if (DateTime.Now > timeout)
                    {
                        if (_state == DicomAssociationState.Sta6_AssociationEstablished)
                        {
                            OnDimseTimeout();
                            timeout = DateTime.Now.AddSeconds(_assoc.ReadTimeout / 1000);
                        } 
                        else if (_state == DicomAssociationState.Sta2_TransportConnectionOpen)
                        {
                            DicomLogger.LogError("ARTIM timeout when waiting for AAssociate Request PDU, closing connection.");
                            _state = DicomAssociationState.Sta13_AwaitingTransportConnectionClose;
                            CloseNetwork(); // TODO

                            if (NetworkClosed!=null)
                                NetworkClosed("ARTIM timeout when waiting for AAssociate Request PDU");
                            
                        }
                        else if (_state == DicomAssociationState.Sta13_AwaitingTransportConnectionClose)
                        {
                            DicomLogger.LogError("Timeout when waiting for transport connection to close from {0} to {1}.  Dropping Connection.", _assoc.CallingAE, _assoc.CalledAE);
                            CloseNetwork(); // TODO
                            if (NetworkClosed != null)
                                NetworkClosed("Timeout when waiting for transport connection to close");
                        }
                        else
                        {
                            OnDimseTimeout();
                            if (_assoc != null)
                                timeout = DateTime.Now.AddSeconds(_assoc.ReadTimeout / 1000);
                            else
                                timeout = DateTime.Now.AddSeconds(_timeout);
                        }
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                _network.Close();
                _network.Dispose();
                _network = null;
            }
            catch (Exception e)
            {
                OnNetworkError(e, true);

                if (NetworkError!=null)
                    NetworkError(e);
            }
        }

        private bool ProcessNextPDU()
        {
            RawPDU raw = new RawPDU(_network);

            if (raw.Type == 0x04)
            {
                if (_dimse == null)
                {
                    _dimse = new DcmDimseInfo();
                    _assoc.TotalDimseReceived++;
                }
            }

            raw.ReadPDU();

            try
            {
                switch (raw.Type)
                {
                    case 0x01:
                        {
                            _assoc = new ServerAssociationParameters();
                            AAssociateRQ pdu = new AAssociateRQ(_assoc);
                            pdu.Read(raw);
                            _state = DicomAssociationState.Sta3_AwaitingLocalAAssociationResponsePrimative;
 							OnReceiveAssociateRequest(_assoc as ServerAssociationParameters);

                            if (_state != DicomAssociationState.Sta13_AwaitingTransportConnectionClose &&
                                _state != DicomAssociationState.Sta6_AssociationEstablished)
                            {
                                DicomLogger.LogError("Association incorrectly not accepted or rejected, aborting.");
                                return false;
                            }

                            //if derived class call SendAssociateAccept, it has fired this event
                            //if (AssociationEstablished != null)
                            //    AssociationEstablished(_assoc);

                            return true;
                        }
                    case 0x02:
                        {

                            AAssociateAC pdu = new AAssociateAC(_assoc);
                            pdu.Read(raw);
                            _state = DicomAssociationState.Sta6_AssociationEstablished;
                             
                            OnReceiveAssociateAccept(_assoc);

                            if (AssociationEstablished != null)
                                AssociationEstablished(_assoc);
                                                    
                            return true;
                        }
                    case 0x03:
                        {
                            AAssociateRJ pdu = new AAssociateRJ();
							pdu.Read(raw);
                            _state = DicomAssociationState.Sta13_AwaitingTransportConnectionClose;

                            if (AssociationRejected != null)
                                AssociationRejected(pdu.Source, pdu.Reason);


                            OnReceiveAssociateReject(pdu.Result, pdu.Source, pdu.Reason);

                            

                            return true;
                        }
                    case 0x04:
                        {
                            PDataTF pdu = new PDataTF();
                            pdu.Read(raw);
                            return ProcessPDataTF(pdu);
                        }
                    case 0x05:
                        {
                            AReleaseRQ pdu = new AReleaseRQ();
                            pdu.Read(raw);
                            _state = DicomAssociationState.Sta8_AwaitingAReleaseRPLocalUser;
                            
                            OnReceiveReleaseRequest();

							return true;
                        }
                    case 0x06:
                        {
                            AReleaseRP pdu = new AReleaseRP();
                            pdu.Read(raw);
                            _state = DicomAssociationState.Sta13_AwaitingTransportConnectionClose;

                            if (AssociationReleased != null)
                                AssociationReleased(_assoc);


                            OnReceiveReleaseResponse();

                            
							return true;
                        }
                    case 0x07:
                        {
                            AAbort pdu = new AAbort();
                            pdu.Read(raw);
                            _state = DicomAssociationState.Sta1_Idle;

                            if (AssociationAborted != null)
                                AssociationAborted(_assoc, pdu.Reason);
                            
                            OnReceiveAbort(pdu.Source, pdu.Reason);
                            
                            
                            return true;
                        }
                    case 0xFF:
                        {
                            return false;
                        }
                    default:
                        throw new NetworkException("Unknown PDU type");
                }
            }
            catch (Exception e)
            {
                OnNetworkError(e, true);

                if (NetworkError != null)
                    NetworkError(e);

                String file = String.Format(@"{0}\Errors\{1}.pdu",
                    Environment.CurrentDirectory, DateTime.Now.Ticks);
                Directory.CreateDirectory(Environment.CurrentDirectory + @"\Errors");
                raw.Save(file);
                return false;
            }
        }

        private bool ProcessPDataTF(PDataTF pdu)
        {
            try
            {
                int bytes = 0, total = 0;
                byte pcid = 0;
                foreach (PDV pdv in pdu.PDVs)
                {
                    pcid = pdv.PCID;
                    if (pdv.IsCommand)
                    {
                        if (_dimse.CommandData == null)
                            _dimse.CommandData = new ChunkStream();

                        _dimse.CommandData.AddChunk(pdv.Value);

                        if (_dimse.Command == null)
                        {
                            _dimse.Command = new DicomAttributeCollection(0x00000000, 0x0000FFFF);
                        }

                        if (_dimse.CommandReader == null)
                        {
                            _dimse.CommandReader = new DicomStreamReader(_dimse.CommandData);
                            _dimse.CommandReader.TransferSyntax = TransferSyntax.ImplicitVrLittleEndian;
                            _dimse.CommandReader.Dataset = _dimse.Command;
                        }

                        DicomReadStatus stat = _dimse.CommandReader.Read(null, DicomReadOptions.Default);
                        if (stat == DicomReadStatus.UnknownError)
                        {
                            DicomLogger.LogError("Unexpected parsing error when reading command group elements.");
                            return false;
                        }
                        bytes += pdv.Value.Length;
                        total = (int)_dimse.CommandReader.BytesEstimated;
                        _assoc.TotalBytesRead += (UInt64)pdv.Value.Length;
                        _dimse.Stats.Tick(pdv.Value.Length, total);
                        if (DimseMessageReceiving != null)
                            DimseMessageReceiving(_assoc, pcid, _dimse.Stats);
                        
                        if (pdv.IsLastFragment)
                        {
                            if (stat == DicomReadStatus.NeedMoreData)
                            {
                                DicomLogger.LogError("Unexpected end of StreamReader.  More data needed after reading last PDV fraagment.");
                                return false;
                            }
                            _dimse.CommandData = null;
                            _dimse.CommandReader = null;

                            bool isLast = true;
                            if (_dimse.Command.Contains(DicomTags.DataSetType))
                            {
                                if (_dimse.Command[DicomTags.DataSetType].GetUInt16(0,0x0) != 0x0101)
                                    isLast = false;
                            }
                            if (isLast)
                            {
								if (_dimse.IsNewDimse)
                                {
                                     OnReceiveDimseBegin(pcid, _dimse.Command, _dimse.Dataset, _dimse.Stats);
                                }
                                OnReceiveDimseProgress(pcid, _dimse.Command, _dimse.Dataset, _dimse.Stats);
                                bool ret = OnReceiveDimse(pcid, _dimse.Command, _dimse.Dataset);

                                //_assoc.TotalBytesRead += (UInt64)total;
                         
                                _dimse = null;
                                return ret;
                            }
                        }
                    }
                    else
                    {
                        if (_dimse.DatasetData == null)
                            _dimse.DatasetData = new ChunkStream();

                        _dimse.DatasetData.AddChunk(pdv.Value);

                        if (_dimse.Dataset == null)
                        {
                            
                            _dimse.Dataset = new DicomAttributeCollection(0x00080000,0xFFFFFFFF);
                        }

                        if (_dimse.DatasetReader == null)
                        {
                            _dimse.DatasetReader = new DicomStreamReader(_dimse.DatasetData);
                            _dimse.DatasetReader.TransferSyntax = _assoc.GetAcceptedTransferSyntax(pdv.PCID);
                            _dimse.DatasetReader.Dataset = _dimse.Dataset;
                        }

                        DicomReadStatus stat = _dimse.DatasetReader.Read(null, DicomReadOptions.Default);
                        if (stat == DicomReadStatus.UnknownError)
                        {
                            DicomLogger.LogError("Unexpected parsing error when reading DataSet.");
                            return false;
                        }

                        bytes += pdv.Value.Length;
                        total = (int)_dimse.DatasetReader.BytesEstimated;
                        _assoc.TotalBytesRead += (UInt64)pdv.Value.Length;
                        _dimse.Stats.Tick(pdv.Value.Length, total);
                        if (DimseMessageReceiving != null)
                            DimseMessageReceiving(_assoc, pcid, _dimse.Stats);
                        
                        if (pdv.IsLastFragment)
                        {
                            if (stat == DicomReadStatus.NeedMoreData)
                            {
                                DicomLogger.LogError("Unexpected end of StreamReader.  More data needed after reading last PDV fraagment.");
                                return false;
                            }
                            _dimse.CommandData = null;
                            _dimse.CommandReader = null;

							if (_dimse.IsNewDimse)
                            {
                                OnReceiveDimseBegin(pcid, _dimse.Command, _dimse.Dataset, _dimse.Stats);
                            }
                            OnReceiveDimseProgress(pcid, _dimse.Command, _dimse.Dataset, _dimse.Stats);
                            bool ret = OnReceiveDimse(pcid, _dimse.Command, _dimse.Dataset);
                         
                         
                            _dimse = null;
                            return ret;
                        }
                    }
                }

                
                if (_dimse.IsNewDimse)
                {
                    OnReceiveDimseBegin(pcid, _dimse.Command, _dimse.Dataset, _dimse.Stats);
                    _dimse.IsNewDimse = false;
                }
                else
                {
                    OnReceiveDimseProgress(pcid, _dimse.Command, _dimse.Dataset, _dimse.Stats);

                }

                return true;
            }
            catch (Exception e)
            {
                //do something here!
                DicomLogger.LogErrorException(e,"Unexpected exception processing P-DATA PDU");
                return false;
            }
        }

        private void SendRawPDU(RawPDU pdu)
        {
            // If the try/catch is reintroduced here, it must
            // throw an exception, if the exception is just eaten, 
            // you can get into a case where there's repetetive errors
            // trying to send PDUs, until a whole message is sent.

            //try
            //{
                pdu.WritePDU(_network);
            //}
            //catch (Exception e)
            //{
            //    OnNetworkError(e);
            //    throw new DicomException("Unexpected exception when writing PDU",e);
            //}
        }

        /// <summary>
        /// Method for sending a DIMSE mesage.
        /// </summary>
        /// <param name="pcid"></param>
        /// <param name="command"></param>
        /// <param name="dataset"></param>
        private void SendDimse(byte pcid, DicomAttributeCollection command, DicomAttributeCollection dataset)
        {
            try
            {
                TransferSyntax ts = _assoc.GetAcceptedTransferSyntax(pcid);

                int total = (int)command.CalculateWriteLength(TransferSyntax.ImplicitVrLittleEndian, DicomWriteOptions.Default | DicomWriteOptions.CalculateGroupLengths);

                if (dataset != null)
                    total += (int)dataset.CalculateWriteLength(ts, DicomWriteOptions.Default);

                PDataTFStream pdustream = new PDataTFStream(this, pcid, (int)_assoc.MaximumPduLength, total);
                pdustream.OnTick += delegate(TransferMonitor stats)
                {
                    OnSendDimseProgress(pcid, command, dataset, stats);

                    if (DimseMessageSending != null)
                        DimseMessageSending(_assoc, pcid, command, dataset, stats);
                };

                OnSendDimseBegin(pcid, command, dataset, pdustream.Stats);


                DicomStreamWriter dsw = new DicomStreamWriter(pdustream);
                dsw.Write(TransferSyntax.ImplicitVrLittleEndian,
                    command, DicomWriteOptions.Default | DicomWriteOptions.CalculateGroupLengths);

                if ((dataset != null) && !dataset.IsEmpty())
                {
                    pdustream.IsCommand = false;
                    dsw.Write(ts, dataset, DicomWriteOptions.Default);
                }

                // flush last pdu
                pdustream.Flush(true);

                _assoc.TotalBytesSent += (ulong) total;
                
                OnDimseSent(pcid, command, dataset);
                
            }
            catch (Exception e)
            {
                OnNetworkError(e, true);

                // TODO
                // Should we throw another exception here?  Should the user know there's an error?  They'll get
                // the error reported to them through the OnNetworkError routine, and throwing an exception here
                // might cause us to call OnNetworkError a second time, because the exception may be caught at a higher
                // level
                //throw new DicomException("Unexpected exception when sending a DIMSE message",e);
            }
        }
        #endregion
    }
}
 
