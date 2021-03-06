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

using System.Collections.Generic;
using System.Diagnostics;
using ClearCanvas.Common;
using ClearCanvas.ImageServer.Common.Utilities;
using ClearCanvas.ImageServer.Core.Data;
using ClearCanvas.ImageServer.Core.Reconcile;
using ClearCanvas.ImageServer.Core.Validation;
using ClearCanvas.ImageServer.Model;

namespace ClearCanvas.ImageServer.Services.WorkQueue.ReconcileStudy
{
    /// <summary>
    /// Processor to handle 'ReconcileStudy' work queue entries
    /// </summary>
    [StudyIntegrityValidation(ValidationTypes = StudyIntegrityValidationModes.Default, Recovery = RecoveryModes.Automatic)]
    class ReconcileStudyItemProcessor : BaseItemProcessor
    {
        #region Private Members
        private ReconcileStudyProcessorContext _context;
        private ReconcileStudyWorkQueueData _reconcileQueueData;
        private IReconcileProcessor _processor;
        #endregion

        #region Overridden Protected Method
		protected override void ProcessItem(Model.WorkQueue item)
		{
			Platform.CheckForNullReference(item, "item");
			Platform.CheckForNullReference(item.Data, "item.Data");

		    _reconcileQueueData = XmlUtils.Deserialize<ReconcileStudyWorkQueueData>(WorkQueueItem.Data);

			LoadUids(item);

            InitializeContext();

            SetupProcessor();
				
			if (WorkQueueUidList.Count == 0)
			{
                Platform.Log(LogLevel.Info,
                             "Completing study reconciliation for Study {0}, Patient {1} (PatientId:{2} A#:{3}) on Partition {4}, {5} objects",
                             Study.StudyInstanceUid, Study.PatientsName, Study.PatientId,
                             Study.AccessionNumber, ServerPartition.Description, WorkQueueUidList.Count);
                ExecuteCommands(true);
			}
			else
			{
				Platform.Log(LogLevel.Info,
				             "Reconciling study {0} for Patient {1} (PatientId:{2} A#:{3}) on Partition {4}, {5} objects",
				             Study.StudyInstanceUid, Study.PatientsName, Study.PatientId,
				             Study.AccessionNumber, ServerPartition.Description, WorkQueueUidList.Count);

				ExecuteCommands(false);
			}
		}

        private void ExecuteCommands(bool complete)
        {
            if(_processor!=null)
            {
                using(_processor)
                {
                    _processor.Initialize(_context, complete);

                    if (!_processor.Execute())
                    {
                        FailQueueItem(WorkQueueItem, _processor.FailureReason);
                    }
                    else
                    {
                        if (complete)
                            OnComplete();
                        else
                            OnBatchComplete();
                    }
                }                
            }            
        }
        
        protected override bool CanStart()
        {
            // cannot start if the existing study is scheduled for update
            IList<Model.WorkQueue> relatedItems = FindRelatedWorkQueueItems(WorkQueueItem, new[]
                                              {
                                                  WorkQueueTypeEnum.ReprocessStudy
                                              }, 
                                              null);
			if (!(relatedItems == null || relatedItems.Count == 0))
            {
				PostponeItem("Study is scheduled for reprocess");
            	return false;
            }
        	return true;
        }

        #endregion

        #region Private Mehods

        private void OnComplete()
        {
			PostProcessing(WorkQueueItem, 
				WorkQueueProcessorStatus.Complete, 
				WorkQueueProcessorDatabaseUpdate.ResetQueueState);
            Platform.Log(LogLevel.Info, "Reconciliation completed");
        }

        private void OnBatchComplete()
        {
			PostProcessing(WorkQueueItem, 
				WorkQueueProcessorStatus.Pending, 
				WorkQueueProcessorDatabaseUpdate.ResetQueueState);
            Platform.Log(LogLevel.Info, "StudyReconcile processed.");
        }

        
        private void SetupProcessor()
        {
            Debug.Assert(_context != null);

            ReconcileCommandXmlParser parser = new ReconcileCommandXmlParser();
            _processor = parser.Parse(_context.History.ChangeDescription);

            Platform.Log(LogLevel.Info, "Using {0}", _processor.Name);
        }

        private void InitializeContext()
        {
            Platform.CheckForNullReference(StorageLocation, "StorageLocation");
            _context = new ReconcileStudyProcessorContext
                       	{
                       		WorkQueueItem = WorkQueueItem,
                       		WorkQueueItemStudyStorage = StorageLocation,
                       		Partition = ServerPartition,
                       		ReconcileWorkQueueData = _reconcileQueueData,
                       		WorkQueueUidList = WorkQueueUidList,
                       		History = WorkQueueItem.StudyHistoryKey != null
                       		          	? StudyHistory.Load(WorkQueueItem.StudyHistoryKey)
                       		          	: null
                       	};
        }

        #endregion
    }
}
