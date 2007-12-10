#region License

// Copyright (c) 2006-2007, ClearCanvas Inc.
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
using System.Text;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Common;

namespace ClearCanvas.Dicom
{
	public class DicomTagPath : IEquatable<DicomTagPath>, IEquatable<DicomTag>, IEquatable<string>, IEquatable<uint>
	{
		private static readonly char[] _pathSeparator = new char[] { '\\' };
		private static readonly char[] _tagSeparator = new char[] { ',' };

		private List<DicomTag> _tags;
		private string _path;

		protected DicomTagPath()
			: this(new DicomTag[] {})
		{
		}

		public DicomTagPath(string path)
			: this(DicomTagPath.GetTags(path))
		{
		}

		public DicomTagPath(uint tag)
			: this(new uint[] { tag })
		{
		}

		public DicomTagPath(IEnumerable<uint> tags)
			: this(DicomTagPath.GetTags(tags))
		{
		}

		public DicomTagPath(DicomTag tag)
			: this(new DicomTag[] { tag })
		{
		}

		public DicomTagPath(IEnumerable<DicomTag> tags)
		{
			BuildPath(tags);
		}

		public string Path
		{
			get { return _path; }
			protected set 
			{
				this.BuildPath(GetTags(value));
			}
		}

		public IList<DicomTag> TagsInPath
		{
			get { return _tags.AsReadOnly(); }
			protected set
			{
				this.BuildPath(value);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
				return true;

			if (obj is DicomTagPath)
				return this.Equals(obj as DicomTagPath);
			if (obj is DicomTag)
				return this.Equals(obj as DicomTag);
			if (obj is string)
				return this.Equals(obj as string);
			if (obj is uint)
				return this.Equals((uint)obj);

			return false;
		}

		#region IEquatable<DicomTagPath> Members

		public bool Equals(DicomTagPath other)
		{
			if (other == null)
				return false;

			return other.Path.Equals(this.Path);
		}

		#endregion	

		#region IEquatable<DicomTag> Members

		public bool Equals(DicomTag other)
		{
			if (other == null)
				return false;

			if (_tags.Count != 1)
				return false;

			return _tags[0].Equals(other);
		}

		#endregion

		#region IEquatable<string> Members

		public bool Equals(string other)
		{
			return this.Path.Equals(other);
		}

		#endregion

		#region IEquatable<uint> Members

		public bool Equals(uint other)
		{
			if (_tags.Count != 1)
				return false;

			return _tags[0].TagValue.Equals(other);
		}

		#endregion

		public override int GetHashCode()
		{
			return _path.GetHashCode();
		}

		public override string ToString()
		{
			return _path;
		}

		/// <summary>
		/// Implicit cast to a String object, for ease of use.
		/// </summary>
		public static implicit operator string(DicomTagPath path)
		{
			return path.ToString();
		}

		private void BuildPath(IEnumerable<DicomTag> dicomTags)
		{
			Platform.CheckForNullReference(dicomTags, "dicomTags");
			_tags = new List<DicomTag>(dicomTags);
			_path = StringUtilities.Combine<DicomTag>(dicomTags, "\\", delegate(DicomTag tag) { return String.Format("({0:x4},{1:x4})", tag.Group, tag.Element); });
		}

		private static IEnumerable<DicomTag> GetTags(string path)
		{
			Platform.CheckForEmptyString(path, "path");

			List<DicomTag> dicomTags = new List<DicomTag>();

			string[] groupElementValues = path.Split(_pathSeparator);

			foreach (string groupElement in groupElementValues)
			{
				string[] values = groupElement.Split(_tagSeparator);
				if (values.Length != 2)
					throw new ArgumentException(String.Format(SR.ExceptionDicomTagPathInvalid, path));

				string group = values[0];
				if (!group.StartsWith("(") || group.Length != 5)
					throw new ArgumentException(String.Format(SR.ExceptionDicomTagPathInvalid, path));

				string element = values[1];
				if (!element.EndsWith(")") || element.Length != 5)
					throw new ArgumentException(String.Format(SR.ExceptionDicomTagPathInvalid, path));

				try
				{
					ushort groupValue = Convert.ToUInt16(group.TrimStart('('), 16);
					ushort elementValue = Convert.ToUInt16(element.TrimEnd(')'), 16);

					dicomTags.Add(DicomTagPath.NewTag(DicomTag.GetTagValue(groupValue, elementValue)));
				}
				catch
				{
					throw new ArgumentException(String.Format(SR.ExceptionDicomTagPathInvalid, path));
				}
			}

			return dicomTags;
		}

		private static IEnumerable<DicomTag> GetTags(IEnumerable<uint> tags)
		{
			List<DicomTag> dicomTags = new List<DicomTag>();

			foreach (uint tag in tags)
				dicomTags.Add(NewTag(tag));

			return dicomTags;
		}

		private static DicomTag NewTag(uint tag)
		{
			DicomTag returnTag = DicomTagDictionary.GetDicomTag(tag);
			if (returnTag == null)
				returnTag = new DicomTag(tag, "Unknown Tag", "UnknownTag", DicomVr.UNvr, false, 1, uint.MaxValue, false);

			return returnTag;
		}
	}
}
