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
using System.Xml.Linq;

namespace Armada_App
{
    public partial class S_OWTR : Form
    {
        private const string TRANSACTIONLOG = "Armada_Service_M_S_OWTR_s";
        private DataTable oDtTransLog = null;
        private string sQuery = string.Empty;
        private SAPbobsCOM.Company oGetCompany = null;
        private SAPbobsCOM.Company oGetCompany_T = null;
        private DataTable oCompDT = null;
        private string strCompany = string.Empty;
        private string strWareHouse = string.Empty;
        private string strCompany_T = string.Empty;
        private string strWareHouse_T = string.Empty;
        BackgroundWorker m_oWorker;
        private DataTable oCompanyDT = null;
        private DataTable oLocationDT = null;
        private string strLocationXML = "";

        public S_OWTR()
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
            S_OWTR.CheckForIllegalCrossThreadCalls = false;
        }

        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                StatusLabel.Text = "Sync Process Completed...";
                loadLocationAndLocationList();
                LoadAll();
                btnFilter.Enabled = true;                
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
                string strInterface = System.Configuration.ConfigurationManager.AppSettings["InterDB"].ToString();
                string strMainDB = System.Configuration.ConfigurationManager.AppSettings["MainDB"].ToString();


                int intCount = 0;
                int intCompleted = dgv_S_OWTR.RowCount;

