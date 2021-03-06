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
	[MenuAction("activate", "global-menus/MenuTools/MenuStandard/MenuFlipHorizontal", "Activate")]
	[ButtonAction("activate", "global-toolbars/ToolbarStandard/ToolbarFlipHorizontal", "Activate", KeyStroke = XKeys.H)]
	[EnabledStateObserver("activate", "Enabled", "EnabledChanged")]
	[Tooltip("activate", "TooltipFlipHorizontal")]
	[IconSet("activate", IconScheme.Colour, "Icons.FlipHorizontalToolSmall.png", "Icons.FlipHorizontalToolMedium.png", "Icons.FlipHorizontalToolLarge.png")]
	[GroupHint("activate", "Tools.Image.Manipulation.Orientation.Flip.Horizontal")]

	[ExtensionOf(typeof(ImageViewerToolExtensionPoint))]
    public class FlipHorizontalTool : ImageViewerTool
	{
		private readonly SpatialTransformImageOperation _operation;

		public FlipHorizontalTool()
		{
			_operation = new SpatialTransformImageOperation(Apply);
		}

		public void Activate()
		{
			if (!_operation.AppliesTo(this.SelectedPresentationImage))
				return;

			ImageOperationApplicator applicator = new ImageOperationApplicator(this.SelectedPresentationImage, _operation);
			UndoableCommand historyCommand = applicator.ApplyToAllImages();
			if (historyCommand != null)
			{
				historyCommand.Name = SR.CommandFlipHorizontal;
				this.Context.Viewer.CommandHistory.AddCommand(historyCommand);
			}
		}

		#region IImageOperation Members

		public void Apply(IPresentationImage image)
		{
			ISpatialTransform transform = (ISpatialTransform)_operation.GetOriginator(image);
			// Do the transform
			if (transform.RotationXY == 0 || transform.RotationXY == 180)
				transform.FlipY = !transform.FlipY;
			// If image is rotated 90 or 270, then a horizontal flip is really a vertical flip
			else
				transform.FlipX = !transform.FlipX;
		}

		#endregion
	}
}
