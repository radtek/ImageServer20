using System;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Desktop
{
    public class WorkspaceEventArgs : CollectionEventArgs<IWorkspace>
	{
		public WorkspaceEventArgs()
		{
		}

        public WorkspaceEventArgs(IWorkspace workspace)
		{
			Platform.CheckForNullReference(workspace, "workspace");

			base.Item = workspace;
		}

        public IWorkspace Workspace
        {
            get { return base.Item; }
        }
	}
}