                for (int i = 0; i < dgv_S_OWTR.RowCount; i++)
                {
                    intCount += 1;
                    if (intCount == (intCompleted / 10))
                    {
                        toolStripProgressBar1.PerformStep();
                        intCount = 0;
                    } 

                    try
                    {
                        bool blnSync = false;
                        string[] strValues = new string[4];

                        sQuery = " Select T1.U_COMPANY,T0.U_WAREHOUSE,T0.U_GIACCT From  ";
                        sQuery += " [@Z_INBOUNDMAPPINGC] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
                        sQuery += " Where T0.U_LOCATION = '" + dgv_S_OWTR.Rows[i].Cells["ShopID"].Value.ToString().Trim() + "'";
                        oCompDT = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
                        if (oCompDT != null)
                        {
                            if (oCompDT.Rows.Count > 0)
                            {
                                strCompany = oCompDT.Rows[0][0].ToString();
                                strWareHouse = oCompDT.Rows[0][1].ToString();
                                strValues[1] = oCompDT.Rows[0]["U_GIACCT"].ToString();
                                oGetCompany = TransLog.GetCompany(strCompany);
                            }
                        }

                        sQuery = " Select T1.U_COMPANY,T0.U_WAREHOUSE,T0.U_PRICELIST,T0.U_GRACCT From  ";
                        sQuery += " [@Z_INBOUNDMAPPINGC] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
                        sQuery += " Where T0.U_LOCATION = '" + dgv_S_OWTR.Rows[i].Cells["ToShop"].Value.ToString().Trim() + "'";
                        oCompDT = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
                        if (oCompDT != null)
                        {
                            if (oCompDT.Rows.Count > 0)
                            {
                                strCompany_T = oCompDT.Rows[0][0].ToString();
                                strWareHouse_T = oCompDT.Rows[0][1].ToString();
                                oGetCompany_T = TransLog.GetCompany(strCompany_T);
                                strValues[0] = oCompDT.Rows[0]["U_PRICELIST"].ToString();
                                strValues[2] = oCompDT.Rows[0]["U_GRACCT"].ToString();
                            }
                        }

                        TransLog.traceService(" Transaction Type : " + dgv_S_OWTR.Rows[i].Cells["Scenario"].Value.ToString().Trim());
                        TransLog.traceService(" Transaction Key : " + dgv_S_OWTR.Rows[i].Cells["Source_Key"].Value.ToString().Trim());

                        if (oGetCompany != null && oGetCompany_T != null)
                        {
                            TransLog.traceService("Source Company DB : " + oGetCompany.CompanyDB);
                            TransLog.traceService("Destination Company DB : " + oGetCompany_T.CompanyDB);

                            if (oGetCompany.Connected && oGetCompany_T.Connected)
                            {
                                if (oGetCompany.CompanyDB != oGetCompany_T.CompanyDB)
                                {
                                    //Overriding the Good Issue Account if transaction happening from Main Company
                                    if (oGetCompany.CompanyDB == strMainDB)
                                    {
                                        sQuery = " Select T0.U_GIACCT From  ";
                                        sQuery += " [@Z_INBOUNDMAPPINGC2] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
                                        sQuery += " Where T1.U_COMPANY = '" + strMainDB + "'";
                                        sQuery += " And T0.U_COMPANY = '" + oGetCompany_T.CompanyDB + "'";
                                        DataTable oCompDT1 = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
                                        if (oCompDT1 != null)
                                        {
                                            if (oCompDT1.Rows.Count > 0)
                                            {
                                                strValues[1] = oCompDT1.Rows[0]["U_GIACCT"].ToString();
                                            }
                                        }
                                    }

                                    //Overriding the Good Recipt Account if transaction happening from Main Company
                                    if (oGetCompany_T.CompanyDB == strMainDB)
                                    {
                                        sQuery = " Select T0.U_GRACCT From  ";
                                        sQuery += " [@Z_INBOUNDMAPPINGC2] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
                                        sQuery += " Where T1.U_COMPANY = '" + strMainDB + "'";
                                        sQuery += " And T0.U_COMPANY = '" + oGetCompany.CompanyDB + "'";
                                        DataTable oCompDT1 = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
                                        if (oCompDT1 != null)
                                        {
                                            if (oCompDT1.Rows.Count > 0)
                                            {
                                                strValues[2] = oCompDT1.Rows[0]["U_GRACCT"].ToString();
                                            }
                                        }
                                    }
                                }


                                Singleton.obj_S_OWTR.Sync((dgv_S_OWTR.Rows[i].Cells["Source_Key"].Value.ToString().Trim()), TransType.A, oGetCompany, oGetCompany_T, strInterface, strWareHouse, strWareHouse_T, strValues);

                                string strQuery = "Select Status,Remarks From dbo.Z_OTXN Where Scenario = '" + dgv_S_OWTR.Rows[i].Cells["Scenario"].Value.ToString().Trim() + "' ";
                                strQuery += " And S_DocNo = '" + dgv_S_OWTR.Rows[i].Cells["Source_Key"].Value.ToString().Trim() + "' ";
                                DataTable oStatus = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(strQuery, strInterface);
                                if (oStatus != null && oStatus.Rows.Count > 0)
                                {
                                    if (oStatus.Rows[0][0].ToString() == "1")
                                    {
                                        blnSync = true;
                                        dgv_S_OWTR.Rows[i].Cells["Remarks"].Value = string.Empty;
                                    }
                                    else
                                    {
                                        blnSync = false;
                                        dgv_S_OWTR.Rows[i].Cells["Remarks"].Value = oStatus.Rows[0][1].ToString();
                                    }
                                }
                                else
                                {
                                    blnSync = false;
                                }
                            }
                            else
                            {
                                TransLog.traceService(" Error : Company Not Connected.");
                            }
                        }
                        else
                            TransLog.traceService(" Error : Company Not Connected.");

                        if (blnSync)
                        {
                            Image image = Armada_App.Properties.Resources.Yes1;
                            dgv_S_OWTR.Rows[i].Cells["Image"].Value = image;
                        }
                        else
                        {
                            Image image = Armada_App.Properties.Resources.Error1;
                            dgv_S_OWTR.Rows[i].Cells["Image"].Value = image;
                        }
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

        private void S_OWTR_Load(object sender, EventArgs e)
        {
            try
            {
                UXUTIL.clsUtilities.setAllControlsThemes(this);
                this.WindowState = FormWindowState.Maximized;
                loadLocationAndLocationList();
                //LoadAll();
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
                DataTable oDS_M_S_OWTR = null;
                DateTime oFDate = Fromdate.Value;
                DateTime oTDate = ToDate.Value;
                string strFLocation = cmbFLocation.SelectedValue.ToString();
                LoadTLocationXML();
                string str_M_S_OWTR = "Exec Armada_Service_M_S_OWTR_s '" + oFDate.ToString("yyyyMMdd") + "','" + oTDate.ToString("yyyyMMdd") + "','" + strFLocation + "','" + strLocationXML + "'";
                oDS_M_S_OWTR = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(str_M_S_OWTR, strInterface);
                dgv_S_OWTR.DataSource = oDS_M_S_OWTR;
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
                for (int i = 0; i < dgv_S_OWTR.RowCount; i++)
                {
                    if (dgv_S_OWTR.Rows[i].Cells[1].Value.ToString() == "-1")
                    {
                        Image image = Armada_App.Properties.Resources.Create1;
                        dgv_S_OWTR.Rows[i].Cells["Image"].Value = image;
                    }
                    else
                    {
                        Image image = Armada_App.Properties.Resources.Red_mark;
                        dgv_S_OWTR.Rows[i].Cells["Image"].Value = image;
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
                m_oWorker.RunWorkerAsync();
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
            S_OWTR.ActiveForm.Close();
        }

        private void cmbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFLocation.SelectedIndex >= 0)
            {
                //filterTLocations(cmbFLocation.SelectedValue.ToString());
                string strInterface = System.Configuration.ConfigurationManager.AppSettings["InterDB"].ToString();

                sQuery = " Select Distinct ToLoc As 'TLocation' From INTR   ";
                sQuery += " Where ISNULL(Status,'') <> 'Success'  ";
                if (cmbFLocation.SelectedValue.ToString() != "ALL")
                {
                    sQuery += " And FrmLoc = '" + cmbFLocation.SelectedValue.ToString() + "'";    
                }
                oLocationDT = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strInterface);
                chkLocationID.Items.Clear();
                foreach (DataRow dr in oLocationDT.Rows)
                {
                    chkLocationID.Items.Add(dr["TLocation"], false);
                }
                chkLocationID.DisplayMember = "TLocation";
                chkLocationID.ValueMember = "TLocation";
            }
        }

        private void loadLocationAndLocationList()
        {
            try
            {
                string strInterface = System.Configuration.ConfigurationManager.AppSettings["InterDB"].ToString();
                sQuery = " Select 'ALL' As 'FLocation' ";
                sQuery += " Union All ";
                sQuery += " Select Distinct FrmLoc As 'FLocation' From INTR Where ISNULL(Status,'') <> 'Success'  ";
                oCompanyDT = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strInterface);                              
                cmbFLocation.DataSource = oCompanyDT;
                cmbFLocation.DisplayMember = "FLocation";
                cmbFLocation.ValueMember = "FLocation";
                cmbFLocation.SelectedIndex = 0;

                sQuery = " Select Distinct ToLoc As 'TLocation' From INTR   ";
                sQuery += " Where ISNULL(Status,'') <> 'Success'  ";
                //sQuery += " And FrmLoc = '" + cmbFLocation.SelectedValue.ToString() + "'";
                oLocationDT = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strInterface);
                chkLocationID.Items.Clear();
                foreach (DataRow dr in oLocationDT.Rows)
                {
                    chkLocationID.Items.Add(dr["TLocation"], false);
                }
                chkLocationID.DisplayMember = "TLocation";
                chkLocationID.ValueMember = "TLocation";

                //filterTLocations(cmbFLocation.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                TransLog.traceService(" Error : " + ex.Message);
            }
        }

