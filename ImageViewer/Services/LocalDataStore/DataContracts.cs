using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ClearCanvas.ImageViewer.Services.LocalDataStore
{
	[Flags]
	public enum CancellationFlags
	{
		None = 0,
		Cancel = 1,
		Clear = 2
	}

	public enum InstanceLevel
	{ 
		Study = 0,
		Series,
		Sop
	}

	[DataContract]
	public class RetrieveStudyInformation
	{
		private string _fromAETitle;
		private StudyInformation _studyInformation;
		
		public RetrieveStudyInformation()
		{ 
		}

		[DataMember(IsRequired = true)]
		public string FromAETitle
		{
			get { return _fromAETitle; }
			set { _fromAETitle = value; }
		}

		[DataMember(IsRequired = true)]
		public StudyInformation StudyInformation
		{
			get { return _studyInformation; }
			set { _studyInformation = value; }
		}
	}

	[DataContract]
	public class ReceiveErrorInformation
	{
		private string _fromAETitle;
		private StudyInformation _studyInformation;
		private string _errorMessage;

		public ReceiveErrorInformation()
		{ 
		}

		[DataMember(IsRequired = true)]
		public string FromAETitle
		{
			get { return _fromAETitle; }
			set { _fromAETitle = value; }
		}

		[DataMember(IsRequired = true)]
		public StudyInformation StudyInformation
		{
			get { return _studyInformation; }
			set { _studyInformation = value; }
		}

		[DataMember(IsRequired = true)]
		public string ErrorMessage
		{
			get { return _errorMessage; }
			set { _errorMessage = value;}
		}
	}

	[DataContract]
	public class SendStudyInformation
	{
		private string _toAETitle;
		private StudyInformation _studyInformation;

		public SendStudyInformation()
		{
		}

		[DataMember(IsRequired = true)]
		public string ToAETitle
		{
			get { return _toAETitle; }
			set { _toAETitle = value; }
		}

		[DataMember(IsRequired = true)]
		public StudyInformation StudyInformation
		{
			get { return _studyInformation; }
			set { _studyInformation = value; }
		}
	}

	[DataContract]
	public class SendErrorInformation
	{
		private string _toAETitle;
		private StudyInformation _studyInformation;
		private string _errorMessage;

		public SendErrorInformation()
		{
		}

		[DataMember(IsRequired = true)]
		public string ToAETitle
		{
			get { return _toAETitle; }
			set { _toAETitle = value; }
		}

		[DataMember(IsRequired = true)]
		public StudyInformation StudyInformation
		{
			get { return _studyInformation; }
			set { _studyInformation = value; }
		}
		
		[DataMember(IsRequired = true)]
		public string ErrorMessage
		{
			get { return _errorMessage; }
			set { _errorMessage = value; }
		}
	}

	[DataContract]
	public class StoreScpReceivedFileInformation
	{
		private string _aeTitle;
		private string _fileName;

		public StoreScpReceivedFileInformation()
		{
		}

		[DataMember(IsRequired = true)]
		public string AETitle
		{
			get { return _aeTitle; }
			set { _aeTitle = value; }
		}

		[DataMember(IsRequired = true)]
		public string FileName
		{
			get { return _fileName; }
			set { _fileName = value; }
		}
	}

	[DataContract]
	public class StoreScuSentFileInformation
	{
		private string _toAETitle;
		private string _fileName;

		public StoreScuSentFileInformation()
		{
		}

		[DataMember(IsRequired = true)]
		public string ToAETitle
		{
			get { return _toAETitle; }
			set { _toAETitle = value; }
		}

		[DataMember(IsRequired = true)]
		public string FileName
		{
			get { return _fileName; }
			set { _fileName = value; }
		}
	}

	[DataContract]
	public abstract class FileOperationProgressItem
	{
		private Guid _identifier;
		private bool _cancelled;
		private bool _removed;
		private CancellationFlags _allowedCancellationOperations;
		private DateTime _startTime;
		private DateTime _lastActive;
		private string _statusMessage;

		public FileOperationProgressItem()
		{ 
		}

		[DataMember(IsRequired = true)]
		public Guid Identifier
		{
			get { return _identifier; }
			set { _identifier = value; }
		}

		[DataMember(IsRequired = true)]
		public CancellationFlags AllowedCancellationOperations
		{
			get { return _allowedCancellationOperations; }
			set { _allowedCancellationOperations = value; }
		}

		[DataMember(IsRequired = true)]
		public bool Cancelled
		{
			get { return _cancelled; }
			set { _cancelled = value; }
		}

		[DataMember(IsRequired = true)]
		public bool Removed
		{
			get { return _removed; }
			set { _removed = value; }
		}

		[DataMember(IsRequired = true)]
		public DateTime StartTime
		{
			get { return _startTime; }
			set { _startTime = value; }
		}

		[DataMember(IsRequired = true)]
		public DateTime LastActive
		{
			get { return _lastActive; }
			set { _lastActive = value; }
		}

		[DataMember(IsRequired = false)]
		public string StatusMessage
		{
			get { return _statusMessage; }
			set { _statusMessage = value; }
		}

		public void CopyTo(FileOperationProgressItem progressItem)
		{
			progressItem.Identifier = this.Identifier;
			progressItem.AllowedCancellationOperations = this.AllowedCancellationOperations;
			progressItem.Cancelled = this.Cancelled;
			progressItem.Removed = this.Removed;
			progressItem.StartTime = this.StartTime;
			progressItem.LastActive = this.LastActive;
			progressItem.StatusMessage = this.StatusMessage;
		}

		public void CopyFrom(FileOperationProgressItem progressItem)
		{
			this.Identifier = progressItem.Identifier;
			this.AllowedCancellationOperations = progressItem.AllowedCancellationOperations;
			this.Cancelled = progressItem.Cancelled;
			this.Removed = progressItem.Removed;
			this.StartTime = progressItem.StartTime;
			this.LastActive = progressItem.LastActive;
			this.StatusMessage = progressItem.StatusMessage;
		}
	}

	[DataContract]
	public class ImportProgressItem : FileOperationProgressItem
	{
		private string _description;
		private int _totalFilesToImport;
		private int _numberOfFailedImports;
		private int _numberOfFilesImported;
		private int _numberOfFilesCommittedToDataStore;

		public ImportProgressItem()
		{
			_totalFilesToImport = 0;
			_numberOfFailedImports = 0;
			_numberOfFilesImported = 0;
			_numberOfFilesCommittedToDataStore = 0;
		}

		[DataMember(IsRequired = true)]
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		[DataMember(IsRequired = true)]
		public int TotalFilesToImport
		{
			get { return _totalFilesToImport; }
			set { _totalFilesToImport = value; }
		}

		[DataMember(IsRequired = true)]
		public int NumberOfFilesImported
		{
			get { return _numberOfFilesImported; }
			set { _numberOfFilesImported = value; }
		}

		[DataMember(IsRequired = true)]
		public int NumberOfFilesCommittedToDataStore
		{
			get { return _numberOfFilesCommittedToDataStore; }
			set { _numberOfFilesCommittedToDataStore = value; }
		}

		[DataMember(IsRequired = true)]
		public int NumberOfFailedImports
		{
			get { return _numberOfFailedImports; }
			set { _numberOfFailedImports = value; }
		}

		public bool IsImportComplete()
		{
			return TotalFilesToImport == (this.NumberOfFilesImported + this.NumberOfFailedImports);
		}

		public bool IsComplete()
		{
			return TotalFilesToImport == (this.NumberOfFilesCommittedToDataStore + this.NumberOfFailedImports);
		}

		public void CopyTo(ImportProgressItem progressItem)
		{
			progressItem.Description = this.Description;
			progressItem.TotalFilesToImport = this.TotalFilesToImport;
			progressItem.NumberOfFailedImports = this.NumberOfFailedImports;
			progressItem.NumberOfFilesImported = this.NumberOfFilesImported;
			progressItem.NumberOfFilesCommittedToDataStore = this.NumberOfFilesCommittedToDataStore;

			base.CopyTo(progressItem);
		}

		public void CopyFrom(ImportProgressItem progressItem)
		{
			this.Description = progressItem.Description;
			this.TotalFilesToImport = progressItem.TotalFilesToImport;
			this.NumberOfFailedImports = progressItem.NumberOfFailedImports;
			this.NumberOfFilesImported = progressItem.NumberOfFilesImported;
			this.NumberOfFilesCommittedToDataStore = progressItem.NumberOfFilesCommittedToDataStore;

			base.CopyFrom(progressItem);
		}

		public ImportProgressItem Clone()
		{
			ImportProgressItem clone = new ImportProgressItem();
			CopyTo(clone);
			return clone;
		}
	}

	[DataContract]
	public class ExportProgressItem : FileOperationProgressItem
	{
		private int _totalFilesToExport;
		private int _numberOfFilesExported;

		public ExportProgressItem()
		{
			_totalFilesToExport = 0;
			_numberOfFilesExported = 0;
		}

		[DataMember(IsRequired = true)]
		public int TotalFilesToExport
		{
			get { return _totalFilesToExport; }
			set { _totalFilesToExport = value; }
		}

		[DataMember(IsRequired = true)]
		public int NumberOfFilesExported
		{
			get { return _numberOfFilesExported; }
			set { _numberOfFilesExported = value; }
		}

		public void CopyTo(ExportProgressItem progressItem)
		{
			progressItem.TotalFilesToExport = this.TotalFilesToExport;
			progressItem.NumberOfFilesExported = this.NumberOfFilesExported;

			base.CopyTo(progressItem);
		}

		public void CopyFrom(ExportProgressItem progressItem)
		{
			this.TotalFilesToExport = progressItem.TotalFilesToExport;
			this.NumberOfFilesExported = progressItem.NumberOfFilesExported;

			base.CopyFrom(progressItem);
		}

		public ExportProgressItem Clone()
		{
			ExportProgressItem clone = new ExportProgressItem();
			CopyTo(clone);
			return clone;
		}
	}

	[DataContract]
	public class ReceiveProgressItem : ImportProgressItem
	{
		private string _fromAETitle;
		private StudyInformation _studyInformation;
		private int _numberOfFilesReceived;

		public ReceiveProgressItem()
		{
		}

		[DataMember(IsRequired = true)]
		public string FromAETitle
		{
			get { return _fromAETitle; }
			set { _fromAETitle = value; }
		}

		[DataMember(IsRequired = true)]
		public StudyInformation StudyInformation
		{
			get { return _studyInformation; }
			set { _studyInformation = value; }
		}

		[DataMember(IsRequired = true)]
		public int NumberOfFilesReceived
		{
			get { return _numberOfFilesReceived; }
			set { _numberOfFilesReceived = value; }
		}

		public void CopyTo(ReceiveProgressItem progressItem)
		{
			progressItem.FromAETitle = this.FromAETitle;
			progressItem.NumberOfFilesReceived = this.NumberOfFilesReceived;

			if (this.StudyInformation != null)
				progressItem.StudyInformation = this.StudyInformation.Clone();
			else
				progressItem.StudyInformation = null;

			base.CopyTo(progressItem);
		}

		public void CopyFrom(ReceiveProgressItem progressItem)
		{
			this.FromAETitle = progressItem.FromAETitle;
			this.NumberOfFilesReceived = progressItem.NumberOfFilesReceived;

			if (progressItem.StudyInformation != null)
				this.StudyInformation = progressItem.StudyInformation.Clone();
			else
				this.StudyInformation = null;

			base.CopyFrom(progressItem);
		}

		public new ReceiveProgressItem Clone()
		{
			ReceiveProgressItem clone = new ReceiveProgressItem();
			CopyTo(clone);
			return clone;
		}
	}

	[DataContract]
	public class SendProgressItem : ExportProgressItem
	{
		private string _toAETitle;
		private StudyInformation _studyInformation;

		public SendProgressItem()
		{
		}

		[DataMember(IsRequired = true)]
		public string ToAETitle
		{
			get { return _toAETitle; }
			set { _toAETitle = value; }
		}

		[DataMember(IsRequired = true)]
		public StudyInformation StudyInformation
		{
			get { return _studyInformation; }
			set { _studyInformation = value; }
		}

		public void CopyTo(SendProgressItem progressItem)
		{
			progressItem.ToAETitle = this.ToAETitle;

			if (this.StudyInformation != null)
				progressItem.StudyInformation = this.StudyInformation.Clone();
			else
				progressItem.StudyInformation = null;

			base.CopyTo(progressItem);
		}

		public void CopyFrom(SendProgressItem progressItem)
		{
			this.ToAETitle = progressItem.ToAETitle;

			if (progressItem.StudyInformation != null)
				this.StudyInformation = progressItem.StudyInformation.Clone();
			else
				this.StudyInformation = null;

			base.CopyFrom(progressItem);
		}

		public new SendProgressItem Clone()
		{
			SendProgressItem clone = new SendProgressItem();
			CopyTo(clone);
			return clone;
		}
	}

	[DataContract]
	public class ReindexProgressItem : ImportProgressItem
	{
		public ReindexProgressItem()
		{
		}

		public new ReindexProgressItem Clone()
		{
			ReindexProgressItem clone = new ReindexProgressItem();
			CopyTo(clone);
			return clone;
		}
	}

	[DataContract]
	public class CancelProgressItemInformation
	{
		private CancellationFlags _cancellationFlags;
		private IEnumerable<Guid> _progressItemIdentifiers;

		public CancelProgressItemInformation()
		{
		}

		[DataMember(IsRequired = true)]
		public CancellationFlags CancellationFlags
		{
			get { return _cancellationFlags; }
			set { _cancellationFlags = value; }
		}

		[DataMember(IsRequired = true)]
		public IEnumerable<Guid> ProgressItemIdentifiers
		{
			get { return _progressItemIdentifiers; }
			set { _progressItemIdentifiers = value; }
		}
	}

	[DataContract]
	public class ImportedSopInstanceInformation
	{
		private string _studyInstanceUid;
		private string _seriesInstanceUid;
		private string _sopInstanceUid;
		private string _sopInstanceFileName;

		public ImportedSopInstanceInformation()
		{
		}

		[DataMember(IsRequired = true)]
		public string StudyInstanceUid
		{
			get { return _studyInstanceUid; }
			set { _studyInstanceUid = value; }
		}

		[DataMember(IsRequired = true)]
		public string SeriesInstanceUid
		{
			get { return _seriesInstanceUid; }
			set { _seriesInstanceUid = value; }
		}
		
		[DataMember(IsRequired = true)]
		public string SopInstanceUid
		{
			get { return _sopInstanceUid; }
			set { _sopInstanceUid = value; }
		}

		[DataMember(IsRequired = false)]
		public string SopInstanceFileName
		{
			get { return _sopInstanceFileName; }
			set { _sopInstanceFileName = value; }
		}
	}

	public enum BadFileBehaviour
	{ 
		Ignore = 0,
		Move,
		Delete
	}

	public enum FileImportBehaviour
	{ 
		Move = 0,
		Copy
	}

	[DataContract]
	public class FileImportRequest
	{
		private bool _recursive;
		private BadFileBehaviour _badFileBehaviour;
		private IEnumerable<string> _fileExtensions;
		private IEnumerable<string> _filePaths;
		private FileImportBehaviour _fileImportBehaviour;

		public FileImportRequest()
		{
		}

		[DataMember(IsRequired = true)]
		public bool Recursive
		{
			get { return _recursive; }
			set { _recursive = value; }
		}

		[DataMember(IsRequired = true)]
		public BadFileBehaviour BadFileBehaviour
		{
			get { return _badFileBehaviour; }
			set { _badFileBehaviour = value; }
		}

		[DataMember(IsRequired = true)]
		public IEnumerable<string> FileExtensions
		{
			get { return _fileExtensions; }
			set { _fileExtensions = value; }
		}

		[DataMember(IsRequired = true)]
		public IEnumerable<string> FilePaths
		{
			get { return _filePaths; }
			set { _filePaths = value; }
		}

		[DataMember(IsRequired = true)]
		public FileImportBehaviour FileImportBehaviour
		{
			get { return _fileImportBehaviour; }
			set { _fileImportBehaviour = value; }
		}
	}

	[DataContract]
	public class InstanceInformation
	{
		private InstanceLevel _instanceLevel;
		private string _instanceUid;

		public InstanceInformation()
		{ 
		}

		[DataMember(IsRequired = true)]
		public InstanceLevel InstanceLevel
		{
			get { return _instanceLevel; }
			set { _instanceLevel = value; }
		}

		[DataMember(IsRequired = true)]
		public string InstanceUid
		{
			get { return _instanceUid; }
			set { _instanceUid = value; }
		}

		public void CopyTo(InstanceInformation other)
		{
			other.InstanceLevel = this.InstanceLevel;
			other.InstanceUid = this.InstanceUid;
		}

		public InstanceInformation Clone()
		{
			InstanceInformation clone = new InstanceInformation();
			CopyTo(clone);
			return clone;
		}
	}

	[DataContract]
	public class DeletedInstanceInformation : InstanceInformation
	{
		private string _errorMessage;
		private long _totalFreedSpace;

		public DeletedInstanceInformation()
		{ 
		}

		public bool Failed
		{
			get { return _errorMessage != null; }
		}

		[DataMember(IsRequired = true)]
		public string ErrorMessage
		{
			get { return _errorMessage; }
			set { _errorMessage = value; }
		}

		[DataMember(IsRequired = true)]
		public long TotalFreedSpace
		{
			get { return _totalFreedSpace; }
			set { _totalFreedSpace = value; }
		}

		public void CopyTo(DeletedInstanceInformation other)
		{
			other.ErrorMessage = this.ErrorMessage;
			other.TotalFreedSpace = this.TotalFreedSpace;
			base.CopyTo(other);
		}

		public DeletedInstanceInformation Clone()
		{
			DeletedInstanceInformation clone = new DeletedInstanceInformation();
			CopyTo(clone);
			return clone;
		}
	}

	public enum DeletePriority
	{ 
		Low = 0,
		High
	}

	[DataContract]
	public class DeleteInstancesRequest
	{
		private DeletePriority _deletePriority;
		private InstanceLevel _instanceLevel;
		private IEnumerable<string> _instanceUids;

		public DeleteInstancesRequest()
		{ 
		}

		[DataMember(IsRequired = true)]
		public DeletePriority DeletePriority
		{
			get { return _deletePriority; }
			set { _deletePriority = value; }
		}

		[DataMember(IsRequired = true)]
		public InstanceLevel InstanceLevel
		{
			get { return _instanceLevel; }
			set { _instanceLevel = value; }
		}

		[DataMember(IsRequired = true)]
		public IEnumerable<string> InstanceUids
		{
			get { return _instanceUids; }
			set { _instanceUids = value; }
		}
	}

	[DataContract]
	public class LocalDataStoreServiceConfiguration
	{
		private string _storageDirectory;
		private string _badFileDirectory;

		public LocalDataStoreServiceConfiguration()
		{
		}

		[DataMember(IsRequired = true)]
		public string StorageDirectory
		{
			get { return _storageDirectory; }
			set { _storageDirectory = value; }
		}

		[DataMember(IsRequired = true)]
		public string BadFileDirectory
		{
			get { return _badFileDirectory; }
			set { _badFileDirectory = value; }
		}
	}
}
