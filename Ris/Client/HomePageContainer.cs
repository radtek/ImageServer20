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
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Tools;
using ClearCanvas.Desktop.Actions;
using System.Collections;

namespace ClearCanvas.Ris.Client
{
    public interface IPreviewComponent : IApplicationComponent
    {
        void SetUrl(string url);
    }

	[ExtensionPoint]
	public class HomePageToolExtensionPoint : ExtensionPoint<ITool>
	{
	}

	public interface IHomePageToolContext : IToolContext
	{
		IEnumerable FolderSystems { get; }
		IFolderSystem SelectedFolderSystem { get; }

		IEnumerable Folders { get; }
		IFolder SelectedFolder { get; }

		IDesktopWindow DesktopWindow { get; }
	}

    public class HomePageContainer : SplitComponentContainer, ISearchDataHandler
    {
        #region IFolderExplorerToolContext implementation

        class FolderExplorerToolContext : ToolContext, IFolderExplorerToolContext
        {
            private readonly HomePageContainer _component;

            public FolderExplorerToolContext(HomePageContainer component)
            {
                _component = component;
            }

            #region IFolderExplorerToolContext Members

            public IDesktopWindow DesktopWindow
            {
                get { return _component.Host.DesktopWindow; }
            }

            public IFolder SelectedFolder
            {
                get { return (IFolder) _component.SelectedFolderExplorer.SelectedFolder.Item; }
                set { _component.SelectedFolderExplorer.SelectedFolder = new Selection(value); }
            }

            public ISelection SelectedItems
            {
                get { return _component._folderContentComponent.SelectedItems; }
            }

            public void RegisterSearchDataHandler(ISearchDataHandler handler)
            {
                _component.RegisterSearchDataHandler(handler);
            }

            #endregion
        }

        #endregion

		#region IHomePageToolContext implementation

		class HomePageToolContext : ToolContext, IHomePageToolContext
		{
			private readonly HomePageContainer _component;

			public HomePageToolContext(HomePageContainer component)
			{
				_component = component;
			}

			#region IHomePageToolContext Members

			public IEnumerable FolderSystems
			{
				get { return _component._folderExplorerComponents.Values; }
			}

			public IFolderSystem SelectedFolderSystem
			{
				get { return _component.SelectedFolderExplorer.FolderSystem; }
			}

			public IEnumerable Folders
			{
				get { return _component.SelectedFolderExplorer.FolderSystem.Folders; }
			}

			public IFolder SelectedFolder
			{
				get { return (IFolder)_component.SelectedFolderExplorer.SelectedFolder.Item; }
			}

			public IDesktopWindow DesktopWindow
			{
				get { return _component.Host.DesktopWindow; }
			}

			#endregion
		}

		#endregion

		#region Search related

		private ISearchDataHandler _searchDataHandler;

        public void RegisterSearchDataHandler(ISearchDataHandler handler)
        {
            _searchDataHandler = handler;
        }

        public SearchData SearchData
        {
            set
            {
                if (_searchDataHandler != null)
                    _searchDataHandler.SearchData = value;
            }
        }

        #endregion

        private readonly Dictionary<IFolderSystem, FolderExplorerComponent> _folderExplorerComponents;
        private readonly FolderContentsComponent _folderContentComponent;
        private readonly IPreviewComponent _previewComponent;
        private readonly StackTabComponentContainer _stackContainers;

        private FolderExplorerComponent _selectedFolderExplorer;
        private readonly ToolSet _folderExplorers;
    	private readonly ToolSet _homePageTools;

