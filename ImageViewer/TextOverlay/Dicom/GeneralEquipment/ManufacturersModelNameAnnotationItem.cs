using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.ImageViewer.Imaging;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.OffisWrapper;

namespace ClearCanvas.ImageViewer.TextOverlay.Dicom.GeneralEquipment
{
	internal class ManufacturersModelNameAnnotationItem : DicomStringAnnotationItem
	{
		public ManufacturersModelNameAnnotationItem(IAnnotationItemProvider ownerProvider)
			: base("GeneralEquipment.ManufacturersModelName", ownerProvider)
		{
		}

		protected override void GetStoredDicomValue(DicomPresentationImage dicomPresentationImage, out string dicomValue, out bool storedValueExists)
		{
			dicomValue = string.Empty;
			storedValueExists = false;

			//!! Uncomment once this item has been implemented in ImageSop class(es).
			//storedValueExists = true; 
			//dicomValue = dicomPresentationImage.ImageSop.ManufacturersModelName;
		}

		protected override DcmTagKey DicomTag
		{
			get { return Dcm.ManufacturersModelName; }
		}
	}
}
