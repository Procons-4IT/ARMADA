using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using SAPbobsCOM;
using System.IO;

namespace Armada_Sync.Scenerio
{
    public class S_OINV : IArmada_Sync
    {
        private DataSet oDataSet = null;
        private string strQuery = string.Empty;

        #region "Construtor"
        public S_OINV()
        { }
        #endregion

        #region "Armada_Sync Members"

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

        public void Sync(string strKey, TransType eTrnType, SAPbobsCOM.Company oCompany, string strLogger, string strWareHouse, string[] strValues, Hashtable htCCdet)
        {
            try
            {
                switch (eTrnType)
                {
                    case TransType.A:
                        ((IArmada_Sync)this).Add(strKey, oCompany, strLogger, strWareHouse, strValues, htCCdet);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Singleton.objSqlDataAccess.UpdateLog(strKey, TransScenerio.ARInvoice.ToString(), "0", "0", 0, "0", ex.Message.ToString(), strLogger);
            }
        }

        void IArmada_Sync.Add(string sKey, SAPbobsCOM.Company oCompany, string strLogger, string strWareHouse, string[] strValues, Hashtable htCCdet)
        {
            DataTable oHeader = null;
            DataTable oDetails = null;
            DataTable oPayments = null;
            SAPbobsCOM.Documents oInvoice = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(BoObjectTypes.oInvoices);
            SAPbobsCOM.Payments oPayment = (SAPbobsCOM.Payments)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);

            try
            {
                string str_S_OINV = "Exec Armada_Service_S_OINV_s '" + sKey + "'";
                traceService("Query :"  + str_S_OINV);
                oDataSet = Singleton.objSqlDataAccess.ExecuteDataSet(str_S_OINV, strLogger);

                if (oDataSet == null && oDataSet.Tables.Count == 0)
                    return;
                else
                {
                    oHeader = oDataSet.Tables[0];
                    oDetails = oDataSet.Tables[1];
                    oPayments = oDataSet.Tables[2];
                    traceService("Adding HASRCROD");

                    if (oHeader != null && oHeader.Rows.Count > 0)
                    {
                        traceService("Adding INvoice");
                        oInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;

                        //Header Table
                        oInvoice.CardCode = oHeader.Rows[0]["Code"].ToString();
                        oInvoice.CardName = oHeader.Rows[0]["Name"].ToString();
                        oInvoice.DocDate = Convert.ToDateTime(oHeader.Rows[0]["DocDate"].ToString());
                        oInvoice.NumAtCard = oHeader.Rows[0]["DocNum"].ToString();
                        oInvoice.DocCurrency = oHeader.Rows[0]["DocCur"].ToString();
                        //oInvoice.DocRate = Convert.ToDouble(oHeader.Rows[0]["DocRate"].ToString());
                        oInvoice.Comments = oHeader.Rows[0]["Remarks"].ToString();
                        oInvoice.DiscountPercent = Convert.ToDouble(oHeader.Rows[0]["DisPrct"].ToString());
                        oInvoice.UserFields.Fields.Item("U_Z_PAYTYPE").Value = oHeader.Rows[0]["DocType"].ToString();
                        oInvoice.UserFields.Fields.Item("U_Z_CASHIER").Value = oHeader.Rows[0]["Cashier"].ToString();
                        oInvoice.UserFields.Fields.Item("U_Z_DOCTIME").Value = oHeader.Rows[0]["DocTime"].ToString();
                        oInvoice.UserFields.Fields.Item("U_Z_TrnNum").Value = oHeader.Rows[0]["DocNum"].ToString();

                        if (oDetails.Rows.Count > 0)
                        {
                            traceService("Adding Details");
                            foreach (DataRow dr in oDetails.Rows)
                            {
                                oInvoice.Lines.ItemCode = dr["ItemCode"].ToString();
                                oInvoice.Lines.ItemDescription = dr["ItemDesc"].ToString();
                                oInvoice.Lines.Quantity = Convert.ToDouble(dr["Qty"].ToString());
                                oInvoice.Lines.UnitPrice = Convert.ToDouble(dr["UnitPrice"].ToString());
                                //oInvoice.Lines.Currency = dr["Currency"].ToString();
                                oInvoice.Lines.WarehouseCode = strWareHouse;
                                if (strValues[0] != "")
                                    oInvoice.Lines.CostingCode = strValues[0];
                                oInvoice.Lines.Add();
                            }
                            traceService("Details Added");
                        }
                        //oInvoice.DocTotal = Convert.ToDouble(oHeader.Rows[0]["DocTotal"].ToString());

                        if (oCompany.InTransaction)
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                        
                        oCompany.StartTransaction();

                        int intError = oInvoice.Add();
                        if (intError != 0)
                        {
                            traceService(oCompany.GetLastErrorDescription());

                            if (oCompany.InTransaction)
                                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                            Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.ARInvoice.ToString(), "0", "0", 0, oCompany.GetLastErrorCode().ToString(), oCompany.GetLastErrorDescription().Replace("'", ""), strLogger);
                        }
                        else
                        {
                            string strDkey;
                            int intDocNum = 0;
                            oCompany.GetNewObjectCode(out strDkey);

                            if (oInvoice.GetByKey(Convert.ToInt32(strDkey)))
                            {
                                intDocNum = oInvoice.DocNum;
                            }

                            if (oHeader.Rows[0]["DocType"].ToString() == "P")
                            {

                                oPayment.CardCode = oHeader.Rows[0]["Code"].ToString();
                                oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                                oPayment.DocDate = Convert.ToDateTime(oHeader.Rows[0]["DocDate"].ToString());
                                oPayment.TaxDate = Convert.ToDateTime(oHeader.Rows[0]["DocDate"].ToString());
                                oPayment.DocCurrency = oHeader.Rows[0]["DocCur"].ToString();

                                oPayment.Invoices.DocEntry = Convert.ToInt32(strDkey);
                                oPayment.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice;
                                oPayment.Invoices.SumApplied = Convert.ToDouble(oHeader.Rows[0]["InvTotal"].ToString());
                                oPayment.Invoices.Add();

                                if (oPayments.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in oPayments.Rows)
                                    {
                                        if (dr["PayMethod"].ToString() == "R")
                                        {
                                            DataRow ccRows = (DataRow)htCCdet[dr["CardType"].ToString()];
                                            if (ccRows != null)
                                            {
                                                oPayment.CreditCards.CreditCard = Convert.ToInt16(ccRows["U_CREDITCARD"].ToString());
                                                oPayment.CreditCards.CreditCardNumber = (ccRows["U_CARDNUMBER"].ToString());
                                                oPayment.CreditCards.CardValidUntil = Convert.ToDateTime(ccRows["U_CARDVALID"].ToString());
                                                oPayment.CreditCards.PaymentMethodCode = Convert.ToInt16(ccRows["U_PAYMENTMETHOD"].ToString());
                                                oPayment.CreditCards.CreditSum = Convert.ToDouble(dr["PayAmount"].ToString());
                                                oPayment.CreditCards.VoucherNum = "1111";
                                                oPayment.CreditCards.Add();
                                            }
                                        }
                                        else
                                        {
                                            oPayment.CashSum = Convert.ToDouble(dr["PayAmount"].ToString());
                                        }
                                    }
                                }

                                int intPayment = oPayment.Add();
                                if (intPayment == 0)
                                {
                                    if (oCompany.InTransaction)
                                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                    traceService("Commited");
                                    Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.ARInvoice.ToString(), strDkey, intDocNum.ToString(), 1, "", "Armada_Sync Completed Sucessfully", strLogger);
                                    traceService("Commited Completed..");

                                }
                                else
                                {
                                    Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.ARInvoice.ToString(), "0", "0", 0, oCompany.GetLastErrorCode().ToString(), oCompany.GetLastErrorDescription().Replace("'", ""), strLogger);
                                    if (oCompany.InTransaction)
                                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                                }

                            }
                            else
                            {
                                if (oCompany.InTransaction)
                                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                traceService("Commited1");
                                Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.ARInvoice.ToString(), strDkey, intDocNum.ToString(), 1, "", "Armada_Sync Completed Sucessfully", strLogger);
                                traceService("Commited Completed..1");
                            }                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.ARInvoice.ToString(), "0", "0", 0, "0", ex.Message.ToString(), strLogger);
                if (oCompany.InTransaction)
                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                traceService("Error 1 : " + ex.Message);
                throw ex;
            }
            finally
            {
                oHeader = null;
                oDetails = null;
                oDataSet = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oInvoice);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oPayment);
            }
        }

        #endregion


        public void Add(string strKey, Company oCompany_S, Company oCompany_D, string strLogger, string strFromWare, string strToWare, string[] strValues)
        {
            throw new NotImplementedException();
        }


        public void Sync(string strKey, TransType eTrnType, Company oCompany_S, Company oCompany_D, string strLogger, string strFromWare, string strToWare, string[] strValues)
        {
            throw new NotImplementedException();
        }
    }
}
