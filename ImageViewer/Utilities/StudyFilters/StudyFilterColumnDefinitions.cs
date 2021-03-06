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
using System.Collections.Generic;
using System.Reflection;
using ClearCanvas.Dicom;
using ClearCanvas.ImageViewer.Utilities.StudyFilters.Columns;

namespace ClearCanvas.ImageViewer.Utilities.StudyFilters
{
	partial class StudyFilterColumn
	{
		public abstract class ColumnDefinition
		{
			public readonly string Name;
			public readonly string Key;

			internal ColumnDefinition(string key, string name)
			{
				this.Key = key;
				this.Name = name;
			}

			public abstract StudyFilterColumn Create();

			public override bool Equals(object obj)
			{
				if (obj is ColumnDefinition)
					return this.Key == ((ColumnDefinition) obj).Key;
				return false;
			}

			public override int GetHashCode()
			{
				return 0x35522EF9 ^ this.Key.GetHashCode();
			}

			public override string ToString()
			{
				return this.Name;
			}
		}

		public static IEnumerable<ColumnDefinition> ColumnDefinitions
		{
			get
			{
				foreach (ColumnDefinition definition in SpecialColumnDefinitions)
					yield return definition;
				foreach (ColumnDefinition definition in DicomTagColumnDefinitions)
					yield return definition;
			}
		}

		public static ColumnDefinition GetColumnDefinition(string key)
		{
			// force initialize definition table
			StudyFilterColumn.SpecialColumnDefinitions.GetHashCode();

			if (_specialColumnDefinitions.ContainsKey(key))
				return _specialColumnDefinitions[key];

			// force initialize definition table
			StudyFilterColumn.DicomTagColumnDefinitions.GetHashCode();

			if (_dicomColumnDefinitions.ContainsKey(key))
				return _dicomColumnDefinitions[key];

			uint dicomTag;
			if (uint.TryParse(key, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out dicomTag))
				return StudyFilterColumn.GetColumnDefinition(dicomTag);

			return null;
		}

		public static StudyFilterColumn CreateColumn(string key)
		{
			ColumnDefinition definition = StudyFilterColumn.GetColumnDefinition(key);
			if (definition != null)
				return definition.Create();
			return null;
		}

		#region Special Columns

		private static Dictionary<string, ColumnDefinition> _specialColumnDefinitions;

		public static IEnumerable<ColumnDefinition> SpecialColumnDefinitions
		{
			get
			{
				if (_specialColumnDefinitions == null)
				{
					_specialColumnDefinitions = new Dictionary<string, ColumnDefinition>();

					SpecialColumnExtensionPoint xp = new SpecialColumnExtensionPoint();
					foreach (ISpecialColumn prototype in xp.CreateExtensions())
					{
						// not worried about the default constructor not existing, since it had to have one for CreateExtensions to work
						_specialColumnDefinitions.Add(prototype.Key, new SpecialColumnDefinition(prototype.Key, prototype.Name, prototype.GetType().GetConstructor(Type.EmptyTypes)));
					}
				}
				return _specialColumnDefinitions.Values;
			}
		}

		private class SpecialColumnDefinition : ColumnDefinition
		{
			private readonly ConstructorInfo _constructor;

			public SpecialColumnDefinition(string key, string name, ConstructorInfo constructor) : base(key, name)
			{
				_constructor = constructor;
			}

			public override StudyFilterColumn Create()
			{
				return (StudyFilterColumn) _constructor.Invoke(null);
			}
		}

		#endregion

		#region DICOM Tag Columns

		private static Dictionary<string, ColumnDefinition> _dicomColumnDefinitions;

		public static IEnumerable<ColumnDefinition> DicomTagColumnDefinitions
		{
			get
			{
				if (_dicomColumnDefinitions == null)
				{
					_dicomColumnDefinitions = new Dictionary<string, ColumnDefinition>();

					foreach (DicomTag dicomTag in DicomTagDictionary.GetDicomTagList())
					{
						if ((dicomTag.Group & 0xFFE1) != 0x6000)
						{
							ColumnDefinition definition = CreateDefinition(dicomTag);
							_dicomColumnDefinitions.Add(definition.Key, definition);
						}
						else
						{
							// the tag we're trying to add is in a repeating group (the overlays, group 6000)
							// so we replicate the tag for each of the other repeated groups (the even numbers from 6002-601E)
							for (uint n = 0; n <= 0x1E; n += 2)
							{
								DicomTag rDicomTag = new DicomTag(
									dicomTag.TagValue + n * 0x10000,
									string.Format(SR.FormatRepeatingDicomTagName, dicomTag.Name, 1 + n / 2), 
									string.Format("{0}{1:X2}", dicomTag.VariableName, n),
									dicomTag.VR, dicomTag.MultiVR, dicomTag.VMLow, dicomTag.VMHigh, dicomTag.Retired);
								ColumnDefinition rDefinition = CreateDefinition(rDicomTag);
								_dicomColumnDefinitions.Add(rDefinition.Key, rDefinition);
							}
						}
					}
				}
				return _dicomColumnDefinitions.Values;
			}
		}

