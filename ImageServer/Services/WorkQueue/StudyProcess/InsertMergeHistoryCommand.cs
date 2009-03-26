using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using ClearCanvas.Common;
using ClearCanvas.Dicom;
using ClearCanvas.ImageServer.Common.CommandProcessor;
using ClearCanvas.ImageServer.Common.Data;
using ClearCanvas.ImageServer.Common.Utilities;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.EntityBrokers;

namespace ClearCanvas.ImageServer.Services.WorkQueue.StudyProcess
{
    
    class InsertMergeToExistingStudyHistoryCommand:ServerDatabaseCommand
    {
        private ReconcileImageContext _context;

        #region Constructors
        public InsertMergeToExistingStudyHistoryCommand(ReconcileImageContext context)
            : base("InsertMergeToExistingStudyHistoryCommand", true)
        {
            Platform.CheckForNullReference(context, "context");
            _context = context;
        }
        #endregion

        string GetUnifiedPatientName(string name1, string name2)
        {
            name1 = DicomNameUtils.Normalize(name1, DicomNameUtils.NormalizeOptions.TrimSpaces | DicomNameUtils.NormalizeOptions.TrimEmptyEndingComponents);
            name2 = DicomNameUtils.Normalize(name2, DicomNameUtils.NormalizeOptions.TrimSpaces | DicomNameUtils.NormalizeOptions.TrimEmptyEndingComponents);

            if (name1.Length!=name2.Length)
            {
                throw new ApplicationException(String.Format("Unable to unify names: {0} and {1}", name1, name2));
            }

            StringBuilder value = new StringBuilder();
            for (int i = 0; i < name1.Length; i++ )
            {
                if (Char.ToUpper(name1[i]) == Char.ToUpper(name2[i]))
                {
                    value.Append(name1[i]);
                }
                else
                {
                    if (name1[i] != '^' && name2[i]!='^')
                    {
                        throw new ApplicationException(String.Format("Unable to unify names: {0} and {1}", name1, name2));
                    }
                    else // one of them is ^
                    {    
                        if (name1[i] != ' ' && name2[i]!=' ')
                        {
                            throw new ApplicationException(String.Format("Unable to unify names: {0} and {1}", name1, name2));
                        }
                        else
                        {
                            value.Append('^');
                        }
                    }
                }
            }
            return value.ToString();
        }

        protected override void OnExecute(ClearCanvas.Enterprise.Core.IUpdateContext updateContext)
        {
            ReconcileMergeToExistingStudyDescriptor desc = new ReconcileMergeToExistingStudyDescriptor();
            desc.ExistingStudy = StudyInformation.CreateFrom(_context.CurrentStudy);
            desc.ImageSetData = _context.ImageSet;
            desc.Action = StudyReconcileAction.Merge;
            string newPatientName = GetUnifiedPatientName(desc.ExistingStudy.PatientInfo.Name, _context.ImageSet[DicomTags.PatientsName].Value);

            if (!desc.ExistingStudy.PatientInfo.Name.Equals(newPatientName))
            {
                SetTagCommand cmd = new SetTagCommand(DicomTags.PatientsName,newPatientName);
                desc.Commands = new List<BaseImageLevelUpdateCommand>();
                desc.Commands.Add(cmd);
            }
            
            desc.Automatic = true;
            desc.Description = "Patient Name Auto-Correction";

            IStudyHistoryEntityBroker broker = updateContext.GetBroker<IStudyHistoryEntityBroker>();
            StudyHistoryUpdateColumns columns = new StudyHistoryUpdateColumns();
            columns.StudyStorageKey = _context.CurrentStudyLocation.GetKey();
            columns.DestStudyStorageKey = null;
            columns.InsertTime = Platform.Time;
            columns.StudyHistoryTypeEnum = StudyHistoryTypeEnum.StudyReconciled;
            columns.StudyData = XmlUtils.SerializeAsXmlDoc(_context.ImageSet);
            XmlDocument changeDesc = XmlUtils.SerializeAsXmlDoc(desc);
            columns.ChangeDescription = changeDesc;

            StudyHistory entry = broker.Insert(columns);

            if (entry==null)
            {
                throw new ApplicationException("Unable to create study history record");
            }

            //update the context
            _context.History = entry;
            Platform.Log(LogLevel.Info, "Corrected patient name will be {0}", newPatientName);
        }
    }
}