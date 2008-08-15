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
using System.Web.UI.WebControls;
using System.Collections.Generic;

using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Web.Common.Data;


namespace ClearCanvas.ImageServer.Web.Application.Pages.Queues.ArchiveQueue
{
    //
    //  Used to display the list of Archive Queue Items.
    //
    public partial class ArchiveQueueItemList : System.Web.UI.UserControl
    {
		#region Delegates
		public delegate void ArchiveQueueDataSourceCreated(ArchiveQueueDataSource theSource);
		public event ArchiveQueueDataSourceCreated DataSourceCreated;
		#endregion

        #region Private members
        // list of studies to display
        private IList<ArchiveQueueSummary> _queueItems;
        private ServerPartition _partition;
        private Unit _height;
    	private ArchiveQueueDataSource _dataSource;
        #endregion Private members

        #region Public properties

		public int ResultCount
		{
			get
			{
				if (_dataSource == null)
				{
					_dataSource = new ArchiveQueueDataSource();

					_dataSource.ArchiveQueueFoundSet += delegate(IList<ArchiveQueueSummary> newlist)
											{
												Items = newlist;
											};
					if (DataSourceCreated != null)
						DataSourceCreated(_dataSource);
					_dataSource.SelectCount();
				}
				if (_dataSource.ResultCount == 0)
				{
					if (DataSourceCreated != null)
						DataSourceCreated(_dataSource);

					_dataSource.SelectCount();
				}
				return _dataSource.ResultCount;
			}
		}

        /// <summary>
        /// Retrieve reference to the grid control being used to display the devices.
        /// </summary>
        public Web.Common.WebControls.UI.GridView ArchiveQueueGrid
        {
            get { return ArchiveQueueGridView; }
        }


        public ServerPartition Partition
        {
            set { _partition = value; }
            get { return _partition; }
        }

        /// <summary>
        /// Gets/Sets the current selected device.
        /// </summary>
        public IList<Model.ArchiveQueue> SelectedItems
        {
            get
            {
                if (Items==null || Items.Count == 0)
                    return null;

                int[] rows = ArchiveQueueGridView.SelectedIndices;
                if (rows == null || rows.Length == 0)
                    return null;

				IList<Model.ArchiveQueue> queueItems = new List<Model.ArchiveQueue>();
                for(int i=0; i<rows.Length; i++)
                {
                    queueItems.Add(Items[rows[i]].TheArchiveQueueItem);
                }

                return queueItems;
            }
        }

        /// <summary>
        /// Gets/Sets the list of devices rendered on the screen.
        /// </summary>
        public IList<ArchiveQueueSummary> Items
        {
            get
            {
                return _queueItems;
            }
            set
            {
                _queueItems = value;
            }
        }

        /// <summary>
        /// Gets/Sets the height of the study list panel
        /// </summary>
        public Unit Height
        {
            get {
                if (ContainerTable != null)
                    return ContainerTable.Height;
                else
                    return _height; 
            }
            set
            {
                _height = value;
                if (ContainerTable != null)
                    ContainerTable.Height = value;
            }
        }

        #endregion

        #region Events
        /// <summary>
        /// Defines the handler for <seealso cref="OnStudySelectionChanged"/> event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="selectedStudies"></param>
        public delegate void QueueItemSelectedEventHandler(object sender, IList<Model.ArchiveQueue> selectedStudies);

        /// <summary>
        /// Occurs when the selected device in the list is changed.
        /// </summary>
        /// <remarks>
        /// The selected device can change programmatically or by users selecting the device in the list.
        /// </remarks>
        public event QueueItemSelectedEventHandler OnQueueItemSelectionChanged;

        #endregion // Events
        
        #region protected methods


        protected void Page_Load(object sender, EventArgs e)
        {
			DataBind();
        }
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Set up the grid
            if (Height != Unit.Empty)
                ContainerTable.Height = _height;

            // The embeded grid control will show pager control if "allow paging" is set to true
            // We want to use our own pager control instead so let's hide it.
            ArchiveQueueGridView.SelectedIndexChanged += new EventHandler(ArchiveQueueGridView_SelectedIndexChanged);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

			if (Items == null)
				return;

            foreach (GridViewRow row in ArchiveQueueGridView.Rows)
            {
                if (row.RowType==DataControlRowType.DataRow)
                {
					ArchiveQueueSummary item = Items[row.RowIndex];
                    
                    if (item!=null)
                    {
                        //row.Attributes.Add("instanceuid", item.TheStudy.StudyInstanceUid);
                        //row.Attributes.Add("serverae", item.ThePartition.AeTitle);
                        //StudyController controller = new StudyController();
                        //bool deleted = controller.IsScheduledForDelete(study.TheStudy);
                        //if (deleted)
                        //    row.Attributes.Add("deleted", "true");
                        //if (study.StudyStatusEnum.Equals(StudyStatusEnum.Nearline))
                        //    row.Attributes.Add("nearline", "true");
                    }
                }
            }
        }


        protected void ArchiveQueueGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            IList<Model.ArchiveQueue> queueItems = SelectedItems;
            if (queueItems != null)
                if (OnQueueItemSelectionChanged != null)
                    OnQueueItemSelectionChanged(this, queueItems);            
        }

        protected void ArchiveQueueGridView_PageIndexChanged(object sender, EventArgs e)
        {
            DataBind();
        }

        protected void ArchiveQueueGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ArchiveQueueGridView.PageIndex = e.NewPageIndex;
            DataBind();
        }

        protected void ImageButton_Command(object sender, CommandEventArgs e)
        {

            // get the current page selected
            int intCurIndex = ArchiveQueueGridView.PageIndex;

            switch (e.CommandArgument.ToString().ToLower())
            {
                case "first":
                    ArchiveQueueGridView.PageIndex = 0;
                    break;
                case "prev":
                    ArchiveQueueGridView.PageIndex = intCurIndex - 1;
                    break;
                case "next":
                    ArchiveQueueGridView.PageIndex = intCurIndex + 1;
                    break;
                case "last":
                    ArchiveQueueGridView.PageIndex = ArchiveQueueGridView.PageCount;
                    break;
            }

            DataBind();
        }

		protected void DisposeStudyDataSource(object sender, ObjectDataSourceDisposingEventArgs e)
		{
			e.Cancel = true;
		}

		protected void GetArchiveQueueDataSource(object sender, ObjectDataSourceEventArgs e)
		{
			if (_dataSource == null)
			{
				_dataSource = new ArchiveQueueDataSource();

				_dataSource.ArchiveQueueFoundSet += delegate(IList<ArchiveQueueSummary> newlist)
										{
											Items = newlist;
										};
			}

			e.ObjectInstance = _dataSource;

			if (DataSourceCreated != null)
				DataSourceCreated(_dataSource);

		}
        #endregion

        #region public methods
        /// <summary>
        /// Binds the list to the control.
        /// </summary>
        /// <remarks>
        /// This method must be called after setting <seeaslo cref="Study"/> to update the grid with the list.
        /// </remarks>
        public override void DataBind()
        {
            ArchiveQueueGridView.PagerSettings.Visible = false;
        }

        #endregion // public methods

    }

}