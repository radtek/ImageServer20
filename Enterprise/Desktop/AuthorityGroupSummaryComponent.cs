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
using ClearCanvas.Common.Authorization;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Enterprise.Common.Admin.AuthorityGroupAdmin;
using ClearCanvas.Enterprise.Desktop;

namespace ClearCanvas.Enterprise.Desktop
{
    [MenuAction("launch", "global-menus/Admin/Authority Groups", "Launch")]
    [ActionPermission("launch", ClearCanvas.Enterprise.Common.AuthorityTokens.Admin.Security.AuthorityGroup)]

    [ExtensionOf(typeof(DesktopToolExtensionPoint))]
    public class AuthorityGroupSummaryTool : Tool<IDesktopToolContext>
    {
        private IWorkspace _workspace;

        public void Launch()
        {
            if (_workspace == null)
            {
                try
                {
                    AuthorityGroupSummaryComponent component = new AuthorityGroupSummaryComponent();

                    _workspace = ApplicationComponent.LaunchAsWorkspace(
                        this.Context.DesktopWindow,
                        component,
                        SR.TitleAuthorityGroup);
                    _workspace.Closed += delegate { _workspace = null; };
                }
                catch (Exception e)
                {
                    ExceptionHandler.Report(e, this.Context.DesktopWindow);
                }
            }
            else
            {
                _workspace.Activate();
            }
        }
    }

    /// <summary>
    /// Extension point for views onto <see cref="AuthorityGroupSummaryComponent"/>
    /// </summary>
    [ExtensionPoint]
    public class AuthorityGroupSummaryComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
    {
    }

    /// <summary>
    /// AuthorityGroupSummaryComponent class
    /// </summary>
    public class AuthorityGroupSummaryComponent : SummaryComponentBase<AuthorityGroupSummary, AuthorityGroupTable>
    {
		/// <summary>
		/// Override this method to perform custom initialization of the action model,
		/// such as adding permissions or adding custom actions.
		/// </summary>
		/// <param name="model"></param>
		protected override void InitializeActionModel(AdminActionModel model)
		{
			base.InitializeActionModel(model);

			model.AddAction("duplicate", "Duplicate","Icons.DuplicateSmall.png", "Duplicate the selected authority group",
								DuplicateSelectedItem,
								ClearCanvas.Enterprise.Common.AuthorityTokens.Admin.Security.AuthorityGroup);

			model.AddAction("import", "Import", "Icons.ImportAuthorityTokensSmall.png", "Import authority tokens and groups from local plugins",
								Import,
								ClearCanvas.Enterprise.Common.AuthorityTokens.Admin.Security.AuthorityGroup);


			model.Add.SetPermissibility(ClearCanvas.Enterprise.Common.AuthorityTokens.Admin.Security.AuthorityGroup);
			model.Edit.SetPermissibility(ClearCanvas.Enterprise.Common.AuthorityTokens.Admin.Security.AuthorityGroup);
			model.Delete.SetPermissibility(ClearCanvas.Enterprise.Common.AuthorityTokens.Admin.Security.AuthorityGroup);
		}

		#region Presentation Model

		public void Import()
		{
			try
			{
				DialogBoxAction action = this.Host.ShowMessageBox("Import authority tokens and groups defined in locally installed plugins?",
								 MessageBoxActions.OkCancel);
				if (action == DialogBoxAction.Ok)
				{
					AuthorityTokenDefinition[] tokens = AuthorityGroupSetup.GetAuthorityTokens();
					AuthorityGroupDefinition[] groups = AuthorityGroupSetup.GetDefaultAuthorityGroups();

					Platform.GetService<IAuthorityGroupAdminService>(
						delegate(IAuthorityGroupAdminService service)
						{
							// first import the tokens, since the default groups will likely depend on these tokens
							service.ImportAuthorityTokens(
								new ImportAuthorityTokensRequest(
									CollectionUtils.Map<AuthorityTokenDefinition, AuthorityTokenSummary>(tokens,
										delegate(AuthorityTokenDefinition t) { return new AuthorityTokenSummary(t.Token, t.Description); })));

							// then import the default groups
							service.ImportAuthorityGroups(
								new ImportAuthorityGroupsRequest(
									CollectionUtils.Map<AuthorityGroupDefinition, AuthorityGroupDetail>(groups,
										delegate(AuthorityGroupDefinition g)
										{
											return new AuthorityGroupDetail(null, g.Name,
												CollectionUtils.Map<string, AuthorityTokenSummary>(g.Tokens,
													delegate(string t) { return new AuthorityTokenSummary(t, null); }));
										})));
						});

				}

			}
			catch (Exception e)
			{
				ExceptionHandler.Report(e, this.Host.DesktopWindow);
			}
		}

