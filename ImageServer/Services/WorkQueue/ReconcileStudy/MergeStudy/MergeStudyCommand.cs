using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom;
using ClearCanvas.Dicom.Utilities.Xml;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.ImageServer.Common;
using ClearCanvas.ImageServer.Common.CommandProcessor;
using ClearCanvas.ImageServer.Common.Utilities;
using ClearCanvas.ImageServer.Enterprise;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.EntityBrokers;
using ClearCanvas.ImageServer.Services.Dicom;
using ClearCanvas.ImageServer.Services.WorkQueue.WebEditStudy;

namespace ClearCanvas.ImageServer.Services.WorkQueue.ReconcileStudy.MergeStudy
{
    /// <summary>
    /// Command for reconciling images by merging new images into an existing study.
    /// </summary>
    /// <remark>
    /// </remark>
    class MergeStudyCommand : ServerCommand, IReconcileServerCommand, IDisposable
    {
        #region Private Members
        private ReconcileStudyProcessorContext _reconcileContext;
        private bool _updateDestination;

        private ServerCommandProcessor _processor;
        private StudyStorageLocation _destStudyStorage = null;
        private readonly Dictionary<string, WorkQueueUid> _fileToUidMap = new Dictionary<string, WorkQueueUid>();
        private readonly string _workingDir = ServerPlatform.GetTempPath();

