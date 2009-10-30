#region License

// Copyright (c) 2009, ClearCanvas Inc.
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
using ClearCanvas.Dicom;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.ImageServer.Common.CommandProcessor;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.Brokers;
using ClearCanvas.ImageServer.Model.Parameters;

namespace ClearCanvas.ImageServer.Services.WorkQueue.ProcessDuplicate
{
    internal class UpdateInstanceCountCommand : ServerDatabaseCommand
    {

        #region Private Members

        private readonly StudyStorageLocation _studyLocation;
        private readonly DicomFile _file;

        #endregion

        #region Constructors

        public UpdateInstanceCountCommand(StudyStorageLocation studyLocation, DicomFile file)
            :base("Update Study Count", true)
        {
            _studyLocation = studyLocation;
            _file = file;
        }

        #endregion

        #region Overridden Protected Methods

        protected override void OnExecute(ServerCommandProcessor theProcessor, IUpdateContext updateContext)
        {
            String seriesUid = _file.DataSet[DicomTags.SeriesInstanceUid].ToString();
            String instanceUid = _file.DataSet[DicomTags.SopInstanceUid].ToString();
            
            IDeleteInstance deleteInstanceBroker = updateContext.GetBroker<IDeleteInstance>();
            DeleteInstanceParameters parameters = new DeleteInstanceParameters
                                                      {
                                                          StudyStorageKey = _studyLocation.GetKey(),
                                                          SeriesInstanceUid = seriesUid,
                                                          SOPInstanceUid = instanceUid
                                                      };
            if (!deleteInstanceBroker.Execute(parameters))
            {
                throw new ApplicationException("Unable to update instance count in db");
            }

        }

        #endregion

    }
}