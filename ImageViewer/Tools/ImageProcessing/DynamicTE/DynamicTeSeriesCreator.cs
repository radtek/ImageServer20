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
using System.Drawing;
using System.IO;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Dicom;
using ClearCanvas.ImageViewer.BaseTools;
using ClearCanvas.ImageViewer.StudyManagement;

namespace ClearCanvas.ImageViewer.Tools.ImageProcessing.DynamicTe
{
	static class DynamicTeSeriesCreator
	{
		public static void Create(IImageViewerToolContext viewerContext)
		{
			IDisplaySet selectedDisplaySet = viewerContext.Viewer.SelectedImageBox.DisplaySet;
			string name = String.Format("{0} - Dynamic TE", selectedDisplaySet.Name);
			IDisplaySet t2DisplaySet = new DisplaySet(name, "");

			double currentSliceLocation = 0.0;

			BackgroundTask task = new BackgroundTask(
				delegate(IBackgroundTaskContext context)
				{
					int i = 0;

					foreach (IPresentationImage image in selectedDisplaySet.PresentationImages)
					{
						IImageSopProvider imageSopProvider = image as IImageSopProvider;

						if (imageSopProvider == null)
							continue;

						ImageSop imageSop = imageSopProvider.ImageSop;

						if (imageSop.SliceLocation != currentSliceLocation)
						{
							currentSliceLocation = imageSop.SliceLocation;

							DynamicTePresentationImage t2Image = CreateT2Image(imageSop);
							t2DisplaySet.PresentationImages.Add(t2Image);
						}

						string message = String.Format("Processing {0} of {1} images", i, selectedDisplaySet.PresentationImages.Count);
						i++;

						BackgroundTaskProgress progress = new BackgroundTaskProgress(i, selectedDisplaySet.PresentationImages.Count, message);
						context.ReportProgress(progress);
					}
				}, false);

			ProgressDialog.Show(task, viewerContext.DesktopWindow, true, ProgressBarStyle.Blocks);

			viewerContext.Viewer.LogicalWorkspace.ImageSets[0].DisplaySets.Add(t2DisplaySet);
		}

		private static DynamicTePresentationImage CreateT2Image(ImageSop imageSop)
		{
			DicomFile pdMap = FindMap(imageSop.StudyInstanceUID, imageSop.SliceLocation, "PD");
			pdMap.Load(DicomReadOptions.Default);

			DicomFile t2Map = FindMap(imageSop.StudyInstanceUID, imageSop.SliceLocation, "T2");
			t2Map.Load(DicomReadOptions.Default);

			DicomFile probMap = FindMap(imageSop.StudyInstanceUID, imageSop.SliceLocation, "CHI2PROB");
			probMap.Load(DicomReadOptions.Default);

			DynamicTePresentationImage t2Image = new DynamicTePresentationImage(
				imageSop,
				ConvertToByte((ushort[])pdMap.DataSet[DicomTags.PixelData].Values),
				ConvertToByte((ushort[])t2Map.DataSet[DicomTags.PixelData].Values),
				ConvertToByte((ushort[])probMap.DataSet[DicomTags.PixelData].Values));

			t2Image.DynamicTe.Te = 50.0f;
			return t2Image;
		}

		private static byte[] ConvertToByte(ushort[] pixelData)
		{
			byte[] newPixelData = new byte[pixelData.Length * 2];
			Buffer.BlockCopy(pixelData, 0, newPixelData, 0, newPixelData.Length);
			return newPixelData;
		}

		private static DicomFile FindMap(string studyUID, double sliceLocation, string suffix)
		{
			string directory = String.Format(".\\T2_MAPS\\{0}", studyUID);
			string[] files = Directory.GetFiles(directory);

			foreach (string file in files)
			{
				string str = String.Format("loc{0:F2}_{1}", sliceLocation, suffix);

				if (file.Contains(str))
					return new DicomFile(file);
			}

			return null;
		}
	}
}
