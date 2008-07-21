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

namespace ClearCanvas.Dicom.DataStore
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ClearCanvas.Dicom;

    /// <summary>
    /// A single result from a C-FIND query, a collection of <see cref="DicomTagPath">DicomTagPath</see>s.
    /// Since QueryResult implements IDictionary, it's possible to use an enumerator to extract
    /// every single tag in the result, as well as provide a key to extract a particular tag.
    /// </summary>
    public class QueryResult : IDictionary<DicomTagPath, string>
    {
		private Dictionary<DicomTagPath, string> _internalDictionary;

		public QueryResult()
		{
			_internalDictionary = new Dictionary<DicomTagPath, string>();
		}

		/// <summary>
		/// Gets the Study Instance UID.
        /// </summary>
		public string StudyInstanceUid
        {
            get
            {
				if (this.ContainsTag(DicomTags.StudyInstanceUid))
					return this[DicomTags.StudyInstanceUid];
				else
					return null;
            }
        }

        /// <summary>
		/// Gets the Patient ID as a <see cref="PatientId"/> object.
        /// </summary>
        public string PatientId
        {
            get
            {
				if (this.ContainsTag(DicomTags.PatientId))
					return this[DicomTags.PatientId];
				else
					return "";
            }
        }

        /// <summary>
		/// Gets Patient's Name as a <see cref="PersonName"/> object.
        /// </summary>
        public PersonName PatientsName
        {
            get
            {
                if (this.ContainsTag(DicomTags.PatientsName))
					return new PersonName(this[DicomTags.PatientsName]);
				else
                    return new PersonName("");
            }
        }

		/// <summary>
		/// Gets Patient's Birth Date as a string.
		/// </summary>
		public string PatientsBirthDate
		{
			get
			{
				if (this.ContainsTag(DicomTags.PatientsBirthDate))
					return this[DicomTags.PatientsBirthDate];
				else
					return "";
			}
		}

		/// <summary>
		/// Gets Patient's Sex as a string.
		/// </summary>
		public string PatientsSex
		{
			get
			{
				if (this.ContainsTag(DicomTags.PatientsSex))
					return this[DicomTags.PatientsSex];
				else
					return "";
			}
		}

		/// <summary>
        /// Gets Modalities In Study as a string.
        /// </summary>
        public string ModalitiesInStudy
        {
            get
            {
                if (this.ContainsTag(DicomTags.ModalitiesInStudy))
					return this[DicomTags.ModalitiesInStudy];
				else
                    return "";
            }
        }

        /// <summary>
        /// Gets Study Description as a string.
        /// </summary>
        public string StudyDescription
        {
            get
            {
                if (this.ContainsTag(DicomTags.StudyDescription))
					return this[DicomTags.StudyDescription];
				else
                    return "";
            }
        }

		/// <summary>
		/// Gets Study ID as a string.
		/// </summary>
		public string StudyID
		{
			get
			{
				if (this.ContainsTag(DicomTags.StudyId))
					return this[DicomTags.StudyId];
				else
					return "";
			}
		}
		
		/// <summary>
        /// Gets Study Date as a string.
        /// </summary>
        public string StudyDate
        {
            get
            {
                if (this.ContainsTag(DicomTags.StudyDate))
					return this[DicomTags.StudyDate];
				else
                    return "";
            }
        }

        /// <summary>
        /// Gets Study Time as a string.
        /// </summary>
        public string StudyTime
        {
            get
            {
                if (this.ContainsTag(DicomTags.StudyTime))
					return this[DicomTags.StudyTime];
				else
                    return "";
            }
        }

        /// <summary>
        /// Gets Accession Number as a string.
        /// </summary>
        public string AccessionNumber
        {
            get
            {
                if (this.ContainsTag(DicomTags.AccessionNumber))
					return this[DicomTags.AccessionNumber];
				else
                    return "";
            }
        }

        /// <summary>
        /// Gets Modality as a string.
        /// </summary>
        public string Modality
        {
            get
            {
                if (this.ContainsTag(DicomTags.Modality))
					return this[DicomTags.Modality];
				else
                    return "";
            }
        }

        /// <summary>
		/// Gets Series Instance UID.
        /// </summary>
		public string SeriesInstanceUid
        {
            get
            {
                if (this.ContainsTag(DicomTags.SeriesInstanceUid))
					return this[DicomTags.SeriesInstanceUid];
				else
                    return null;
            }
        }

        /// <summary>
        /// Gets Series Description as a string.
        /// </summary>
        public string SeriesDescription
        {
            get
            {
                if (this.ContainsTag(DicomTags.SeriesDescription))
					return this[DicomTags.SeriesDescription];
				else
                    return "";
            }
        }

        /// <summary>
        /// Gets Series Number as a string.
        /// </summary>
        public string SeriesNumber
        {
            get
            {
                if (this.ContainsTag(DicomTags.SeriesNumber))
					return this[DicomTags.SeriesNumber];
				else
                    return "";
            }
        }

        /// <summary>
        /// Gets Number Of Study Related Instances as a number.
        /// </summary>
        public uint NumberOfStudyRelatedInstances
        {
            get
            {
                if (this.ContainsTag(DicomTags.NumberOfStudyRelatedInstances))
				{
					String temp = this[DicomTags.NumberOfStudyRelatedInstances];
                    uint output;
                    if (uint.TryParse(temp, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out output))
                        return output;
				}

				return 0;
            }
        }

        /// <summary>
        /// Gets Number of Series Related Instances as a number.
        /// </summary>
        public uint NumberOfSeriesRelatedInstances
        {
            get
            {
                if (this.ContainsTag(DicomTags.NumberOfSeriesRelatedInstances))
				{
					String temp = this[DicomTags.NumberOfSeriesRelatedInstances];
                    uint output;
					if (uint.TryParse(temp, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out output))
                        return output;
				}

				return 0;
			}
        }

		/// <summary>
		/// Gets Specific Character Set as a string.
		/// </summary>
		public string SpecificCharacterSet
        {
            get
            {
                if (this.ContainsTag(DicomTags.SpecificCharacterSet))
					return this[DicomTags.SpecificCharacterSet];
				else
                    return "";
            }
        }

        /// <summary>
        /// Gets the whole collection of DICOM tag paths in this query result.
        /// </summary>
        public ICollection<DicomTagPath> DicomTagPathCollection
        {
            get { return (this as IDictionary<DicomTagPath, string>).Keys; }
        }

        /// <summary>
		/// Indexer based on the <see cref="DicomTagPath"/> key to this collection.
        /// </summary>
		/// <param name="path">The tag path corresponding to the value the client wants to obtain.</param>
        /// <returns>The string representation of that value.</returns>
        public string this[DicomTagPath path]
        {
            get 
            {
                if (this.Contains(path))
                    return (this as IDictionary<DicomTagPath, string>)[path];
                else
                    return "";
            }
            set { (this as IDictionary<DicomTagPath, string>)[path] = value; }
        }

		/// <summary>
		/// Indexer based on a <see cref="DicomTag"/> key to this collection.
		/// </summary>
		/// <param name="tag">The tag corresponding to the value the client wants to obtain.</param>
		/// <returns>The string representation of that value.</returns>
		public string this[DicomTag tag]
		{
			get
			{
				DicomTagPath key = new DicomTagPath(tag);
				if (this.Contains(key))
					return (this as IDictionary<DicomTagPath, string>)[key];
				else
					return "";
			}
			set 
			{
				DicomTagPath key = new DicomTagPath(tag);
				(this as IDictionary<DicomTagPath, string>)[key] = value; 
			}
		}

		/// <summary>
		/// Indexer based on a <see cref="uint"/> (representation of a Dicom tag) key to this collection.
		/// </summary>
		/// <param name="tag">The tag corresponding to the value the client wants to obtain.</param>
		/// <returns>The string representation of that value.</returns>
		public string this[uint tag]
		{
			get
			{
				DicomTagPath key = new DicomTagPath(tag);
				if (this.Contains(key))
					return (this as IDictionary<DicomTagPath, string>)[key];
				else
					return "";
			}
			set
			{
				DicomTagPath key = new DicomTagPath(tag);
				(this as IDictionary<DicomTagPath, string>)[key] = value;
			}
		}

		/// <summary>
        /// Add a new tag/value pair into the collection.
        /// </summary>
        /// <param name="key">The tag path.</param>
        /// <param name="value">The value of the tag.</param>
        public void Add(DicomTagPath key, string value)
        {
            (this as IDictionary<DicomTagPath, string>).Add(key, value);
        }

		/// <summary>
		/// Add a new tag/value pair into the collection.
		/// </summary>
		/// <param name="tag">A Dicom tag.</param>
		/// <param name="value">The value of the tag.</param>
		public void Add(DicomTag tag, string value)
		{
			(this as IDictionary<DicomTagPath, string>).Add(new DicomTagPath(tag), value);
		}

		/// <summary>
		/// Add a new tag/value pair into the collection.
		/// </summary>
		/// <param name="tag">A Dicom tag.</param>
		/// <param name="value">The value of the tag.</param>
		public void Add(uint tag, string value)
		{
			(this as IDictionary<DicomTagPath, string>).Add(new DicomTagPath(tag), value);
		}

		/// <summary>
        /// Empties out the collection.
        /// </summary>
        public void Clear()
        {
			(this as ICollection<KeyValuePair<DicomTagPath, string>>).Clear();
        }

		/// <summary>
		/// Determines whether the collection contains a particular tag.
		/// </summary>
		/// <param name="path">The path to the tag to check.</param>
		/// <returns>True - tag exists; False - tag does not exist.</returns>
		public Boolean Contains(DicomTagPath path)
		{
			return (this as IDictionary<DicomTagPath, string>).ContainsKey(path);
		}
		
		/// <summary>
        /// Determines whether the collection contains a particular tag.
        /// </summary>
        /// <param name="tag">The tag to check.</param>
        /// <returns>True - tag exists; False - tag does not exist.</returns>
		public Boolean ContainsTag(DicomTag tag)
        {
			return (this as IDictionary<DicomTagPath, string>).ContainsKey(new DicomTagPath(tag));
        }

		/// <summary>
		/// Determines whether the collection contains a particular tag.
		/// </summary>
		/// <param name="tag">The tag to check.</param>
		/// <returns>True - tag exists; False - tag does not exist.</returns>
		public Boolean ContainsTag(uint tag)
		{
			return (this as IDictionary<DicomTagPath, string>).ContainsKey(new DicomTagPath(tag));
		}

		/// <summary>
        /// Returns string representation of this query result.
        /// </summary>
        /// <returns>Study Description if that is available, otherwise the Study Instance UID.</returns>
        public override string ToString()
        {
            if (null != StudyDescription)
                return StudyDescription;
            else 
                return StudyInstanceUid.ToString();
        }

		#region IDictionary<DicomTagPath,string> Members

		void IDictionary<DicomTagPath, string>.Add(DicomTagPath key, string value)
        {
            _internalDictionary.Add(key, value);
        }

        bool IDictionary<DicomTagPath, string>.ContainsKey(DicomTagPath key)
        {
            return _internalDictionary.ContainsKey(key);
        }

        ICollection<DicomTagPath> IDictionary<DicomTagPath, string>.Keys
        {
            get { return _internalDictionary.Keys; }
        }

        bool IDictionary<DicomTagPath, string>.Remove(DicomTagPath key)
        {
            return _internalDictionary.Remove(key);
        }

        bool IDictionary<DicomTagPath, string>.TryGetValue(DicomTagPath key, out string value)
        {
            return _internalDictionary.TryGetValue(key, out value);
        }

        ICollection<string> IDictionary<DicomTagPath, string>.Values
        {
            get { return _internalDictionary.Values; }
        }

        string IDictionary<DicomTagPath, string>.this[DicomTagPath key]
        {
            get
            {
                return _internalDictionary[key];
            }
            set
            {
                _internalDictionary[key] = value;
            }
        }

        #endregion

        #region ICollection<KeyValuePair<DicomTagPath,string>> Members

        void ICollection<KeyValuePair<DicomTagPath, string>>.Add(KeyValuePair<DicomTagPath, string> item)
        {
            (_internalDictionary as ICollection<KeyValuePair<DicomTagPath, string>>).Add(item);
        }

        void ICollection<KeyValuePair<DicomTagPath, string>>.Clear()
        {
            (_internalDictionary as ICollection<KeyValuePair<DicomTagPath, string>>).Clear();
        }

        bool ICollection<KeyValuePair<DicomTagPath, string>>.Contains(KeyValuePair<DicomTagPath, string> item)
        {
            return (_internalDictionary as ICollection<KeyValuePair<DicomTagPath, string>>).Contains(item);
        }

        void ICollection<KeyValuePair<DicomTagPath, string>>.CopyTo(KeyValuePair<DicomTagPath, string>[] array, int arrayIndex)
        {
            (_internalDictionary as ICollection<KeyValuePair<DicomTagPath, string>>).CopyTo(array, arrayIndex);
        }

        int ICollection<KeyValuePair<DicomTagPath, string>>.Count
        {
            get { return (_internalDictionary as ICollection<KeyValuePair<DicomTagPath, string>>).Count; }
        }

        bool ICollection<KeyValuePair<DicomTagPath, string>>.IsReadOnly
        {
            get { return (_internalDictionary as ICollection<KeyValuePair<DicomTagPath, string>>).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<DicomTagPath, string>>.Remove(KeyValuePair<DicomTagPath, string> item)
        {
			return (_internalDictionary as ICollection<KeyValuePair<DicomTagPath, string>>).Remove(item);
        }

        #endregion

		#region IEnumerable<KeyValuePair<DicomTagPath,string>> Members

		IEnumerator<KeyValuePair<DicomTagPath, string>> IEnumerable<KeyValuePair<DicomTagPath, string>>.GetEnumerator()
        {
			return (_internalDictionary as ICollection<KeyValuePair<DicomTagPath, string>>).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _internalDictionary.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// A helper class. This class is identical in
    /// every respect to QueryResult, but the name. A new
    /// name has been given in order to have the correct
    /// semantic when a collection of tags is used to define
    /// a query or retrieve, rather than when used as a
    /// collection of query results.
    /// </summary>
    public class QueryKey : QueryResult
    {

    }
}