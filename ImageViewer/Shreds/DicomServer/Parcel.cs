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
using System.Collections;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Dicom;
using ClearCanvas.Dicom.OffisNetwork;
using ClearCanvas.Dicom.DataStore;

namespace ClearCanvas.ImageViewer.Shreds.DicomServer
{
    internal abstract class Parcel
    {
        private ApplicationEntity _destinationAE;
        private ApplicationEntity _sourceAE;
        private Guid _parcelOid;
        private ParcelTransferState _parcelTransferState = ParcelTransferState.Pending;
        private string _description;
        private int _currentProgressStep;
        private int _totalProgressSteps;

        public Parcel(ApplicationEntity sourceAE, ApplicationEntity destinationAE, string parcelDescription)
        {
            _sourceAE = sourceAE;
            _destinationAE = destinationAE;
            _description = parcelDescription;
        }

        protected Parcel()
        {
        }

        #region Properties

        public ParcelTransferState ParcelTransferState
        {
            get { return _parcelTransferState; }
            protected set { _parcelTransferState = value; }
        }

        protected virtual Guid ParcelOid
        {
            get { return _parcelOid; }
            set { _parcelOid = value; }
        }

        public virtual ApplicationEntity DestinationAE
        {
            get { return _destinationAE; }
            protected set { _destinationAE = value; }
        }

        public virtual ApplicationEntity SourceAE
        {
            get { return _sourceAE; }
            protected set { _sourceAE = value; }
        }

        public virtual int TotalProgressSteps
        {
            get { return _totalProgressSteps; }
            set { _totalProgressSteps = value; }
        }

        public virtual int CurrentProgressStep
        {
            get { return _currentProgressStep; }
            set { _currentProgressStep = value; }
        }

        public virtual string Description
        {
            get { return _description; }
            private set { _description = value; }
        }

        #endregion
    }
}
