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
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.ImageViewer.Volume.Mpr
{
	public interface IMprStandardSliceSet : IMprSliceSet
	{
		bool IsReadOnly { get; }
		IVolumeSlicerParams SlicerParams { get; set; }
		event EventHandler SlicerParamsChanged;
	}

	/// <summary>
	/// A basic, mutable, single-plane slice view of an MPR <see cref="Volume"/>.
	/// </summary>
	public class MprStandardSliceSet : MprSliceSet, IMprStandardSliceSet
	{
		private event EventHandler _slicerParamsChanged;
		private IVolumeSlicerParams _slicerParams;

		public MprStandardSliceSet(Volume volume, IVolumeSlicerParams slicerParams) : base(volume)
		{
			Platform.CheckForNullReference(slicerParams, "slicerParams");
			_slicerParams = slicerParams;

			base.Description = slicerParams.Description;
			this.Reslice();
		}

		bool IMprStandardSliceSet.IsReadOnly
		{
			get { return false; }
		}

		public IVolumeSlicerParams SlicerParams
		{
			get { return _slicerParams; }
			set
			{
				if (_slicerParams != value)
				{
					_slicerParams = value;
					this.OnSlicerParamsChanged();
				}
			}
		}

		public event EventHandler SlicerParamsChanged
		{
			add { _slicerParamsChanged += value; }
			remove { _slicerParamsChanged -= value; }
		}

		protected virtual void OnSlicerParamsChanged()
		{
			this.Reslice();
			base.Description = this.SlicerParams.Description;
			EventsHelper.Fire(_slicerParamsChanged, this, EventArgs.Empty);
		}

		protected void Reslice()
		{
			base.SuspendSliceSopsChangedEvent();
			try
			{
				base.ClearAndDisposeSops();

				using (VolumeSlicer slicer = new VolumeSlicer(base.Volume, _slicerParams, base.Uid))
				{
					foreach (ISliceSopDataSource dataSource in slicer.CreateSlices())
					{
						base.SliceSops.Add(new MprSliceSop(dataSource));
					}
				}
			}
			finally
			{
				base.ResumeSliceSopsChangedEvent(true);
			}
		}

		#region Old Stuff for Experimental Tools

#if EXPERIMENTAL_TOOLS

		private Vector3D _startPoint, _endPoint;
		public void SetCutLine(Vector3D sourceOrientationColumn, Vector3D sourceOrientationRow, Vector3D startPoint, Vector3D endPoint)
		{
			if (_identifier != MprDisplaySetIdentifier.Oblique)
				throw new InvalidOperationException("Display set must be oblique.");

			_startPoint = startPoint;
			_endPoint = endPoint;

			int currentIndex = ImageBox.TopLeftPresentationImageIndex;

			object transformMemento = null;
			IPresentationImage oldImage = this.ImageBox.TopLeftPresentationImage;
			if (oldImage is ISpatialTransformProvider)
				transformMemento = (oldImage as ISpatialTransformProvider).SpatialTransform.CreateMemento();

			IComposableLut lut = null;
			if (oldImage is IVoiLutProvider)
				lut = (oldImage as IVoiLutProvider).VoiLutManager.GetLut();

			_slicer.Slicing = VolumeSlicerParams.CreateSlicing(_slicer.Volume, sourceOrientationColumn, sourceOrientationRow, startPoint, endPoint);
			this.Reslice();

			if (lut != null || transformMemento != null)
			{
				// Hacked this in so that the Imagebox wouldn't jump to first image all the time
				foreach (IPresentationImage image in PresentationImages)
				{
					if (lut != null && image is IVoiLutProvider)
						(image as IVoiLutProvider).VoiLutManager.InstallLut(lut.Clone());
					if (transformMemento != null && image is ISpatialTransformProvider)
						(image as ISpatialTransformProvider).SpatialTransform.SetMemento(transformMemento);
				}
			}

			//TODO: this seems a bit slow
			IPresentationImage closestImage = GetClosestSlice(startPoint + (endPoint - startPoint) * 2);
			if (closestImage == null)
				ImageBox.TopLeftPresentationImageIndex = currentIndex;
			else
				ImageBox.TopLeftPresentationImage = closestImage;

			Draw();
		}

		public void CreateFullObliqueDisplaySet()
		{
			if (_startPoint == null || _endPoint == null)
				return;

			object transformMemento = null;
			IPresentationImage oldImage = this.ImageBox.TopLeftPresentationImage;
			if (oldImage is ISpatialTransformProvider)
				transformMemento = (oldImage as ISpatialTransformProvider).SpatialTransform.CreateMemento();

			IComposableLut lut = null;
			if (oldImage is IVoiLutProvider)
				lut = (oldImage as IVoiLutProvider).VoiLutManager.GetLut();

			this.Reslice();

			if (lut != null || transformMemento != null)
			{
				// Hacked this in so that the Imagebox wouldn't jump to first image all the time
				foreach (IPresentationImage image in PresentationImages)
				{
					if (lut != null && image is IVoiLutProvider)
						(image as IVoiLutProvider).VoiLutManager.InstallLut(lut.Clone());
					if (transformMemento != null && image is ISpatialTransformProvider)
						(image as ISpatialTransformProvider).SpatialTransform.SetMemento(transformMemento);
				}
			}

			//TODO: this seems a bit slow
			IPresentationImage closestImage = GetClosestSlice(_startPoint + (_endPoint - _startPoint) * 2);
			if (closestImage == null)
				ImageBox.TopLeftPresentationImageIndex = 0;
			else
				ImageBox.TopLeftPresentationImage = closestImage;

			Draw();
		}

		public void Rotate(int rotateX, int rotateY, int rotateZ)
		{
			if (_identifier != MprDisplaySetIdentifier.Oblique)
				throw new InvalidOperationException("Display set must be oblique.");

			// Hang on to the current index, we'll keep it the same with the new DisplaySet
			int currentIndex = ImageBox.TopLeftPresentationImageIndex;

			object transformMemento = null;
			IPresentationImage oldImage = this.ImageBox.TopLeftPresentationImage;
			if (oldImage is ISpatialTransformProvider)
				transformMemento = (oldImage as ISpatialTransformProvider).SpatialTransform.CreateMemento();

			IComposableLut lut = null;
			if (oldImage is IVoiLutProvider)
				lut = (oldImage as IVoiLutProvider).VoiLutManager.GetLut();

			try
			{
				_slicerParams = new VolumeSlicerParams(rotateX, rotateY, rotateZ);
				this.Reslice();
			}
			catch
			{
				//SB: do this for now until we can handle things properly
				return;
			}

			if (lut != null || transformMemento != null)
			{
				// Hacked this in so that the Imagebox wouldn't jump to first image all the time
				foreach (IPresentationImage image in PresentationImages)
				{
					if (lut != null && image is IVoiLutProvider)
						(image as IVoiLutProvider).VoiLutManager.InstallLut(lut.Clone());
					if (transformMemento != null && image is ISpatialTransformProvider)
						(image as ISpatialTransformProvider).SpatialTransform.SetMemento(transformMemento);
				}
			}

			ImageBox.TopLeftPresentationImageIndex = currentIndex;

			Draw();
		}

		private IPresentationImage GetClosestSlice(Vector3D positionPatient)
		{
			float closestDistance = float.MaxValue;
			IPresentationImage closestImage = null;

			foreach (IPresentationImage image in PresentationImages)
			{
				if (image is IImageSopProvider)
				{
					Frame frame = (image as IImageSopProvider).Frame;
					Vector3D positionCenterOfImage = frame.ImagePlaneHelper.ConvertToPatient(new PointF((frame.Columns - 1) / 2F, (frame.Rows - 1) / 2F));
					Vector3D distanceVector = positionCenterOfImage - positionPatient;
					float distance = distanceVector.Magnitude;

					if (distance <= closestDistance)
					{
						closestDistance = distance;
						closestImage = image;
					}
				}
			}

			return closestImage;
		}

#endif

		#endregion
	}
}