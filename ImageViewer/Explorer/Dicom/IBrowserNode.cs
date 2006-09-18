using System;
using System.Collections.Generic;
using System.Text;

namespace ClearCanvas.ImageViewer.Explorer.Dicom
{
    public interface IBrowserNode
    {
        List<IBrowserNode> ChildNodes
        {
            get;
        }

        string ServerName
        {
            get;
        }

        string ServerPath
        {
            get;
        }

        string Details
        {
            get;
        }
    }
}