		public static ColumnDefinition GetColumnDefinition(uint dicomTag)
		{
			DicomTag tag = DicomTagDictionary.GetDicomTag(dicomTag);
			if (tag == null)
				tag = new DicomTag(dicomTag, string.Empty, string.Empty, DicomVr.UNvr, false, uint.MinValue, uint.MaxValue, false);
			return GetColumnDefinition(tag);
		}

		public static ColumnDefinition GetColumnDefinition(DicomTag dicomTag)
		{
			// initialize defintion table
			DicomTagColumnDefinitions.GetHashCode();

			string key = dicomTag.TagValue.ToString("x8");

			if (_dicomColumnDefinitions.ContainsKey(key))
				return _dicomColumnDefinitions[key];

			return CreateDefinition(dicomTag);
		}

		private static ColumnDefinition CreateDefinition(DicomTag dicomTag)
		{
			switch (dicomTag.VR.Name)
			{
				case "AE":
				case "CS":
				case "LO":
				case "PN":
				case "SH":
				case "UI":
					// multi-valued strings
					return new StringDicomColumnDefintion(dicomTag);
				case "LT":
				case "ST":
				case "UT":
					// single-valued strings
					return new TextDicomColumnDefintion(dicomTag);
				case "IS":
				case "SL":
				case "SS":
					// multi-valued integers
					return new IntegerDicomColumnDefintion(dicomTag);
				case "UL":
				case "US":
					// multi-valued unsigned integers
					return new UnsignedDicomColumnDefintion(dicomTag);
				case "DS":
				case "FL":
				case "FD":
					// multi-valued floating-point numbers
					return new FloatingPointDicomColumnDefintion(dicomTag);
				case "DA":
				case "DT":
				case "TM":
					// multi-valued dates/times
					return new DateTimeDicomColumnDefintion(dicomTag);
				case "AS":
					// multi-valued time spans
					return new AgeDicomColumnDefintion(dicomTag);
				case "AT":
					// multi-valued DICOM tags
					return new AttributeTagDicomColumnDefintion(dicomTag);
				case "SQ":
				case "OB":
				case "OF":
				case "OW":
				case "UN":
				default:
					// sequence, binary and unknown data
					return new BinaryDicomColumnDefintion(dicomTag);
			}
		}

		#region Definitions

		private abstract class DicomColumnDefinition : ColumnDefinition
		{
			protected readonly DicomTag Tag;

			protected DicomColumnDefinition(DicomTag tag)
				: base(tag.TagValue.ToString("x8"), string.Format(SR.FormatDicomTag, tag.Group, tag.Element, tag.Name))
			{
				Tag = tag;
			}
		}

		private class StringDicomColumnDefintion : DicomColumnDefinition
		{
			public StringDicomColumnDefintion(DicomTag tag) : base(tag) {}

			public override StudyFilterColumn Create()
			{
				return new StringDicomTagColumn(Tag);
			}
		}

		private class IntegerDicomColumnDefintion : DicomColumnDefinition
		{
			public IntegerDicomColumnDefintion(DicomTag tag) : base(tag) {}

			public override StudyFilterColumn Create()
			{
				return new IntegerDicomTagColumn(Tag);
			}
		}

		private class UnsignedDicomColumnDefintion : DicomColumnDefinition
		{
			public UnsignedDicomColumnDefintion(DicomTag tag) : base(tag) {}

			public override StudyFilterColumn Create()
			{
				return new UnsignedDicomTagColumn(Tag);
			}
		}

		private class FloatingPointDicomColumnDefintion : DicomColumnDefinition
		{
			public FloatingPointDicomColumnDefintion(DicomTag tag) : base(tag) {}

			public override StudyFilterColumn Create()
			{
				return new FloatingPointDicomTagColumn(Tag);
			}
		}

		private class AgeDicomColumnDefintion : DicomColumnDefinition
		{
			public AgeDicomColumnDefintion(DicomTag tag) : base(tag) {}

			public override StudyFilterColumn Create()
			{
				return new AgeDicomTagColumn(Tag);
			}
		}

		private class DateTimeDicomColumnDefintion : DicomColumnDefinition
		{
			public DateTimeDicomColumnDefintion(DicomTag tag) : base(tag) {}

			public override StudyFilterColumn Create()
			{
				return new DateTimeDicomTagColumn(Tag);
			}
		}

		private class TextDicomColumnDefintion : DicomColumnDefinition
		{
			public TextDicomColumnDefintion(DicomTag tag) : base(tag) {}

			public override StudyFilterColumn Create()
			{
				return new TextDicomTagColumn(Tag);
			}
		}

		private class AttributeTagDicomColumnDefintion : DicomColumnDefinition
		{
			public AttributeTagDicomColumnDefintion(DicomTag tag) : base(tag) {}

			public override StudyFilterColumn Create()
			{
				return new AttributeTagDicomTagColumn(Tag);
			}
		}

		private class BinaryDicomColumnDefintion : DicomColumnDefinition
		{
			public BinaryDicomColumnDefintion(DicomTag tag) : base(tag) {}

			public override StudyFilterColumn Create()
			{
				return new BinaryDicomTagColumn(Tag);
			}
		}

		#endregion

		#endregion
	}
}