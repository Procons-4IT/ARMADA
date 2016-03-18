using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Armada_Sync;
using System.Collections;

namespace Armada_App
{
    public partial class S_OCRD : Form
    {
        private const string TRANSACTIONLOG = "[Armada_Service_M_S_OCRD_s]";
        private string sQuery = string.Empty;
        BackgroundWorker m_oWorker;
        DataTable oDtCountry;

        public S_OCRD()
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
        }

        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                StatusLabel.Text = "Sync Process Completed...";
                btnFilter.Enabled = true;
                LoadAll();
            }
            catch (Exception ex)
            {
                TransLog.traceService(ex.StackTrace.ToString());
                TransLog.traceService(ex.Message.ToString());
            }
        }

        void m_oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
        }

        void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string[] strValues = new string[4];
                Hashtable htCCDet = new Hashtable();
                string strInterface = System.Configuration.ConfigurationManager.AppSettings["InterDB"].ToString();
                string strMainDB = System.Configuration.ConfigurationManager.AppSettings["MainDB"].ToString();
                
                for (int i = 0; i < dgv_S_OCRD.RowCount; i++)
                {
                    try
                    {
                        //dgv_S_OCRD.FirstDisplayedScrollingRowIndex = i;
                        //m_oWorker.ReportProgress(i);

                        bool blnSync = false;
                        foreach (SAPbobsCOM.Company item in TransLog.objCompany)
                        {
                            TransLog.traceService(" Transaction Type : " + dgv_S_OCRD.Rows[i].Cells["Scenario"].Value.ToString().Trim());
                            TransLog.traceService(" Transaction Key : " + dgv_S_OCRD.Rows[i].Cells["Source_Key"].Value.ToString().Trim());
                            TransLog.traceService("Company DB : " + item.CompanyDB);

                            if (item.Connected)
                            {
                                string strCountry = item.GetCompanyService().GetAdminInfo().Country;
                                if (dgv_S_OCRD.Rows[i].Cells["Country"].Value.ToString() == strCountry)
                                {
                                    if (strMainDB != item.CompanyDB)
                                    {
                                        Image image = Armada_App.Properties.Resources.Sync1;
                                        dgv_S_OCRD.Rows[i].Cells["Image"].Value = image;

                                        Singleton.obj_S_OCRD.Sync((dgv_S_OCRD.Rows[i].Cells["Source_Key"].Value.ToString().Trim()), TransType.A, item, strInterface, "", strValues, htCCDet);
                                        
                                        string strQuery = "Select Status,Remarks From dbo.Z_OTXN Where Scenario = '" + dgv_S_OCRD.Rows[i].Cells["Scenario"].Value.ToString().Trim() + "' ";
                                        strQuery += " And S_DocNo = '" + dgv_S_OCRD.Rows[i].Cells["Source_Key"].Value.ToString().Trim() + "' ";
                                        DataTable oStatus = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(strQuery, strInterface);
                                        if (oStatus != null && oStatus.Rows.Count > 0)
                                        {
                                            if (oStatus.Rows[0][0].ToString() == "1")
                                            {
                                                blnSync = true;
                                                dgv_S_OCRD.Rows[i].Cells["Remarks"].Value = string.Empty;
                                            }
                                            else
                                            {
                                                blnSync = false;
                                                dgv_S_OCRD.Rows[i].Cells["Remarks"].Value = oStatus.Rows[0][1].ToString();
                                            }
                                        }
                                        else
                                        {
                                            blnSync = false;
                                        }
                                    }
                                }
                                if (blnSync)
                                {
                                    Image image = Armada_App.Properties.Resources.Yes1;
                                    dgv_S_OCRD.Rows[i].Cells["Image"].Value = image;
                                    //System.Threading.Thread.Sleep(100);
                                }
                                else
                                {
                                    Image image = Armada_App.Properties.Resources.Error1;
                                    dgv_S_OCRD.Rows[i].Cells["Image"].Value = image;
                                    //System.Threading.Thread.Sleep(100);
                                }
                            }
                            else
                            {
                                TransLog.traceService(" Error : Company Not Connected.");
                                //Image image = Armada_App.Properties.Resources.Error1;
                                //dgv_S_OCRD.Rows[i].Cells["Image"].Value = image;
                                //System.Threading.Thread.Sleep(100);
                            }
                            //toolStripProgressBar1.Value = i;
                        }
                        //toolStripProgressBar1.Value = 0;
                    }
                    catch (Exception ex)
                    {
                        TransLog.traceService(ex.StackTrace.ToString());
                        TransLog.traceService(ex.Message.ToString());
                    }
                }
                m_oWorker.CancelAsync();
            }
            catch (Exception ex)
            {
                TransLog.traceService(ex.StackTrace.ToString());
                TransLog.traceService(ex.Message.ToString());
            }
            finally
            {
                toolStripProgressBar1.Value = 0;
            }
        }

        private void S_OCRD_Load(object sender, EventArgs e)
        {
            try
            {
                UXUTIL.clsUtilities.setAllControlsThemes(this);
                this.WindowState = FormWindowState.Maximized;
                loadCountry();
                //LoadAll();
                //loadError();
                //S_OCRD.CheckForIllegalCrossThreadCalls = true;
            }
            catch (Exception ex)
            {
                TransLog.traceService(" Error : " + ex.Message);
            }
        }


        private void loadCountry()
        {
            try
            {
                string strInterface = System.Configuration.ConfigurationManager.AppSettings["InterDB"].ToString();
                string str_Qry = " Select Distinct Country From Customers Where Status Is Null ";
                oDtCountry = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(str_Qry, strInterface);
                CmbCountry.DataSource = oDtCountry;
                CmbCountry.DisplayMember = "Country";
                CmbCountry.ValueMember = "Country";
                CmbCountry.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                TransLog.traceService(" Error : " + ex.Message);
            }
        }

        private void LoadAll()
        {
            try
            {
                string strInterface = System.Configuration.ConfigurationManager.AppSettings["InterDB"].ToString();
                DataTable oDS_OCRD = null;
                string strCountry = CmbCountry.SelectedValue.ToString();
                string str_M_S_OCRD = "Exec [Armada_Service_M_S_OCRD_s] '" + strCountry + "'";
                oDS_OCRD = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(str_M_S_OCRD, strInterface);
                dgv_S_OCRD.DataSource = oDS_OCRD;
                loadError();
            }
            catch (Exception ex)
            {
                TransLog.traceService(" Error : " + ex.Message);
            }
        }

        private void loadError()
        {
            try
            {
                for (int i = 0; i < dgv_S_OCRD.RowCount; i++)
                {
                    if (dgv_S_OCRD.Rows[i].Cells[1].Value.ToString() == "-1")
                    {
                        Image image = Armada_App.Properties.Resources.Create1;
                        dgv_S_OCRD.Rows[i].Cells["Image"].Value = image;
                    }
                    else
                    {
                        Image image = Armada_App.Properties.Resources.Red_mark;
                        dgv_S_OCRD.Rows[i].Cells["Image"].Value = image;
                    }
                }
            }
            catch (Exception ex)
            {
                TransLog.traceService(" Error : " + ex.Message);
            }
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            try
            {
                btnFilter.Enabled = false;
                toolStripProgressBar1.Maximum = dgv_S_OCRD.RowCount;
                m_oWorker.RunWorkerAsync();

                //string[] strValues = new string[4];
                //Hashtable htCCDet = new Hashtable();
                //string strInterface = System.Configuration.ConfigurationManager.AppSettings["InterDB"].ToString();
                //string strMainDB = System.Configuration.ConfigurationManager.AppSettings["MainDB"].ToString();
                //oDtTransLog = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(TRANSACTIONLOG, strInterface);
                //if (oDtTransLog != null && oDtTransLog.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in oDtTransLog.Rows)
                //    {
                //        try
                //        {
                //            foreach (SAPbobsCOM.Company item in TransLog.objCompany)
                //            {
                //                TransLog.traceService(" Transaction Type : " + dr["Scenario"].ToString());
                //                TransLog.traceService(" Transaction Key : " + dr["Key"].ToString());
                //                TransLog.traceService("Company DB : " + item.CompanyDB);
                //                if (item.Connected)
                //                {
                //                    string strCountry = item.GetCompanyService().GetAdminInfo().Country;
                //                    if (dr["Country"].ToString() == strCountry)
                //                    {
                //                        if (strMainDB != item.CompanyDB)
                //                        {
                //                            Singleton.obj_S_OCRD.Sync((dr["Key"].ToString()), TransType.A, item, strInterface, "", strValues, htCCDet);
                //                        }
                //                    }
                //                    else
                //                    {
                //                        TransLog.traceService(" Error : Company Not Connected.");
                //                    }
                //                }
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            TransLog.traceService(" Error : " + ex.Message);
                //        }
                //    }
                //}
                //MessageBox.Show("Manual Sync Completed...");                

            }
            catch (Exception ex)
            {
                TransLog.traceService("Error Message : " + ex.Message);
                TransLog.traceService(" Error Source : " + ex.Source);
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            S_OCRD.ActiveForm.Close();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                LoadAll();
            }
            catch (Exception ex)
            {                
                 TransLog.traceService("Error Message : " + ex.Message);
            }
        }
    }
}
