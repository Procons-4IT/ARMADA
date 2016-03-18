using System;
using System.Data;
using System.Windows.Forms;
using Armada_Sync;
using System.Drawing;
using System.ServiceProcess;
using System.Configuration;
using System.Collections;
using System.IO;
using System.ComponentModel;

namespace Armada_App
{
    public partial class TransLog : Form
    {
        private ServiceController service;
        private Timer oLogTimer = new Timer();
        public static Hashtable oCompanyht = null;
        public string sQuery = string.Empty;
        //public SAPbobsCOM.Company oGetCompany = null;
        public static SAPbobsCOM.Company[] objCompany = null;
        BackgroundWorker m_oWorker;
        BackgroundWorker i_oWorker;
        private object _locker = new object();
        private object _locker1 = new object();
        private object _locker2 = new object();

        public TransLog()
        {
            try
            {
                InitializeComponent();
                m_oWorker = new BackgroundWorker();
                m_oWorker.DoWork += new DoWorkEventHandler(m_oWorker_DoWork);
                m_oWorker.ProgressChanged += new ProgressChangedEventHandler
                        (m_oWorker_ProgressChanged);
                m_oWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                        (m_oWorker_RunWorkerCompleted);
                m_oWorker.WorkerReportsProgress = true;
                m_oWorker.WorkerSupportsCancellation = true;

                i_oWorker = new BackgroundWorker();
                i_oWorker.DoWork += new DoWorkEventHandler(i_oWorker_DoWork);
                i_oWorker.ProgressChanged += new ProgressChangedEventHandler
                        (i_oWorker_ProgressChanged);
                i_oWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                        (i_oWorker_RunWorkerCompleted);
                i_oWorker.WorkerReportsProgress = true;
                i_oWorker.WorkerSupportsCancellation = true;
                TransLog.CheckForIllegalCrossThreadCalls = true;              
            }
            catch (Exception)
            {
                throw;
            }
        }

        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StatusLabel.Text = "Successfully Logged Into SAP Business One";
        }

