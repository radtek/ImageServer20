using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.ImageViewer.Imaging;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.OffisWrapper;

namespace ClearCanvas.ImageViewer.TextOverlay.Dicom.GeneralSeries
{
	internal class LateralityAnnotationItem : DicomStringAnnotationItem
	{
		public LateralityAnnotationItem(IAnnotationItemProvider ownerProvider)
			: base("GeneralSeries.Laterality", ownerProvider)
		{
		}

		protected override void GetStoredDicomValue(DicomPresentationImage dicomPresentationImage, out string dicomValue, out bool storedValueExists)
		{
			storedValueExists = true;
			dicomValue = dicomPresentationImage.ImageSop.Laterality;
		}

		protected override DcmTagKey DicomTag
		{
			get { return Dcm.Laterality; }
		}
	}
}
