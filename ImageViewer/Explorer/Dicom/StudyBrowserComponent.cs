using System;
using System.Collections.Generic;
using System.Text;
using ClearCanvas.Desktop;
using ClearCanvas.Common;
using ClearCanvas.Desktop.Explorer;
using ClearCanvas.ImageViewer.StudyManagement;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Desktop.Actions;

namespace ClearCanvas.ImageViewer.Explorer.Dicom
{
	[ExtensionPoint()]
	public class StudyBrowserToolExtensionPoint : ExtensionPoint<ITool>
	{
	}

	[ExtensionPoint()]
	public class StudyBrowserComponentViewExtensionPoint : ExtensionPoint<IApplicationComponentView>
	{
	}

	public interface IStudyBrowserToolContext : IToolContext
	{
		StudyBrowserComponent StudyBrowserComponent { get; }

		ClickHandlerDelegate DefaultActionHandler { get; set; }

		IDesktopWindow DesktopWindow { get; }
	}
	
	[AssociateView(typeof(StudyBrowserComponentViewExtensionPoint))]
	public class StudyBrowserComponent : ApplicationComponent
	{
		public class StudyBrowserToolContext : ToolContext, IStudyBrowserToolContext
		{
			StudyBrowserComponent _component;

			public StudyBrowserToolContext(StudyBrowserComponent component)
			{
				Platform.CheckForNullReference(component, "component");
				_component = component;
			}

			public StudyBrowserComponent StudyBrowserComponent
			{
				get { return _component; }
			}

			public ClickHandlerDelegate DefaultActionHandler
			{
				get { return _component._defaultActionHandler; }
				set { _component._defaultActionHandler = value; }
			}
			
			public IDesktopWindow DesktopWindow
			{
				get { return _component.Host.DesktopWindow; }
			}
		}
	
		#region Fields

		private IStudyFinder _studyFinder;
		private IStudyLoader _studyLoader;
		private TableData<StudyItem> _studyList;

		private string _title;

		private string _lastName = "";
		private string _firstName = "";
		private string _patientID = "";
		private string _accessionNumber = "";
		private string _studyDescription = "";

		private ISelection _currentSelection;
		private event EventHandler _selectedStudyChanged;
		private ClickHandlerDelegate _defaultActionHandler;
		private ToolSet _toolSet;

		private ActionModelRoot _toolbarModel;
		private ActionModelRoot _contextMenuModel;

		#endregion

		public IStudyFinder StudyFinder
		{
			get { return _studyFinder; }
			set { _studyFinder = value; }
		}

		public IStudyLoader StudyLoader
		{
			get { return _studyLoader; }
			set { _studyLoader = value; }
		}

		public TableData<StudyItem> StudyList
		{
			get { return _studyList; }
		}

		public ActionModelRoot ToolbarModel
		{
			get { return _toolbarModel; }
		}

		public ActionModelRoot ContextMenuModel
		{
			get { return _contextMenuModel; }
		}
	

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		public string AccessionNumber
		{
			get { return _accessionNumber; }
			set { _accessionNumber = value; }
		}

		public string PatientID
		{
			get { return _patientID; }
			set { _patientID = value; }
		}

		public string FirstName
		{
			get { return _firstName; }
			set { _firstName = value; }
		}

		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; }
		}

		public string StudyDescription
		{
			get { return _studyDescription; }
			set { _studyDescription = value; }
		}

		public StudyItem SelectedStudy
		{
			get { return _currentSelection.Item as StudyItem; }
		}

		public event EventHandler SelectedStudyChanged
		{
			add { _selectedStudyChanged += value; }
			remove { _selectedStudyChanged -= value; }
		}

		#region IApplicationComponent overrides

		public override void Start()
		{
			base.Start();

			_studyList = new TableData<StudyItem>();

			AddColumns();

			_toolSet = new ToolSet(new StudyBrowserToolExtensionPoint(), new StudyBrowserToolContext(this));
			_toolbarModel = ActionModelRoot.CreateModel(this.GetType().FullName, "dicomstudybrowser-toolbar", _toolSet.Actions);
			_contextMenuModel = ActionModelRoot.CreateModel(this.GetType().FullName, "dicomstudybrowser-contextmenu", _toolSet.Actions);
		}

		public override void Stop()
		{
			base.Stop();
		}

		#endregion

		public void Search()
		{
			Platform.CheckMemberIsSet(_studyFinder, "StudyFinder");

			QueryParameters queryParams = new QueryParameters();
			queryParams.Add("PatientsName",_lastName);
			queryParams.Add("PatientId", _patientID);
			queryParams.Add("AccessionNumber", _accessionNumber);
			queryParams.Add("StudyDescription", _studyDescription);
			
			StudyItemList studyItemList = _studyFinder.Query(queryParams);

			_studyList.Clear();

			foreach (StudyItem item in studyItemList)
				_studyList.Add(item);
		}

		public void Clear()
		{
			this.PatientID = "";
			this.FirstName = "";
			this.LastName = "";
			this.AccessionNumber = "";
			this.StudyDescription = "";
		}

		public void Open()
		{
			if (this.SelectedStudy == null)
				return;

			string studyInstanceUid = this.SelectedStudy.StudyInstanceUID;
			string label = String.Format("{0}, {1} | {2}",
				this.SelectedStudy.LastName,
				this.SelectedStudy.FirstName,
				this.SelectedStudy.PatientId);

			_studyLoader.LoadStudy(studyInstanceUid);

			ImageViewerComponent imageViewer = new ImageViewerComponent(studyInstanceUid);
			ApplicationComponent.LaunchAsWorkspace(this.Host.DesktopWindow, imageViewer, label, null);
		}

		public void ItemDoubleClick()
		{
			if (_defaultActionHandler != null)
			{
				_defaultActionHandler();
			}
		}

		public void SetSelection(ISelection selection)
		{
			if (_currentSelection != selection)
			{
				_currentSelection = selection;
				EventsHelper.Fire(_selectedStudyChanged, this, EventArgs.Empty);
			}
		}

		private void AddColumns()
		{
			_studyList.Columns.Add(
				new TableColumn<StudyItem, string>(
					"Patient ID",
					delegate(StudyItem item) { return item.PatientId; }
					));
			_studyList.Columns.Add(
				new TableColumn<StudyItem, string>(
					"Last Name",
					delegate(StudyItem item) { return item.LastName; }
					));
			_studyList.Columns.Add(
				new TableColumn<StudyItem, string>(
					"First Name",
					delegate(StudyItem item) { return item.FirstName; }
					));
			_studyList.Columns.Add(
				new TableColumn<StudyItem, string>(
					"DOB",
					delegate(StudyItem item) { return item.PatientsBirthDate; }
					));
			_studyList.Columns.Add(
				new TableColumn<StudyItem, string>(
					"Description",
					delegate(StudyItem item) { return item.StudyDescription; }
					));
		}
	}
}