		public void DuplicateSelectedItem()
		{
			try
			{
				AuthorityGroupSummary item = CollectionUtils.FirstElement(this.SelectedItems);
				if(item == null) return;

				AuthorityGroupEditorComponent editor = new AuthorityGroupEditorComponent(item, true);
				ApplicationComponentExitCode exitCode = LaunchAsDialog(
					this.Host.DesktopWindow, editor, SR.TitleUpdateAuthorityGroup);
				if (exitCode == ApplicationComponentExitCode.Accepted)
				{
					this.Table.Items.Add(editor.AuthorityGroupSummary);
					this.SummarySelection = new Selection(editor.AuthorityGroupSummary);
				}
			}
			catch (Exception e)
			{
				ExceptionHandler.Report(e, this.Host.DesktopWindow);
			}
		}



		#endregion

		protected override bool SupportsDelete
		{
			get { return true; }
		}

		/// <summary>
		/// Gets the list of items to show in the table, according to the specifed first and max items.
		/// </summary>
		/// <returns></returns>
		protected override IList<AuthorityGroupSummary> ListItems(int firstRow, int maxRows)
		{
			ListAuthorityGroupsRequest request = new ListAuthorityGroupsRequest();
			request.Page.FirstRow = firstRow;
			request.Page.MaxRows = maxRows;

			ListAuthorityGroupsResponse listResponse = null;
			Platform.GetService<IAuthorityGroupAdminService>(
				delegate(IAuthorityGroupAdminService service)
				{
					listResponse = service.ListAuthorityGroups(request);
				});

			return listResponse.AuthorityGroups;
		}

		/// <summary>
		/// Called to handle the "add" action.
		/// </summary>
		/// <param name="addedItems"></param>
		/// <returns>True if items were added, false otherwise.</returns>
		protected override bool AddItems(out IList<AuthorityGroupSummary> addedItems)
		{
			addedItems = new List<AuthorityGroupSummary>();
			AuthorityGroupEditorComponent editor = new AuthorityGroupEditorComponent();
			ApplicationComponentExitCode exitCode = LaunchAsDialog(
				this.Host.DesktopWindow, editor, SR.TitleAddAuthorityGroup);
			if (exitCode == ApplicationComponentExitCode.Accepted)
			{
				addedItems.Add(editor.AuthorityGroupSummary);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Called to handle the "edit" action.
		/// </summary>
		/// <param name="items">A list of items to edit.</param>
		/// <param name="editedItems">The list of items that were edited.</param>
		/// <returns>True if items were edited, false otherwise.</returns>
		protected override bool EditItems(IList<AuthorityGroupSummary> items, out IList<AuthorityGroupSummary> editedItems)
		{
			editedItems = new List<AuthorityGroupSummary>();
			AuthorityGroupSummary item = CollectionUtils.FirstElement(items);

			AuthorityGroupEditorComponent editor = new AuthorityGroupEditorComponent(item, false);
			ApplicationComponentExitCode exitCode = LaunchAsDialog(
				this.Host.DesktopWindow, editor, SR.TitleUpdateAuthorityGroup + " - " + item.Name);
			if (exitCode == ApplicationComponentExitCode.Accepted)
			{
				editedItems.Add(editor.AuthorityGroupSummary);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Called to handle the "delete" action, if supported.
		/// </summary>
		/// <param name="items"></param>
		/// <param name="deletedItems">The list of items that were deleted.</param>
		/// <param name="failureMessage">The message if there any errors that occurs during deletion.</param>
		/// <returns>True if items were deleted, false otherwise.</returns>
		protected override bool DeleteItems(IList<AuthorityGroupSummary> items, out IList<AuthorityGroupSummary> deletedItems, out string failureMessage)
		{
			failureMessage = null;
			deletedItems = new List<AuthorityGroupSummary>();

			foreach (AuthorityGroupSummary item in items)
			{
				try
				{
					Platform.GetService<IAuthorityGroupAdminService>(
						delegate(IAuthorityGroupAdminService service)
						{
							service.DeleteAuthorityGroup(new DeleteAuthorityGroupRequest(item.AuthorityGroupRef));
						});

					deletedItems.Add(item);
				}
				catch (Exception e)
				{
					failureMessage = e.Message;
				}
			}

			return deletedItems.Count > 0;
		}

		/// <summary>
		/// Compares two items to see if they represent the same item.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		protected override bool IsSameItem(AuthorityGroupSummary x, AuthorityGroupSummary y)
		{
            return x.AuthorityGroupRef.Equals(y.AuthorityGroupRef, true);
		}
	}
}
