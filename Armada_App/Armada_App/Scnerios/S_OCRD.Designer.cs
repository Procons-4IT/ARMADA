namespace Armada_App
{
    partial class S_OCRD
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(S_OCRD));
            this.dgv_S_OCRD = new System.Windows.Forms.DataGridView();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.DocEntry = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Scenario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransactionType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Source_Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Country = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCancel = new System.Windows.Forms.Button();
            this.scForm = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.scHeader = new System.Windows.Forms.SplitContainer();
            this.btnFilter = new System.Windows.Forms.Button();
            this.CmbCountry = new System.Windows.Forms.ComboBox();
            this.lblCountry = new System.Windows.Forms.Label();
            this.flpImages = new System.Windows.Forms.FlowLayoutPanel();
            this.scDetail = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.btnSync = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_S_OCRD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scForm)).BeginInit();
            this.scForm.Panel1.SuspendLayout();
            this.scForm.Panel2.SuspendLayout();
            this.scForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            // dgv_S_OCRD
            // 
            this.dgv_S_OCRD.AllowUserToAddRows = false;
            this.dgv_S_OCRD.AllowUserToDeleteRows = false;
            this.dgv_S_OCRD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_S_OCRD.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Image,
            this.DocEntry,
            this.Scenario,
            this.TransactionType,
            this.Source_Key,
            this.Remarks,
            this.Country});
            this.dgv_S_OCRD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_S_OCRD.Location = new System.Drawing.Point(0, 0);
            this.dgv_S_OCRD.Margin = new System.Windows.Forms.Padding(0);
            this.dgv_S_OCRD.Name = "dgv_S_OCRD";
            this.dgv_S_OCRD.ReadOnly = true;
            this.dgv_S_OCRD.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv_S_OCRD.Size = new System.Drawing.Size(894, 328);
            this.dgv_S_OCRD.TabIndex = 0;
            // 
            // Image
            // 
            this.Image.DataPropertyName = "Image";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle1.NullValue")));
            this.Image.DefaultCellStyle = dataGridViewCellStyle1;
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
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";
            this.Remarks.ReadOnly = true;
            // 
            // Country
            // 
            this.Country.DataPropertyName = "Country";
            this.Country.HeaderText = "Country";
            this.Country.Name = "Country";
            this.Country.ReadOnly = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnCancel.Location = new System.Drawing.Point(116, 7);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(98, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // scForm
            // 
            this.scForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scForm.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scForm.IsSplitterFixed = true;
            this.scForm.Location = new System.Drawing.Point(0, 0);
            this.scForm.Margin = new System.Windows.Forms.Padding(4);
            this.scForm.Name = "scForm";
            this.scForm.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scForm.Panel1
            // 
            this.scForm.Panel1.Controls.Add(this.splitContainer1);
            // 
            // scForm.Panel2
            // 
            this.scForm.Panel2.Controls.Add(this.scDetail);
            this.scForm.Size = new System.Drawing.Size(894, 472);
            this.scForm.SplitterDistance = 71;
            this.scForm.TabIndex = 7;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainer1.Panel1Collapsed = true;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.scHeader);
            this.splitContainer1.Size = new System.Drawing.Size(894, 71);
            this.splitContainer1.SplitterDistance = 33;
            this.splitContainer1.TabIndex = 1;
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
            this.scHeader.Panel1.Controls.Add(this.btnFilter);
            this.scHeader.Panel1.Controls.Add(this.CmbCountry);
            this.scHeader.Panel1.Controls.Add(this.lblCountry);
            // 
            // scHeader.Panel2
            // 
            this.scHeader.Panel2.Controls.Add(this.flpImages);
            this.scHeader.Size = new System.Drawing.Size(894, 71);
            this.scHeader.SplitterDistance = 715;
            this.scHeader.TabIndex = 1;
            // 
            // btnFilter
            // 
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFilter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnFilter.Location = new System.Drawing.Point(508, 40);
            this.btnFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(88, 23);
            this.btnFilter.TabIndex = 12;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // CmbCountry
            // 
            this.CmbCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbCountry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CmbCountry.Location = new System.Drawing.Point(160, 13);
            this.CmbCountry.Margin = new System.Windows.Forms.Padding(4);
            this.CmbCountry.Name = "CmbCountry";
            this.CmbCountry.Size = new System.Drawing.Size(435, 21);
            this.CmbCountry.TabIndex = 6;
            // 
            // lblCountry
            // 
            this.lblCountry.AutoSize = true;
            this.lblCountry.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblCountry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountry.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCountry.Location = new System.Drawing.Point(15, 17);
            this.lblCountry.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(50, 13);
            this.lblCountry.TabIndex = 7;
            this.lblCountry.Text = "Country";
            // 
            // flpImages
            // 
            this.flpImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpImages.Location = new System.Drawing.Point(0, 0);
            this.flpImages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.flpImages.Name = "flpImages";
            this.flpImages.Size = new System.Drawing.Size(175, 71);
            this.flpImages.TabIndex = 0;
            // 
            // scDetail
            // 
            this.scDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scDetail.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scDetail.IsSplitterFixed = true;
            this.scDetail.Location = new System.Drawing.Point(0, 0);
            this.scDetail.Margin = new System.Windows.Forms.Padding(4);
            this.scDetail.Name = "scDetail";
            this.scDetail.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scDetail.Panel1
            // 
            this.scDetail.Panel1.Controls.Add(this.dgv_S_OCRD);
            // 
            // scDetail.Panel2
            // 
            this.scDetail.Panel2.Controls.Add(this.statusStrip1);
            this.scDetail.Panel2.Controls.Add(this.btnCancel);
            this.scDetail.Panel2.Controls.Add(this.btnSync);
            this.scDetail.Size = new System.Drawing.Size(894, 397);
            this.scDetail.SplitterDistance = 328;
            this.scDetail.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 43);
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
            this.btnSync.Location = new System.Drawing.Point(5, 7);
            this.btnSync.Margin = new System.Windows.Forms.Padding(4);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(105, 30);
            this.btnSync.TabIndex = 0;
            this.btnSync.Text = "Sync";
            this.btnSync.UseVisualStyleBackColor = true;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // S_OCRD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(894, 472);
            this.Controls.Add(this.scForm);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "S_OCRD";
            this.ShowInTaskbar = false;
            this.Text = "BUSINESS PARTNER SYNC";
            this.Load += new System.EventHandler(this.S_OCRD_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_S_OCRD)).EndInit();
            this.scForm.Panel1.ResumeLayout(false);
            this.scForm.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scForm)).EndInit();
            this.scForm.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
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

        private System.Windows.Forms.DataGridView dgv_S_OCRD;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.SplitContainer scForm;
        private System.Windows.Forms.SplitContainer scDetail;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.DataGridViewImageColumn Image;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocEntry;
        private System.Windows.Forms.DataGridViewTextBoxColumn Scenario;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransactionType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Source_Key;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn Country;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer scHeader;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.ComboBox CmbCountry;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.FlowLayoutPanel flpImages;
    }
}