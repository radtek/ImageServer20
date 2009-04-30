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

namespace ClearCanvas.Dicom.Utilities.StudyBuilder
{
	/// <summary>
	/// A <see cref="StudyBuilderNode"/> representing a series-level data node in the <see cref="StudyBuilder"/> tree hierarchy.
	/// </summary>
	public sealed class SeriesNode : StudyBuilderNode
	{
		private readonly SopInstanceNodeCollection _images;
		private string _instanceUid;
		private string _description;
		private int _seriesNum;
		private DateTime? _dateTime;

		/// <summary>
		/// Constructs a new <see cref="SeriesNode"/> using default values.
		/// </summary>
		public SeriesNode()
		{
			_images = new SopInstanceNodeCollection(this);
			_instanceUid = StudyBuilder.NewUid();
			_description = "Untitled Series";
			_dateTime = System.DateTime.Now;
		}

		/// <summary>
		/// Constructs a new <see cref="SeriesNode"/> using actual values from attributes from the given <see cref="DicomAttributeCollection"/>.
		/// </summary>
		/// <param name="dicomDataSet">The data set from which to initialize this node.</param>
		public SeriesNode(DicomAttributeCollection dicomDataSet)
		{
			_images = new SopInstanceNodeCollection(this);
			_description = dicomDataSet[DicomTags.SeriesDescription].GetString(0, "");
			_dateTime =
				DicomConverter.GetDateTime(dicomDataSet[DicomTags.SeriesDate].GetDateTime(0),
				                           dicomDataSet[DicomTags.SeriesTime].GetDateTime(0));
			_instanceUid = dicomDataSet[DicomTags.SeriesInstanceUid].GetString(0, "");
			if (_instanceUid == "")
				_instanceUid = StudyBuilder.NewUid();
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="source"></param>
		/// <param name="copyDescendants"></param>
		private SeriesNode(SeriesNode source, bool copyDescendants) {
			_images = new SopInstanceNodeCollection(this);
			_instanceUid = StudyBuilder.NewUid();
			_description = source._description;
			_dateTime = source._dateTime;

			if(copyDescendants)
			{
				foreach (SopInstanceNode sop in source._images)
				{
					_images.Add(sop.Copy());
				}
			}
		}

		#region Misc

		/// <summary>
		/// Gets the parent of this node, or null if the node is not in a study builder tree.
		/// </summary>
		public new StudyNode Parent {
			get { return base.Parent as StudyNode; }
			internal set { base.Parent = value; }
		}

		#endregion

		#region Data Properties

		/// <summary>
		/// Gets or sets the series instance UID.
		/// </summary>
		public string InstanceUid
		{
			get { return _instanceUid; }
			internal set
			{
				if (_instanceUid != value)
				{
					if (string.IsNullOrEmpty(value))
						value = StudyBuilder.NewUid();

					_instanceUid = value;
					FirePropertyChanged("InstanceUid");
				}
			}
		}

		/// <summary>
		/// Gets or sets the series description.
		/// </summary>
		public string Description
		{
			get { return _description; }
			set
			{
				if (_description != value)
				{
					_description = value;
					FirePropertyChanged("Description");
				}
			}
		}

		/// <summary>
		/// Gets or sets the series date/time stamp.
		/// </summary>
		public DateTime? DateTime
		{
			get { return _dateTime; }
			set
			{
				if (_dateTime != value)
				{
					_dateTime = value;
					FirePropertyChanged("DateTime");
				}
			}
		}

		public int SeriesNumber
		{
			get { return _seriesNum; }
			set
			{
				if(_seriesNum != value)
				{
					_seriesNum = value;
					FirePropertyChanged("SeriesNumber");
				}
			}
		}

		#endregion

		#region Update Methods

		/// <summary>
		/// Writes the data in this node into the given <see cref="DicomAttributeCollection"/>.
		/// </summary>
		/// <param name="dicomDataSet">The data set to write data into.</param>
		/// <param name="writeUid"></param>
		internal void Update(DicomAttributeCollection dicomDataSet, bool writeUid)
		{
			dicomDataSet[DicomTags.SeriesDescription].SetStringValue(_description);
			DicomConverter.SetDate(dicomDataSet[DicomTags.SeriesDate], _dateTime);
			DicomConverter.SetTime(dicomDataSet[DicomTags.SeriesTime], _dateTime);
			DicomConverter.SetInt32(dicomDataSet[DicomTags.SeriesNumber], _seriesNum);

			if (writeUid)
				dicomDataSet[DicomTags.SeriesInstanceUid].SetStringValue(_instanceUid);
		}

		#endregion

		#region Copy Methods

		/// <summary>
		/// Creates a new <see cref="SeriesNode"/> with the same node data, nulling all references to other nodes.
		/// </summary>
		/// <returns>A copy of the node.</returns>
		public SeriesNode Copy() {
			return this.Copy(false, false);
		}

		/// <summary>
		/// Creates a new <see cref="SeriesNode"/> with the same node data, nulling all references to nodes outside of the copy scope.
		/// </summary>
		/// <param name="copyDescendants">Specifies that all the descendants of the node should also be copied.</param>
		/// <returns>A copy of the node.</returns>
		public SeriesNode Copy(bool copyDescendants) {
			return this.Copy(copyDescendants, false);
		}

		/// <summary>
		/// Creates a new <see cref="SeriesNode"/> with the same node data.
		/// </summary>
		/// <param name="copyDescendants">Specifies that all the descendants of the node should also be copied.</param>
		/// <param name="keepExtLinks">Specifies that references to nodes outside of the copy scope should be kept. If False, all references are nulled.</param>
		/// <returns>A copy of the node.</returns>
		public SeriesNode Copy(bool copyDescendants, bool keepExtLinks) {
			return new SeriesNode(this, copyDescendants);
		}

		#endregion

		#region Insert Methods

		/// <summary>
		/// Convenience method to insert SOP instance-level data nodes into the study builder tree under this series.
		/// </summary>
		/// <param name="sopInstances">An array of <see cref="SopInstanceNode"/>s to insert into the study builder tree.</param>
		public void InsertSopInstances(SopInstanceNode[] sopInstances)
		{
			foreach (SopInstanceNode node in sopInstances)
			{
				this.Images.Add(node);
			}
		}

		#endregion

		#region Images Collection

		/// <summary>
		/// Gets a list of all the <see cref="SopInstanceNode"/>s that belong to this series.
		/// </summary>
		public SopInstanceNodeCollection Images
		{
			get { return _images; }
		}

		#endregion
	}
}