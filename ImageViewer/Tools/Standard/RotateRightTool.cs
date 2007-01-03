using System;
using System.Drawing;
using System.Diagnostics;
using ClearCanvas.Common;
using ClearCanvas.ImageViewer.Imaging;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Desktop.Actions;

namespace ClearCanvas.ImageViewer.Tools.Standard
{
	[MenuAction("activate", "global-menus/MenuTools/Standard/MenuToolsStandardRotateRight")]
	[KeyboardAction("activate", "imageviewer-keyboard/ToolsStandardRotateRight", KeyStroke = XKeys.R)]
    [ButtonAction("activate", "global-toolbars/ToolbarStandard/ToolbarToolsStandardRotateRight")]
    [ClickHandler("activate", "Activate")]
    [Tooltip("activate", "ToolbarToolsStandardRotateRight")]
	[IconSet("activate", IconScheme.Colour, "", "Icons.RotateRightMedium.png", "Icons.RotateRightLarge.png")]
	[GroupHint("activate", "Tools.Image.Manipulation.Orientation.Rotate.Right")]

    [ClearCanvas.Common.ExtensionOf(typeof(ImageViewerToolExtensionPoint))]
    public class RotateRightTool : Tool<IImageViewerToolContext>
	{
		public RotateRightTool()
		{
		}

		public void Activate()
		{
			IPresentationImage selectedImage = this.Context.Viewer.SelectedPresentationImage;

			if (selectedImage == null)
				return;

			SpatialTransformApplicator applicator = new SpatialTransformApplicator(selectedImage);
			UndoableCommand command = new UndoableCommand(applicator);
			command.Name = SR.CommandRotateRight;
			command.BeginState = applicator.CreateMemento();

			SpatialTransform spatialTransform = selectedImage.LayerManager.SelectedLayerGroup.SpatialTransform;
			spatialTransform.Rotation = spatialTransform.Rotation + 90;
			spatialTransform.Calculate();

			command.EndState = applicator.CreateMemento();

			// Apply the final state to all linked images
			applicator.SetMemento(command.EndState);

            this.Context.Viewer.CommandHistory.AddCommand(command);
		}
	}
}
