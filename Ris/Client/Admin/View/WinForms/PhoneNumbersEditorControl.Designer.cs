namespace ClearCanvas.Ris.Client.Admin.View.WinForms
{
    partial class PhoneNumbersEditorControl
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
            this._acceptButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this._number = new ClearCanvas.Controls.WinForms.TextField();
            this._countryCode = new ClearCanvas.Controls.WinForms.TextField();
            this._extension = new ClearCanvas.Controls.WinForms.TextField();
            this._areaCode = new ClearCanvas.Controls.WinForms.TextField();
            this._use = new ClearCanvas.Controls.WinForms.ComboBoxField();
            this._equipment = new ClearCanvas.Controls.WinForms.ComboBoxField();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _acceptButton
            // 
            this._acceptButton.Location = new System.Drawing.Point(247, 3);
            this._acceptButton.Name = "_acceptButton";
            this._acceptButton.Size = new System.Drawing.Size(75, 23);
            this._acceptButton.TabIndex = 0;
            this._acceptButton.Text = "Accept";
            this._acceptButton.UseVisualStyleBackColor = true;
            this._acceptButton.Click += new System.EventHandler(this._acceptButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.Location = new System.Drawing.Point(328, 3);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 1;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this._number, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this._countryCode, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._extension, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this._areaCode, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(412, 132);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 4);
            this.flowLayoutPanel1.Controls.Add(this._cancelButton);
            this.flowLayoutPanel1.Controls.Add(this._acceptButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 99);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(406, 30);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel2, 4);
            this.flowLayoutPanel2.Controls.Add(this._use);
            this.flowLayoutPanel2.Controls.Add(this._equipment);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(406, 45);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // _number
            // 
            this._number.Dock = System.Windows.Forms.DockStyle.Fill;
            this._number.LabelText = "Number";
            this._number.Location = new System.Drawing.Point(166, 53);
            this._number.Margin = new System.Windows.Forms.Padding(2);
            this._number.Name = "_number";
            this._number.Size = new System.Drawing.Size(160, 41);
            this._number.TabIndex = 3;
            this._number.Value = null;
            // 
            // _countryCode
            // 
            this._countryCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this._countryCode.LabelText = "Country Code";
            this._countryCode.Location = new System.Drawing.Point(2, 53);
            this._countryCode.Margin = new System.Windows.Forms.Padding(2);
            this._countryCode.Name = "_countryCode";
            this._countryCode.Size = new System.Drawing.Size(78, 41);
            this._countryCode.TabIndex = 1;
            this._countryCode.Value = null;
            // 
            // _extension
            // 
            this._extension.Dock = System.Windows.Forms.DockStyle.Fill;
            this._extension.LabelText = "Extension";
            this._extension.Location = new System.Drawing.Point(330, 53);
            this._extension.Margin = new System.Windows.Forms.Padding(2);
            this._extension.Name = "_extension";
            this._extension.Size = new System.Drawing.Size(80, 41);
            this._extension.TabIndex = 4;
            this._extension.Value = null;
            // 
            // _areaCode
            // 
            this._areaCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this._areaCode.LabelText = "Area Code";
            this._areaCode.Location = new System.Drawing.Point(84, 53);
            this._areaCode.Margin = new System.Windows.Forms.Padding(2);
            this._areaCode.Name = "_areaCode";
            this._areaCode.Size = new System.Drawing.Size(78, 41);
            this._areaCode.TabIndex = 2;
            this._areaCode.Value = null;
            // 
            // _use
            // 
            this._use.DataSource = null;
            this._use.LabelText = "Use";
            this._use.Location = new System.Drawing.Point(2, 2);
            this._use.Margin = new System.Windows.Forms.Padding(2);
            this._use.Name = "_use";
            this._use.Size = new System.Drawing.Size(75, 41);
            this._use.TabIndex = 0;
            this._use.Value = null;
            // 
            // _equipment
            // 
            this._equipment.DataSource = null;
            this._equipment.LabelText = "Equipment";
            this._equipment.Location = new System.Drawing.Point(81, 2);
            this._equipment.Margin = new System.Windows.Forms.Padding(2);
            this._equipment.Name = "_equipment";
            this._equipment.Size = new System.Drawing.Size(105, 41);
            this._equipment.TabIndex = 1;
            this._equipment.Value = null;
            // 
            // PhoneNumbersEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PhoneNumbersEditorControl";
            this.Size = new System.Drawing.Size(412, 132);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ClearCanvas.Controls.WinForms.ComboBoxField _equipment;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private ClearCanvas.Controls.WinForms.ComboBoxField _use;
        private ClearCanvas.Controls.WinForms.TextField _number;
        private ClearCanvas.Controls.WinForms.TextField _countryCode;
        private ClearCanvas.Controls.WinForms.TextField _extension;
        private ClearCanvas.Controls.WinForms.TextField _areaCode;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    }
}
