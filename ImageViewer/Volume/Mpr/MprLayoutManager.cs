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

using ClearCanvas.ImageViewer.Mathematics;
using ClearCanvas.ImageViewer.StudyManagement;

namespace ClearCanvas.ImageViewer.Volume.Mpr
{
	// Hack to expose LayoutManager
	internal class MprImageViewerComponent : ImageViewerComponent
	{
		private readonly ILayoutManager _layoutManager;

		public MprImageViewerComponent(ILayoutManager layoutManager) : base(layoutManager, null)
		{
			_layoutManager = layoutManager;
		}

		public new ILayoutManager LayoutManager
		{
			get { return _layoutManager; }
		}
	}

	internal class MprLayoutManager : LayoutManager
	{
		#region ImageViewerComponent, LayoutManager creation method

		//ggerade ToRef: We're thinking we may introduce an MprImageViewerComponent that will own the volume
		//	and take care of the layout creation and such. For now I just needed a place to put this
		//	utility method which bootstraps the Mpr Component
		public ImageSop _tempSop;

		public static ImageViewerComponent CreateMprLayoutAndComponent(Volume volume)
		{
			MprLayoutManager layoutManager = new MprLayoutManager(volume);

			MprImageViewerComponent imageViewer = new MprImageViewerComponent(layoutManager);

			//ggerade ToRes: I almost got rid of the displayset members... hacked this in so that
			//	the workspace title would get set
			VolumeSlicer slicer = new VolumeSlicer(volume);
			slicer.SetSlicePlaneIdentity();
			DisplaySet displaySet = slicer.CreateDisplaySet("Temporary");
			layoutManager._tempSop = ((DicomGrayscalePresentationImage)displaySet.PresentationImages[0]).ImageSop;
			imageViewer.StudyTree.AddSop(layoutManager._tempSop);

			return imageViewer;
		}

		#endregion

		#region Private fields

		private readonly Volume _volume;
		private readonly VolumeSlicer _identitySlicer;
		private readonly VolumeSlicer _orthoXSlicer;
		private readonly VolumeSlicer _orthoYSlicer;
		private readonly VolumeSlicer _obliqueSlicer;

		#endregion

		#region Simple MPR layout

		public MprLayoutManager(Volume volume)
		{
			_volume = volume;
			_identitySlicer = new VolumeSlicer(volume);
			_orthoXSlicer = new VolumeSlicer(volume);
			_orthoYSlicer = new VolumeSlicer(volume);
			_obliqueSlicer = new VolumeSlicer(volume);
		}

		public override void Layout()
		{
			LayoutPhysicalWorkspace();

			FillPhysicalWorkspace();

			ImageViewer.PhysicalWorkspace.Draw();
		}

		protected override void LayoutPhysicalWorkspace()
		{
			ImageViewer.PhysicalWorkspace.SetImageBoxGrid(2, 2);

			foreach (IImageBox imageBox in ImageViewer.PhysicalWorkspace.ImageBoxes)
				imageBox.SetTileGrid(1, 1);
		}

		protected override void FillPhysicalWorkspace()
		{
			_identitySlicer.SetSlicePlaneIdentity();
			DisplaySet identityDisplaySet = _identitySlicer.CreateOrthoDisplaySet("Identity");
			_orthoXSlicer.SetSlicePlaneOrthoX();
			DisplaySet orthoXDisplaySet = _orthoXSlicer.CreateOrthoDisplaySet("OrthoX");
			_orthoYSlicer.SetSlicePlaneOrthoY();
			DisplaySet orthoYDisplaySet = _orthoYSlicer.CreateOrthoDisplaySet("OrthoY");

			// Hey, I said it was a hack!
			_obliqueSlicer.SetSlicePlaneOblique(45, 0, 0);
			//Vector3D sliceThroughPatient = _volume.CenterPointPatient;
			//sliceThroughPatient.Y -= 100;
			//sliceThroughPatient.Z += 100;
			//_obliqueSlicer.SliceThroughPointPatient = sliceThroughPatient;
			//_obliqueSlicer.SliceExtentMillimeters = 150;
			DisplaySet obliqueDisplaySet = _obliqueSlicer.CreateDisplaySet("Oblique");

			ImageViewer.StudyTree.RemoveSop(this._tempSop);

			// Here we add the Mpr DisplaySets to the IVC's StudyTree, this keeps the framework happy
			AddAllSopsToStudyTree(ImageViewer.StudyTree, identityDisplaySet);
			AddAllSopsToStudyTree(ImageViewer.StudyTree, orthoXDisplaySet);
			AddAllSopsToStudyTree(ImageViewer.StudyTree, orthoYDisplaySet);
			AddAllSopsToStudyTree(ImageViewer.StudyTree, obliqueDisplaySet);

			IPhysicalWorkspace physicalWorkspace = ImageViewer.PhysicalWorkspace;
			physicalWorkspace.ImageBoxes[0].DisplaySet = identityDisplaySet;
			// Let's start out in the middle of each stack
			physicalWorkspace.ImageBoxes[0].TopLeftPresentationImageIndex = identityDisplaySet.PresentationImages.Count / 2;
			physicalWorkspace.ImageBoxes[1].DisplaySet = orthoYDisplaySet;
			physicalWorkspace.ImageBoxes[1].TopLeftPresentationImageIndex = orthoYDisplaySet.PresentationImages.Count / 2;
			physicalWorkspace.ImageBoxes[2].DisplaySet = orthoXDisplaySet;
			physicalWorkspace.ImageBoxes[2].TopLeftPresentationImageIndex = orthoXDisplaySet.PresentationImages.Count / 2;
			physicalWorkspace.ImageBoxes[3].DisplaySet = obliqueDisplaySet;
			physicalWorkspace.ImageBoxes[3].TopLeftPresentationImageIndex = obliqueDisplaySet.PresentationImages.Count / 2;

			//TODO: Add this property and use it to disable the Layout Components (in Layout.Basic).
			//physicalWorkspace.IsReadOnly = true;
		}

