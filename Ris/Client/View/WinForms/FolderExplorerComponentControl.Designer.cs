#region License

// Copyright (c) 2006-2007, ClearCanvas Inc.
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

using System.Drawing;

namespace ClearCanvas.Ris.Client.View.WinForms
{
    partial class FolderExplorerComponentControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            ClearCanvas.Desktop.Selection selection1 = new ClearCanvas.Desktop.Selection();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FolderExplorerComponentControl));
            ClearCanvas.Desktop.Selection selection2 = new ClearCanvas.Desktop.Selection();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._folderTreeView = new ClearCanvas.Desktop.View.WinForms.BindingTreeView();
            this._folderTreeImageList = new System.Windows.Forms.ImageList(this.components);
            this._folderContentsTableView = new ClearCanvas.Desktop.View.WinForms.TableView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._folderTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._folderContentsTableView);
            this.splitContainer1.Size = new System.Drawing.Size(529, 493);
            this.splitContainer1.SplitterDistance = 176;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 0;
            // 
            // _folderTreeView
            // 
            this._folderTreeView.AllowDrop = true;
            this._folderTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._folderTreeView.ImageList = this._folderTreeImageList;
            this._folderTreeView.Location = new System.Drawing.Point(0, 0);
            this._folderTreeView.Margin = new System.Windows.Forms.Padding(2);
            this._folderTreeView.MenuModel = null;
            this._folderTreeView.Name = "_folderTreeView";
            this._folderTreeView.Selection = selection1;
            this._folderTreeView.ShowLines = false;
            this._folderTreeView.ShowRootLines = false;
            this._folderTreeView.Size = new System.Drawing.Size(176, 493);
            this._folderTreeView.TabIndex = 1;
            this._folderTreeView.ToolbarModel = null;
            this._folderTreeView.ToolStripItemDisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this._folderTreeView.ToolStripRightToLeft = System.Windows.Forms.RightToLeft.No;
            this._folderTreeView.Tree = null;
            // 
            // _folderTreeImageList
            // 
            //this._folderTreeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_folderTreeImageList.ImageStream")));
            //this._folderTreeImageList.TransparentColor = System.Drawing.Color.Transparent;
//            this._folderTreeImageList.Images.SetKeyName(0, "FolderClosedMedium.png");
            //this._folderTreeImageList.Images.SetKeyName(1, "FolderOpenMedium.png");
            // 
            // _folderContentsTableView
            // 
            this._folderContentsTableView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._folderContentsTableView.Location = new System.Drawing.Point(0, 0);
            this._folderContentsTableView.MenuModel = null;
            this._folderContentsTableView.MultiSelect = false;
            this._folderContentsTableView.Name = "_folderContentsTableView";
            this._folderContentsTableView.ReadOnly = false;
            this._folderContentsTableView.Selection = selection2;
            this._folderContentsTableView.Size = new System.Drawing.Size(350, 493);
            this._folderContentsTableView.SortButtonVisible = true;
            this._folderContentsTableView.TabIndex = 0;
            this._folderContentsTableView.Table = null;
            this._folderContentsTableView.ToolbarModel = null;
            this._folderContentsTableView.ToolStripItemDisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this._folderContentsTableView.ToolStripRightToLeft = System.Windows.Forms.RightToLeft.No;
            this._folderContentsTableView.ItemDrag += new System.EventHandler<System.Windows.Forms.ItemDragEventArgs>(this._folderContentsTableView_ItemDrag);
            this._folderContentsTableView.ItemDoubleClicked += new System.EventHandler(this._folderContentsTableView_ItemDoubleClicked);
            // 
            // FolderExplorerComponentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FolderExplorerComponentControl";
            this.Size = new System.Drawing.Size(529, 493);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ClearCanvas.Desktop.View.WinForms.BindingTreeView _folderTreeView;
        private ClearCanvas.Desktop.View.WinForms.TableView _folderContentsTableView;
        private System.Windows.Forms.ImageList _folderTreeImageList;
    }
}
