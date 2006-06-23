using System;
using System.Collections.Generic;
using System.Text;

using ClearCanvas.Common;
using ClearCanvas.Workstation.ExtensionBrowser;

namespace ClearCanvas.Workstation.ExtensionBrowser.ExtensionPointView
{
    public class ExtensionPointViewExtensionsNode : BrowserNode
    {
        private ExtensionInfo[] _extensions;

        internal ExtensionPointViewExtensionsNode(ExtensionInfo[] extensions)
        {
            _extensions = extensions;
        }

        protected override void CreateChildNodes()
        {
            foreach (ExtensionInfo e in _extensions)
            {
                AddChild(new ExtensionPointViewExtensionNode(e));
            }
        }

        public override string DisplayName
        {
            get { return string.Format("Extensions ({0})", ChildNodes.Count); }
        }

        public override string Details
        {
            get { return ""; }
        }
    }
}
