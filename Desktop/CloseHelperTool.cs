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

using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Common;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Desktop
{
    /// <summary>
    /// Provides "Close Assistant" services, which inform the user of workspaces that require attention prior
    /// to a desktop window close or application quit.
    /// </summary>
    [ExtensionOf(typeof(ApplicationToolExtensionPoint))]
    class CloseHelperTool : Tool<IApplicationToolContext>
    {
        private Shelf _closeHelperShelf;

        public CloseHelperTool()
        {

        }

        public override void Initialize()
        {
            Application.Quitting += new EventHandler<QuittingEventArgs>(Application_Quitting);
            Application.DesktopWindows.ItemClosing += new EventHandler<ClosingItemEventArgs<DesktopWindow>>(Windows_ItemClosing);

            base.Initialize();
        }

        void Windows_ItemClosing(object sender, ClosingItemEventArgs<DesktopWindow> e)
        {
            // if application is quitting, don't do anything here because it will be handled by the Quitting handler
            if (e.Interaction != UserInteraction.Allowed || e.Cancel || e.Reason == CloseReason.ApplicationQuit)
                return;

            // find all the workspaces that can't be closed
            DesktopWindow window = e.Item;
            bool showHelper = CollectionUtils.Contains<Workspace>(window.Workspaces,
                delegate(Workspace w) { return !w.QueryCloseReady(); });

            if (showHelper)
            {
                e.Cancel = true;
                ShowShelf(window, e.Reason);
            }
        }

        void Application_Quitting(object sender, QuittingEventArgs e)
        {
            if (e.Interaction != UserInteraction.Allowed || e.Cancel)
                return;

            // check if we need to show the shelf
            bool showHelper = CollectionUtils.Contains<DesktopWindow>(Application.DesktopWindows,
                delegate(DesktopWindow w) { return !w.QueryCloseReady(); });

            if (showHelper)
            {
                e.Cancel = true;
                ShowShelf(Application.ActiveDesktopWindow, CloseReason.ApplicationQuit);
            }
        }

        private void ShowShelf(DesktopWindow window, CloseReason reason)
        {
            if (_closeHelperShelf != null && _closeHelperShelf.DesktopWindow != window)
            {
                // the shelf is in another window, close it
                _closeHelperShelf.Close();
                _closeHelperShelf = null;
            }

            // the shelf is not currently open
            if (_closeHelperShelf == null)
            {
                // launch it
                CloseHelperComponent component = new CloseHelperComponent();
                _closeHelperShelf = ApplicationComponent.LaunchAsShelf(window, component, "Close Assistant",
                    ShelfDisplayHint.DockLeft, delegate(IApplicationComponent c) { _closeHelperShelf = null; });
            }

            CloseHelperComponent helper = (CloseHelperComponent)_closeHelperShelf.Component;
            helper.Refresh(reason != CloseReason.ApplicationQuit);
        }
    }
}
