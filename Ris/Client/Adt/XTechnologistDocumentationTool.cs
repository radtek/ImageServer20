using System;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Ris.Application.Common.ModalityWorkflow;

namespace ClearCanvas.Ris.Client.Adt
{
    [MenuAction("apply", "folderexplorer-items-contextmenu/XDocument", "Apply")]
    [ButtonAction("apply", "folderexplorer-items-toolbar/XDocument", "Apply")]
    [IconSet("apply", IconScheme.Colour, "StartToolSmall.png", "StartToolMedium.png", "StartToolLarge.png")]
    //[ExtensionOf(typeof(TechnologistWorkflowItemToolExtensionPoint))]
    public class XTechnologistDocumentationTool : Tool<ITechnologistWorkflowItemToolContext>
    {
        private IWorkspace _workspace;

        public void Apply()
        {
            try
            {
                if (_workspace == null)
                {
                    ModalityWorklistItem item = CollectionUtils.FirstElement<ModalityWorklistItem>(this.Context.SelectedItems);

                    _workspace = ApplicationComponent.LaunchAsWorkspace(
                        this.Context.DesktopWindow,
                        new XTechnologistDocumentationComponent(item),
                        "Technologist Documentation (OLD)",
                        delegate(IApplicationComponent c) { _workspace = null;  });
                }
                else
                {
                    _workspace.Activate();
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Report(e, this.Context.DesktopWindow);
            }
        }
    }
}