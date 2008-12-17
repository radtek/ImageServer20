using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ClearCanvas.Common;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.ImageServer.Common;
using ClearCanvas.ImageServer.Common.CommandProcessor;
using ClearCanvas.ImageServer.Common.Utilities;
using ClearCanvas.ImageServer.Enterprise;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.EntityBrokers;

namespace ClearCanvas.ImageServer.Services.WorkQueue.DeleteStudy.Extensions
{
    /// <summary>
    /// Plugin to backup study when it is deleted for auditing purpose.
    /// </summary>
    [ExtensionOf(typeof(DeleteStudyProcessorExtensionPoint))]
    internal class StudyDeleteProcessorExtension:IDeleteStudyProcessorExtension
    {
        #region Private Members
        private DeleteStudyContext _context;
        private bool _enabled = true;
        private string _backupPath;
        private DeletedStudyArchiveInfoCollection _archives;
        private string BackupSubPath
        {
            get
            {
                if (String.IsNullOrEmpty(_backupPath))
                {

                    string pathPrefix = Path.Combine("Deleted", _context.ServerPartition.PartitionFolder);

                    _backupPath = Path.Combine(pathPrefix, _context.Study.StudyInstanceUid);

                    int counter = 1;
                    while (Directory.Exists(_context.Filesystem.ResolveAbsolutePath(_backupPath)))
                    {
                        _backupPath = Path.Combine(pathPrefix, String.Format("{0}({1})", _context.Study.StudyInstanceUid, counter++));
                    }
                }

                return _backupPath;
            }
        }
        #endregion

        #region IDeleteStudyProcessorExtension Members

        public bool Enabled
        {
            get { return _enabled; }
        }

        #endregion

        #region IDeleteStudyProcessorExtension Members

        public void Initialize(DeleteStudyContext context)
        {
            Platform.CheckForNullReference(context, "context");
            Platform.CheckForNullReference(context.Study, "context.Study");
            _context = context;

            // TODO: Load from configuration
            _enabled = true;
        }

        public void OnStudyDeleting()
        {
            StudyStorageLocation storage = _context.StorageLocation;
            IList < ArchiveStudyStorage> archives = StudyStorageLocation.GetArchiveLocations(storage.GetKey());
            if (archives!=null && archives.Count>0)
            {
                _archives = new DeletedStudyArchiveInfoCollection();
                foreach (ArchiveStudyStorage archive in archives)
                {
                    DeletedStudyArchiveInfo archiveInfo = new DeletedStudyArchiveInfo();
                    archiveInfo.ArchiveTime = archive.ArchiveTime;
                    archiveInfo.ArchiveXml = archive.ArchiveXml;
                    
                    archiveInfo.PartitionArchiveRef = PartitionArchive.Load(archive.PartitionArchiveKey).GetKey().Key;
                    archiveInfo.TransferSyntaxUid = archive.ServerTransferSyntax.Uid;
                    _archives.Add(archiveInfo);
                }
            }
            
                        

            // only backup if study is manually deleted
            if (_context.WorkQueueItem.WorkQueueTypeEnum == WorkQueueTypeEnum.WebDeleteStudy)
            {
                
                using (ServerCommandProcessor processor = new ServerCommandProcessor("Backup deleted study"))
                {
                    string path = _context.Filesystem.ResolveAbsolutePath(BackupSubPath);

                    Platform.Log(LogLevel.Info, "Backing up study to {0}...", path);

                    CreateDirectoryCommand mkdir = new CreateDirectoryCommand(path);
                    processor.AddCommand(mkdir);

                    string zipStudyBackupPath = Path.Combine(path, _context.Study.StudyInstanceUid + ".zip");
                    ZipStudyFolderCommand zip = new ZipStudyFolderCommand(storage.GetStudyPath(), zipStudyBackupPath);
                    processor.AddCommand(zip);

                    if (!processor.Execute())
                    {
                        throw new ApplicationException(String.Format("Unable to backup study: {0}", processor.FailureReason));
                    }
                }
            }
            
        }

