﻿#region License

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
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Dicom;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.ImageServer.Common.CommandProcessor;
using ClearCanvas.ImageServer.Common.Utilities;
using ClearCanvas.ImageServer.Core.Data;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.Brokers;
using ClearCanvas.ImageServer.Model.EntityBrokers;
using ClearCanvas.ImageServer.Model.Parameters;

namespace ClearCanvas.ImageServer.Core.Reconcile
{
	public class InsertOrUpdateEntryCommand:ServerDatabaseCommand
	{
        private readonly string _groupId;
        private readonly DicomFile _file;
		private readonly StudyStorageLocation _studyLocation;
	    private readonly string _relativePath;
        private readonly List<DicomAttributeComparisonResult> _reasons;

	    public InsertOrUpdateEntryCommand(String groupId,
		                                        StudyStorageLocation studyLocation, 
                                                DicomFile file, String relativePath,
                                                 List<DicomAttributeComparisonResult> reasons) 
			: base("Insert Duplicate Queue Entry Command", true)
		{
            Platform.CheckForNullReference(groupId, "groupId");
			Platform.CheckForNullReference(studyLocation, "studyLocation");
			Platform.CheckForNullReference(file, "file");

			_file = file;
			_studyLocation = studyLocation;
            _groupId = groupId;
		    _relativePath = relativePath;
	        _reasons = reasons;
		}

		protected override void OnExecute(ServerCommandProcessor theProcessor, IUpdateContext updateContext)
		{
			IInsertDuplicateSopReceivedQueue broker = updateContext.GetBroker<IInsertDuplicateSopReceivedQueue>();
			InsertDuplicateSopReceivedQueueParameters parms = new InsertDuplicateSopReceivedQueueParameters
                      	{
                      		GroupID = _groupId,
                      		ServerPartitionKey = _studyLocation.ServerPartitionKey,
                      		StudyStorageKey = _studyLocation.Key,
                      		StudyInstanceUid = _file.DataSet[DicomTags.StudyInstanceUid].ToString(),
                      		SeriesDescription = _file.DataSet[DicomTags.SeriesDescription].ToString(),
                      		SeriesInstanceUid = _file.DataSet[DicomTags.SeriesInstanceUid].ToString(),
                      		SopInstanceUid = _file.MediaStorageSopInstanceUid
                      	};
			ReconcileStudyQueueDescription queueDesc = CreateQueueEntryDescription(_file);
		    parms.Description = queueDesc != null ? queueDesc.ToString() : String.Empty;
            DuplicateSIQQueueData queueData = new DuplicateSIQQueueData {Details = new ImageSetDetails(_file.DataSet)};
			if (_reasons != null && _reasons.Count>0)
            {
                queueData.ComparisonResults = _reasons;
            }
            
			ImageSetDescriptor imageSet = new ImageSetDescriptor(_file.DataSet);
			parms.StudyData = XmlUtils.SerializeAsXmlDoc(imageSet);
            parms.Details = XmlUtils.SerializeAsXmlDoc(queueData);
		    parms.UidRelativePath = _relativePath;
			IList<DuplicateSopReceivedQueue> entries = broker.Find(parms);

			Platform.CheckForNullReference(entries, "entries");
			Platform.CheckTrue(entries.Count == 1, "entries.Count==1");

			DuplicateSopReceivedQueue queueEntry = entries[0];

			DuplicateSIQQueueData data = XmlUtils.Deserialize<DuplicateSIQQueueData>(queueEntry.Details);
			data.Details.InsertFile(_file);

			queueEntry.Details = XmlUtils.SerializeAsXmlDoc(data);

			IStudyIntegrityQueueEntityBroker siqBroker = updateContext.GetBroker<IStudyIntegrityQueueEntityBroker>();
			if (!siqBroker.Update(queueEntry))
				throw new ApplicationException("Unable to update duplicate queue entry");
		}

        private ReconcileStudyQueueDescription CreateQueueEntryDescription(DicomFile file)
	    {
	        using(ExecutionContext context = new ExecutionContext())
	        {
	            Study study = _studyLocation.LoadStudy(context.PersistenceContext);
                if (study!=null)
                {
                    ReconcileStudyQueueDescription desc = new ReconcileStudyQueueDescription();
                    desc.ExistingPatientId = study.PatientId;
                    desc.ExistingPatientName = study.PatientsName;
                    desc.ExistingAccessionNumber = study.AccessionNumber;

                    desc.ConflictingPatientName = file.DataSet[DicomTags.PatientsName].ToString();
                    desc.ConflictingPatientId = file.DataSet[DicomTags.PatientId].ToString();
                    desc.ConflictingAccessionNumber = file.DataSet[DicomTags.AccessionNumber].ToString();

                    return desc;
                }

	        	return null;
	        }       
        }
	}
}