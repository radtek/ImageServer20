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

using System;
using ClearCanvas.ImageViewer.StudyManagement;

namespace ClearCanvas.ImageViewer.Volume.Mpr
{
	public class MprLayoutManager : LayoutManager
	{
		#region ImageViewerComponent, LayoutManager creation method

		//ggerade ToRef: We're thinking we may introduce an MprImageViewerComponent that will own the volume
		//	and take care of the layout creation and such. For now I just needed a place to put this
		//	utility method which bootstraps the Mpr Component
		public static ImageViewerComponent CreateMprLayoutAndComponent(Volume volume)
		{
			MprLayoutManager layoutManager = new MprLayoutManager(volume);

			ImageViewerComponent imageViewer = new ImageViewerComponent(layoutManager);

			// Here we add the Mpr DisplaySets to the IVC's StudyTree, this keeps the framework happy
			layoutManager.AddDisplaySetsToStudyTree(imageViewer.StudyTree);

			return imageViewer;
		}

		#endregion

		#region Private fields

		private Volume _volume;
		private DisplaySet _sagittalDisplaySet;
		private DisplaySet _coronalDisplaySet;
		private DisplaySet _axialDisplaySet;
		
		#endregion

		public MprLayoutManager(Volume volume)
		{
			_volume = volume;

			//ggerade ToRes: It might be better to create these on demand - this could be a relatively expensive
			//	ctor. Consider doing in FillPhysicalWorkspace maybe? 
			// Recall that the Sops need to go into the IVC's StudyTree, so we have to manage that somehow.
			_sagittalDisplaySet = VolumeSlicer.CreateSagittalDisplaySet(volume);
			_coronalDisplaySet = VolumeSlicer.CreateCoronalDisplaySet(volume);
			_axialDisplaySet = VolumeSlicer.CreateAxialDisplaySet(volume);
		}

		#region ILayoutManager Members

		public override void Layout()
		{
			LayoutPhysicalWorkspace();

			FillPhysicalWorkspace();

			ImageViewer.PhysicalWorkspace.Draw();
		}

		#endregion

		#region Protected Virtual Methods

		protected override void LayoutPhysicalWorkspace()
		{
			ImageViewer.PhysicalWorkspace.SetImageBoxGrid(1, 3);

			foreach (IImageBox imageBox in ImageViewer.PhysicalWorkspace.ImageBoxes)
				imageBox.SetTileGrid(1, 1);
		}

		protected override void FillPhysicalWorkspace()
		{
			IPhysicalWorkspace physicalWorkspace = ImageViewer.PhysicalWorkspace;
			physicalWorkspace.ImageBoxes[0].DisplaySet = _sagittalDisplaySet;
			// Let's start out in the middle of each stack
			physicalWorkspace.ImageBoxes[0].TopLeftPresentationImageIndex = _sagittalDisplaySet.PresentationImages.Count/2;
			physicalWorkspace.ImageBoxes[1].DisplaySet = _coronalDisplaySet;
			physicalWorkspace.ImageBoxes[1].TopLeftPresentationImageIndex = _coronalDisplaySet.PresentationImages.Count/2;
			physicalWorkspace.ImageBoxes[2].DisplaySet = _axialDisplaySet;
			physicalWorkspace.ImageBoxes[2].TopLeftPresentationImageIndex = _axialDisplaySet.PresentationImages.Count/2;

			//TODO: Add this property and use it to disable the Layout Components (in Layout.Basic).
			//physicalWorkspace.IsReadOnly = true;
		}

		#endregion

		#region StudyTree helpers

		public void AddDisplaySetsToStudyTree(StudyTree tree)
		{
			AddAllSopsToStudyTree(tree, _sagittalDisplaySet);
			AddAllSopsToStudyTree(tree, _coronalDisplaySet);
			AddAllSopsToStudyTree(tree, _axialDisplaySet);
		}

		// Note: The overlays expect that a Sop is parented by a Series, so this was the easiest way
		//	to keep the IVC happy.
		private static void AddAllSopsToStudyTree(StudyTree tree, IDisplaySet displaySet)
		{
			// Now load the generated images into the viewer
			foreach (PresentationImage presentationImage in displaySet.PresentationImages)
			{
				DicomGrayscalePresentationImage dicomGrayscalePresentationImage =
					(DicomGrayscalePresentationImage) presentationImage;

				ImageSop sop = dicomGrayscalePresentationImage.ImageSop;
				tree.AddSop(sop);
			}
		}

		#endregion

		#region Disposal

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_volume != null)
					_volume.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion
	}
}