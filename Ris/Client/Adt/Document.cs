using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Desktop;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Ris.Client.Adt
{
    public abstract class Document
    {
        private string _docID;
        private IWorkspace _workspace;
        private IDesktopWindow _desktopWindow;

        private event EventHandler _closed;

        public Document(string docID, IDesktopWindow desktopWindow)
        {
            _docID = docID;
            _desktopWindow = desktopWindow;
        }

        public void Open()
        {
            LaunchWorkspace(GetComponent(), GetTitle());
        }

        public bool Close()
        {
            if (_workspace != null && _desktopWindow.WorkspaceManager.Workspaces.Remove(_workspace))
            {
                _workspace = null;
                return true;
            }
            return false;
        }

        public event EventHandler Closed
        {
            add { _closed += value; }
            remove { _closed -= value; }
        }

        public void Activate()
        {
            _workspace.Activate();
        }

        protected abstract string GetTitle();

        protected abstract IApplicationComponent GetComponent();

        protected void LaunchWorkspace(IApplicationComponent component, string title)
        {
            _workspace = ApplicationComponent.LaunchAsWorkspace(
                _desktopWindow,
                component,
                title,
                delegate(IApplicationComponent c)
                {
                    // remove from list of open editors
                    DocumentManager.Remove(_docID);
                    EventsHelper.Fire(_closed, this, EventArgs.Empty);
                });

            DocumentManager.Set(_docID, this);
        }
    }
}