        //private void filterTLocations(string strFLocation)
        //{
        //    try
        //    {              
        //        DataView dv = new DataView();
        //        dv = oLocationDT.DefaultView;
        //        DataTable dt = dv.Table;
        //        dt.DefaultView.RowFilter = "FLocation = '" + strFLocation + "'";
        //        chkLocationID.Items.Clear();
        //        foreach (DataRow dr in dt.DefaultView.ToTable().Rows)
        //        {
        //            chkLocationID.Items.Add(dr["TLocation"], false);
        //        }

        //        chkLocationID.DisplayMember = "TLocation";
        //        chkLocationID.ValueMember = "TLocation";
        //    }
        //    catch (Exception ex)
        //    {
        //        TransLog.traceService(" Error : " + ex.Message);
        //    }
        //}

        private void LoadTLocationXML()
        {
            try
            {
                int arrcnt = 0;
                string Shop = null;
                string Code = null;
                string[] LocArr = new string[chkLocationID.CheckedItems.Count];
                for (arrcnt = 0; arrcnt <= chkLocationID.CheckedItems.Count - 1; arrcnt++)
                {
                    LocArr[arrcnt] = chkLocationID.CheckedItems[arrcnt].ToString();
                }
                Shop = "Location";
                Code = "LocationID";
                strLocationXML = ShopStringArrayToXML(LocArr, Shop, Code);
            }
            catch (Exception ex)
            {
                TransLog.traceService(" Error : " + ex.Message);
            }

        }

        private String ShopStringArrayToXML(String[] Array, String Element, String Attribute)
        {
            XElement identity = new XElement(Element);
            try
            {
                for (int i = 0; i <= Array.Length - 1; i++)
                {
                    if (Array[i] != null)
                    {
                        XElement elm = new XElement(Attribute, Array[i].Trim());
                        identity.Add(elm);
                    }
                }
            }
            catch (Exception ex)
            {
                TransLog.traceService(" Error : " + ex.Message);
            }

            return identity.ToString();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                LoadAll();
            }
            catch (Exception ex)
            {
                TransLog.traceService(" Error : " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                for (int index = 0; index <= chkLocationID.Items.Count - 1; index++)
                {
                    chkLocationID.SetItemChecked(index, false);
                }
            }
            catch (Exception ex)
            {
                TransLog.traceService(" Error : " + ex.Message);
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                for (int index = 0; index <= chkLocationID.Items.Count - 1; index++)
                {
                    chkLocationID.SetItemChecked(index, true);
                }
            }
            catch (Exception ex)
            {
                TransLog.traceService(" Error : " + ex.Message);
            }
        }
    }
}




//string strInterface = System.Configuration.ConfigurationManager.AppSettings["InterDB"].ToString();
//                string strMainDB = System.Configuration.ConfigurationManager.AppSettings["MainDB"].ToString();
//                oDtTransLog = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(TRANSACTIONLOG, strInterface);
//                if (oDtTransLog != null && oDtTransLog.Rows.Count > 0)
//                {
//                    foreach (DataRow dr in oDtTransLog.Rows)
//                    {
//                        try
//                        {
//                            string[] strValues = new string[4];