        void m_oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            StatusLabel.Text = "Companys Connected Successfully...!";            
        }

        void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ConnectAllSapCompany();
                m_oWorker.CancelAsync();
            }
            catch (Exception ex)
            {
                traceService(ex.StackTrace.ToString());
                traceService(ex.Message.ToString());
            }
        }

        void i_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StatusLabel.Text = "Data Loaded Successfully....!";
        }

        void i_oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
            StatusLabel.Text = "Processing......" + toolStripProgressBar1.Value.ToString() + "%";
        }

        void i_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //setRowColorBasedonStatus();

                Image[] image = new Image[3];
                image[0] = Armada_App.Properties.Resources.Yes1;
                image[1] = Armada_App.Properties.Resources.Error1;
                image[2] = Armada_App.Properties.Resources.Create1;

                int intRow = 0;
                for (int i = 0; i < DgvTxnLogParent.RowCount-1; i++)
                {
                    i_oWorker.ReportProgress(intRow);

                    string RowType = DgvTxnLogParent.Rows[i].Cells["Status"].Value.ToString();
                    //System.Threading.Thread.Sleep(1000);
                    if (RowType == "Success")
                    {
                        lock (_locker)
                        {
                            DgvTxnLogParent.Rows[i].Cells["Image"].Value = image[0].Clone();                            
                        }
                        
                        DgvTxnLogParent.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
                        //Image image = Armada_App.Properties.Resources.Yes1;                        
                    }
                    else if (RowType == "Failed")
                    {
                        lock (_locker1)
                        {
                            DgvTxnLogParent.Rows[i].Cells["Image"].Value = image[1].Clone();
                        }
                        DgvTxnLogParent.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                        //Image image = Armada_App.Properties.Resources.Error1;                        
                    }
                    else if (RowType == "Open")
                    {
                        lock (_locker2)
                        {
                            DgvTxnLogParent.Rows[i].Cells["Image"].Value = image[2].Clone();
                        }
                        DgvTxnLogParent.Rows[i].DefaultCellStyle.ForeColor = Color.SteelBlue;
                        //Image image = Armada_App.Properties.Resources.Create1;                        
                    }
                    intRow += 1;
                    if (intRow == 100)
                    {
                        intRow = 1;
                    }
                }
                //foreach (DataGridViewRow row in DgvTxnLogParent.Rows)
                //{
                    
                //}
                //else
                //    i_oWorker.CancelAsync();
            }
            catch (Exception ex)
            {
                traceService(ex.StackTrace.ToString());
                traceService(ex.Message.ToString());
            }
        }

        #region "Menus"

        private void tsm_S_ORIN_Click(object sender, EventArgs e)
        {
            S_ORIN obj_S_ORIN = new S_ORIN();
            obj_S_ORIN.MdiParent = this.MdiParent;
            obj_S_ORIN.Show();
        }

        private void tsm_S_OINV_Click(object sender, EventArgs e)
        {
            S_OINV objS_OINV = new S_OINV();
            objS_OINV.MdiParent = this.MdiParent;
            objS_OINV.Show();
        }

        private void tsm_S_OCRD_Click(object sender, EventArgs e)
        {
            S_OCRD obj_OCRD = new S_OCRD();
            obj_OCRD.MdiParent = this.MdiParent;
            obj_OCRD.Show();
        }

        private void tsm_S_OWTR_Click(object sender, EventArgs e)
        {
            S_OWTR obj_S_OWTR = new S_OWTR();
            obj_S_OWTR.MdiParent = this.MdiParent;
            obj_S_OWTR.Show();
        }

        private void tsm_S_OPDN_Click(object sender, EventArgs e)
        {
            S_OPDN obj_S_OPDN = new S_OPDN();
            obj_S_OPDN.MdiParent = this.MdiParent;
            obj_S_OPDN.Show();
        }

        private void itemMasterImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            S_OITM obj_S_OITM = new S_OITM();
            obj_S_OITM.MdiParent = this.MdiParent;
            obj_S_OITM.ShowDialog();
        }

        private void logOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransLog.ActiveForm.Close();
        }
        #endregion

        #region "Events"

        private void TransLog_Load(object sender, EventArgs e)
        {
            try
            {                
                UXUTIL.clsUtilities.setAllControlsThemes(this);
                this.WindowState = FormWindowState.Maximized;
                companyStatusLabel.Text = "Connecting with SAP Company!!";
                oTimer.Start();
                BinddataScenario("0");
                service = new ServiceController("Armada_Service");
                oLogTimer.Enabled = true;
                oLogTimer.Interval = 180000;
                oLogTimer.Start();
                oLogTimer.Tick += new EventHandler(oLogTimer_Tick);
                m_oWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void oTimer_Tick(object sender, EventArgs e)
        {
            timeStatusLabel.Text = Convert.ToString(DateTime.Now) + "                                                                                                                                                           ";
        }

        private void oLogTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                //bindTrnDetailsAuto();
                //setRowColorBasedonStatus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TransLog_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.ShowInTaskbar = false;
            }
        }

        private void BtnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = BindTxnDetails();
                DgvTxnLogParent.DataSource = ds.Tables[0];
                //i_oWorker.RunWorkerAsync();
                setRowColorBasedonStatus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DgvTxnLogParent_Sorted(object sender, EventArgs e)
        {
            //setRowColorBasedonStatus();
        }

        private void niTaskBar_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            this.ShowInTaskbar = true;
            //setRowColorBasedonStatus();
        }

        private void niTaskBar_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            this.ShowInTaskbar = true;
            //setRowColorBasedonStatus();
        }

        private void SystemtryShow_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            this.ShowInTaskbar = true;
            //setRowColorBasedonStatus();
        }

        private void SystemtryExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                service.Refresh();
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    service.Start();
                }
                MessageBox.Show(string.Format("{0} --> started", service.DisplayName));
                enableService();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), @"Error Starting Service");
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                service.Refresh();

                if (service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();
                    MessageBox.Show(string.Format("{0} --> stopped", this.service.DisplayName));
                }

                MessageBox.Show(string.Format("{0} --> stopped", service.DisplayName));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), @"Error Stopping Service");
            }
        }
        #endregion

        #region"Functions"

        public void BinddataScenario(String val)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = Scenario();
            CmbScenario.DataSource = bs;
            CmbScenario.DisplayMember = "Text";
            CmbScenario.ValueMember = "ID";
        }

        private DataTable Scenario()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Text", typeof(string));
            DataRow dr = dt.NewRow();
            dr["ID"] = 0;
            dr["Text"] = "-ALL-";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "Customer";
            dr["Text"] = "Business Partner";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "ARInvoice";
            dr["Text"] = "A/R Invoice(Sale)";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "ARCreditMemo";
            dr["Text"] = "A/R Credit Memo(Sale)";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "InventoryTransfer";
            dr["Text"] = "Inventory Transfer";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "GRPO";
            dr["Text"] = "Purchase Good Receipt(Purchase)";
            dt.Rows.Add(dr);

            return dt;
        }

        private DataSet BindTxnDetails()
        {
            BOTransation_Log_Report oBOTransation_Log_Report = null;
            oBOTransation_Log_Report = new BOTransation_Log_Report();
            DataSet ds = oBOTransation_Log_Report.List(CmbScenario.SelectedValue.ToString(), FromReqdate.Text.Trim(), ResponseDate.Text.Trim(), ChbFailed.Checked.ToString());
            return ds;
        }

        private void bindTrnDetailsAuto()
        {
            string strInterface = System.Configuration.ConfigurationManager.AppSettings["InterDB"].ToString();
            DataTable oDtRefresh = null;
            string strQuery = "Exec Armada_Service_TxnLogReportRefresh_S";
            oDtRefresh = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(strQuery, strInterface);
            DgvTxnLogParent.DataSource = oDtRefresh;
        }

        private void setRowColorBasedonStatus()
        {
            try
            {
                // string[] strValues = new string[4];
                //Image[] image = new Image[3];
                //image[0] = Armada_App.Properties.Resources.Yes1;
                //image[1] = Armada_App.Properties.Resources.Error1;
                //image[2] = Armada_App.Properties.Resources.Create1;

                int intRow = 0;
                toolStripProgressBar1.Maximum = DgvTxnLogParent.RowCount;
                foreach (DataGridViewRow row in DgvTxnLogParent.Rows)
                {
                    string RowType = row.Cells["Status"].Value.ToString();
                    //System.Threading.Thread.Sleep(1000);
                    if (RowType == "Success")
                    {
                        //row.Cells["Image"].Value = image[0];
                        row.DefaultCellStyle.ForeColor = Color.Green;
                        //Image image = Armada_App.Properties.Resources.Yes1;                        
                    }
                    else if (RowType == "Failed")
                    {
                       // row.Cells["Image"].Value = image[1];
                        row.DefaultCellStyle.ForeColor = Color.Red;
                        //Image image = Armada_App.Properties.Resources.Error1;                        
                    }
                    else if (RowType == "Open")
                    {
                        //row.Cells["Image"].Value = image[2];
                        row.DefaultCellStyle.ForeColor = Color.SteelBlue;
                        //Image image = Armada_App.Properties.Resources.Create1;                        
                    }

                    //if (intRow == 100)
                    //{
                    //    intRow = 1;
                    //    toolStripProgressBar1.Value = 0;
                    //}

                    toolStripProgressBar1.Value = intRow;
                    intRow += 1;
                }
               // toolStripProgressBar1.Value = 0;
                //Application.DoEvents();
            }
            catch (Exception ex)
            {
                traceService(ex.Message);
            }
            finally
            {
                toolStripProgressBar1.Value = 0;
            }
        }

        private void enableService()
        {
            service.Refresh();

            if (service.Status == ServiceControllerStatus.Stopped)
            {
                service.Start();
                btnStop.Enabled = true;
                btnStart.Enabled = false;
            }
            else if (service.Status == ServiceControllerStatus.Running)
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }

        }

        #endregion

        private void mailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Buson_SendMail.clsTransMail oSendMail = new Buson_SendMail.clsTransMail();
            //oSendMail.LoadMailDetails();
        }

        public void ConnectAllSapCompany()
        {
            try
            {
                SAPbobsCOM.Company oCompany = null;
                oCompanyht = new Hashtable();

                string strMaiDB = System.Configuration.ConfigurationManager.AppSettings["MainDB"].ToString();
                string DBServer = System.Configuration.ConfigurationManager.AppSettings["SAPServer"].ToString();
                string ServerType = System.Configuration.ConfigurationManager.AppSettings["DbServerType"].ToString();
                string DBUserName = System.Configuration.ConfigurationManager.AppSettings["DbUserName"].ToString();
                string DBPwd = System.Configuration.ConfigurationManager.AppSettings["DbPassword"].ToString();
                string LicenseServer = System.Configuration.ConfigurationManager.AppSettings["SAPlicense"].ToString();

                sQuery = " SELECT U_COMPANY,U_SAPUSERNAME,U_SAPPASSWORD FROM [@Z_INBOUNDMAPPING] ";
                string ConnectionString = String.Format(System.Configuration.ConfigurationManager.AppSettings["Logger"].ToString(), strMaiDB);
                DataSet oDS = Singleton.objSqlDataAccess.ExecuteDataSet(sQuery, strMaiDB);
                if (oDS != null)
                {
                    DataTable oDT_C = oDS.Tables[0];
                    int intCompany = oDT_C.Rows.Count;
                    objCompany = new SAPbobsCOM.Company[intCompany];
                    int intCompCount = 0;

                    foreach (DataRow dr_company in oDT_C.Rows)
                    {
                        //StatusLabel.Text = "Connecting SAP Business One Company : " + dr_company["U_COMPANY"].ToString();

                        oCompany = new SAPbobsCOM.Company();
                        oCompany.Server = DBServer;
                        switch (ServerType)
                        {
                            case "2008":
                                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008;
                                break;
                            case "2012":
                                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
                                break;
                            default:
                                break;
                        }
                        oCompany.DbUserName = DBUserName;
                        oCompany.DbPassword = DBPwd;
                        oCompany.CompanyDB = dr_company["U_COMPANY"].ToString();
                        oCompany.UserName = dr_company["U_SAPUSERNAME"].ToString();
                        oCompany.Password = dr_company["U_SAPPASSWORD"].ToString();
                        oCompany.UseTrusted = false;

                        if (oCompany.Connect() != 0)
                        {
                            objCompany[intCompCount] = oCompany;
                            oCompanyht.Add(dr_company["U_COMPANY"].ToString(), oCompany);
                            traceService("Company : " + oCompany.CompanyDB);
                            traceService("Error Code : " + oCompany.GetLastErrorDescription());
                        }
                        else
                        {
                            objCompany[intCompCount] = oCompany;
                            oCompanyht.Add(dr_company["U_COMPANY"].ToString(), oCompany);
                            traceService("Company : " + oCompany.CompanyDB);
                            traceService("Connected");
                        }
                        intCompCount += 1;
                    }
                }
            }
            catch (Exception ex)
            {
                traceService(ex.StackTrace.ToString());
                traceService(ex.Message.ToString());
                throw;
            }
        }

        public static SAPbobsCOM.Company GetCompany(string strCompany)
        {
            SAPbobsCOM.Company _retVal = null;
            try
            {
                bool blnCompanyExist = false;

                foreach (string key in oCompanyht.Keys)
                {
                    if (key.ToString() == strCompany)
                    {
                        _retVal = (SAPbobsCOM.Company)oCompanyht[key];
                        traceService("Get Company");
                        blnCompanyExist = true;
                        break;
                    }
                }

                if (!blnCompanyExist)
                {
                    throw new Exception("Company Not Found...");
                }
            }
            catch (Exception ex)
            {
                traceService(ex.Message);
            }
            return _retVal;
        }

        public void disConnectCompany()
        {
            try
            {
                foreach (SAPbobsCOM.Company item in objCompany)
                {
                    if (item.Connected)
                    {
                        item.Disconnect();
                    }
                }
            }
            catch (Exception ex)
            {
                traceService(ex.Message);
            }
        }

        public static void traceService(string content)
        {
            try
            {
                string strFile = @"\Armada_Service_" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt";
                string strPath = System.Windows.Forms.Application.StartupPath.ToString() + strFile;
                if (!File.Exists(strPath))
                {
                    File.Create(strPath);
                }
                FileStream fs = new FileStream(strPath, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.WriteLine(content);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        private void TransLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            disConnectCompany();
        }

    }
}
