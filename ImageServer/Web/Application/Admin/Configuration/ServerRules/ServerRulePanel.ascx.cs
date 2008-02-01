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
using System.Web.UI;
using System.Web.UI.WebControls;
using ClearCanvas.ImageServer.Model;
using ClearCanvas.ImageServer.Model.EntityBrokers;
using ClearCanvas.ImageServer.Web.Common.Data;

namespace ClearCanvas.ImageServer.Web.Application.Admin.Configuration.ServerRules
{
    public partial class ServerRulePanel : UserControl
    {
        private readonly ServerRuleController _controller = new ServerRuleController();
        private ServerPartition _partition;
        private ServerRulePage _enclosingPage;

        public ServerPartition ServerPartition
        {
            get { return _partition; }
            set { _partition = value; }
        }

        public ServerRulePage EnclosingPage
        {
            get { return _enclosingPage; }
            set { _enclosingPage = value; }
        }

        public void LoadRules()
        {
            ServerRuleSelectCriteria criteria = new ServerRuleSelectCriteria();

            // only query for device in this partition
            criteria.ServerPartitionKey.EqualTo(ServerPartition.GetKey());

            if (!String.IsNullOrEmpty(RuleApplyTimeDropDownList.Text))
            {
                if (!RuleApplyTimeDropDownList.Text.Equals("All"))
                {
                    ServerRuleApplyTimeEnum en = new ServerRuleApplyTimeEnum();
                    en.SetEnum(short.Parse(RuleApplyTimeDropDownList.SelectedItem.Value));
                    criteria.ServerRuleApplyTimeEnum.EqualTo(en);
                }
            }
            if (!String.IsNullOrEmpty(RuleTypeDropDownList.Text))
            {
                if (!RuleTypeDropDownList.Text.Equals("All"))
                {
                    ServerRuleTypeEnum en = new ServerRuleTypeEnum();
                    en.SetEnum(short.Parse(RuleTypeDropDownList.SelectedItem.Value));
                    criteria.ServerRuleTypeEnum.EqualTo(en);
                }
            }

            if (EnabledOnlyFilter.Checked)
            {
                criteria.Enabled.EqualTo(true);
            }

            if (DefaultOnlyFilter.Checked)
            {
                criteria.DefaultRule.EqualTo(true);
            }

            ServerRuleGridViewControl.ServerRules = _controller.GetServerRules(criteria);
            ServerRuleGridViewControl.DataBind();
        }

        /// <summary>
        /// Updates the rules list window in the panel.
        /// </summary>
        /// <remarks>
        /// This method should only be called when necessary as the information in the list window needs to be transmitted back to the client.
        /// If the list is not changed, call <seealso cref="LoadRules()"/> instead.
        /// </remarks>
        public void UpdateUI()
        {
            LoadRules();

            // UpdatePanel UpdateMode must be set to "conditional"
            // Calling UpdatePanel.Update() will force the client to refresh the screen
            ServerRuleUpdatePanel.Update();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ServerRuleGridViewControl.ServerRulePanel = this;

            // setup child controls
            GridPager.ItemName = "Rule";
            GridPager.PuralItemName = "Rules";
            GridPager.Grid = ServerRuleGridViewControl.TheGrid;

            GridPager.GetRecordCountMethod = delegate { return ServerRuleGridViewControl.ServerRules.Count; };

            int prevSelectIndex = RuleApplyTimeDropDownList.SelectedIndex;
            RuleApplyTimeDropDownList.Items.Clear();
            RuleApplyTimeDropDownList.Items.Add(new ListItem("All"));
            foreach (ServerRuleApplyTimeEnum applyTimeEnum in ServerRuleApplyTimeEnum.GetAll())
            {
                RuleApplyTimeDropDownList.Items.Add(
                    new ListItem(applyTimeEnum.Description, applyTimeEnum.Enum.ToString()));
            }
            RuleApplyTimeDropDownList.SelectedIndex = prevSelectIndex;


            prevSelectIndex = RuleTypeDropDownList.SelectedIndex;
            RuleTypeDropDownList.Items.Clear();
            RuleTypeDropDownList.Items.Add(new ListItem("All"));
            foreach (ServerRuleTypeEnum typeEnum in ServerRuleTypeEnum.GetAll())
            {
                RuleTypeDropDownList.Items.Add(new ListItem(typeEnum.Description, typeEnum.Enum.ToString()));
            }
            RuleTypeDropDownList.SelectedIndex = prevSelectIndex;


            LoadRules();
        }


        protected override void OnPreRender(EventArgs e)
        {
            ServerRule dev = ServerRuleGridViewControl.SelectedRule;
            if (dev == null)
            {
                // no rule being selected

                EditButton.Enabled = false;
                EditButton.ImageUrl = "~/images/icons/EditDisabled.png";

                DeleteButton.Enabled = false;
                DeleteButton.ImageUrl = "~/images/icons/DeleteDisabled.png";
            }
            else
            {
                EditButton.Enabled = true;
                EditButton.ImageUrl = "~/images/icons/EditEnabled.png";

                if (dev.DefaultRule)
                {
                    DeleteButton.Enabled = false;
                    DeleteButton.ImageUrl = "~/images/icons/DeleteDisabled.png";
                }
                else
                {
                    DeleteButton.Enabled = true;
                    DeleteButton.ImageUrl = "~/images/icons/DeleteEnabled.png";
                }
            }

            base.OnPreRender(e);
        }

        protected void AddButton_Click(object sender, ImageClickEventArgs e)
        {
            EnclosingPage.OnAddRule(null, ServerPartition);
        }

        protected void EditButton_Click(object sender, ImageClickEventArgs e)
        {
            if (ServerRuleGridViewControl.SelectedRule != null)
                EnclosingPage.OnEditRule(ServerRuleGridViewControl.SelectedRule, ServerPartition);
        }

        protected void DeleteButton_Click(object sender, ImageClickEventArgs e)
        {
            if (ServerRuleGridViewControl.SelectedRule != null)
                EnclosingPage.OnDeleteRule(ServerRuleGridViewControl.SelectedRule, ServerPartition);
        }

        protected void RefreshButton_Click(object sender, ImageClickEventArgs e)
        {
            RuleApplyTimeDropDownList.SelectedIndex = 0;
            RuleTypeDropDownList.SelectedIndex = 0;
            EnabledOnlyFilter.Checked = false;
            DefaultOnlyFilter.Checked = false;

            LoadRules();
        }

        protected void FilterButton_Click(object sender, ImageClickEventArgs e)
        {
            LoadRules();
        }

        public void OnRowSelected(int index)
        {
        }
    }
}