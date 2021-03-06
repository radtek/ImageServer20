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
using System.Text.RegularExpressions;
using ClearCanvas.Dicom.Iod;

namespace ClearCanvas.Dicom.Validation
{
	//TODO: leave this, or incorporate elsewhere?

	/// <summary>
	/// Validation helper for Dicom data, including image data.
	/// </summary>
	public static class DicomValidator
	{
		//Regex construction is expensive.
		[ThreadStatic] private static Regex _uidValidationRegex;

		private static Regex UidValidationRegex
		{
			get
			{
				if (_uidValidationRegex == null)
					_uidValidationRegex = new Regex("^[0-9]+([\\.][0-9]+)*$");

				return _uidValidationRegex;
			}	
		}

		#region Image Validation

		/// <summary>
		/// Validate that the number of rows > 0.
		/// </summary>
		/// <param name="rows"></param>
		public static void ValidateRows(int rows)
		{
			if (rows <= 0)
				throw new DicomDataException(String.Format(SR.ExceptionInvalidRows, rows));
		}

		/// <summary>
		/// Validate that the number of columns > 0.
		/// </summary>
		/// <param name="columns"></param>
		public static void ValidateColumns(int columns)
		{
			if (columns <= 0)
				throw new DicomDataException(String.Format(SR.ExceptionInvalidColumns, columns));
		}

		/// <summary>
		/// Validate that the number of bits allocated is either 8 or 16.
		/// </summary>
		/// <param name="bitsAllocated"></param>
		public static void ValidateBitsAllocated(int bitsAllocated)
		{
			if (bitsAllocated != 8 &&
			    bitsAllocated != 16)
			{
				throw new DicomDataException(String.Format(SR.ExceptionInvalidBitsAllocated, bitsAllocated));
			}
		}

		/// <summary>
		/// Validate that the number of bits stored is >= 1.
		/// </summary>
		/// <param name="bitsStored"></param>
		public static void ValidateBitsStored(int bitsStored)
		{
			if (bitsStored <= 0)
				throw new DicomDataException(String.Format(SR.ExceptionInvalidBitsStored, bitsStored));
		}

		/// <summary>
		/// Validates that the high bit is >= 1.
		/// </summary>
		/// <param name="highBit"></param>
		public static void ValidateHighBit(int highBit)
		{
			if (highBit <= 0)
				throw new DicomDataException(String.Format(SR.ExceptionInvalidHighBit, highBit));
		}

		/// <summary>
		/// Validates that the number of samples per pixel is either 1 or 3.
		/// </summary>
		/// <param name="samplesPerPixel"></param>
		/// <remarks>
		/// ARGB and CMYK photometric interpretations have been retired by DICOM and
		/// so <see cref="SamplesPerPixel"/> can only be 1 or 3.
		/// </remarks>
		public static void ValidateSamplesPerPixel(int samplesPerPixel)
		{
			if (samplesPerPixel != 1 &&
			    samplesPerPixel != 3)
			{
				throw new DicomDataException(String.Format(SR.ExceptionInvalidSamplesPerPixel, samplesPerPixel));
			}
		}

		/// <summary>
		/// Validates that the pixel representation is either 0 or 1.
		/// </summary>
		/// <param name="pixelRepresentation"></param>
		public static void ValidatePixelRepresentation(int pixelRepresentation)
		{
			if (pixelRepresentation != 0 && pixelRepresentation != 1)
				throw new DicomDataException(String.Format(SR.ExceptionInvalidPixelRepresentation, pixelRepresentation));
		}

		/// <summary>
		/// Validates that the photometric interpretation is not unknown.
		/// </summary>
		/// <param name="photometricInterpretation"></param>
		public static void ValidatePhotometricInterpretation(PhotometricInterpretation photometricInterpretation)
		{
			if (photometricInterpretation == PhotometricInterpretation.Unknown)
			{
				throw new DicomDataException(String.Format(SR.ExceptionInvalidPhotometricInterpretation, photometricInterpretation));
			}
		}

		/// <summary>
		/// Validates that the size of the pixel data byte buffer is equal
		/// to <i>rows</i> x <i>columns</i> x <i>bitsPerPixel</i> / 8.
		/// </summary>
		/// <param name="pixelData"></param>
		/// <param name="rows"></param>
		/// <param name="columns"></param>
		/// <param name="bitsPerPixel">Can be 8 or 16 in the case of grayscale images,
		/// or 32 in the case of colour images.  <i>BitsPerPixel</i> is the product
		/// of DICOM's <i>Bits Allocated</i> and DICOM's <i>Samples Per Pixel</i></param>.
		public static void ValidatePixelData(byte[] pixelData, int rows, int columns, int bitsPerPixel)
		{
			int sizeInBytes = rows * columns * bitsPerPixel / 8;

			if (pixelData.Length != sizeInBytes)
				throw new DicomDataException(string.Format(SR.ExceptionInvalidPixelData, pixelData.Length, sizeInBytes));
		}

