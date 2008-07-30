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
using ClearCanvas.DicomServices.Xml;

namespace ClearCanvas.Dicom.DataStore
{
    internal class SopInstance : ISopInstance
    {
		private readonly Series _parentSeries;
		private readonly InstanceXml _xml;

		internal SopInstance(Series parentSeries, InstanceXml instanceXml)
		{
			_parentSeries = parentSeries;
			_xml = instanceXml;
		}

		#region ISopInstance Members

		public ISeries GetParentSeries()
		{
			return _parentSeries;
		}

		[QueryableProperty(DicomTags.StudyInstanceUid, IsComputed = true, ReturnAlways = true, ReturnOnly = true)]
		public string StudyInstanceUid
		{
			get { return _parentSeries.GetParentStudy().StudyInstanceUid; }
		}

		[QueryableProperty(DicomTags.SeriesInstanceUid, IsComputed = true, ReturnAlways = true, ReturnOnly = true)]
		public string SeriesInstanceUid
		{
			get { return _parentSeries.SeriesInstanceUid; }
		}

		[QueryableProperty(DicomTags.SopInstanceUid, IsComputed = true, ReturnAlways = true)]
		public string SopInstanceUid
		{
			get { return _xml.SopInstanceUid; }
		}

		[QueryableProperty(DicomTags.InstanceNumber, IsComputed = true, ReturnOnly = true)]
		public int InstanceNumber
		{
			get { return _xml[DicomTags.InstanceNumber].GetInt32(0, 0); }
		}

		[QueryableProperty(DicomTags.SopClassUid, IsComputed = true)]
		public string SopClassUid
		{
			get
			{
				if (_xml.SopClass == null)
					return ""; //shouldn't happen.

				return _xml.SopClass.Uid;
			}
		}

		public string TransferSyntaxUid
		{
			get { return _xml.TransferSyntax.UidString; }
		}

		public DicomUri GetLocationUri()
		{
			UriBuilder uriBuilder = new UriBuilder();
			uriBuilder.Scheme = "file";
			uriBuilder.Path = _xml.SourceFileName;
			return new DicomUri(uriBuilder.Uri);
		}

    	public DicomAttribute this[DicomTag tag]
    	{
			get
			{
				if (tag == null)
					return null;

				bool isBinary = tag.VR == DicomVr.OBvr || tag.VR == DicomVr.OWvr || tag.VR == DicomVr.OFvr;
				//we don't store these in the xml.
				if (isBinary || tag.IsPrivate || tag.VR == DicomVr.UNvr)
					return null;

				return _xml[tag];
			}
    	}

		public DicomAttribute this[uint tag]
		{
			get
			{
				return this[DicomTagDictionary.GetDicomTag(tag)];
			}
		}

		#endregion
	}
}
