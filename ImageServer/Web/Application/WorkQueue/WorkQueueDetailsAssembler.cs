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

using System.Collections;
using System.Collections.Generic;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.EntityBrokers;
using ClearCanvas.ImageServer.Web.Common.Data;

namespace ClearCanvas.ImageServer.Web.Application.WorkQueue
{
    /// <summary>
    /// Assembles an instance of  <see cref="WorkQueueDetails"/> based on a <see cref="WorkQueue"/> object.
    /// </summary>
    public class WorkQueueDetailsAssembler
    {
        /// <summary>
        /// Creates an instance of <see cref="WorkQueueDetails"/> base on a <see cref="WorkQueue"/> object.
        /// </summary>
        /// <param name="workqueue"></param>
        /// <returns></returns>
        public WorkQueueDetails CreateWorkQueueDetail(Model.WorkQueue workqueue)
        {
            WorkQueueDetails detail = new WorkQueueDetails();

            detail.GUID = workqueue.GetKey();
            detail.ScheduledDateTime = workqueue.ScheduledTime;
            detail.ExpirationTime = workqueue.ExpirationTime;
            detail.FailureCount = workqueue.FailureCount;
            detail.Type = workqueue.WorkQueueTypeEnum;
            detail.Status = workqueue.WorkQueueStatusEnum;
            detail.ProcessorID = workqueue.ProcessorID;

            // Fetch UIDs
            WorkQueueUidAdaptor wqUidsAdaptor = new WorkQueueUidAdaptor();
            WorkQueueUidSelectCriteria uidCriteria = new WorkQueueUidSelectCriteria();
            uidCriteria.WorkQueueKey.EqualTo(workqueue.GetKey());
            IList<Model.WorkQueueUid> uids = wqUidsAdaptor.Get(uidCriteria);

            Hashtable mapSeries = new Hashtable();
            foreach (Model.WorkQueueUid uid in uids)
            {
                if (mapSeries.ContainsKey(uid.SeriesInstanceUid) == false)
                    mapSeries.Add(uid.SeriesInstanceUid, uid.SopInstanceUid);
            }

            detail.NumInstancesPending = uids.Count;
            detail.NumSeriesPending = mapSeries.Count;


            // Fetch the study and patient info
            StudyStorageAdaptor ssAdaptor = new StudyStorageAdaptor();
            StudyStorage storages = ssAdaptor.Get(workqueue.StudyStorageKey);

            StudyAdaptor studyAdaptor = new StudyAdaptor();
            StudySelectCriteria studycriteria = new StudySelectCriteria();
            studycriteria.StudyInstanceUid.EqualTo(storages.StudyInstanceUid);
            IList<Model.Study> studyList = studyAdaptor.Get(studycriteria);

            // Study may not be available until the images are processed.
            if (studyList != null && studyList.Count > 0)
            {
                StudyDetailsAssembler studyAssembler = new StudyDetailsAssembler();
                detail.Study = studyAssembler.CreateStudyDetail(studyList[0]);
            }

            return detail;
        }
    }
}
