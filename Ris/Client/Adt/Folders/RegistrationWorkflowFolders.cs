#region License

// Copyright (c) 2006-2007, ClearCanvas Inc.
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
using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Ris.Application.Common.RegistrationWorkflow;

namespace ClearCanvas.Ris.Client.Adt.Folders
{
    [ExtensionOf(typeof(RegistrationMainWorkflowFolderExtensionPoint))]
    [FolderForWorklistType(WorklistTokens.RegistrationScheduledWorklist)]
    [FolderPath("Registration/Scheduled", true)]
    public class ScheduledFolder : RegistrationWorkflowFolder
    {
        public ScheduledFolder(RegistrationWorkflowFolderSystemBase folderSystem, string folderDisplayName, string folderDescription, EntityRef worklistRef)
            : base(folderSystem, folderDisplayName, folderDescription, worklistRef)
        {
        }

        public ScheduledFolder(RegistrationWorkflowFolderSystemBase folderSystem)
            : this(folderSystem, null, null, null)
        {
        }

        public ScheduledFolder()
            : this(null)
        {
        }
    }

    [ExtensionOf(typeof(RegistrationMainWorkflowFolderExtensionPoint))]
    [FolderForWorklistType(WorklistTokens.RegistrationCheckedInWorklist)]
    [FolderPath("Registration/Checked In")]
    public class CheckedInFolder : RegistrationWorkflowFolder
    {
        [ExtensionPoint]
        public class DropHandlerExtensionPoint : ExtensionPoint<IDropHandler<RegistrationWorklistItem>>
        {
        }

        public CheckedInFolder(RegistrationWorkflowFolderSystemBase folderSystem, string folderDisplayName, string folderDescription, EntityRef worklistRef)
            : base(folderSystem, folderDisplayName, folderDescription, worklistRef, new DropHandlerExtensionPoint())
        {
        }

        public CheckedInFolder(RegistrationWorkflowFolderSystemBase folderSystem)
            : this(folderSystem, null, null, null)
        {
        }

        public CheckedInFolder()
            : this(null)
        {
        }
    }

    [ExtensionOf(typeof(RegistrationMainWorkflowFolderExtensionPoint))]
    [FolderForWorklistType(WorklistTokens.RegistrationInProgressWorklist)]
    [FolderPath("Registration/In Progress")]
    public class InProgressFolder : RegistrationWorkflowFolder
    {
        public InProgressFolder(RegistrationWorkflowFolderSystemBase folderSystem, string folderDisplayName, string folderDescription, EntityRef worklistRef)
            : base(folderSystem, folderDisplayName, folderDescription, worklistRef)
        {
        }

        public InProgressFolder(RegistrationWorkflowFolderSystemBase folderSystem)
            : this(folderSystem, null, null, null)
        {
        }

        public InProgressFolder()
            : this(null)
        {
        }
    }

    [ExtensionOf(typeof(RegistrationMainWorkflowFolderExtensionPoint))]
    [FolderForWorklistType(WorklistTokens.RegistrationCompletedWorklist)]
    [FolderPath("Registration/Completed")]
    public class CompletedFolder : RegistrationWorkflowFolder
    {
        public CompletedFolder(RegistrationWorkflowFolderSystemBase folderSystem, string folderDisplayName, string folderDescription, EntityRef worklistRef)
            : base(folderSystem, folderDisplayName, folderDescription, worklistRef)
        {
        }

        public CompletedFolder(RegistrationWorkflowFolderSystemBase folderSystem)
            : this(folderSystem, null, null, null)
        {
        }

        public CompletedFolder()
            : this(null)
        {
        }
    }

    [ExtensionOf(typeof(RegistrationMainWorkflowFolderExtensionPoint))]
    [FolderForWorklistType(WorklistTokens.RegistrationCancelledWorklist)]
    [FolderPath("Registration/Cancelled")]
    public class CancelledFolder : RegistrationWorkflowFolder
    {
        [ExtensionPoint]
        public class DropHandlerExtensionPoint : ExtensionPoint<IDropHandler<RegistrationWorklistItem>>
        {
        }

        public CancelledFolder(RegistrationWorkflowFolderSystemBase folderSystem, string folderDisplayName, string folderDescription, EntityRef worklistRef)
            : base(folderSystem, folderDisplayName, folderDescription, worklistRef, new DropHandlerExtensionPoint())
        {
        }

        public CancelledFolder(RegistrationWorkflowFolderSystemBase folderSystem)
            : this(folderSystem, null, null, null)
        {
        }

        public CancelledFolder()
            : this(null)
        {
        }
    }

    [ExtensionOf(typeof(RegistrationBookingWorkflowFolderExtensionPoint))]
    [FolderForWorklistType(WorklistTokens.RegistrationCompletedProtocolWorklist)]
    [FolderPath("Protocolling/Completed Protocol")]
    public class CompletedProtocolFolder : RegistrationWorkflowFolder
    {
        public CompletedProtocolFolder(RegistrationWorkflowFolderSystemBase folderSystem, string folderDisplayName, string folderDescription, EntityRef worklistRef)
            : base(folderSystem, folderDisplayName, folderDescription, worklistRef)
        {
        }

        public CompletedProtocolFolder(RegistrationWorkflowFolderSystemBase folderSystem)
            : this(folderSystem, null, null, null)
        {
        }

