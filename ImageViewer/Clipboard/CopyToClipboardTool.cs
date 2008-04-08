#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
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

using ClearCanvas.Common;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using ClearCanvas.ImageViewer.BaseTools;
using System;

namespace ClearCanvas.ImageViewer.Clipboard
{
	[MenuAction("copyImage", "imageviewer-contextmenu/MenuCopyImageToClipboard", "CopyImage")]
	[IconSet("copyImage", IconScheme.Colour, "Icons.CopyToClipboardToolSmall.png", "Icons.CopyToClipboardToolMedium.png", "Icons.CopyToClipboardToolLarge.png")]
	[MenuAction("copyDisplaySet", "imageviewer-contextmenu/MenuCopyDisplaySetToClipboard", "CopyDisplaySet")]
	[IconSet("copyDisplaySet", IconScheme.Colour, "Icons.CopyToClipboardToolSmall.png", "Icons.CopyToClipboardToolMedium.png", "Icons.CopyToClipboardToolLarge.png")]
	[EnabledStateObserver("copyImage", "Enabled", "EnabledChanged")]
	[ExtensionOf(typeof(ImageViewerToolExtensionPoint))]
	public class CopyToClipboardTool : ImageViewerTool
	{
		public CopyToClipboardTool()
		{
		}

		public void CopyImage()
		{
			try
			{
				BlockingOperation.Run(
					delegate
						{
							Clipboard.Add(this.SelectedPresentationImage);
						});
			}
			catch(Exception e)
			{
				ExceptionHandler.Report(e, this.Context.DesktopWindow);
			}
		}

		public void CopyDisplaySet()
		{
			try
			{
				BlockingOperation.Run(
					delegate
						{
							Clipboard.Add(this.SelectedPresentationImage.ParentDisplaySet);
						});
			}
			catch (Exception e)
			{
				ExceptionHandler.Report(e, this.Context.DesktopWindow);
			}
		}

		protected override void OnTileSelected(object sender, TileSelectedEventArgs e)
		{
			base.OnTileSelected(sender, e);

			if (e.SelectedTile.PresentationImage != null)
				this.Enabled = true;
			else
				this.Enabled = false;
		}
	}
}
