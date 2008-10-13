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
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Web.Application.Pages.Common;

namespace ClearCanvas.ImageServer.Web.Application.Pages.Queues.StudyIntegrityQueue
{
    public partial class Default : BasePage
    {        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ServerPartitionTabs.SetupLoadPartitionTabs(delegate(ServerPartition partition)
                                                           {
                                                               SearchPanel panel =
                                                                   LoadControl("SearchPanel.ascx") as SearchPanel;
                                                               panel.ServerPartition = partition;
                                                               panel.ID = "SearchPanel_" + partition.AeTitle;
                                                               return panel;
                                                           });
        }

        public void OnReconcileItem(ReconcileDetails details)
        {
            ReconcileDialog.ReconcileDetails = details;
            ReconcileDialog.StudyIntegrityQueueItem = details.StudyIntegrityQueueItem;
            ReconcileDialog.Show();
        }


        public void UpdateUI()
        {
			foreach (ServerPartition partition in ServerPartitionTabs.ServerPartitionList)
			{
				SearchPanel panel =
					ServerPartitionTabs.GetUserControlForPartition(partition.GetKey()).FindControl("SearchPanel_" +
																								   partition.AeTitle) as
					SearchPanel;
				panel.UpdateUI();
			}
        }
    }
}
