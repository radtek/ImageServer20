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

using ClearCanvas.Common;
using ClearCanvas.Dicom.Iod;
using ClearCanvas.ImageViewer.StudyManagement;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.DataStore;

namespace ClearCanvas.ImageViewer.StudyFinders.LocalDataStore
{
    [ClearCanvas.Common.ExtensionOf(typeof(ClearCanvas.ImageViewer.StudyManagement.StudyFinderExtensionPoint))]
    public class LocalDataStoreStudyFinder : IStudyFinder
    {
        public LocalDataStoreStudyFinder()
        {

        }

        public string Name
        {
            get
            {
                return "DICOM_LOCAL";
            }
        }

        public StudyItemList Query(QueryParameters queryParams, object targetServer)
        {
			Platform.CheckForNullReference(queryParams, "queryParams");

			DicomAttributeCollection collection = new DicomAttributeCollection();
        	collection[DicomTags.QueryRetrieveLevel].SetStringValue("STUDY");
            collection[DicomTags.PatientId].SetStringValue(queryParams["PatientId"]);
            collection[DicomTags.AccessionNumber].SetStringValue(queryParams["AccessionNumber"]);
            collection[DicomTags.PatientsName].SetStringValue(queryParams["PatientsName"]);
            collection[DicomTags.StudyDate].SetStringValue(queryParams["StudyDate"]);
            collection[DicomTags.StudyDescription].SetStringValue(queryParams["StudyDescription"]);
        	collection[DicomTags.PatientsBirthDate].SetStringValue("");
            collection[DicomTags.ModalitiesInStudy].SetStringValue(queryParams["ModalitiesInStudy"]);
            collection[DicomTags.SpecificCharacterSet].SetStringValue("");
			collection[DicomTags.StudyInstanceUid].SetStringValue(queryParams["StudyInstanceUid"]);

            StudyItemList studyItemList = new StudyItemList();
			using (IDataStoreReader reader = DataAccessLayer.GetIDataStoreReader())
			{
				foreach (DicomAttributeCollection result in reader.PerformStudyRootQuery(collection))
				{
					StudyItem item = new StudyItem();
					item.SpecificCharacterSet = result.SpecificCharacterSet;
					item.PatientId = result[DicomTags.PatientId].ToString();
					item.PatientsName = new PersonName(result[DicomTags.PatientsName].ToString());
					item.PatientsBirthDate = result[DicomTags.PatientsBirthDate].ToString();
					item.StudyDate = result[DicomTags.StudyDate].ToString();
					item.StudyDescription = result[DicomTags.StudyDescription].ToString();
					item.ModalitiesInStudy = result[DicomTags.ModalitiesInStudy].ToString();
					item.AccessionNumber = result[DicomTags.AccessionNumber].ToString();
					item.StudyInstanceUID = result[DicomTags.StudyInstanceUid].ToString();
					item.StudyLoaderName = this.Name;

					studyItemList.Add(item);
				}
			}

            return studyItemList;
        }
    }
}
