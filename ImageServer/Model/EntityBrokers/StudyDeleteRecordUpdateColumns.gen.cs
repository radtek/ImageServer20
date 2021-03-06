#region License

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

// This file is auto-generated by the ClearCanvas.Model.SqlServer2005.CodeGenerator project.

namespace ClearCanvas.ImageServer.Model.EntityBrokers
{
    using System;
    using System.Xml;
    using ClearCanvas.Dicom;
    using ClearCanvas.ImageServer.Enterprise;

   public class StudyDeleteRecordUpdateColumns : EntityUpdateColumns
   {
       public StudyDeleteRecordUpdateColumns()
       : base("StudyDeleteRecord")
       {}
       [DicomField(DicomTags.StudyInstanceUid, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="StudyInstanceUid")]
        public String StudyInstanceUid
        {
            set { SubParameters["StudyInstanceUid"] = new EntityUpdateColumn<String>("StudyInstanceUid", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="Timestamp")]
        public DateTime Timestamp
        {
            set { SubParameters["Timestamp"] = new EntityUpdateColumn<DateTime>("Timestamp", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="ServerPartitionAE")]
        public String ServerPartitionAE
        {
            set { SubParameters["ServerPartitionAE"] = new EntityUpdateColumn<String>("ServerPartitionAE", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="FilesystemGUID")]
        public ServerEntityKey FilesystemKey
        {
            set { SubParameters["FilesystemKey"] = new EntityUpdateColumn<ServerEntityKey>("FilesystemKey", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="BackupPath")]
        public String BackupPath
        {
            set { SubParameters["BackupPath"] = new EntityUpdateColumn<String>("BackupPath", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="Reason")]
        public String Reason
        {
            set { SubParameters["Reason"] = new EntityUpdateColumn<String>("Reason", value); }
        }
       [DicomField(DicomTags.AccessionNumber, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="AccessionNumber")]
        public String AccessionNumber
        {
            set { SubParameters["AccessionNumber"] = new EntityUpdateColumn<String>("AccessionNumber", value); }
        }
       [DicomField(DicomTags.PatientId, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="PatientId")]
        public String PatientId
        {
            set { SubParameters["PatientId"] = new EntityUpdateColumn<String>("PatientId", value); }
        }
       [DicomField(DicomTags.PatientsName, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="PatientsName")]
        public String PatientsName
        {
            set { SubParameters["PatientsName"] = new EntityUpdateColumn<String>("PatientsName", value); }
        }
       [DicomField(DicomTags.StudyId, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="StudyId")]
        public String StudyId
        {
            set { SubParameters["StudyId"] = new EntityUpdateColumn<String>("StudyId", value); }
        }
       [DicomField(DicomTags.StudyDescription, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="StudyDescription")]
        public String StudyDescription
        {
            set { SubParameters["StudyDescription"] = new EntityUpdateColumn<String>("StudyDescription", value); }
        }
       [DicomField(DicomTags.StudyDate, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="StudyDate")]
        public String StudyDate
        {
            set { SubParameters["StudyDate"] = new EntityUpdateColumn<String>("StudyDate", value); }
        }
       [DicomField(DicomTags.StudyTime, DefaultValue = DicomFieldDefault.Null)]
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="StudyTime")]
        public String StudyTime
        {
            set { SubParameters["StudyTime"] = new EntityUpdateColumn<String>("StudyTime", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="ArchiveInfo")]
        public XmlDocument ArchiveInfo
        {
            set { SubParameters["ArchiveInfo"] = new EntityUpdateColumn<XmlDocument>("ArchiveInfo", value); }
        }
        [EntityFieldDatabaseMappingAttribute(TableName="StudyDeleteRecord", ColumnName="ExtendedInfo")]
        public String ExtendedInfo
        {
            set { SubParameters["ExtendedInfo"] = new EntityUpdateColumn<String>("ExtendedInfo", value); }
        }
    }
}
