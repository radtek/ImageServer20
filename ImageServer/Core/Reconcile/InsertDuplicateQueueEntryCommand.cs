using System;
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Dicom;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.ImageServer.Common.CommandProcessor;
using ClearCanvas.ImageServer.Common.Utilities;
using ClearCanvas.ImageServer.Core.Data;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.Brokers;
using ClearCanvas.ImageServer.Model.EntityBrokers;
using ClearCanvas.ImageServer.Model.Parameters;

namespace ClearCanvas.ImageServer.Core.Reconcile
{
	class InsertDuplicateQueueEntryCommand:ServerDatabaseCommand
	{
		private readonly DicomFile _file;
		private readonly StudyStorageLocation _studyLocation;
		private readonly string _receiverId;
		private readonly string _sourceId;
		private DuplicateSopReceivedQueue _queueEntry;

		public InsertDuplicateQueueEntryCommand(String receiverId, String sourceId, 
		                                        StudyStorageLocation studyLocation, Study study, DicomFile file) 
			: base("Insert Duplicate Queue Entry Command", true)
		{
			Platform.CheckForNullReference(receiverId, "receiverID");
			Platform.CheckForNullReference(sourceId, "sourceId");
			Platform.CheckForNullReference(studyLocation, "studyLocation");
			Platform.CheckForNullReference(file, "file");

			// Platform.CheckForNullReference(study, "study"); could be null if the existing files haven't been processed
			_file = file;
			_studyLocation = studyLocation;
			_receiverId = receiverId;
			_sourceId = sourceId;
		}

		public DuplicateSopReceivedQueue QueueEntry
		{
			get { return _queueEntry; }
		}

		protected override void OnExecute(IUpdateContext updateContext)
		{
			IInsertDuplicateSopReceivedQueue broker = updateContext.GetBroker<IInsertDuplicateSopReceivedQueue>();
			InsertDuplicateSopReceivedQueueParameters parms = new InsertDuplicateSopReceivedQueueParameters();
			parms.Receiver = _receiverId;
			parms.Source = _sourceId;
			parms.ServerPartitionKey = _studyLocation.ServerPartitionKey;
			parms.StudyStorageKey = _studyLocation.GetKey();
			parms.StudyInstanceUid = _file.DataSet[DicomTags.StudyInstanceUid].ToString();
			parms.SeriesDescription = _file.DataSet[DicomTags.SeriesDescription].ToString();
			parms.SeriesInstanceUid = _file.DataSet[DicomTags.SeriesInstanceUid].ToString();
			parms.SopInstanceUid = _file.MediaStorageSopInstanceUid;
			parms.Timestamp = Platform.Time;

			ReconcileStudyWorkQueueData data = new ReconcileStudyWorkQueueData();
			data.Details = new ImageSetDetails(_file.DataSet);
            
			ImageSetDescriptor imageSet = new ImageSetDescriptor(_file.DataSet);

			parms.StudyData = XmlUtils.SerializeAsXmlDoc(imageSet);
			parms.QueueData = XmlUtils.SerializeAsXmlDoc(data);
            
			IList<DuplicateSopReceivedQueue> entries = broker.Find(parms);

			Platform.CheckForNullReference(entries, "entries");
			Platform.CheckTrue(entries.Count == 1, "entries.Count==1");

			_queueEntry = entries[0];

		}
	}

	class UpdateDuplicateQueueEntryCommand:ServerDatabaseCommand
	{
		private GetDuplicateSopReceivedQueueDelegateMethod _getDuplicateSopReceivedQueueDelegate;
		private DicomMessageBase _file;

		public delegate DuplicateSopReceivedQueue GetDuplicateSopReceivedQueueDelegateMethod();

        
		public UpdateDuplicateQueueEntryCommand(
			GetDuplicateSopReceivedQueueDelegateMethod getDuplicateSopReceivedQueueDelegate, 
			DicomMessageBase file)
			: base("UpdateDuplicateQueueEntryCommand", true)
		{
			_file = file;
			_getDuplicateSopReceivedQueueDelegate = getDuplicateSopReceivedQueueDelegate;
		}

		protected override void OnExecute(IUpdateContext updateContext)
		{
			DuplicateSopReceivedQueue queueEntry = _getDuplicateSopReceivedQueueDelegate();

			ReconcileStudyWorkQueueData data = XmlUtils.Deserialize<ReconcileStudyWorkQueueData>(queueEntry.QueueData);
			data.Details.InsertFile(_file);

			queueEntry.QueueData = XmlUtils.SerializeAsXmlDoc(data);

			IStudyIntegrityQueueEntityBroker broker = updateContext.GetBroker<IStudyIntegrityQueueEntityBroker>();
			if (!broker.Update(queueEntry))
				throw new ApplicationException("Unable to update duplicate queue entry");
		}
	}
}