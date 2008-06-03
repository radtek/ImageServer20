using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ClearCanvas.Common;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.ImageServer.Enterprise;
using ClearCanvas.ImageServer.Model.EntityBrokers;

namespace ClearCanvas.ImageServer.TestApp
{
    public partial class TestEditStudyForm : Form
    {
        public TestEditStudyForm()
        {
            InitializeComponent();

        }

        private void TestEditStudyForm_Load(object sender, EventArgs e)
        {
            this.studyStorageTableAdapter.Fill(this.imageServerDataSet.StudyStorage);
            this.studyTableAdapter.Fill(this.imageServerDataSet.Study);

            dataGridView1.DataSource = this.imageServerDataSet;
            dataGridView1.DataMember = "Study";
            dataGridView1.Update();

        }

        private void Apply_Click(object sender, EventArgs e)
        {
            DataRowView view = dataGridView1.SelectedRows[0].DataBoundItem as DataRowView;
            if (view != null)
            {
                Guid guid = (Guid) view.Row["GUID"];

                IPersistentStore store = PersistentStoreRegistry.GetDefaultStore();

                using (IUpdateContext ctx = store.OpenUpdateContext(UpdateContextSyncMode.Flush))
                {
                    IStudyEntityBroker studyBroker = ctx.GetBroker<IStudyEntityBroker>();
                    StudySelectCriteria criteria = new StudySelectCriteria();
                    ServerEntityKey key = new ServerEntityKey("Study", guid);
                    Model.Study study = studyBroker.Load(key);

                    IStudyStorageEntityBroker storageBroker = ctx.GetBroker<IStudyStorageEntityBroker>();
                    StudyStorageSelectCriteria parms = new StudyStorageSelectCriteria();
                    parms.ServerPartitionKey.EqualTo(study.ServerPartitionKey);
                    parms.StudyInstanceUid.EqualTo(study.StudyInstanceUid);

                    Model.StudyStorage storage = storageBroker.Find(parms)[0];


                    IWorkQueueEntityBroker workQueueBroker = ctx.GetBroker<IWorkQueueEntityBroker>();
                    WorkQueueUpdateColumns columns = new WorkQueueUpdateColumns();
                    columns.ServerPartitionKey = study.ServerPartitionKey;
                    columns.StudyStorageKey = storage.GetKey();
                    columns.ExpirationTime = DateTime.Now.AddHours(1);
                    columns.ScheduledTime = DateTime.Now;
                    columns.InsertTime = DateTime.Now;
                    columns.WorkQueuePriorityEnum = Model.WorkQueuePriorityEnum.Medium;
                    columns.WorkQueueStatusEnum = Model.WorkQueueStatusEnum.Pending;
                    columns.WorkQueueTypeEnum = Model.WorkQueueTypeEnum.WebEditStudy;

                    XmlDocument doc = new XmlDocument();
                    doc.Load(new StringReader(textBox1.Text));

                    columns.Data = doc;

                    workQueueBroker.Insert(columns);

                    ctx.Commit();
                }
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            Guid guid = Guid.Empty;
            if (dataGridView1.SelectedRows != null)
            {
                DataRowView view = dataGridView1.SelectedRows[0].DataBoundItem as DataRowView;
                if (view!=null)
                {
                    guid = (Guid) view["GUID"];
                    Trace.WriteLine(guid);
                }
                
            }
            this.studyStorageTableAdapter.Fill(this.imageServerDataSet.StudyStorage);
            this.studyTableAdapter.Fill(this.imageServerDataSet.Study);

            if (guid!=Guid.Empty)
            {
                int index = studyBindingSource.Find("GUID", guid);
                Trace.WriteLine(index);
                this.studyBindingSource.Position = index;
                dataGridView1.Rows[index].Selected = true;
            }
            
        }

    }
}