		/// <summary>
		/// Validates that the input image property relationships are compatible.
		/// </summary>
		public static void ValidateImagePropertyRelationships(int bitsStored, int bitsAllocated, int highBit, PhotometricInterpretation photometricInterpretation, int planarConfiguration, int samplesPerPixel)
		{
			if (bitsStored > bitsAllocated)
				throw new DicomDataException(String.Format(SR.ExceptionInvalidBitsStoredBitsAllocated, bitsStored, bitsAllocated));

			if (highBit > bitsAllocated - 1)
				throw new DicomDataException(String.Format(SR.ExceptionInvalidHighBitBitsAllocated, highBit, bitsAllocated));

			if ((photometricInterpretation == PhotometricInterpretation.Monochrome1
			     || photometricInterpretation == PhotometricInterpretation.Monochrome2) &&
			    samplesPerPixel != 1)
			{
				throw new DicomDataException(String.Format(SR.ExceptionInvalidPhotometricInterpretationSamplesPerPixel, photometricInterpretation, samplesPerPixel));
			}

			if (samplesPerPixel != 1)
			{
				if (planarConfiguration != 0 && planarConfiguration != 1)
					throw new DicomDataException(String.Format(SR.ExceptionInvalidPlanarConfiguration));
			}

			if ((photometricInterpretation == PhotometricInterpretation.Rgb ||
			     photometricInterpretation == PhotometricInterpretation.YbrFull ||
			     photometricInterpretation == PhotometricInterpretation.YbrFull422 ||
			     photometricInterpretation == PhotometricInterpretation.YbrPartial422 ||
			     photometricInterpretation == PhotometricInterpretation.YbrIct ||
			     photometricInterpretation == PhotometricInterpretation.YbrRct) &&
			    samplesPerPixel != 3)
			{
				throw new DicomDataException(String.Format(SR.ExceptionInvalidPhotometricInterpretationSamplesPerPixel, photometricInterpretation, samplesPerPixel));
			}
		}

		#endregion

		#region General Validation

		/// <summary>
		/// Validate the specified uid conforms to the Dicom standard.
		/// </summary>
		public static void ValidateUid(string uid)
		{
			if (String.IsNullOrEmpty(uid) || uid.TrimEnd(' ').Length == 0)
				return;// ok

			if (uid.Length > 64)
				throw new DicomDataException(String.Format(SR.ExceptionGeneralUIDLength, uid));

			if (!UidValidationRegex.IsMatch(uid))
			{
				throw new DicomDataException(String.Format(SR.ExceptionGeneralUIDFormat, uid));
			}
		}

		/// <summary>
		/// Validate that the Transfer Syntax UID conforms to the Dicom standard.
		/// </summary>
		public static void ValidateTransferSyntaxUID(string uid)
		{

			try
			{
				ValidateUid(uid);
			}
			catch (DicomDataException e)
			{
				throw new DicomDataException(String.Format("Invalid transfer syntax: {0} ", e.Message));
			} 
            
			if (String.IsNullOrEmpty(uid) || uid.TrimEnd(' ').Length == 0)
				throw new DicomDataException(String.Format(SR.ExceptionInvalidTransferUID));

		}

		/// <summary>
		/// Validate that the Sop Class UID conforms to the Dicom standard.
		/// </summary>
		public static void ValidateSopClassUid(string uid)
		{

			try
			{
				ValidateUid(uid);
			}
			catch (DicomDataException e)
			{
				throw new DicomDataException(String.Format("Invalid sop class: {0} ", e.Message));
			}

			if (String.IsNullOrEmpty(uid) || uid.TrimEnd(' ').Length == 0)
				throw new DicomDataException("The sop class uid cannot be empty.");
		}

		/// <summary>
		/// Validate that the Study Instance UID conforms to the Dicom standard.
		/// </summary>
		public static void ValidateStudyInstanceUID(string uid)
		{
			try
			{
				ValidateUid(uid);
			}
			catch (DicomDataException e)
			{
				throw new DicomDataException(String.Format("Invalid Study Instance UID: {0} ", e.Message));
			}

			if (String.IsNullOrEmpty(uid) || uid.TrimEnd(' ').Length == 0)
				throw new DicomDataException(String.Format(SR.ExceptionInvalidStudyInstanceUID));

            
		}

		/// <summary>
		/// Validate that the Series Instance UID conforms to the Dicom standard.
		/// </summary>
		public static void ValidateSeriesInstanceUID(string uid)
		{
			try
			{
				ValidateUid(uid);
			}
			catch (DicomDataException e)
			{
				throw new DicomDataException(String.Format("Invalid Series Instance UID: {0} ", e.Message));
			}

			if (String.IsNullOrEmpty(uid) || uid.TrimEnd(' ').Length == 0)
				throw new DicomDataException(String.Format(SR.ExceptionInvalidSeriesInstanceUID));

            
		}

		/// <summary>
		/// Validate that the Sop Instance UID conforms to the Dicom standard.
		/// </summary>
		public static void ValidateSOPInstanceUID(string uid)
		{
            
			try
			{
				ValidateUid(uid);
			}
			catch (DicomDataException e)
			{
				throw new DicomDataException(String.Format("Invalid SOP Instance UID: {0} ", e.Message));
			}

			if (String.IsNullOrEmpty(uid) || uid.TrimEnd(' ').Length == 0)
				throw new DicomDataException(String.Format(SR.ExceptionInvalidSOPInstanceUID));

		}

		#endregion
	}
}