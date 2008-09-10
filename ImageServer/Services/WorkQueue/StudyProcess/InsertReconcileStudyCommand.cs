using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ClearCanvas.Common;
using ClearCanvas.Dicom;
using ClearCanvas.ImageServer.Common.CommandProcessor;
using ClearCanvas.ImageServer.Common.Utilities;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.Brokers;
using ClearCanvas.ImageServer.Model.EntityBrokers;
using ClearCanvas.ImageServer.Model.Parameters;

namespace ClearCanvas.ImageServer.Services.WorkQueue.StudyProcess
{
    /// <summary>
    /// Command for inserting or updating 'ReconcileStudy' work queue entries.
    /// </summary>
    class InsertReconcileStudyCommand : ServerDatabaseCommand
    {
        #region Private Members
        private readonly ReconcileImageContext _context;
        #endregion

        #region Constructors
        public InsertReconcileStudyCommand(ReconcileImageContext context)
            : base("InsertReconcileStudyCommand", true)
        {
            Platform.CheckForNullReference(context, "context");
            _context = context;
        }
        #endregion

        #region Overridden Protected Methods
        protected override void OnExecute(ClearCanvas.Enterprise.Core.IUpdateContext updateContext)
        {
            Platform.CheckForNullReference(updateContext, "updateContext");
            Platform.CheckForNullReference(_context, "_context");


            WorkQueueSettings settings = WorkQueueSettings.Instance;
            IInsertWorkQueueReconcileStudy broker = updateContext.GetBroker<IInsertWorkQueueReconcileStudy>();
            WorkQueueReconcileStudyInsertParameters parameters = new WorkQueueReconcileStudyInsertParameters();
            parameters.ServerPartitionKey = _context.Partition.GetKey();
            parameters.StudyStorageKey = _context.CurrentStudyLocation.GetKey();
            parameters.StudyHistoryKey = _context.History.GetKey();
            parameters.SeriesInstanceUid = _context.File.DataSet[DicomTags.SeriesInstanceUid].GetString(0, String.Empty);
            parameters.SopInstanceUid = _context.File.DataSet[DicomTags.SopInstanceUid].GetString(0, String.Empty);

            ReconcileStudyWorkQueueData data = new ReconcileStudyWorkQueueData();
            data.StoragePath = _context.TempStoragePath; 
            XmlDocument xmlData = new XmlDocument();
            XmlNode node = xmlData.ImportNode(XmlUtils.Serialize(data), true);
            xmlData.AppendChild(node);
            parameters.WorkQueueData = xmlData;

            DateTime now = Platform.Time;
            parameters.ScheduledTime = now;
            parameters.ExpirationTime = now.AddSeconds(settings.WorkQueueExpireDelaySeconds);
            parameters.Priority = WorkQueuePriorityEnum.Medium;
            Model.WorkQueue workqueue = broker.FindOne(parameters);
            if (workqueue==null)
            {
                throw new ApplicationException("Unable to insert ReconcileStudy work queue entry");
            }

            data = XmlUtils.Deserialize<ReconcileStudyWorkQueueData>(workqueue.Data);
            _context.TempStoragePath = data.StoragePath;

        }

        #endregion

    }
}