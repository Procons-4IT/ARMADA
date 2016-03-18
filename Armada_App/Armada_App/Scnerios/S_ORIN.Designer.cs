namespace Armada_App
{
    partial class S_ORIN
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(S_ORIN));
            this.dgv_S_ORIN = new System.Windows.Forms.DataGridView();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.DocEntry = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Scenario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransactionType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Source_Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShopID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCancel = new System.Windows.Forms.Button();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.scHeader = new System.Windows.Forms.SplitContainer();
            this.ToDate = new System.Windows.Forms.DateTimePicker();
            this.Fromdate = new System.Windows.Forms.DateTimePicker();
            this.lblFromRequestDate = new System.Windows.Forms.Label();
            this.chkShopID = new System.Windows.Forms.CheckedListBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.cmbCompany = new System.Windows.Forms.ComboBox();
            this.lblCountry = new System.Windows.Forms.Label();
            this.flpImages = new System.Windows.Forms.FlowLayoutPanel();
            this.scDetail = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.btnSync = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_S_ORIN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scHeader)).BeginInit();
            this.scHeader.Panel1.SuspendLayout();
            this.scHeader.Panel2.SuspendLayout();
            this.scHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scDetail)).BeginInit();
            this.scDetail.Panel1.SuspendLayout();
            this.scDetail.Panel2.SuspendLayout();
            this.scDetail.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_S_ORIN
            // 
            this.dgv_S_ORIN.AllowUserToAddRows = false;
            this.dgv_S_ORIN.AllowUserToDeleteRows = false;
            this.dgv_S_ORIN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_S_ORIN.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Image,
            this.DocEntry,
            this.Scenario,
            this.TransactionType,
            this.Source_Key,
            this.ShopID,
            this.Remarks});
            this.dgv_S_ORIN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_S_ORIN.Location = new System.Drawing.Point(0, 0);
            this.dgv_S_ORIN.Margin = new System.Windows.Forms.Padding(4);
            this.dgv_S_ORIN.Name = "dgv_S_ORIN";
            this.dgv_S_ORIN.ReadOnly = true;
            this.dgv_S_ORIN.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv_S_ORIN.Size = new System.Drawing.Size(894, 295);
            this.dgv_S_ORIN.TabIndex = 0;
            // 
            // Image
            // 
            this.Image.DataPropertyName = "Image";
            this.Image.HeaderText = "Image";
            this.Image.Name = "Image";
            this.Image.ReadOnly = true;
            // 
            // DocEntry
            // 
            this.DocEntry.DataPropertyName = "DocEntry";
            this.DocEntry.HeaderText = "DocEntry";
            this.DocEntry.Name = "DocEntry";
            this.DocEntry.ReadOnly = true;
            // 
            // Scenario
            // 
            this.Scenario.DataPropertyName = "Scenario";
            this.Scenario.HeaderText = "Scenario";
            this.Scenario.Name = "Scenario";
            this.Scenario.ReadOnly = true;
            // 
            // TransactionType
            // 
            this.TransactionType.DataPropertyName = "TransType";
            this.TransactionType.HeaderText = "TransactionType";
            this.TransactionType.Name = "TransactionType";
            this.TransactionType.ReadOnly = true;
            this.TransactionType.Width = 120;
            // 
            // Source_Key
            // 
            this.Source_Key.DataPropertyName = "Key";
            this.Source_Key.HeaderText = "Source_Key";
            this.Source_Key.Name = "Source_Key";
            this.Source_Key.ReadOnly = true;
            // 
            // ShopID
            // 
            this.ShopID.DataPropertyName = "ShopID";
            this.ShopID.HeaderText = "Shop ID";
            this.ShopID.Name = "ShopID";
            this.ShopID.ReadOnly = true;
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";
            this.Remarks.ReadOnly = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnCancel.Location = new System.Drawing.Point(116, 6);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scMain.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scMain.IsSplitterFixed = true;
            this.scMain.Location = new System.Drawing.Point(0, 0);
            this.scMain.Margin = new System.Windows.Forms.Padding(4);
            this.scMain.Name = "scMain";
            this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.scHeader);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.scDetail);
            this.scMain.Size = new System.Drawing.Size(894, 472);
            this.scMain.SplitterDistance = 105;
            this.scMain.TabIndex = 4;
            // 
            // scHeader
            // 
            this.scHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scHeader.Location = new System.Drawing.Point(0, 0);
            this.scHeader.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.scHeader.Name = "scHeader";
            // 
            // scHeader.Panel1
            // 
            this.scHeader.Panel1.Controls.Add(this.btnSelectAll);
            this.scHeader.Panel1.Controls.Add(this.ToDate);
            this.scHeader.Panel1.Controls.Add(this.btnClear);
            this.scHeader.Panel1.Controls.Add(this.Fromdate);
            this.scHeader.Panel1.Controls.Add(this.lblFromRequestDate);
            this.scHeader.Panel1.Controls.Add(this.chkShopID);
            this.scHeader.Panel1.Controls.Add(this.btnFilter);
            this.scHeader.Panel1.Controls.Add(this.cmbCompany);
            this.scHeader.Panel1.Controls.Add(this.lblCountry);
            // 
            // scHeader.Panel2
            // 
            this.scHeader.Panel2.Controls.Add(this.flpImages);
            this.scHeader.Panel2Collapsed = true;
            this.scHeader.Size = new System.Drawing.Size(894, 105);
            this.scHeader.SplitterDistance = 729;
            this.scHeader.TabIndex = 3;
            // 
            // ToDate
            // 
            this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ToDate.Location = new System.Drawing.Point(298, 12);
            this.ToDate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ToDate.Name = "ToDate";
            this.ToDate.Size = new System.Drawing.Size(134, 21);
            this.ToDate.TabIndex = 16;
            // 
            // Fromdate
            // 
            this.Fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Fromdate.Location = new System.Drawing.Point(160, 12);
            this.Fromdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Fromdate.Name = "Fromdate";
            this.Fromdate.Size = new System.Drawing.Size(134, 21);
            this.Fromdate.TabIndex = 14;
            // 
            // lblFromRequestDate
            // 
            this.lblFromRequestDate.AutoSize = true;
            this.lblFromRequestDate.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblFromRequestDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromRequestDate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblFromRequestDate.Location = new System.Drawing.Point(15, 12);
            this.lblFromRequestDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFromRequestDate.Name = "lblFromRequestDate";
            this.lblFromRequestDate.Size = new System.Drawing.Size(126, 13);
            this.lblFromRequestDate.TabIndex = 15;
            this.lblFromRequestDate.Text = "From Document Date";
            // 
            // chkShopID
            // 
            this.chkShopID.FormattingEnabled = true;
            this.chkShopID.Location = new System.Drawing.Point(443, 12);
            this.chkShopID.Name = "chkShopID";
            this.chkShopID.Size = new System.Drawing.Size(120, 84);
            this.chkShopID.TabIndex = 13;
            // 
            // btnFilter
            // 
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFilter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnFilter.Location = new System.Drawing.Point(570, 71);
            this.btnFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(88, 23);
            this.btnFilter.TabIndex = 12;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // cmbCompany
            // 
            this.cmbCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCompany.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCompany.Location = new System.Drawing.Point(160, 38);
            this.cmbCompany.Margin = new System.Windows.Forms.Padding(4);
            this.cmbCompany.Name = "cmbCompany";
            this.cmbCompany.Size = new System.Drawing.Size(271, 21);
            this.cmbCompany.TabIndex = 6;
            this.cmbCompany.SelectedIndexChanged += new System.EventHandler(this.cmbCompany_SelectedIndexChanged);
            // 
            // lblCountry
            // 
            this.lblCountry.AutoSize = true;
            this.lblCountry.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblCountry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountry.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCountry.Location = new System.Drawing.Point(15, 39);
            this.lblCountry.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(86, 13);
            this.lblCountry.TabIndex = 7;
            this.lblCountry.Text = "SAP Company";
            // 
            // flpImages
            // 
            this.flpImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpImages.Location = new System.Drawing.Point(0, 0);
            this.flpImages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.flpImages.Name = "flpImages";
            this.flpImages.Size = new System.Drawing.Size(96, 100);
            this.flpImages.TabIndex = 0;
            // 
            // scDetail
            // 
            this.scDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scDetail.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scDetail.Location = new System.Drawing.Point(0, 0);
            this.scDetail.Margin = new System.Windows.Forms.Padding(4);
            this.scDetail.Name = "scDetail";
            this.scDetail.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scDetail.Panel1
            // 
            this.scDetail.Panel1.Controls.Add(this.dgv_S_ORIN);
            // 
            // scDetail.Panel2
            // 
            this.scDetail.Panel2.Controls.Add(this.statusStrip1);
            this.scDetail.Panel2.Controls.Add(this.btnCancel);
            this.scDetail.Panel2.Controls.Add(this.btnSync);
            this.scDetail.Size = new System.Drawing.Size(894, 363);
            this.scDetail.SplitterDistance = 295;
            this.scDetail.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 42);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusStrip1.Size = new System.Drawing.Size(894, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(765, 17);
            this.StatusLabel.Text = resources.GetString("StatusLabel.Text");
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // btnSync
            // 
            this.btnSync.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnSync.Location = new System.Drawing.Point(5, 6);
            this.btnSync.Margin = new System.Windows.Forms.Padding(4);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(105, 30);
            this.btnSync.TabIndex = 0;
            this.btnSync.Text = "Sync";
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelectAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSelectAll.Location = new System.Drawing.Point(570, 42);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(88, 23);
            this.btnSelectAll.TabIndex = 20;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnClear
            // 
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClear.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClear.Location = new System.Drawing.Point(570, 13);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(88, 23);
            this.btnClear.TabIndex = 19;
            this.btnClear.Text = "Clear All";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // S_ORIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(894, 472);
            this.Controls.Add(this.scMain);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "S_ORIN";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "A/R Credit Memo";
            this.Load += new System.EventHandler(this.S_ORIN_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_S_ORIN)).EndInit();
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.scHeader.Panel1.ResumeLayout(false);
            this.scHeader.Panel1.PerformLayout();
            this.scHeader.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scHeader)).EndInit();
            this.scHeader.ResumeLayout(false);
            this.scDetail.Panel1.ResumeLayout(false);
            this.scDetail.Panel2.ResumeLayout(false);
            this.scDetail.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scDetail)).EndInit();
            this.scDetail.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_S_ORIN;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.SplitContainer scDetail;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.DataGridViewImageColumn Image;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocEntry;
        private System.Windows.Forms.DataGridViewTextBoxColumn Scenario;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransactionType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Source_Key;
        private System.Windows.Forms.DataGridViewTextBoxColumn ShopID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.SplitContainer scHeader;
        private System.Windows.Forms.DateTimePicker ToDate;
        private System.Windows.Forms.DateTimePicker Fromdate;
        private System.Windows.Forms.Label lblFromRequestDate;
        private System.Windows.Forms.CheckedListBox chkShopID;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.ComboBox cmbCompany;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.FlowLayoutPanel flpImages;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnClear;
    }
}