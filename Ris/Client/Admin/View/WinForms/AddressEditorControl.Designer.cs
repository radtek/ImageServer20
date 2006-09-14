namespace ClearCanvas.Ris.Client.Admin.View.WinForms
{
    partial class AddressEditorControl
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
            this._postalCode = new ClearCanvas.Controls.WinForms.TextField();
            this._validFrom = new ClearCanvas.Controls.WinForms.DateTimeField();
            this._validUntil = new ClearCanvas.Controls.WinForms.DateTimeField();
            this._type = new ClearCanvas.Controls.WinForms.ComboBoxField();
            this._city = new ClearCanvas.Controls.WinForms.TextField();
            this._street = new ClearCanvas.Controls.WinForms.TextField();
            this._province = new ClearCanvas.Controls.WinForms.ComboBoxField();
            this._country = new ClearCanvas.Controls.WinForms.ComboBoxField();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _acceptButton
            // 
            this._acceptButton.Location = new System.Drawing.Point(523, 4);
            this._acceptButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._acceptButton.Name = "_acceptButton";
            this._acceptButton.Size = new System.Drawing.Size(100, 28);
            this._acceptButton.TabIndex = 0;
            this._acceptButton.Text = "Accept";
            this._acceptButton.UseVisualStyleBackColor = true;
            this._acceptButton.Click += new System.EventHandler(this._acceptButton_Click);
            // 
            // _cancelButton
            // 
            this._cancelButton.Location = new System.Drawing.Point(631, 4);
            this._cancelButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(100, 28);
            this._cancelButton.TabIndex = 1;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
            // 
            // _postalCode
            // 
            this._postalCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this._postalCode.LabelText = "Postal Code";
            this._postalCode.Location = new System.Drawing.Point(373, 110);
            this._postalCode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._postalCode.Name = "_postalCode";
            this._postalCode.Size = new System.Drawing.Size(179, 50);
            this._postalCode.TabIndex = 6;
            this._postalCode.Value = null;
            // 
            // _validFrom
            // 
            this._validFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this._validFrom.LabelText = "Valid From";
            this._validFrom.Location = new System.Drawing.Point(373, 2);
            this._validFrom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._validFrom.Name = "_validFrom";
            this._validFrom.Nullable = true;
            this._validFrom.Size = new System.Drawing.Size(179, 50);
            this._validFrom.TabIndex = 1;
            this._validFrom.Value = new System.DateTime(2006, 7, 26, 16, 37, 8, 953);
            // 
            // _validUntil
            // 
            this._validUntil.Dock = System.Windows.Forms.DockStyle.Fill;
            this._validUntil.LabelText = "Valid Until";
            this._validUntil.Location = new System.Drawing.Point(558, 2);
            this._validUntil.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._validUntil.Name = "_validUntil";
            this._validUntil.Nullable = true;
            this._validUntil.Size = new System.Drawing.Size(182, 50);
            this._validUntil.TabIndex = 2;
            this._validUntil.Value = new System.DateTime(2006, 7, 26, 16, 37, 6, 765);
            // 
            // _type
            // 
            this._type.DataSource = null;
            this._type.Dock = System.Windows.Forms.DockStyle.Fill;
            this._type.LabelText = "Type";
            this._type.Location = new System.Drawing.Point(3, 2);
            this._type.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._type.Name = "_type";
            this._type.Size = new System.Drawing.Size(179, 50);
            this._type.TabIndex = 0;
            this._type.Value = null;
            // 
            // _city
            // 
            this._city.Dock = System.Windows.Forms.DockStyle.Fill;
            this._city.LabelText = "City";
            this._city.Location = new System.Drawing.Point(3, 110);
            this._city.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._city.Name = "_city";
            this._city.Size = new System.Drawing.Size(179, 50);
            this._city.TabIndex = 4;
            this._city.Value = null;
            // 
            // _street
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._street, 4);
            this._street.Dock = System.Windows.Forms.DockStyle.Fill;
            this._street.LabelText = "Street";
            this._street.Location = new System.Drawing.Point(3, 56);
            this._street.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._street.Name = "_street";
            this._street.Size = new System.Drawing.Size(737, 50);
            this._street.TabIndex = 3;
            this._street.Value = null;
            // 
            // _province
            // 
            this._province.DataSource = null;
            this._province.Dock = System.Windows.Forms.DockStyle.Fill;
            this._province.LabelText = "Province";
            this._province.Location = new System.Drawing.Point(188, 110);
            this._province.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._province.Name = "_province";
            this._province.Size = new System.Drawing.Size(179, 50);
            this._province.TabIndex = 5;
            this._province.Value = null;
            // 
            // _country
            // 
            this._country.DataSource = null;
            this._country.Dock = System.Windows.Forms.DockStyle.Fill;
            this._country.LabelText = "Country";
            this._country.Location = new System.Drawing.Point(558, 110);
            this._country.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._country.Name = "_country";
            this._country.Size = new System.Drawing.Size(182, 50);
            this._country.TabIndex = 7;
            this._country.Value = null;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this._type, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._country, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this._street, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._validUntil, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this._city, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._validFrom, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this._postalCode, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this._province, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(743, 210);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 4);
            this.flowLayoutPanel1.Controls.Add(this._cancelButton);
            this.flowLayoutPanel1.Controls.Add(this._acceptButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 166);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(735, 40);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // AddressEditorControl
            // 
            this.AcceptButton = this._acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AddressEditorControl";
            this.Size = new System.Drawing.Size(743, 210);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ClearCanvas.Controls.WinForms.TextField _street;
        private ClearCanvas.Controls.WinForms.TextField _city;
        private ClearCanvas.Controls.WinForms.ComboBoxField _type;
        private System.Windows.Forms.Button _acceptButton;
        private System.Windows.Forms.Button _cancelButton;
        private ClearCanvas.Controls.WinForms.DateTimeField _validUntil;
        private ClearCanvas.Controls.WinForms.DateTimeField _validFrom;
        private ClearCanvas.Controls.WinForms.TextField _postalCode;
        private ClearCanvas.Controls.WinForms.ComboBoxField _province;
        private ClearCanvas.Controls.WinForms.ComboBoxField _country;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
