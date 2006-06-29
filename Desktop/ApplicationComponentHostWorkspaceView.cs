using System;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Common;

namespace ClearCanvas.Desktop
{
    internal class ApplicationComponentHostWorkspaceView : IWorkspaceView
    {
        private ApplicationComponentHostWorkspace _workspace;
        private IApplicationComponentView _componentView;

        internal ApplicationComponentHostWorkspaceView(ApplicationComponentHostWorkspace workspace, IApplicationComponentView componentView)
        {
            _workspace = workspace;
            _componentView = componentView;
        }

        #region IWorkspaceView Members

        public void SetWorkspace(Workspace workspace)
        {
            // not used - workspace is set in constructor
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IView Members

        public GuiToolkitID GuiToolkitID
        {
            get { return _componentView.GuiToolkitID; }
        }

        public object GuiElement
        {
            get { return _componentView.GuiElement; }
        }

        #endregion
    }
}
