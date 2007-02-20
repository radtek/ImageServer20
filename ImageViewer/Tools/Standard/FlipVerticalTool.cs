using System;
using System.Drawing;
using System.Diagnostics;
using ClearCanvas.Common;
using ClearCanvas.ImageViewer.Imaging;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.ImageViewer.BaseTools;
using ClearCanvas.ImageViewer.Graphics;

namespace ClearCanvas.ImageViewer.Tools.Standard
{
	[MenuAction("activate", "global-menus/MenuTools/Standard/MenuToolsStandardFlipVertical")]
	[KeyboardAction("activate", "imageviewer-keyboard/ToolsStandardFlipVertical", KeyStroke = XKeys.V)]
    [ButtonAction("activate", "global-toolbars/ToolbarStandard/ToolbarToolsStandardFlipVertical")]
	[ClickHandler("activate", "Activate")]
    [Tooltip("activate", "ToolbarToolsStandardFlipVertical")]
	[IconSet("activate", IconScheme.Colour, "", "Icons.FlipVerticalMedium.png", "Icons.FlipVerticalLarge.png")]
	[GroupHint("activate", "Tools.Image.Manipulation.Orientation.Flip.Vertical")]

    [ClearCanvas.Common.ExtensionOf(typeof(ImageViewerToolExtensionPoint))]
    public class FlipVerticalTool : ImageViewerTool
	{
		public FlipVerticalTool()
		{
		}

		public void Activate()
		{
			if (this.SelectedPresentationImage == null ||
				this.SelectedSpatialTransformProvider == null)
				return;

			// Save the old state
			SpatialTransformApplicator applicator = new SpatialTransformApplicator(this.SelectedPresentationImage);
			UndoableCommand command = new UndoableCommand(applicator);
			command.Name = SR.CommandFlipVertical;
			command.BeginState = applicator.CreateMemento();

			ISpatialTransformProvider image = this.SelectedSpatialTransformProvider;

			// Do the transform
			if (image.SpatialTransform.Rotation == 0 || image.SpatialTransform.Rotation == 180)
				image.SpatialTransform.FlipVertical = !image.SpatialTransform.FlipVertical;
			// If image is rotated 90 or 270, then a vertical flip is really a horizontal flip
			else
				image.SpatialTransform.FlipHorizontal = !image.SpatialTransform.FlipHorizontal;

			// Save the new state
			command.EndState = applicator.CreateMemento();

			// Apply the final state to all linked images
			applicator.SetMemento(command.EndState);

            this.Context.Viewer.CommandHistory.AddCommand(command);
		}
	}
}
