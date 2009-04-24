using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Ris.Application.Common.Admin.WorklistAdmin;
using System.Threading;

namespace ClearCanvas.Ris.Client
{
    /// <summary>
    /// Allows users to add/edit/delete user-defined worklists directly in the folder explorer.
    /// </summary>
	[ButtonAction("add", "folderexplorer-folders-toolbar/New Worklist", "Add")]
	[MenuAction("add", "folderexplorer-folders-contextmenu/New Worklist", "Add")]
	[Tooltip("add", "Add a new worklist")]
	[EnabledStateObserver("add", "AddEnabled", "EnablementChanged")]
    [VisibleStateObserver("add", "Visible", "VisibleChanged")]

	[ButtonAction("edit", "folderexplorer-folders-toolbar/Edit Worklist", "Edit")]
	[MenuAction("edit", "folderexplorer-folders-contextmenu/Edit Worklist", "Edit")]
	[Tooltip("edit", "Edit worklist")]
	[EnabledStateObserver("edit", "EditEnabled", "EnablementChanged")]
    [VisibleStateObserver("edit", "Visible", "VisibleChanged")]

	[ButtonAction("delete", "folderexplorer-folders-toolbar/Delete Worklist", "Delete")]
	[MenuAction("delete", "folderexplorer-folders-contextmenu/Delete Worklist", "Delete")]
	[Tooltip("delete", "Delete worklist")]
	[EnabledStateObserver("delete", "DeleteEnabled", "EnablementChanged")]
    [VisibleStateObserver("delete", "Visible", "VisibleChanged")]

	[ExtensionOf(typeof(FolderExplorerGroupToolExtensionPoint))]
	public class UserWorklistTool : Tool<IFolderExplorerGroupToolContext>
    {
        #region Public API

        public bool Visible
        {
            get { return IsWorklistFolderSystem && (HasGroupAdminAuthority || HasPersonalAdminAuthority); }
        }

        public event EventHandler VisibleChanged
        {
            add { this.Context.SelectedFolderSystemChanged += value; }
            remove { this.Context.SelectedFolderSystemChanged -= value; }
        }


		public event EventHandler EnablementChanged
		{
			add
            {
                this.Context.SelectedFolderChanged += value;
                this.Context.SelectedFolderSystemChanged += value;
            }
			remove
            {
                this.Context.SelectedFolderChanged -= value;
                this.Context.SelectedFolderSystemChanged -= value;
            }
		}

		public bool AddEnabled
		{
			get { return CanAdd(); }
		}

		public bool EditEnabled
		{
			get { return CanEdit(this.Context.SelectedFolder); }
		}

		public bool DeleteEnabled
		{
			get { return CanDelete(this.Context.SelectedFolder); }
		}

		public void Add()
		{
			if (!CanAdd())
				return;

			WorklistEditorComponent editor = new WorklistEditorComponent(false);
			ApplicationComponentExitCode exitCode = ApplicationComponent.LaunchAsDialog(this.Context.DesktopWindow, editor, "New Worklist");
			if(exitCode == ApplicationComponentExitCode.Accepted)
			{
				IWorklistFolderSystem fs = (IWorklistFolderSystem) this.Context.SelectedFolderSystem;
				foreach (WorklistAdminSummary worklist in editor.EditedWorklistSummaries)
				{
                    // try to add worklist to this folder system
					IWorklistFolder folder = fs.AddWorklistFolder(worklist);

                    // if add was successful, refresh the folder
                    if (folder != null)
                    {
                        fs.InvalidateFolder(folder);
                    }
				}
			}
		}

		public void Edit()
		{
            IWorklistFolder folder = (IWorklistFolder)this.Context.SelectedFolder;
            if (!CanEdit(folder))
                return;

			WorklistEditorComponent editor = new WorklistEditorComponent(folder.WorklistRef, false, false);
            ApplicationComponentExitCode exitCode = ApplicationComponent.LaunchAsDialog(this.Context.DesktopWindow, editor, "Edit Worklist");
            if (exitCode == ApplicationComponentExitCode.Accepted)
            {
                // refresh the folder
                IWorklistFolderSystem fs = (IWorklistFolderSystem)this.Context.SelectedFolderSystem;
                fs.InvalidateFolder(folder);
            }
		}

		public void Delete()
		{
            IWorklistFolder folder = (IWorklistFolder)this.Context.SelectedFolder;
            if (!CanDelete(folder))
                return;

            // confirm deletion
            if (this.Context.DesktopWindow.ShowMessageBox(
                "Are you sure you want to delete the selected folder?", MessageBoxActions.OkCancel)
                != DialogBoxAction.Ok)
                return;

            try
            {
                // delete worklist
                Platform.GetService<IWorklistAdminService>(
                    delegate(IWorklistAdminService service)
                    {
                        service.DeleteWorklist(
                            new DeleteWorklistRequest(folder.WorklistRef));
                    });

                // if successful, remove folder from folder system
                IWorklistFolderSystem fs = (IWorklistFolderSystem)this.Context.SelectedFolderSystem;
                fs.Folders.Remove(folder);
            }
            catch (Exception e)
            {
                ExceptionHandler.Report(e, this.Context.DesktopWindow);
            }
        }

        #endregion

        #region Helpers

        private bool CanAdd()
		{
            return IsWorklistFolderSystem;
		}

		private bool CanEdit(IFolder folder)
		{
            return CheckAccess(folder);
		}

		private bool CanDelete(IFolder folder)
		{
            return CheckAccess(folder);
        }

        /// <summary>
        /// Checks if the current user can modify/delete this folder.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private bool CheckAccess(IFolder folder)
        {
            IWorklistFolder wf = folder as IWorklistFolder;

            // if not a worklist folder, or not user defined, can't edit it
            if (wf == null || wf.Ownership == WorklistOwnership.Admin)
                return false;

            // if staff owned, must have personal authority
            if (wf.Ownership == WorklistOwnership.Staff && !HasPersonalAdminAuthority)
                return false;

            // if group owned, must have group authority
            if (wf.Ownership == WorklistOwnership.Group && !HasGroupAdminAuthority)
                return false;

            return true;
        }

        private bool IsWorklistFolderSystem
        {
            get { return this.Context.SelectedFolderSystem is IWorklistFolderSystem; }
        }

        private bool HasGroupAdminAuthority
        {
            get { return Thread.CurrentPrincipal.IsInRole(ClearCanvas.Ris.Application.Common.AuthorityTokens.Workflow.Worklist.Group); }
        }

        private bool HasPersonalAdminAuthority
        {
            get { return Thread.CurrentPrincipal.IsInRole(ClearCanvas.Ris.Application.Common.AuthorityTokens.Workflow.Worklist.Personal); }
        }

        #endregion
    }
}
