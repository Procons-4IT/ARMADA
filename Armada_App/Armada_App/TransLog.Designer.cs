namespace Armada_App
{
    partial class TransLog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransLog));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MasterStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.itemMasterImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modulesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_S_OCRD = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_S_OINV = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_S_ORIN = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_S_OWTR = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_S_OPDN = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.companyStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.timeStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.oTimer = new System.Windows.Forms.Timer(this.components);
            this.scTransLog = new System.Windows.Forms.SplitContainer();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.scHeader = new System.Windows.Forms.SplitContainer();
            this.ChbFailed = new System.Windows.Forms.CheckBox();
            this.BtnProcess = new System.Windows.Forms.Button();
            this.ResponseDate = new System.Windows.Forms.DateTimePicker();
            this.CmbScenario = new System.Windows.Forms.ComboBox();
            this.lblScenario = new System.Windows.Forms.Label();
            this.FromReqdate = new System.Windows.Forms.DateTimePicker();
            this.lblFromRequestDate = new System.Windows.Forms.Label();
            this.flpImages = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.DgvTxnLogParent = new System.Windows.Forms.DataGridView();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.TransType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Scenerio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.S_Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.S_DocNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DestinationKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.D_DocNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResponseTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ErroCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.niTaskBar = new System.Windows.Forms.NotifyIcon(this.components);
            this.Systemtry = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scTransLog)).BeginInit();
            this.scTransLog.Panel1.SuspendLayout();
            this.scTransLog.Panel2.SuspendLayout();
            this.scTransLog.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scHeader)).BeginInit();
            this.scHeader.Panel1.SuspendLayout();
            this.scHeader.Panel2.SuspendLayout();
            this.scHeader.SuspendLayout();
            this.flpImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvTxnLogParent)).BeginInit();
            this.Systemtry.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MasterStripMenuItem1,
            this.modulesToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(963, 26);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MasterStripMenuItem1
            // 
            this.MasterStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemMasterImportToolStripMenuItem});
            this.MasterStripMenuItem1.Enabled = false;
            this.MasterStripMenuItem1.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MasterStripMenuItem1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.MasterStripMenuItem1.Name = "MasterStripMenuItem1";
            this.MasterStripMenuItem1.Size = new System.Drawing.Size(140, 22);
            this.MasterStripMenuItem1.Text = "Master Import";
            // 
            // itemMasterImportToolStripMenuItem
            // 
            this.itemMasterImportToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemMasterImportToolStripMenuItem.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.itemMasterImportToolStripMenuItem.Name = "itemMasterImportToolStripMenuItem";
            this.itemMasterImportToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.itemMasterImportToolStripMenuItem.Text = "Item Master Import";
            this.itemMasterImportToolStripMenuItem.Click += new System.EventHandler(this.itemMasterImportToolStripMenuItem_Click);
            // 
            // modulesToolStripMenuItem
            // 
            this.modulesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm_S_OCRD,
            this.tsm_S_OINV,
            this.tsm_S_ORIN,
            this.tsm_S_OWTR,
            this.tsm_S_OPDN});
            this.modulesToolStripMenuItem.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modulesToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.modulesToolStripMenuItem.Name = "modulesToolStripMenuItem";
            this.modulesToolStripMenuItem.Size = new System.Drawing.Size(87, 22);
            this.modulesToolStripMenuItem.Text = "Modules";
            // 
            // tsm_S_OCRD
            // 
            this.tsm_S_OCRD.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsm_S_OCRD.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.tsm_S_OCRD.Name = "tsm_S_OCRD";
            this.tsm_S_OCRD.Size = new System.Drawing.Size(227, 22);
            this.tsm_S_OCRD.Text = "Business Partner ";
            this.tsm_S_OCRD.Click += new System.EventHandler(this.tsm_S_OCRD_Click);
            // 
            // tsm_S_OINV
            // 
            this.tsm_S_OINV.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsm_S_OINV.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.tsm_S_OINV.Name = "tsm_S_OINV";
            this.tsm_S_OINV.Size = new System.Drawing.Size(227, 22);
            this.tsm_S_OINV.Text = "A/R Invoice(Sale)";
            this.tsm_S_OINV.Click += new System.EventHandler(this.tsm_S_OINV_Click);
            // 
            // tsm_S_ORIN
            // 
            this.tsm_S_ORIN.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsm_S_ORIN.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.tsm_S_ORIN.Name = "tsm_S_ORIN";
            this.tsm_S_ORIN.Size = new System.Drawing.Size(227, 22);
            this.tsm_S_ORIN.Text = "A/R Credit Memo";
            this.tsm_S_ORIN.Click += new System.EventHandler(this.tsm_S_ORIN_Click);
            // 
            // tsm_S_OWTR
            // 
            this.tsm_S_OWTR.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsm_S_OWTR.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.tsm_S_OWTR.Name = "tsm_S_OWTR";
            this.tsm_S_OWTR.Size = new System.Drawing.Size(227, 22);
            this.tsm_S_OWTR.Text = "Inventory Transfer";
            this.tsm_S_OWTR.Click += new System.EventHandler(this.tsm_S_OWTR_Click);
            // 
            // tsm_S_OPDN
            // 
            this.tsm_S_OPDN.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.tsm_S_OPDN.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.tsm_S_OPDN.Name = "tsm_S_OPDN";
            this.tsm_S_OPDN.Size = new System.Drawing.Size(227, 22);
            this.tsm_S_OPDN.Text = "Purchase Good Reeipt";
            this.tsm_S_OPDN.Click += new System.EventHandler(this.tsm_S_OPDN_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logOffToolStripMenuItem});
            this.exitToolStripMenuItem.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(51, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // logOffToolStripMenuItem
            // 
            this.logOffToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logOffToolStripMenuItem.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.logOffToolStripMenuItem.Name = "logOffToolStripMenuItem";
            this.logOffToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.logOffToolStripMenuItem.Text = "Log Off";
            this.logOffToolStripMenuItem.Click += new System.EventHandler(this.logOffToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 384);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 18, 0);
            this.statusStrip1.Size = new System.Drawing.Size(963, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(693, 17);
            this.StatusLabel.Text = resources.GetString("StatusLabel.Text");
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.MarqueeAnimationSpeed = 50;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // statusStrip2
            // 
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.companyStatusLabel,
            this.timeStatusLabel});
            this.statusStrip2.Location = new System.Drawing.Point(0, 362);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Padding = new System.Windows.Forms.Padding(1, 0, 18, 0);
            this.statusStrip2.Size = new System.Drawing.Size(963, 22);
            this.statusStrip2.TabIndex = 4;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // companyStatusLabel
            // 
            this.companyStatusLabel.Name = "companyStatusLabel";
            this.companyStatusLabel.Size = new System.Drawing.Size(208, 17);
            this.companyStatusLabel.Text = "Company Details                                     ";
            // 
            // timeStatusLabel
            // 
            this.timeStatusLabel.Name = "timeStatusLabel";
            this.timeStatusLabel.Size = new System.Drawing.Size(379, 17);
            this.timeStatusLabel.Text = "                                                   Time                          " +
    "                                      ";
            // 
            // oTimer
            // 
            this.oTimer.Enabled = true;
            this.oTimer.Interval = 10;
            this.oTimer.Tick += new System.EventHandler(this.oTimer_Tick);
            // 
            // scTransLog
            // 
            this.scTransLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scTransLog.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scTransLog.IsSplitterFixed = true;
            this.scTransLog.Location = new System.Drawing.Point(0, 26);
            this.scTransLog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.scTransLog.Name = "scTransLog";
            this.scTransLog.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scTransLog.Panel1
            // 
            this.scTransLog.Panel1.Controls.Add(this.HeaderPanel);
            // 
            // scTransLog.Panel2
            // 
            this.scTransLog.Panel2.Controls.Add(this.DgvTxnLogParent);
            this.scTransLog.Size = new System.Drawing.Size(963, 336);
            this.scTransLog.SplitterDistance = 83;
            this.scTransLog.TabIndex = 10;
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackColor = System.Drawing.SystemColors.Info;
            this.HeaderPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.HeaderPanel.Controls.Add(this.scHeader);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(963, 83);
            this.HeaderPanel.TabIndex = 8;
            // 
            // scHeader
            // 
            this.scHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scHeader.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scHeader.IsSplitterFixed = true;
            this.scHeader.Location = new System.Drawing.Point(0, 0);
            this.scHeader.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.scHeader.Name = "scHeader";
            // 
            // scHeader.Panel1
            // 
            this.scHeader.Panel1.Controls.Add(this.ChbFailed);
            this.scHeader.Panel1.Controls.Add(this.BtnProcess);
            this.scHeader.Panel1.Controls.Add(this.ResponseDate);
            this.scHeader.Panel1.Controls.Add(this.CmbScenario);
            this.scHeader.Panel1.Controls.Add(this.lblScenario);
            this.scHeader.Panel1.Controls.Add(this.FromReqdate);
            this.scHeader.Panel1.Controls.Add(this.lblFromRequestDate);
            // 
            // scHeader.Panel2
            // 
            this.scHeader.Panel2.Controls.Add(this.flpImages);
            this.scHeader.Size = new System.Drawing.Size(963, 83);
            this.scHeader.SplitterDistance = 628;
            this.scHeader.TabIndex = 0;
            // 
            // ChbFailed
            // 
            this.ChbFailed.AutoSize = true;
            this.ChbFailed.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ChbFailed.Location = new System.Drawing.Point(440, 45);
            this.ChbFailed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ChbFailed.Name = "ChbFailed";
            this.ChbFailed.Size = new System.Drawing.Size(59, 17);
            this.ChbFailed.TabIndex = 11;
            this.ChbFailed.Text = "Failed";
            this.ChbFailed.UseVisualStyleBackColor = true;
            // 
            // BtnProcess
            // 
            this.BtnProcess.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnProcess.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtnProcess.Location = new System.Drawing.Point(508, 40);
            this.BtnProcess.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnProcess.Name = "BtnProcess";
            this.BtnProcess.Size = new System.Drawing.Size(88, 23);
            this.BtnProcess.TabIndex = 12;
            this.BtnProcess.Text = "Process";
            this.BtnProcess.UseVisualStyleBackColor = true;
            this.BtnProcess.Click += new System.EventHandler(this.BtnProcess_Click);
            // 
            // ResponseDate
            // 
            this.ResponseDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ResponseDate.Location = new System.Drawing.Point(298, 42);
            this.ResponseDate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ResponseDate.Name = "ResponseDate";
            this.ResponseDate.Size = new System.Drawing.Size(134, 21);
            this.ResponseDate.TabIndex = 10;
            // 
            // CmbScenario
            // 
            this.CmbScenario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbScenario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CmbScenario.Location = new System.Drawing.Point(160, 13);
            this.CmbScenario.Margin = new System.Windows.Forms.Padding(4);
            this.CmbScenario.Name = "CmbScenario";
            this.CmbScenario.Size = new System.Drawing.Size(435, 21);
            this.CmbScenario.TabIndex = 6;
            // 
            // lblScenario
            // 
            this.lblScenario.AutoSize = true;
            this.lblScenario.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblScenario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScenario.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblScenario.Location = new System.Drawing.Point(15, 17);
            this.lblScenario.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScenario.Name = "lblScenario";
            this.lblScenario.Size = new System.Drawing.Size(57, 13);
            this.lblScenario.TabIndex = 7;
            this.lblScenario.Text = "Scenario";
            // 
            // FromReqdate
            // 
            this.FromReqdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.FromReqdate.Location = new System.Drawing.Point(160, 41);
            this.FromReqdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.FromReqdate.Name = "FromReqdate";
            this.FromReqdate.Size = new System.Drawing.Size(134, 21);
            this.FromReqdate.TabIndex = 8;
            // 
            // lblFromRequestDate
            // 
            this.lblFromRequestDate.AutoSize = true;
            this.lblFromRequestDate.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblFromRequestDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromRequestDate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblFromRequestDate.Location = new System.Drawing.Point(15, 43);
            this.lblFromRequestDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFromRequestDate.Name = "lblFromRequestDate";
            this.lblFromRequestDate.Size = new System.Drawing.Size(125, 13);
            this.lblFromRequestDate.TabIndex = 9;
            this.lblFromRequestDate.Text = "From Response Date";
            // 
            // flpImages
            // 
            this.flpImages.Controls.Add(this.btnStop);
            this.flpImages.Controls.Add(this.btnStart);
            this.flpImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpImages.Location = new System.Drawing.Point(0, 0);
            this.flpImages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.flpImages.Name = "flpImages";
            this.flpImages.Size = new System.Drawing.Size(331, 83);
            this.flpImages.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.ForeColor = System.Drawing.Color.Red;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStop.Location = new System.Drawing.Point(4, 3);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(138, 69);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Visible = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
            this.btnStart.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStart.Location = new System.Drawing.Point(150, 3);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(144, 69);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Visible = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // DgvTxnLogParent
            // 
            this.DgvTxnLogParent.AllowUserToAddRows = false;
            this.DgvTxnLogParent.AllowUserToDeleteRows = false;
            this.DgvTxnLogParent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DgvTxnLogParent.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.DgvTxnLogParent.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.DgvTxnLogParent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Image,
            this.TransType,
            this.Scenerio,
            this.S_Key,
            this.S_DocNo,
            this.RequestDate,
            this.RequestTime,
            this.DestinationKey,
            this.D_DocNo,
            this.ResDate,
            this.ResponseTime,
            this.Status,
            this.ErroCode,
            this.Remarks});
            this.DgvTxnLogParent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvTxnLogParent.Location = new System.Drawing.Point(0, 0);
            this.DgvTxnLogParent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.DgvTxnLogParent.MultiSelect = false;
            this.DgvTxnLogParent.Name = "DgvTxnLogParent";
            this.DgvTxnLogParent.ReadOnly = true;
            this.DgvTxnLogParent.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DgvTxnLogParent.Size = new System.Drawing.Size(963, 249);
            this.DgvTxnLogParent.TabIndex = 6;
            this.DgvTxnLogParent.Sorted += new System.EventHandler(this.DgvTxnLogParent_Sorted);
            // 
            // Image
            // 
            this.Image.DataPropertyName = "Image";
            this.Image.HeaderText = "Image";
            this.Image.Name = "Image";
            this.Image.ReadOnly = true;
            this.Image.Visible = false;
            this.Image.Width = 50;
            // 
            // TransType
            // 
            this.TransType.DataPropertyName = "TransType";
            this.TransType.HeaderText = "Trans Type";
            this.TransType.Name = "TransType";
            this.TransType.ReadOnly = true;
            this.TransType.Width = 96;
            // 
            // Scenerio
            // 
            this.Scenerio.DataPropertyName = "Scenario";
            this.Scenerio.HeaderText = "Scenario";
            this.Scenerio.Name = "Scenerio";
            this.Scenerio.ReadOnly = true;
            this.Scenerio.Width = 82;
            // 
            // S_Key
            // 
            this.S_Key.DataPropertyName = "SourceKey";
            this.S_Key.FillWeight = 91.83587F;
            this.S_Key.HeaderText = "Src Key";
            this.S_Key.Name = "S_Key";
            this.S_Key.ReadOnly = true;
            this.S_Key.Width = 77;
            // 
            // S_DocNo
            // 
            this.S_DocNo.DataPropertyName = "SourceNo";
            this.S_DocNo.HeaderText = "Src No";
            this.S_DocNo.Name = "S_DocNo";
            this.S_DocNo.ReadOnly = true;
            this.S_DocNo.Width = 70;
            // 
            // RequestDate
            // 
            this.RequestDate.DataPropertyName = "RequestDate";
            this.RequestDate.FillWeight = 91.83587F;
            this.RequestDate.HeaderText = "Req Date";
            this.RequestDate.Name = "RequestDate";
            this.RequestDate.ReadOnly = true;
            this.RequestDate.Width = 85;
            // 
            // RequestTime
            // 
            this.RequestTime.DataPropertyName = "RequestTime";
            this.RequestTime.FillWeight = 91.83587F;
            this.RequestTime.HeaderText = "Req Time";
            this.RequestTime.Name = "RequestTime";
            this.RequestTime.ReadOnly = true;
            this.RequestTime.Width = 86;
            // 
            // DestinationKey
            // 
            this.DestinationKey.DataPropertyName = "DestinationKey";
            this.DestinationKey.FillWeight = 91.83587F;
            this.DestinationKey.HeaderText = "Dest Key";
            this.DestinationKey.Name = "DestinationKey";
            this.DestinationKey.ReadOnly = true;
            this.DestinationKey.Width = 84;
            // 
            // D_DocNo
            // 
            this.D_DocNo.DataPropertyName = "DestinationNo";
            this.D_DocNo.HeaderText = "Dest No";
            this.D_DocNo.Name = "D_DocNo";
            this.D_DocNo.ReadOnly = true;
            this.D_DocNo.Width = 77;
            // 
            // ResDate
            // 
            this.ResDate.DataPropertyName = "ResponseDate";
            this.ResDate.FillWeight = 91.83587F;
            this.ResDate.HeaderText = "Resp Date";
            this.ResDate.Name = "ResDate";
            this.ResDate.ReadOnly = true;
            this.ResDate.Width = 91;
            // 
            // ResponseTime
            // 
            this.ResponseTime.DataPropertyName = "ResponseTime";
            this.ResponseTime.FillWeight = 91.83587F;
            this.ResponseTime.HeaderText = "RespTime";
            this.ResponseTime.Name = "ResponseTime";
            this.ResponseTime.ReadOnly = true;
            this.ResponseTime.Width = 88;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "Status";
            this.Status.FillWeight = 91.83587F;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 68;
            // 
            // ErroCode
            // 
            this.ErroCode.DataPropertyName = "ErroCode";
            this.ErroCode.FillWeight = 91.83587F;
            this.ErroCode.HeaderText = "Error Code";
            this.ErroCode.Name = "ErroCode";
            this.ErroCode.ReadOnly = true;
            this.ErroCode.Width = 95;
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.FillWeight = 197.9695F;
            this.Remarks.HeaderText = "Remarks(Status)";
            this.Remarks.Name = "Remarks";
            this.Remarks.ReadOnly = true;
            this.Remarks.Width = 129;
            // 
            // niTaskBar
            // 
            this.niTaskBar.Icon = ((System.Drawing.Icon)(resources.GetObject("niTaskBar.Icon")));
            this.niTaskBar.Text = "notifyIcon1";
            this.niTaskBar.Visible = true;
            this.niTaskBar.Click += new System.EventHandler(this.niTaskBar_Click);
            this.niTaskBar.DoubleClick += new System.EventHandler(this.niTaskBar_DoubleClick);
            // 
            // Systemtry
            // 
            this.Systemtry.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.exitToolStripMenuItem1});
            this.Systemtry.Name = "Systemtry";
            this.Systemtry.Size = new System.Drawing.Size(104, 48);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.SystemtryShow_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.SystemtryExit_Click);
            // 
            // TransLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 406);
            this.Controls.Add(this.scTransLog);
            this.Controls.Add(this.statusStrip2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "TransLog";
            this.Text = "Transaction Log Status";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TransLog_FormClosing);
            this.Load += new System.EventHandler(this.TransLog_Load);
            this.Resize += new System.EventHandler(this.TransLog_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.scTransLog.Panel1.ResumeLayout(false);
            this.scTransLog.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scTransLog)).EndInit();
            this.scTransLog.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            this.scHeader.Panel1.ResumeLayout(false);
            this.scHeader.Panel1.PerformLayout();
            this.scHeader.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scHeader)).EndInit();
            this.scHeader.ResumeLayout(false);
            this.flpImages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvTxnLogParent)).EndInit();
            this.Systemtry.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem modulesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsm_S_ORIN;
        private System.Windows.Forms.ToolStripMenuItem tsm_S_OINV;
        private System.Windows.Forms.ToolStripMenuItem tsm_S_OCRD;
        private System.Windows.Forms.ToolStripMenuItem tsm_S_OWTR;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logOffToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStripStatusLabel companyStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel timeStatusLabel;
        internal System.Windows.Forms.Timer oTimer;
        private System.Windows.Forms.SplitContainer scTransLog;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.DataGridView DgvTxnLogParent;
        private System.Windows.Forms.NotifyIcon niTaskBar;
        private System.Windows.Forms.ContextMenuStrip Systemtry;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.SplitContainer scHeader;
        private System.Windows.Forms.CheckBox ChbFailed;
        private System.Windows.Forms.Button BtnProcess;
        private System.Windows.Forms.DateTimePicker ResponseDate;
        private System.Windows.Forms.ComboBox CmbScenario;
        private System.Windows.Forms.Label lblScenario;
        private System.Windows.Forms.DateTimePicker FromReqdate;
        private System.Windows.Forms.Label lblFromRequestDate;
        private System.Windows.Forms.FlowLayoutPanel flpImages;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ToolStripMenuItem tsm_S_OPDN;
        private System.Windows.Forms.ToolStripMenuItem MasterStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem itemMasterImportToolStripMenuItem;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.DataGridViewImageColumn Image;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Scenerio;
        private System.Windows.Forms.DataGridViewTextBoxColumn S_Key;
        private System.Windows.Forms.DataGridViewTextBoxColumn S_DocNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn DestinationKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn D_DocNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResponseTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErroCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
    }
}

