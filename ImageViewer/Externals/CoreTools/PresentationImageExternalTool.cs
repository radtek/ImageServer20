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
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;

namespace ClearCanvas.ImageViewer.Externals.CoreTools
{
	[ExtensionOf(typeof (ImageViewerToolExtensionPoint))]
	public class PresentationImageExternalTool : ExternalToolBase
	{
		private readonly IResourceResolver _resourceResolver = new ResourceResolver(typeof (PresentationImageExternalTool).Assembly);

		private IActionSet _actions = null;
		private IPresentationImage _selectedPresentationImage = null;

		public override IActionSet Actions
		{
			get
			{
				if (_actions == null)
				{
					List<IAction> actions = new List<IAction>();
					foreach (IExternal external in ExternalCollection.SavedExternals)
					{
						if (!external.IsValid || string.IsNullOrEmpty(external.Label))
							continue;

						IPresentationImageExternal consumer = external as IPresentationImageExternal;
						if (consumer != null && consumer.CanLaunch(base.SelectedPresentationImage))
						{
							string id = Guid.NewGuid().ToString();
							ActionPath actionPath = new ActionPath(string.Format("imageviewer-contextmenu/MenuExternals/{0}", id), _resourceResolver);
							MenuAction action = new MenuAction(id, actionPath, ClickActionFlags.None, _resourceResolver);
							action.Label = string.Format(SR.FormatOpenImageWith, consumer.Label);
							action.SetClickHandler(delegate
							                       	{
							                       		try
							                       		{
							                       			consumer.Launch(base.SelectedPresentationImage);
							                       		}
							                       		catch (Exception ex)
							                       		{
							                       			ExceptionHandler.Report(ex, base.Context.DesktopWindow);
							                       		}
							                       	});
							actions.Add(action);
						}
					}
					_actions = new ActionSet(actions);
				}
				return _actions;
			}
		}

		protected override void OnPresentationImageSelected(object sender, PresentationImageSelectedEventArgs e)
		{
			base.OnPresentationImageSelected(sender, e);
			if (_selectedPresentationImage != this.SelectedPresentationImage)
			{
				_selectedPresentationImage = this.SelectedPresentationImage;
				_actions = null;
			}
		}

		protected override void OnExternalsChanged(EventArgs e)
		{
			base.OnExternalsChanged(e);
			_actions = null;
		}
	}
}