        public void OnStudyDeleted()
        {
            if (_context.WorkQueueItem.WorkQueueTypeEnum == WorkQueueTypeEnum.WebDeleteStudy)
            {
                Study study = _context.Study;
                StudyStorageLocation storage = _context.StorageLocation;

                using (IUpdateContext updateContext = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
                {
                    // Setup the parameters
                    IStudyDeleteRecordEntityBroker broker = updateContext.GetBroker<IStudyDeleteRecordEntityBroker>();

                    StudyDeleteRecordUpdateColumns parms = new StudyDeleteRecordUpdateColumns();
                    parms.Timestamp = Platform.Time;
                    WebDeleteStudyData extendedInfo =
                        XmlUtils.Deserialize<WebDeleteStudyData>(_context.WorkQueueItem.Data);

                    parms.Reason = extendedInfo != null? 
                                                           extendedInfo.Reason:_context.WorkQueueItem.WorkQueueTypeEnum.LongDescription;

                    parms.ServerPartitionAE = _context.ServerPartition.AeTitle;
                    parms.FilesystemKey = storage.FilesystemKey;

                    parms.AccessionNumber = study.AccessionNumber;
                    parms.PatientId = study.PatientId;
                    parms.PatientsName = study.PatientsName;
                    parms.StudyInstanceUid = study.StudyInstanceUid;
                    parms.StudyDate = study.StudyDate;
                    parms.StudyDescription = study.StudyDescription;
                    parms.StudyTime = study.StudyTime;

                    parms.BackupPath = BackupSubPath;

                    if (_archives != null && _archives.Count>0)
                    {
                        parms.ArchiveInfo = XmlUtils.SerializeAsXmlDoc(_archives);
                    }

                    StudyDeleteExtendedInfo extInfo = new StudyDeleteExtendedInfo();
                    extInfo.ServerInstanceId = ServiceTools.ServerInstanceId;
                    parms.ExtendedInfo = XmlUtils.SerializeAsString(extInfo);

                    StudyDeleteRecord deleteRecord = broker.Insert(parms);
                    if (deleteRecord == null)
                    {
                        Platform.Log(LogLevel.Error, "Unexpected error when trying to create study delete record: {0} on partition {1}",
                                     study.StudyInstanceUid, _context.ServerPartition.Description);
                    }
                    else
                        updateContext.Commit();
                }
            }
        }

        #endregion
       
    }

    /// <summary>
    /// Extended information for a deleted study record
    /// </summary>
    [Serializable]
    public class StudyDeleteExtendedInfo
    {
        #region Private Fields
        private string _serverInstanceId;
        #endregion

        #region Public Properties
        /// <summary>
        /// The server which processed the study delete request.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public string ServerInstanceId
        {
            get { return _serverInstanceId; }
            set { _serverInstanceId = value; }
        }
        #endregion

    }

    /// <summary>
    /// Archive entry information for a deleted study
    /// </summary>
    [Serializable]
    public class DeletedStudyArchiveInfo
    {
        #region Private Fields
        private DateTime _archiveTime;
        private object _partitionArchiveRef;
        private string _transferSyntaxUid;
        private XmlDocument _archiveXml;
        private PartitionArchive _partitionArchive;
        #endregion

        #region Public Properties
        public DateTime ArchiveTime
        {
            get { return _archiveTime; }
            set { _archiveTime = value; }
        }

        public object PartitionArchiveRef
        {
            get { return _partitionArchiveRef; }
            set { _partitionArchiveRef = value; }
        }

        public string TransferSyntaxUid
        {
            get { return _transferSyntaxUid; }
            set { _transferSyntaxUid = value; }
        }

        public XmlDocument ArchiveXml
        {
            get { return _archiveXml; }
            set { _archiveXml = value; }
        }

        [XmlIgnore]
        public PartitionArchive PartitionArchive
        {
            get
            {
                if (PartitionArchiveRef == null)
                    throw new ApplicationException("PartitionArchiveRef must be set");

                if (_partitionArchive == null)
                {
                    ServerEntityKey key = new ServerEntityKey("ParitionArchive", PartitionArchiveRef);
                    _partitionArchive = PartitionArchive.Load(key);

                }

                return _partitionArchive;
            }
        }

        #endregion

    }

    /// <summary>
    /// Collection of archive entry information for a deleted study
    /// </summary>
    [Serializable]
    public class DeletedStudyArchiveInfoCollection : Collection<DeletedStudyArchiveInfo>
    {
    }
}