        public CompletedProtocolFolder()
            : this(null)
        {
        }
    }

    [ExtensionOf(typeof(RegistrationBookingWorkflowFolderExtensionPoint))]
    [FolderForWorklistType(WorklistTokens.RegistrationSuspendedProtocolWorklist)]
    [FolderPath("Protocolling/Suspended Protocol")]
    public class SuspendedProtocolFolder : RegistrationWorkflowFolder
    {
        public SuspendedProtocolFolder(RegistrationWorkflowFolderSystemBase folderSystem, string folderDisplayName, string folderDescription, EntityRef worklistRef)
            : base(folderSystem, folderDisplayName, folderDescription, worklistRef)
        {
        }

        public SuspendedProtocolFolder(RegistrationWorkflowFolderSystemBase folderSystem)
            : this(folderSystem, null, null, null)
        {
        }

        public SuspendedProtocolFolder()
            : this(null)
        {
        }
    }

    [ExtensionOf(typeof(RegistrationBookingWorkflowFolderExtensionPoint))]
    [FolderForWorklistType(WorklistTokens.RegistrationRejectedProtocolWorklist)]
    [FolderPath("Protocolling/Rejected Protocol")]
    public class RejectedProtocolFolder : RegistrationWorkflowFolder
    {
        public RejectedProtocolFolder(RegistrationWorkflowFolderSystemBase folderSystem, string folderDisplayName, string folderDescription, EntityRef worklistRef)
            : base(folderSystem, folderDisplayName, folderDescription, worklistRef)
        {
        }

        public RejectedProtocolFolder(RegistrationWorkflowFolderSystemBase folderSystem)
            : this(folderSystem, null, null, null)
        {
        }

        public RejectedProtocolFolder()
            : this(null)
        {
        }
    }

    [ExtensionOf(typeof(RegistrationBookingWorkflowFolderExtensionPoint))]
    [FolderForWorklistType(WorklistTokens.RegistrationPendingProtocolWorklist)]
    [FolderPath("Protocolling/Pending Protocol")]
    public class PendingProtocolFolder : RegistrationWorkflowFolder
    {
        public PendingProtocolFolder(RegistrationWorkflowFolderSystemBase folderSystem, string folderDisplayName, string folderDescription, EntityRef worklistRef)
            : base(folderSystem, folderDisplayName, folderDescription, worklistRef)
        {
        }

        public PendingProtocolFolder(RegistrationWorkflowFolderSystemBase folderSystem)
            : this(folderSystem, null, null, null)
        {
        }

        public PendingProtocolFolder()
            : this(null)
        {
        }
    }

    [ExtensionOf(typeof(RegistrationBookingWorkflowFolderExtensionPoint))]
    [FolderForWorklistType(WorklistTokens.RegistrationToBeScheduledWorklist)]
    [FolderPath("Protocolling/To Be Scheduled")]
    public class ToBeScheduledFolder : RegistrationWorkflowFolder
    {
        public ToBeScheduledFolder(RegistrationWorkflowFolderSystemBase folderSystem, string folderDisplayName, string folderDescription, EntityRef worklistRef)
            : base(folderSystem, folderDisplayName, folderDescription, worklistRef)
        {
        }

        public ToBeScheduledFolder(RegistrationWorkflowFolderSystemBase folderSystem)
            : this(folderSystem, null, null, null)
        {
        }

        public ToBeScheduledFolder()
            : this(null)
        {
        }
    }

    [FolderPath("Registration/Search")]
    public class RegistrationSearchFolder : RegistrationWorkflowFolder
    {
        private SearchData _searchData;

        public RegistrationSearchFolder(RegistrationWorkflowFolderSystemBase folderSystem)
            : base(folderSystem, null)
        {
			this.OpenIconSet = new IconSet(IconScheme.Colour, "SearchFolderOpenSmall.png", "SearchFolderOpenMedium.png", "SearchFolderOpenLarge.png");
			this.ClosedIconSet = new IconSet(IconScheme.Colour, "SearchFolderClosedSmall.png", "SearchFolderClosedMedium.png", "SearchFolderClosedLarge.png");
            this.IconSet = this.ClosedIconSet;
            this.RefreshTime = 0;
        }

        public SearchData SearchData
        {
            get { return _searchData; }
            set
            {
                _searchData = value;
                this.Refresh();
            }
        }

        protected override bool CanQuery()
        {
            if (this.SearchData != null)
                return true;

            return false;
        }

        protected override IList<RegistrationWorklistItem> QueryItems()
        {
            List<RegistrationWorklistItem> worklistItems = null;
            Platform.GetService<IRegistrationWorkflowService>(
                delegate(IRegistrationWorkflowService service)
                {
                    SearchRequest request = new SearchRequest();
                    request.TextQuery = this.SearchData.TextSearch;
                    request.ShowActiveOnly = this.SearchData.ShowActiveOnly;
                    TextQueryResponse<RegistrationWorklistItem> response = service.Search(request);
                    worklistItems = response.Matches;
                });

            if (worklistItems == null)
                worklistItems = new List<RegistrationWorklistItem>();

            return worklistItems;
        }

        protected override int QueryCount()
        {
            return this.ItemCount;
        }

    }
}