		#endregion

		#region Oblique Image Rotations

		public void SetObliqueCutLine(Vector3D sourceOrientationColumn, Vector3D sourceOrientationRow, Vector3D startPoint, Vector3D endPoint)
		{
			IPhysicalWorkspace physicalWorkspace = ImageViewer.PhysicalWorkspace;
			IImageBox obliqueImageBox = physicalWorkspace.ImageBoxes[3];
			IDisplaySet displaySet = obliqueImageBox.TopLeftPresentationImage.ParentDisplaySet;

			// Hang on to the current index, we'll keep it the same with the new DisplaySet
			int currentIndex = obliqueImageBox.TopLeftPresentationImageIndex;

			//ggerade ToRes: What about other settings like Window/level? It is lost when the DisplaySets
			// are swapped out.

			// Clear out the old DisplaySet
			obliqueImageBox.DisplaySet = null;
			RemoveAllSopsFromStudyTree(ImageViewer.StudyTree, displaySet);

			//foreach (PresentationImage presentationImage in displaySet.PresentationImages)
			//	presentationImage.Dispose();

			_obliqueSlicer.SetSlicePlanePatient(sourceOrientationColumn, sourceOrientationRow, startPoint, endPoint);

			displaySet = _obliqueSlicer.CreateDisplaySet("Oblique");

			AddAllSopsToStudyTree(ImageViewer.StudyTree, displaySet);

			// Hacked this in so that the Imagebox wouldn't jump to first image all the time
			ImageBox box = (ImageBox)obliqueImageBox;
			box.PresetTopLeftPresentationImageIndex = currentIndex;
			obliqueImageBox.DisplaySet = displaySet;
			obliqueImageBox.Draw();
		}

		public void RotateObliqueImage(IPresentationImage presImage, int rotateX, int rotateY, int rotateZ)
		{
			IPhysicalWorkspace physicalWorkspace = ImageViewer.PhysicalWorkspace;

			IImageBox obliqueImageBox = physicalWorkspace.ImageBoxes[3];

			IDisplaySet displaySet = presImage.ParentDisplaySet;

			if (presImage != obliqueImageBox.TopLeftPresentationImage)
				return;

			// Hang on to the current index, we'll keep it the same with the new DisplaySet
			int currentIndex = obliqueImageBox.TopLeftPresentationImageIndex;

			//ggerade ToRes: What about other settings like Window/level? It is lost when the DisplaySets
			// are swapped out.

			// Clear out the old DisplaySet
			obliqueImageBox.DisplaySet = null;
			RemoveAllSopsFromStudyTree(ImageViewer.StudyTree, displaySet);

			_obliqueSlicer.SetSlicePlaneOblique(rotateX, rotateY, rotateZ);

			//Vector3D sliceThroughPatient = _volume.CenterPointPatient;
			//sliceThroughPatient.X = 0;
			//sliceThroughPatient.Y = 0;
			//sliceThroughPatient.Z = 0;
			//_obliqueSlicer.SliceThroughPointPatient = sliceThroughPatient;
			//_obliqueSlicer.SliceExtentMillimeters = 150;
		
			displaySet = _obliqueSlicer.CreateDisplaySet("Oblique");

			AddAllSopsToStudyTree(ImageViewer.StudyTree, displaySet);

			// Hacked this in so that the Imagebox wouldn't jump to first image all the time
			ImageBox box = (ImageBox) obliqueImageBox;
			box.PresetTopLeftPresentationImageIndex = currentIndex;

			obliqueImageBox.DisplaySet = displaySet;
			// Taken care of by hack above
			//obliqueImageBox.TopLeftPresentationImageIndex = currentIndex;

			obliqueImageBox.Draw();
		}

		public int GetObliqueImageRotationX(IPresentationImage presImage)
		{
			if (presImage == ImageViewer.PhysicalWorkspace.ImageBoxes[3].TopLeftPresentationImage)
				return _obliqueSlicer.RotateAboutX;

			return 0;
		}

		public int GetObliqueImageRotationY(IPresentationImage presImage)
		{
			if (presImage == ImageViewer.PhysicalWorkspace.ImageBoxes[3].TopLeftPresentationImage)
				return _obliqueSlicer.RotateAboutY;

			return 0;
		}

		public int GetObliqueImageRotationZ(IPresentationImage presImage)
		{
			if (presImage == ImageViewer.PhysicalWorkspace.ImageBoxes[3].TopLeftPresentationImage)
				return _obliqueSlicer.RotateAboutZ;

			return 0;
		}

		#endregion

		#region StudyTree helpers

		public void AddDisplaySetsToStudyTree(StudyTree tree)
		{
			IPhysicalWorkspace physicalWorkspace = ImageViewer.PhysicalWorkspace;

			foreach (IImageBox box in physicalWorkspace.ImageBoxes)
			{
				AddAllSopsToStudyTree(tree, box.TopLeftPresentationImage.ParentDisplaySet);
			}
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

		private static void RemoveAllSopsFromStudyTree(StudyTree tree, IDisplaySet displaySet)
		{
			// Now load the generated images into the viewer
			foreach (PresentationImage presentationImage in displaySet.PresentationImages)
			{
				DicomGrayscalePresentationImage dicomGrayscalePresentationImage =
					(DicomGrayscalePresentationImage) presentationImage;

				ImageSop sop = dicomGrayscalePresentationImage.ImageSop;
				tree.RemoveSop(sop);
				//sop.Dispose();
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