        public HomePageContainer(IExtensionPoint folderExplorerExtensionPoint, IPreviewComponent preview)
            : base(Desktop.SplitOrientation.Vertical)
        {
            _folderExplorerComponents = new Dictionary<IFolderSystem, FolderExplorerComponent>();
            _folderContentComponent = new FolderContentsComponent();
            _previewComponent = preview;

            _folderExplorers = new ToolSet(folderExplorerExtensionPoint, new FolderExplorerToolContext(this));
			_homePageTools = new ToolSet(new HomePageToolExtensionPoint(), new HomePageToolContext(this));

            // Construct the explorer component and place each into a stack tab
            _stackContainers = new StackTabComponentContainer(
				HomePageSettings.Default.ShowMultipleFolderSystems ? StackStyle.ShowMultiple : StackStyle.ShowOneOnly,
				HomePageSettings.Default.OpenAllFolderSystemsInitially);

        	_stackContainers.ContextMenuModel = this.HomePageContextMenuModel;
        	_stackContainers.ToolbarModel = this.HomePageToolbarModel;
            _stackContainers.CurrentPageChanged += OnSelectedFolderSystemChanged;

            List<IFolderSystem> folderSystems = CollectionUtils.Map<ITool, IFolderSystem, List<IFolderSystem>>(_folderExplorers.Tools,
                delegate(ITool tool)
                {
                    FolderExplorerToolBase folderExplorerTool = (FolderExplorerToolBase) tool;
                    return folderExplorerTool.FolderSystem;
                });

            // Order the Folder Systems
            folderSystems = FolderExplorerComponentSettings.Default.OrderFolderSystems(folderSystems);

            CollectionUtils.ForEach(folderSystems,
                delegate(IFolderSystem folderSystem)
                    {
                        FolderExplorerComponent component = new FolderExplorerComponent(folderSystem);
                        component.SelectedFolderChanged += OnSelectedFolderChanged;
                        // TODO: what does this suppress??
                        //component.SuppressSelectionChanged += _folderContentComponent.OnSuppressSelectionChanged;

                        _folderExplorerComponents.Add(folderSystem, component);

                    	StackTabPage thisPage = new StackTabPage(
                    		folderSystem.Title,
                    		component,
                    		string.Empty,
                    		folderSystem.Title,
                    		string.Empty,
                    		folderSystem.TitleIcon,
							folderSystem.ResourceResolver);

                        _stackContainers.Pages.Add(thisPage);

                    	folderSystem.TextChanged += delegate { thisPage.SetTitle(string.Empty, folderSystem.Title, string.Empty); };
						folderSystem.IconChanged += delegate { thisPage.IconSet = folderSystem.TitleIcon; };
					});

            // Construct the home page
            SplitComponentContainer contentAndPreview = new SplitComponentContainer(
                new SplitPane("Folder Contents", _folderContentComponent, 0.4f),
                new SplitPane("Content Preview", _previewComponent, 0.6f),
                SplitOrientation.Vertical);

            this.Pane1 = new SplitPane("Folders", _stackContainers, 0.2f);
            this.Pane2 = new SplitPane("Contents", contentAndPreview, 0.8f);
        }

        public override void Start()
        {
            base.Start();

            CollectionUtils.ForEach(_folderExplorerComponents.Keys,
                delegate(IFolderSystem folderSystem)
                    {
                        DocumentManager.RegisterFolderSystem(folderSystem);
                    });
        }

        public override void Stop()
        {
            CollectionUtils.ForEach(_folderExplorerComponents.Keys,
                delegate(IFolderSystem folderSystem)
                {
                    DocumentManager.UnregisterFolderSystem(folderSystem);
                });

            base.Stop();
        }

        public FolderExplorerComponent SelectedFolderExplorer
        {
            get { return _selectedFolderExplorer; }
            set
            {
                _selectedFolderExplorer = value;

                if (_selectedFolderExplorer == null)
                {
                    _folderContentComponent.FolderSystem = null;
                    _previewComponent.SetUrl(null);
                    _folderContentComponent.SelectedFolder = null;
                }
                else
                {
                    _folderContentComponent.FolderSystem = _selectedFolderExplorer.FolderSystem;
                    _previewComponent.SetUrl(_selectedFolderExplorer.FolderSystem.PreviewUrl);

                    IFolder selectedFolder = ((IFolder)_selectedFolderExplorer.SelectedFolder.Item);
                    _folderContentComponent.SelectedFolder = selectedFolder;

					if(_folderContentComponent.SelectedFolder != null)
						_folderContentComponent.SelectedFolder.Refresh();
                }
            }
        }

        private void OnSelectedFolderSystemChanged(object sender, EventArgs e)
        {
			ChangeFolderExplorer(
				this.SelectedFolderExplorer,
				(FolderExplorerComponent)_stackContainers.CurrentPage.Component);
		}

        private void OnSelectedFolderChanged(object sender, EventArgs e)
        {
			ChangeFolderExplorer(
				this.SelectedFolderExplorer,
				(FolderExplorerComponent)sender);
		}

		private void ChangeFolderExplorer(FolderExplorerComponent prevComponent, FolderExplorerComponent newComponent)
		{
			IFolder previousFolderSelection = _folderContentComponent.SelectedFolder;
			if (newComponent != prevComponent)
				this.SelectedFolderExplorer = newComponent;

			IFolder newSelectedFolder = ((IFolder)_selectedFolderExplorer.SelectedFolder.Item);
			if (newSelectedFolder == null)
				newSelectedFolder = (IFolder) _selectedFolderExplorer.FolderTree.Items[0];

			if (newSelectedFolder != previousFolderSelection)
			{
				_folderContentComponent.SelectedFolder = newSelectedFolder;
				_selectedFolderExplorer.SelectedFolder = new Selection(newSelectedFolder);
			}
		}

        public FolderContentsComponent ContentsComponent
        {
            get { return _folderContentComponent; }
        }

		public ActionModelRoot HomePageContextMenuModel
		{
			get
			{
				return ActionModelRoot.CreateModel(this.GetType().FullName, "homepage-contextmenu", _homePageTools.Actions);
			}
		}

		public ActionModelRoot HomePageToolbarModel
		{
			get
			{
				return ActionModelRoot.CreateModel(this.GetType().FullName, "homepage-toolbar", _homePageTools.Actions);
			}
		}
	}
}