        private Study _study;
        private readonly List<IImageLevelUpdateCommand> _imageLevelCommands = new List<IImageLevelUpdateCommand>();
        private readonly List<WorkQueueUid> _processedUidList = new List<WorkQueueUid>();
        private readonly List<WorkQueueUid> _failedUidList = new List<WorkQueueUid>();
        private readonly List<WorkQueueUid> _duplicateList = new List<WorkQueueUid>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an instance of <see cref="MergeStudyCommand"/>
        /// </summary>
        public MergeStudyCommand()
            : base("Merge Study", true)
        {

        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the destination of the merged study
        /// </summary>
        public StudyStorageLocation DestStudyStorage
        {
            get { return _destStudyStorage; }
            set { _destStudyStorage = value; }
        }

        /// <summary>
        /// Sets or gets the value that indicates whether the current data in the merged study should be updated
        /// using the commands in <see cref="ImageLevelCommands"/>
        /// </summary>
        /// <remarks>
        /// </remarks>
        public bool UpdateDestination
        {
            get { return _updateDestination; }
            set { _updateDestination = value; }
        }

        /// <summary>
        /// Gets the list of <see cref="IImageLevelUpdateCommand"/> for updating new images.
        /// </summary>
        public List<IImageLevelUpdateCommand> ImageLevelCommands
        {
            get { return _imageLevelCommands; }
        }

        #endregion

        protected override void OnExecute()
        {
            Platform.CheckForNullReference(_reconcileContext, "_reconcileContext");
            Platform.CheckForNullReference(DestStudyStorage, "DestStudyStorage");
            
            if (_updateDestination)
                UpdateExistingStudy();
            
            LoadMergedStudyEntities();

            PrepareWorkingFolder();

            UpdateFilesystem();
            
            UpdateHistory();

            LogResult();

        }

        protected override void OnUndo()
        {
            if (_processor != null)
            {
                _processor.Rollback();
                _processor = null;
            }
        }

        #region Private Members
        private void LogResult()
        {
            StringBuilder log = new StringBuilder();
            log.AppendFormat("Destination location: {0}", _destStudyStorage.GetStudyPath());
            log.AppendLine();
            if (_failedUidList.Count > 0)
            {
                log.AppendFormat("{0} images failed to be reconciled.", _failedUidList.Count);
                log.AppendLine();
            }
            if (_processedUidList.Count > 0)
            {
                log.AppendFormat("{0} images have been reconciled and will be processed.", _processedUidList.Count);
                log.AppendLine();
            }
            if (_duplicateList.Count > 0)
            {
                log.AppendFormat("{0} images are duplicate.", _duplicateList.Count);
                log.AppendLine();
            }
            Platform.Log(LogLevel.Info, log);
        }

        private void UpdateHistory()
        {
            IPersistentStore store = PersistentStoreRegistry.GetDefaultStore();
            using (IUpdateContext ctx = store.OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                IStudyHistoryEntityBroker historyUpdateBroker = ctx.GetBroker<IStudyHistoryEntityBroker>();
                StudyHistoryUpdateColumns parms = new StudyHistoryUpdateColumns();
                parms.DestStudyStorageKey = DestStudyStorage.GetKey();
                historyUpdateBroker.Update(_reconcileContext.History.GetKey(), parms);
                ctx.Commit();
            }
        }


        private void UpdateExistingStudy()
        {

            Platform.Log(LogLevel.Info, "Updating existing study...");
            using(ServerCommandProcessor updateProcessor = new ServerCommandProcessor("Update Study"))
            {
                UpdateStudyCommand studyUpdateCommand =
                    new UpdateStudyCommand(_reconcileContext.Partition,
                                            _destStudyStorage,
                                            _imageLevelCommands);
                updateProcessor.AddCommand(studyUpdateCommand);
                updateProcessor.Execute();    
            }
            
        }

        private void UpdateFilesystem()
        {
            Platform.Log(LogLevel.Info, "Populating new images into study folder");
            FileProcessor.Process(_workingDir, "*.dcm",
              delegate(string path)
              {
                  // this should be the updated study instance
                  DicomFile file = new DicomFile(path);
                  file.Load(DicomReadOptions.StorePixelDataReferences);
                  SaveFile(file);

              }, true);

        }

        private void LoadMergedStudyEntities()
        {
            StudyStorage storage = StudyStorage.Load(_destStudyStorage.GetKey());
            _study = Study.Find(storage.StudyInstanceUid, _reconcileContext.Partition);
            Debug.Assert(_study != null);
        }

        
        private void CleanupWorkingFolder()
        {
            DirectoryUtility.DeleteIfExists(_workingDir);
        }

        private void SaveFile(DicomFile file)
        {

            string workingImagePath = file.Filename;
            WorkQueueUid uid = null;
            if (_fileToUidMap.ContainsKey(workingImagePath))
                uid = _fileToUidMap[workingImagePath];

            String seriesInstanceUid = file.DataSet[DicomTags.SeriesInstanceUid].GetString(0, String.Empty);
            String sopInstanceUid = file.DataSet[DicomTags.SopInstanceUid].GetString(0, String.Empty);
            ServerCommandProcessor processor = new ServerCommandProcessor("Update file system");

            String destPath = _destStudyStorage.FilesystemPath;
            String extension = ".dcm";
            bool dupImage = false;

            processor.AddCommand(new CreateDirectoryCommand(destPath));

            destPath = Path.Combine(destPath, _destStudyStorage.PartitionFolder);
            processor.AddCommand(new CreateDirectoryCommand(destPath));

            destPath = Path.Combine(destPath, _destStudyStorage.StudyFolder);
            processor.AddCommand(new CreateDirectoryCommand(destPath));

            destPath = Path.Combine(destPath, _destStudyStorage.StudyInstanceUid);
            processor.AddCommand(new CreateDirectoryCommand(destPath));

            destPath = Path.Combine(destPath, seriesInstanceUid);
            processor.AddCommand(new CreateDirectoryCommand(destPath));

            destPath = Path.Combine(destPath, sopInstanceUid);
            destPath += extension;

            if (File.Exists(destPath))
            {
                if (uid!=null)
                {
                    #region Duplicate SOP

                    // TODO: Add code to handle duplicate sop here
                    Platform.Log(LogLevel.Warn, "Image {0} is discarded because of duplicate", file.Filename);
                    DeleteUid(uid);
                    File.Delete(GetUidPath(uid));
                    _duplicateList.Add(uid);
                    return;

                    #endregion
                }
            }


            Platform.Log(LogLevel.Debug, "Saving {0}", destPath);
            SaveDicomFileCommand saveCommand = new SaveDicomFileCommand(destPath, file);
            processor.AddCommand(saveCommand);
            if (uid!=null)
            {
                processor.AddCommand(new UpdateWorkQueueCommand(file, _destStudyStorage, dupImage, extension));
            }

            if (!processor.Execute())
            {
                _failedUidList.Add(uid);
                throw new ApplicationException(String.Format("Unable to reconcile image {0} : {1}", file.Filename, processor.FailureReason));
            }

            if (uid!=null)
            {
                DeleteUid(uid);
                File.Delete(GetUidPath(uid));
                _processedUidList.Add(uid);
            }

        }
        private void DeleteUid(WorkQueueUid sop)
        {
            using (IUpdateContext updateContext = PersistentStoreRegistry.GetDefaultStore().OpenUpdateContext(UpdateContextSyncMode.Flush))
            {
                IWorkQueueUidEntityBroker delete = updateContext.GetBroker<IWorkQueueUidEntityBroker>();

                delete.Delete(sop.GetKey());

                updateContext.Commit();
            }
        }


        private string GetUidPath(WorkQueueUid sop)
        {
            string imagePath = Path.Combine(_reconcileContext.ReconcileWorkQueueData.StoragePath, sop.SopInstanceUid + ".dcm");
            return imagePath;
        }

        private void PrepareWorkingFolder()
        {

            Platform.Log(LogLevel.Info, "Preparing new images in {0}", _workingDir);
            Directory.CreateDirectory(_workingDir);
            foreach (WorkQueueUid uid in _reconcileContext.WorkQueueUidList)
            {
                string imagePath = GetUidPath(uid);
                string destPath = Path.Combine(_workingDir, uid.SopInstanceUid + ".dcm");
                File.Copy(imagePath, destPath);

                _fileToUidMap.Add(destPath, uid);
            }


            List<IImageLevelUpdateCommand> updateCommandList = BuildUpdateCommandList();
            PrintUpdateCommands(updateCommandList);

            FileProcessor.Process(_workingDir, "*.dcm",
                                  delegate(string path)
                                  {
                                      DicomFile file = new DicomFile(path);
                                      file.Load(DicomReadOptions.StorePixelDataReferences);
                                      Platform.Log(LogLevel.Info, "Processing {0}", path);
                                      foreach (IImageLevelUpdateCommand command in updateCommandList)
                                      {
                                          command.Apply(file);
                                      }

                                      // work around a bug in dicom toolkit
                                      file.Save(path + ".temp");
                                      File.Delete(path);
                                      File.Move(path + ".temp", path);

                                  }, true);
        }

        private void PrintUpdateCommands(IList<IImageLevelUpdateCommand> updateCommandList)
        {
            StringBuilder log = new StringBuilder();
            log.AppendLine();
            log.AppendFormat("Update on merged images:");
            log.AppendLine();
            foreach (IImageLevelUpdateCommand cmd in updateCommandList)
            {
                log.AppendFormat("{0}", cmd);
                log.AppendLine();
            }
            Platform.Log(LogLevel.Info, log);
        }


        private List<IImageLevelUpdateCommand> BuildUpdateCommandList()
        {
            List<IImageLevelUpdateCommand> updateCommandList = new List<IImageLevelUpdateCommand>();
            
            ImageUpdateCommandBuilder builder = new ImageUpdateCommandBuilder();
            updateCommandList.AddRange(builder.BuildCommands<DemographicInfo>(_destStudyStorage));
            updateCommandList.AddRange(builder.BuildCommands<StudyInfoMapping>(_destStudyStorage));

            
            return updateCommandList;
        }

        #endregion

       
        #region IDisposable Members

        public void Dispose()
        {
            CleanupWorkingFolder();

            DirectoryUtility.DeleteIfEmpty(_reconcileContext.ReconcileWorkQueueData.StoragePath);
        }

        #endregion

        #region IReconcileServerCommand Members

        public void SetContext(ReconcileStudyProcessorContext context)
        {
            _reconcileContext = context;
        }
        #endregion
    }

}