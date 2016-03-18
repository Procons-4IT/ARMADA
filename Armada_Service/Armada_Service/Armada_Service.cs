using System;
using System.Data;
using System.ServiceProcess;
using Armada_Sync;
using System.Timers;
using System.IO;
using System.Collections;

namespace Armada_Service
{
    public partial class Armada_Service : ServiceBase
    {
        private const string TRANSACTIONLOG = "Armada_Service_TxnLog_S";
        private DataTable oDtTransLog = null;
        private SAPbobsCOM.Company[] objCompany = null;
        Timer tmrReset = new Timer();
        private bool blnInProcess = false;
        private Hashtable oCompanyht = null;
        private string sQuery = string.Empty;
        private SAPbobsCOM.Company oGetCompany = null;
        private SAPbobsCOM.Company oGetCompany_T = null;
        private DataTable oCompDT = null;
        private DataTable oCCDT = null;
        private string strCompany = string.Empty;
        private string strWareHouse = string.Empty;
        private string strCompany_T = string.Empty;
        private string strWareHouse_T = string.Empty;


        public Armada_Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                traceService("Started Service :" + DateTime.Now);
                tmrReset.Interval = 30000;
                tmrReset.Enabled = true;
                tmrReset.Elapsed += new ElapsedEventHandler(tmrReset_Elapsed);
                this.ConnectAllSapCompany();
            }
            catch (Exception ex)
            {
                traceService(ex.StackTrace.ToString());
            }
        }

        protected override void OnStop()
        {
            try
            {
                traceService("Stopped Service :" + DateTime.Now);
                disConnectCompany();
                tmrReset.Stop();
            }
            catch (Exception ex)
            {
                traceService(ex.StackTrace.ToString());
            }
        }

        protected override void OnPause()
        {
            try
            {
                traceService("Service Pause :" + DateTime.Now);
                tmrReset.Stop();
            }
            catch (Exception ex)
            {
                traceService(ex.StackTrace.ToString());
            }
        }

        protected override void OnContinue()
        {
            try
            {
                traceService("Service Continues :" + DateTime.Now);
                tmrReset.Start();
            }
            catch (Exception ex)
            {
                traceService(ex.StackTrace.ToString());
            }
        }

        private void tmrReset_Elapsed(object source, ElapsedEventArgs e)
        {
            try
            {
                traceService("Timer Reset At :" + DateTime.Now);
                if (!blnInProcess)
                {
                    transactionLogic();
                    if (true)
                    {
                        SendMail();
                    }
                }
                else
                {
                    traceService("Still In Process..." + DateTime.Now);
                }
                traceService("Timer Elapses At :" + DateTime.Now);
            }
            catch (Exception ex)
            {
                traceService(ex.StackTrace.ToString());
            }
        }

        private void transactionLogic()
        {
            try
            {                
                this.traceService(this.blnInProcess.ToString());
                string strInterface = System.Configuration.ConfigurationManager.AppSettings["InterDB"].ToString();
                string strMainDB = System.Configuration.ConfigurationManager.AppSettings["MainDB"].ToString();
                oDtTransLog = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(TRANSACTIONLOG, strInterface);
                if (oDtTransLog != null && oDtTransLog.Rows.Count > 0)
                {
                    blnInProcess = true;
                    traceService("Sync Starts...");

                    foreach (DataRow dr in oDtTransLog.Rows)
                    {
                        try
                        {

                            traceService(" Transaction Type : " + dr["Scenario"].ToString());
                            traceService(" Transaction Key : " + dr["Key"].ToString());

                            Hashtable htCCDet = new Hashtable();
                            string[] strValues = new string[4];

                            if (dr["Scenario"].ToString() != "InventoryTransfer")
                            {
                                sQuery = " Select T1.U_COMPANY,T0.U_WAREHOUSE,ISNULL(T0.U_COSTCEN,'') As U_COSTCEN From  ";
                                sQuery += " [@Z_INBOUNDMAPPINGC] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
                                sQuery += " Where T0.U_SHOPID = '" + dr["ShopID"].ToString() + "'";
                                oCompDT = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
                                if (oCompDT != null)
                                {
                                    if (oCompDT.Rows.Count > 0)
                                    {
                                        strCompany = oCompDT.Rows[0]["U_COMPANY"].ToString();
                                        strWareHouse = oCompDT.Rows[0]["U_WAREHOUSE"].ToString();
                                        oGetCompany = GetCompany(strCompany);
                                        strValues[0] = oCompDT.Rows[0]["U_COSTCEN"].ToString();
                                    }
                                }

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
                            else
                            {
                                sQuery = " Select T1.U_COMPANY,T0.U_WAREHOUSE,T0.U_GIACCT From  ";
                                sQuery += " [@Z_INBOUNDMAPPINGC] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
                                sQuery += " Where T0.U_SHOPID = '" + dr["ShopID"].ToString() + "'";
                                oCompDT = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
                                if (oCompDT != null)
                                {
                                    if (oCompDT.Rows.Count > 0)
                                    {
                                        strCompany = oCompDT.Rows[0]["U_COMPANY"].ToString();
                                        strWareHouse = oCompDT.Rows[0]["U_WAREHOUSE"].ToString();
                                        strValues[1] = oCompDT.Rows[0]["U_GIACCT"].ToString();
                                        oGetCompany = GetCompany(strCompany);
                                    }
                                }

                                sQuery = " Select T1.U_COMPANY,T0.U_WAREHOUSE,T0.U_PRICELIST,T0.U_GRACCT From  ";
                                sQuery += " [@Z_INBOUNDMAPPINGC] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
                                sQuery += " Where T0.U_SHOPID = '" + dr["ShopID_T"].ToString() + "'";
                                oCompDT = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
                                if (oCompDT != null)
                                {
                                    if (oCompDT.Rows.Count > 0)
                                    {
                                        strCompany_T = oCompDT.Rows[0]["U_COMPANY"].ToString();
                                        strWareHouse_T = oCompDT.Rows[0]["U_WAREHOUSE"].ToString();
                                        oGetCompany_T = GetCompany(strCompany_T);
                                        strValues[0] = oCompDT.Rows[0]["U_PRICELIST"].ToString();
                                        strValues[2] = oCompDT.Rows[0]["U_GRACCT"].ToString();                                      
                                    }
                                }
                            }

                            switch ((TransScenerio)Enum.Parse(typeof(TransScenerio), dr["Scenario"].ToString()))
                            {
                                case TransScenerio.Customer:
                                    foreach (SAPbobsCOM.Company item in objCompany)
                                    {
                                        if (item.Connected)
                                        {
                                            string strCountry = item.GetCompanyService().GetAdminInfo().Country;
                                            traceService("Company DB : " + item.CompanyDB);
                                            if (dr["Country"].ToString() == strCountry)
                                            {
                                                Singleton.obj_S_OCRD.Sync((dr["Key"].ToString()), TransType.A, item, strInterface, "", strValues, htCCDet);
                                            }
                                        }
                                        else
                                            traceService(" Error : Company Not Connected.");
                                    }
                                    break;
                                case TransScenerio.ARInvoice:
                                    if (oGetCompany != null)
                                    {
                                        if (oGetCompany.Connected)
                                        {
                                            traceService("Company DB : " + oGetCompany.CompanyDB);
                                            Singleton.obj_S_OINV.Sync((dr["Key"].ToString()), TransType.A, oGetCompany, strInterface, strWareHouse, strValues, htCCDet);
                                        }
                                        else
                                            traceService(" Error : Company Not Connected.");
                                    }
                                    else
                                        traceService(" Error : Company Not Found.");
                                    break;
                                case TransScenerio.ARCreditMemo:
                                    if (oGetCompany != null)
                                    {
                                        if (oGetCompany.Connected)
                                        {
                                            traceService("Company DB : " + oGetCompany.CompanyDB);
                                            Singleton.obj_S_ORIN.Sync((dr["Key"].ToString()), TransType.A, oGetCompany, strInterface, strWareHouse, strValues, htCCDet);
                                        }
                                        else
                                            traceService(" Error : Company Not Connected.");
                                    }
                                    else
                                        traceService(" Error : Company Not Found.");
                                    break;
                                case TransScenerio.InventoryTransfer:
                                    if (oGetCompany != null && oGetCompany_T != null)
                                    {
                                        if (oGetCompany.Connected && oGetCompany_T.Connected)
                                        {
                                            traceService("From Company DB : " + oGetCompany.CompanyDB);
                                            traceService("To Company DB : " + oGetCompany.CompanyDB);

                                            if (oGetCompany.CompanyDB != oGetCompany_T.CompanyDB)
                                            {
                                                //Overriding the Good Issue Account if transaction happening from Main Company
                                                if (oGetCompany.CompanyDB == strMainDB)
                                                {
                                                    sQuery = " Select T0.U_GIACCT From  ";
                                                    sQuery += " [@Z_INBOUNDMAPPINGC2] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
                                                    sQuery += " Where T1.U_COMPANY = '" + oGetCompany_T.CompanyDB + "'";
                                                    DataTable oCompDT1 = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
                                                    if (oCompDT1 != null)
                                                    {
                                                        if (oCompDT1.Rows.Count > 0)
                                                        {
                                                            strValues[1] = oCompDT.Rows[0]["U_GIACCT"].ToString();
                                                        }
                                                    }
                                                }

                                                //Overriding the Good Recipt Account if transaction happening from Main Company
                                                if (oGetCompany_T.CompanyDB == strMainDB)
                                                {
                                                    sQuery = " Select T0.U_GRACCT From  ";
                                                    sQuery += " [@Z_INBOUNDMAPPINGC2] T0 JOIN [@Z_INBOUNDMAPPING] T1 On T0.Code = T1.Code ";
                                                    sQuery += " Where T1.U_COMPANY = '" + oGetCompany.CompanyDB + "'";
                                                    DataTable oCompDT1 = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader(sQuery, strMainDB);
                                                    if (oCompDT1 != null)
                                                    {
                                                        if (oCompDT1.Rows.Count > 0)
                                                        {
                                                            strValues[2] = oCompDT.Rows[0]["U_GRACCT"].ToString();
                                                        }
                                                    }
                                                }
                                            }

                                            Singleton.obj_S_OWTR.Sync((dr["Key"].ToString()), TransType.A, oGetCompany, oGetCompany_T, strInterface, strWareHouse, strWareHouse_T,strValues);
                                        }
                                        else
                                            traceService(" Error : Either One of the Company Not Connected.");
                                    }
                                    else
                                        traceService(" Error : Either One of the Company Not Found.");
                                    break;
                                case TransScenerio.GRPO:
                                    if (oGetCompany != null)
                                    {
                                        if (oGetCompany.Connected)
                                        {
                                            traceService("Company DB : " + oGetCompany.CompanyDB);
                                            Singleton.obj_S_OPDN.Sync((dr["Key"].ToString()), TransType.A, oGetCompany, strInterface, strWareHouse, strValues, htCCDet);
                                        }
                                        else
                                            traceService(" Error : Company Not Connected.");
                                    }
                                    else
                                        traceService(" Error : Company Not Found.");
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            traceService(ex.StackTrace.ToString());
                            traceService(ex.Message.ToString());
                        }
                    }
                    
                    blnInProcess = false;
                    traceService("Sync Ends...");
                    this.traceService(this.blnInProcess.ToString());
                }
            }
            catch (Exception ex)
            {
                traceService(ex.StackTrace.ToString());
                traceService(ex.Message.ToString());
            }
            finally
            {
                blnInProcess = false;
                oDtTransLog = null;
            }
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

        public SAPbobsCOM.Company GetCompany(string strCompany)
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

        private void disConnectCompany()
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
        
        private void SendMail()
        {
            try
            {
                string strInterface = System.Configuration.ConfigurationManager.AppSettings["InterDB"].ToString();
                DataTable dtMailLog = Armada_Sync.Singleton.objSqlDataAccess.ExecuteReader("Select DocEntry From Z_OMTX Where Convert(VarChar(8),TransDate,112) = Convert(VarChar(8),GetDate()-1,112)", strInterface);
                if (dtMailLog != null)
                {
                    if (dtMailLog.Rows.Count == 0)
                    {
                        Armada_SendMail.clsTransMail oSendMail = new Armada_SendMail.clsTransMail();
                        oSendMail.LoadMailDetails(strInterface);
                    }
                }               
            }
            catch (Exception ex)
            {
                traceService(ex.StackTrace.ToString());
                traceService(ex.Message.ToString());
                traceService(ex.Source.ToString());
            }
        }

        private void traceService(string content)
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

    }
}