//                            sQuery = " Select T1.U_COMPANY,T0.U_WAREHOUSE,T0.U_GIACCT From  ";
//                            sQuery += " [@Z_INBOUNDMAPPINGC] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
//                            sQuery += " Where T0.U_LOCATION = '" + dr["ShopID"].ToString() + "'";
//                            oCompDT = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
//                            if (oCompDT != null)
//                            {
//                                if (oCompDT.Rows.Count > 0)
//                                {
//                                    strCompany = oCompDT.Rows[0][0].ToString();
//                                    strWareHouse = oCompDT.Rows[0][1].ToString();
//                                    strValues[1] = oCompDT.Rows[0]["U_GIACCT"].ToString();
//                                    oGetCompany = TransLog.GetCompany(strCompany);                                    
//                                }
//                            }

//                            sQuery = " Select T1.U_COMPANY,T0.U_WAREHOUSE,T0.U_PRICELIST,T0.U_GRACCT From  ";
//                            sQuery += " [@Z_INBOUNDMAPPINGC] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
//                            sQuery += " Where T0.U_LOCATION = '" + dr["ShopID_T"].ToString() + "'";
//                            oCompDT = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
//                            if (oCompDT != null)
//                            {
//                                if (oCompDT.Rows.Count > 0)
//                                {
//                                    strCompany_T = oCompDT.Rows[0][0].ToString();
//                                    strWareHouse_T = oCompDT.Rows[0][1].ToString();
//                                    oGetCompany_T = TransLog.GetCompany(strCompany_T);
//                                    strValues[0] = oCompDT.Rows[0]["U_PRICELIST"].ToString();                                   
//                                    strValues[2] = oCompDT.Rows[0]["U_GRACCT"].ToString();                                    
//                                }
//                            }


//                            TransLog.traceService(" Transaction Type : " + dr["Scenario"].ToString());
//                            TransLog.traceService(" Transaction Key : " + dr["Key"].ToString());

//                            if (oGetCompany != null && oGetCompany_T != null)
//                            {
//                                TransLog.traceService("From Company DB : " + oGetCompany.CompanyDB);
//                                TransLog.traceService("To Company DB : " + oGetCompany_T.CompanyDB);

//                                if (oGetCompany.Connected && oGetCompany_T.Connected)
//                                {
//                                    if (oGetCompany.CompanyDB != oGetCompany_T.CompanyDB)
//                                    {
//                                        //Overriding the Good Issue Account if transaction happening from Main Company
//                                        if (oGetCompany.CompanyDB == strMainDB)
//                                        {
//                                            sQuery = " Select T0.U_GIACCT From  ";
//                                            sQuery += " [@Z_INBOUNDMAPPINGC2] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
//                                            sQuery += " Where T1.U_COMPANY = '" + oGetCompany_T.CompanyDB + "'";
//                                            DataTable oCompDT1 = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
//                                            if (oCompDT1 != null)
//                                            {
//                                                if (oCompDT1.Rows.Count > 0)
//                                                {
//                                                    strValues[1] = oCompDT.Rows[0]["U_GIACCT"].ToString();
//                                                }
//                                            }
//                                        }

//                                        //Overriding the Good Recipt Account if transaction happening from Main Company
//                                        if (oGetCompany_T.CompanyDB == strMainDB)
//                                        {
//                                            sQuery = " Select T0.U_GRACCT From  ";
//                                            sQuery += " [@Z_INBOUNDMAPPINGC2] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
//                                            sQuery += " Where T1.U_COMPANY = '" + oGetCompany.CompanyDB + "'";
//                                            DataTable oCompDT1 = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
//                                            if (oCompDT1 != null)
//                                            {
//                                                if (oCompDT1.Rows.Count > 0)
//                                                {
//                                                    strValues[2] = oCompDT.Rows[0]["U_GRACCT"].ToString();
//                                                }
//                                            }
//                                        }
//                                    }                                   

//                                    Singleton.obj_S_OWTR.Sync((dr["Key"].ToString()), TransType.A, oGetCompany, oGetCompany_T, strInterface, strWareHouse, strWareHouse_T, strValues);
//                                }
//                                else
//                                {
//                                    TransLog.traceService(" Error : Either One of the Company Not Connected.");
//                                }
//                            }
//                            else
//                                TransLog.traceService(" Error : Either One of the Company Not Found.");

//                        }
//                        catch (Exception ex)
//                        {
//                            TransLog.traceService(" Error : " + ex.Message);
//                        }                        
//                    }                    
//                }
//                MessageBox.Show("Manual Sync Completed...");
//                LoadAll();