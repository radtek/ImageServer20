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

using System.Collections.Generic;

namespace ClearCanvas.Dicom.Iod.Modules
{
	/// <summary>
	/// BitmapDisplayShutter Module
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section C.7.6.15 (Table ?)</remarks>
	public class BitmapDisplayShutterModuleIod : IodBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BitmapDisplayShutterModuleIod"/> class.
		/// </summary>	
		public BitmapDisplayShutterModuleIod() : base() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="BitmapDisplayShutterModuleIod"/> class.
		/// </summary>
		public BitmapDisplayShutterModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider) { }

		/// <summary>
		/// Gets or sets the shutter shape.  Type 1.
		/// </summary>
		public ShutterShape ShutterShape
		{
			get
			{
				ShutterShape returnValue = ShutterShape.None;
				string[] values = base.DicomAttributeProvider[DicomTags.ShutterShape].Values as string[];
				if (values != null && values.Length > 0)
				{
					foreach (string value in values)
					{
						string upperValue = value.ToUpperInvariant();
						if (upperValue == "CIRCULAR")
							returnValue |= Iod.ShutterShape.Circular;
						else if (upperValue == "RECTANGULAR")
							returnValue |= Iod.ShutterShape.Rectangular;
						else if (upperValue == "POLYGONAL")
							returnValue |= Iod.ShutterShape.Polygonal;
						else if (upperValue == "BITMAP")
							returnValue |= Iod.ShutterShape.Bitmap;
					}
				}

				return returnValue;
			}
			set
			{
				if (value == ShutterShape.None)
				{
					base.DicomAttributeProvider[DicomTags.ShutterShape] = null;
				}
				else
				{
					if ((value & ShutterShape.Bitmap) == ShutterShape.Bitmap)
						base.DicomAttributeProvider[DicomTags.ShutterShape].SetString(0, ShutterShape.Bitmap.ToString().ToUpperInvariant());
				}
			}
		}

		/// <summary>
		/// Gets or set the group of an overlay in the presentation state Iod.
		/// </summary>
		public uint ShutterOverlayGroup
		{
			get { return base.DicomAttributeProvider[DicomTags.ShutterOverlayGroup].GetUInt32(0, 0); }	
			set
			{
				//TODO: validate group
				base.DicomAttributeProvider[DicomTags.ShutterOverlayGroup].SetUInt32(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the shutter presentation value.  Type 1.
		/// </summary>
		public int? ShutterPresentationValue
		{
			get
			{
				DicomAttribute attribute;
				if (base.DicomAttributeProvider.TryGetAttribute(DicomTags.ShutterPresentationValue, out attribute))
					return attribute.GetInt32(0, 0);
				else
					return null;
			}
			set
			{
				if (!value.HasValue)
					base.DicomAttributeProvider[DicomTags.ShutterPresentationValue] = null;
				else
					base.DicomAttributeProvider[DicomTags.ShutterPresentationValue].SetInt32(0, value.Value);
			}
		}

		/// <summary>
		/// Gets or sets the shutter presentation color value.  Type 3.
		/// </summary>
		public CIELabColor? ShutterPresentationColorCielabValue
		{
			get
			{
				DicomAttribute attribute = base.DicomAttributeProvider[DicomTags.ShutterPresentationColorCielabValue];
				if (attribute.IsEmpty || attribute.IsNull)
					return null;

				ushort[] values = attribute.Values as ushort[];
				if (values != null && values.Length >= 3)
					return new CIELabColor(values[0], values[1], values[2]);
				else
					return null;
			}
			set
			{
				if (!value.HasValue)
					base.DicomAttributeProvider[DicomTags.ShutterPresentationColorCielabValue] = null;
				else
					base.DicomAttributeProvider[DicomTags.ShutterPresentationColorCielabValue].Values = value.Value.ToArray();
			}
		}

		/// <summary>
		/// Gets an enumeration of <see cref="DicomTag"/>s used by this module.
		/// </summary>
		public static IEnumerable<uint> DefinedTags
		{
			get
			{
				yield return DicomTags.ShutterShape;
				yield return DicomTags.ShutterOverlayGroup;
				yield return DicomTags.ShutterPresentationValue;
				yield return DicomTags.ShutterPresentationColorCielabValue;
				yield break;
			}
		}
	}
}