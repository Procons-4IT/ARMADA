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
    public partial class S_OINV : Form
    {
        private const string TRANSACTIONLOG = "Armada_Service_M_S_OINV_s";
        //private DataTable oDtTransLog = null;
        private string sQuery = string.Empty;
        private DataTable oCompDT = null;
        private string strCompany = string.Empty;
        private string strWareHouse = string.Empty;
        private SAPbobsCOM.Company oGetCompany = null;
        private DataTable oCCDT = null;
        BackgroundWorker m_oWorker;
        private DataTable oCompanyDT = null;
        private DataTable oShopDT = null;
        private DataTable oShopDT_O = null;
        private string strShopXML = "";

        public S_OINV()
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
            S_OINV.CheckForIllegalCrossThreadCalls = false;
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
                string strInterface = System.Configuration.ConfigurationManager.AppSettings["InterDB"].ToString();
                string strMainDB = System.Configuration.ConfigurationManager.AppSettings["MainDB"].ToString();

                int intCount = 0;
                int intCompleted = dgv_S_OINV.RowCount;

                for (int i = 0; i < dgv_S_OINV.RowCount; i++)
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
                        Hashtable htCCDet = new Hashtable();
                        string[] strValues = new string[4];

                        sQuery = " Select T1.U_COMPANY,T0.U_WAREHOUSE,ISNULL(T0.U_COSTCEN,'') As U_COSTCEN From  ";
                        sQuery += " [@Z_INBOUNDMAPPINGC] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
                        sQuery += " Where T0.U_SHOPID = '" + dgv_S_OINV.Rows[i].Cells["ShopID"].Value.ToString().Trim() + "'";
                        oCompDT = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
                        if (oCompDT != null)
                        {
                            if (oCompDT.Rows.Count > 0)
                            {
                                strCompany = oCompDT.Rows[0]["U_COMPANY"].ToString();
                                strWareHouse = oCompDT.Rows[0]["U_WAREHOUSE"].ToString();
                                strValues[0] = oCompDT.Rows[0]["U_COSTCEN"].ToString();
                                oGetCompany = TransLog.GetCompany(strCompany);

                                sQuery = " Select T0.U_SCREDITCARD,T0.U_CREDITCARD,T0.U_CARDNUMBER,T0.U_CARDVALID,T0.U_PAYMENTMETHOD From  ";
                                sQuery += " [@Z_INBOUNDMAPPINGC1] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
                                sQuery += " Where T1.U_COMPANY = '" + strCompany + "'";
                                oCCDT = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
                                if (oCCDT != null)
                                {
                                    if (oCCDT.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr1 in oCCDT.Rows)
                                        {
                                            htCCDet.Add(dr1["U_SCREDITCARD"], dr1);
                                        }
                                    }
                                }
                            }
                        }

                        TransLog.traceService(" Transaction Type : " + dgv_S_OINV.Rows[i].Cells["Scenario"].Value.ToString().Trim());
                        TransLog.traceService(" Transaction Key : " + dgv_S_OINV.Rows[i].Cells["Source_Key"].Value.ToString().Trim());
                        if (oGetCompany != null)
                        {
                            TransLog.traceService("Company DB : " + oGetCompany.CompanyDB);
                            if (oGetCompany.Connected)
                            {
                                Singleton.obj_S_OINV.Sync((dgv_S_OINV.Rows[i].Cells["Source_Key"].Value.ToString().Trim()), TransType.A, oGetCompany, strInterface, strWareHouse, strValues, htCCDet);

                                string strQuery = "Select Status,Remarks From dbo.Z_OTXN Where Scenario = '" + dgv_S_OINV.Rows[i].Cells["Scenario"].Value.ToString().Trim() + "' ";
                                strQuery += " And S_DocNo = '" + dgv_S_OINV.Rows[i].Cells["Source_Key"].Value.ToString().Trim() + "' ";
                                DataTable oStatus = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(strQuery, strInterface);
                                if (oStatus != null && oStatus.Rows.Count > 0)
                                {
                                    if (oStatus.Rows[0][0].ToString() == "1")
                                    {
                                        blnSync = true;
                                        dgv_S_OINV.Rows[i].Cells["Remarks"].Value = string.Empty;
                                    }
                                    else
                                    {
                                        blnSync = false;
                                        dgv_S_OINV.Rows[i].Cells["Remarks"].Value = oStatus.Rows[0][1].ToString();
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
                            TransLog.traceService(" Error : Company Not Found.");

                        if (blnSync)
                        {
                            Image image = Armada_App.Properties.Resources.Yes1;
                            dgv_S_OINV.Rows[i].Cells["Image"].Value = image;
                        }
                        else
                        {
                            Image image = Armada_App.Properties.Resources.Error1;
                            dgv_S_OINV.Rows[i].Cells["Image"].Value = image;
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

        private void S_OINV_Load(object sender, EventArgs e)
        {
            try
            {
                UXUTIL.clsUtilities.setAllControlsThemes(this);
                this.WindowState = FormWindowState.Maximized;
                loadCompanyAndBranchList();
                //LoadAll();
                //loadError();
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
                DataTable oDS_M_S_OINV = null;
                DateTime oFDate = Fromdate.Value;
                DateTime oTDate = ToDate.Value;
                LoadShopXML();
                string str_M_S_OINV = "Exec Armada_Service_M_S_OINV_s '" + oFDate.ToString("yyyyMMdd") + "','" + oTDate.ToString("yyyyMMdd") + "','" + strShopXML + "'";
                oDS_M_S_OINV = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(str_M_S_OINV, strInterface);
                dgv_S_OINV.DataSource = oDS_M_S_OINV;
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
                for (int i = 0; i < dgv_S_OINV.RowCount; i++)
                {
                    if (dgv_S_OINV.Rows[i].Cells[1].Value.ToString() == "-1")
                    {
                        Image image = Armada_App.Properties.Resources.Create1;
                        dgv_S_OINV.Rows[i].Cells["Image"].Value = image;
                    }
                    else
                    {
                        Image image = Armada_App.Properties.Resources.Red_mark;
                        dgv_S_OINV.Rows[i].Cells["Image"].Value = image;
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
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            S_OINV.ActiveForm.Close();
        }

        private void cmbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCompany.SelectedIndex >= 0)
            {
                filterShops(cmbCompany.SelectedValue.ToString());
            }
        }

        private void loadCompanyAndBranchList()
        {
            try
            {
                string strMainDB = System.Configuration.ConfigurationManager.AppSettings["MainDB"].ToString();
                string strInterface = System.Configuration.ConfigurationManager.AppSettings["InterDB"].ToString();

                sQuery = " Select 'ALL' As 'Company' ";
                sQuery += " Union All ";
                sQuery += " Select T1.U_COMPANY As 'Company' From  ";
                sQuery += " [@Z_INBOUNDMAPPING] T1 Where ISNULL(T1.U_COMPANY,'') <> '' ";
              
                //sQuery += " Where T0.U_SHOPID = '" + dgv_S_OINV.Rows[i].Cells["ShopID"].Value.ToString().Trim() + "'";               
                oCompanyDT = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
                               
                sQuery = " Select T1.U_COMPANY As 'Company' ,T0.U_SHOPID As 'Shop' From  ";
                sQuery += " [@Z_INBOUNDMAPPINGC] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
                oShopDT = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);

                sQuery = " SELECT DISTINCT Shop As 'Shop' ";
                sQuery += " FROM SALE T0  ";
                sQuery += " Where ISNULL([Status],'') = '' ";
                oShopDT_O = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strInterface);
                
                cmbCompany.DataSource = oCompanyDT;
                cmbCompany.DisplayMember = "Company";
                cmbCompany.ValueMember = "Company";
                cmbCompany.SelectedIndex = 0;

                filterShops(cmbCompany.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                TransLog.traceService(" Error : " + ex.Message);
            }
        }

        private void filterShops(string strCompany)
        {
            try
            {
                //bool Checked = false;
                DataTable oFilterCompany = null;
                DataView dv = new DataView();
                dv = oShopDT.DefaultView;
                DataTable dt = dv.Table;
                if (strCompany != "ALL")
                {
                    dt.DefaultView.RowFilter = "Company = '" + strCompany + "'";
                }
                else
                {
                    dt.DefaultView.RowFilter = " 1 = 1";
                }
            
                //chkShopID.Items.Clear();
                //foreach (DataRow dr in dt.DefaultView.ToTable().Rows)
                //{
                //    chkShopID.Items.Add(dr["Shop"], false);
                //}               
                
                oFilterCompany = dt.DefaultView.ToTable();
                var query =
                from shop in oFilterCompany.AsEnumerable()
                join shop_o in oShopDT_O.AsEnumerable()
                on shop.Field<string>("Shop") equals
                    shop_o.Field<string>("Shop")        
                select new
                {
                    shop = shop_o.Field<string>("Shop")                    
                };

                chkShopID.Items.Clear();
                foreach (var shop_o in query)
                {
                    chkShopID.Items.Add(shop_o.shop, false);
                }

                //Checked = LstLocation.GetItemCheckState(LstLocation.SelectedIndex);
                //if (Checked == true)
                //{
                //    for (index = 0; index <= clbBranch.Items.Count - 1; index++)
                //    {
                //        clbBranch.SetItemChecked(index, true);
                //    }
                //}
                //else
                //{
                //    for (index = 0; index <= clbBranch.Items.Count - 1; index++)
                //    {
                //        clbBranch.SetItemChecked(index, false);
                //    }
                //}

                chkShopID.DisplayMember = "Shop";
                chkShopID.ValueMember = "Shop";
            }
            catch (Exception ex)
            {
                TransLog.traceService(" Error : " + ex.Message);
            }
        }

        private void LoadShopXML()
        {
            try
            {
                int arrcnt = 0;
                string Shop = null;
                string Code = null;
                string[] ShopArr = new string[chkShopID.CheckedItems.Count];
                for (arrcnt = 0; arrcnt <= chkShopID.CheckedItems.Count - 1; arrcnt++)
                {
                    ShopArr[arrcnt] = chkShopID.CheckedItems[arrcnt].ToString();
                }
                Shop = "Shop";
                Code = "ShopID";
                strShopXML = ShopStringArrayToXML(ShopArr, Shop, Code);
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
                for (int index = 0; index <= chkShopID.Items.Count - 1; index++)
                {
                    chkShopID.SetItemChecked(index, false);
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
                for (int index = 0; index <= chkShopID.Items.Count - 1; index++)
                {
                    chkShopID.SetItemChecked(index, true);
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
//string strMainDB = System.Configuration.ConfigurationManager.AppSettings["MainDB"].ToString();
//oDtTransLog = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(TRANSACTIONLOG, strInterface);
//if (oDtTransLog != null && oDtTransLog.Rows.Count > 0)
//{
//    foreach (DataRow dr in oDtTransLog.Rows)
//    {
//        try
//        {
//            Hashtable htCCDet = new Hashtable();
//            string[] strValues = new string[4];

//            sQuery = " Select T1.U_COMPANY,T0.U_WAREHOUSE,ISNULL(T0.U_COSTCEN,'') As U_COSTCEN From  ";
//            sQuery += " [@Z_INBOUNDMAPPINGC] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
//            sQuery += " Where T0.U_SHOPID = '" + dr["ShopID"].ToString() + "'";
//            oCompDT = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
//            if (oCompDT != null)
//            {
//                if (oCompDT.Rows.Count > 0)
//                {
//                    strCompany = oCompDT.Rows[0]["U_COMPANY"].ToString();
//                    strWareHouse = oCompDT.Rows[0]["U_WAREHOUSE"].ToString();
//                    strValues[0] = oCompDT.Rows[0]["U_COSTCEN"].ToString();
//                    oGetCompany = TransLog.GetCompany(strCompany);

//                    sQuery = " Select T0.U_SCREDITCARD,T0.U_CREDITCARD,T0.U_CARDNUMBER,T0.U_CARDVALID,T0.U_PAYMENTMETHOD From  ";
//                    sQuery += " [@Z_INBOUNDMAPPINGC1] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
//                    sQuery += " Where T1.U_COMPANY = '" + strCompany + "'";
//                    oCCDT = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
//                    if (oCCDT != null)
//                    {
//                        if (oCCDT.Rows.Count > 0)
//                        {
//                            foreach (DataRow dr1 in oCCDT.Rows)
//                            {
//                                htCCDet.Add(dr1["U_SCREDITCARD"], dr1);
//                            }
//                        }
//                    }
//                }
//            }

//            TransLog.traceService(" Transaction Type : " + dr["Scenario"].ToString());
//            TransLog.traceService(" Transaction Key : " + dr["Key"].ToString());
//            if (oGetCompany != null)
//            {
//                TransLog.traceService("Company DB : " + oGetCompany.CompanyDB);
//                if (oGetCompany.Connected)
//                {
//                    Singleton.obj_S_OINV.Sync((dr["Key"].ToString()), TransType.A, oGetCompany, strInterface, strWareHouse, strValues, htCCDet);
//                }
//                else
//                {
//                    TransLog.traceService(" Error : Company Not Connected.");
//                }
//            }
//            else
//                TransLog.traceService(" Error : Company Not Found.");

//        }
//        catch (Exception ex)
//        {
//            TransLog.traceService(" Error : " + ex.Message);
//        }                                             
//    }
//}
//MessageBox.Show("Manual Sync Completed...");
//LoadAll();