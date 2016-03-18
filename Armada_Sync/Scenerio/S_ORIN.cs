using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using SAPbobsCOM;

namespace Armada_Sync.Scenerio
{
    public class S_ORIN : IArmada_Sync
    {
        private DataSet oDataSet = null;
        private string strQuery = string.Empty;

        #region "Construtor"
        public S_ORIN()
        { }
        #endregion

        #region "Armada_Sync Members"

        public void Sync(string strKey, TransType eTrnType, SAPbobsCOM.Company oCompany, string strLogger, string strWareHouse, string[] strValues, Hashtable htCCdet)
        {
            try
            {
                switch (eTrnType)
                {
                    case TransType.A:
                        //Singleton.traceService("Has Record1");
                        ((IArmada_Sync)this).Add(strKey, oCompany, strLogger, strWareHouse,strValues,htCCdet);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Singleton.objSqlDataAccess.UpdateLog(strKey, TransScenerio.ARCreditMemo.ToString(), "0", "0", 0, "0", ex.Message.ToString(), strLogger);
                //throw;
            }
        }

        void IArmada_Sync.Add(string sKey, SAPbobsCOM.Company oCompany, string strLogger, string strWareHouse, string[] strValues, Hashtable htCCdet)
        {
            //Singleton.traceService("Has Record2");
            DataTable oHeader = null;
            DataTable oDetails = null;
            DataTable oPayments = null;
            SAPbobsCOM.Documents oInvoiceR = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(BoObjectTypes.oCreditNotes);
            SAPbobsCOM.Payments oPayment = (SAPbobsCOM.Payments)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oVendorPayments);

            try
            {
                string str_S_ORIN = "Exec Armada_Service_S_ORIN_s '" + sKey + "'";
                oDataSet = Singleton.objSqlDataAccess.ExecuteDataSet(str_S_ORIN , strLogger);
                
                if (oDataSet == null && oDataSet.Tables.Count == 0)
                    return;
                else
                {
                    oHeader = oDataSet.Tables[0];
                    oDetails = oDataSet.Tables[1];
                    oPayments = oDataSet.Tables[2];

                    if (oHeader != null && oHeader.Rows.Count > 0)
                    {
                        //Singleton.traceService("Has Record");
                        oInvoiceR.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;

                        //Header Table
                        oInvoiceR.CardCode = oHeader.Rows[0]["Code"].ToString();
                        oInvoiceR.CardName = oHeader.Rows[0]["Name"].ToString();
                        oInvoiceR.DocDate = Convert.ToDateTime(oHeader.Rows[0]["DocDate"].ToString());
                        oInvoiceR.NumAtCard = oHeader.Rows[0]["DocNum"].ToString();
                        oInvoiceR.DocCurrency = oHeader.Rows[0]["DocCur"].ToString();

                        //oInvoice.DocRate = Convert.ToDouble(oHeader.Rows[0]["DocRate"].ToString());
                        oInvoiceR.Comments = oHeader.Rows[0]["Remarks"].ToString();
                        oInvoiceR.UserFields.Fields.Item("U_Z_PAYTYPE").Value = oHeader.Rows[0]["DocType"].ToString();
                        oInvoiceR.UserFields.Fields.Item("U_Z_CASHIER").Value = oHeader.Rows[0]["Cashier"].ToString();
                        oInvoiceR.UserFields.Fields.Item("U_Z_DOCTIME").Value = oHeader.Rows[0]["DocTime"].ToString();
                        oInvoiceR.UserFields.Fields.Item("U_Z_TrnNum").Value = oHeader.Rows[0]["DocNum"].ToString();

                        if (oDetails.Rows.Count > 0)
                        {
                            foreach (DataRow dr in oDetails.Rows)
                            {
                                oInvoiceR.Lines.ItemCode = dr["ItemCode"].ToString();
                                oInvoiceR.Lines.ItemDescription = dr["ItemDesc"].ToString();
                                oInvoiceR.Lines.Quantity = Convert.ToDouble(dr["Qty"].ToString());
                                oInvoiceR.Lines.UnitPrice = Convert.ToDouble(dr["UnitPrice"].ToString());
                                //oInvoice.Lines.Currency = dr["Currency"].ToString();
                                oInvoiceR.Lines.WarehouseCode = strWareHouse;
                                if (strValues[1] != "")
                                    oInvoiceR.Lines.CostingCode = strValues[1];
                                oInvoiceR.Lines.Add();
                            }
                        }
                        //oInvoice.DocTotal = Convert.ToDouble(oHeader.Rows[0]["DocTotal"].ToString());

                        if (oCompany.InTransaction)
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                        oCompany.StartTransaction();

                        int intError = oInvoiceR.Add();
                        if (intError != 0)
                        {
                            //Singleton.traceService(intError.ToString());
                            Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.ARCreditMemo.ToString(), "0", "0", 0, intError.ToString(), oCompany.GetLastErrorDescription().Replace("'", ""), strLogger);   
                            if (oCompany.InTransaction)
                                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);                                                     
                        }
                        else
                        {
                            //Singleton.traceService("Success");
                            string strDkey;
                            int intDocNum = 0;
                            oCompany.GetNewObjectCode(out strDkey);
                            if (oInvoiceR.GetByKey(Convert.ToInt32(strDkey)))
                            {
                                intDocNum = oInvoiceR.DocNum;
                            }

                            //Singleton.traceService("Success2");

                            oPayment.CardCode = oHeader.Rows[0]["Code"].ToString();
                            oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                            oPayment.DocDate = Convert.ToDateTime(oHeader.Rows[0]["DocDate"].ToString());
                            oPayment.TaxDate = Convert.ToDateTime(oHeader.Rows[0]["DocDate"].ToString());
                            oPayment.DocCurrency = oHeader.Rows[0]["DocCur"].ToString();
                            //Singleton.traceService("Success3");

                            oPayment.Invoices.DocEntry = Convert.ToInt32(strDkey);
                            oPayment.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_CredItnote;
                            oPayment.Invoices.SumApplied = Convert.ToDouble(oHeader.Rows[0]["InvTotal"].ToString());
                            oPayment.Invoices.Add();

                            //Singleton.traceService("Success4");

                            if (oPayments.Rows.Count > 0)
                            {
                                foreach (DataRow dr in oPayments.Rows)
                                {
                                    //if (dr["PayMethod"].ToString() == "R")
                                    //{
                                    //    oPayment.CreditCards.CreditCard = 1;
                                    //    oPayment.CreditCards.CreditCardNumber = "1111";
                                    //    oPayment.CreditCards.CardValidUntil = System.DateTime.Now;
                                    //    oPayment.CreditCards.PaymentMethodCode = 1;
                                    //    oPayment.CreditCards.CreditSum = Convert.ToDouble(dr["PayAmount"].ToString());
                                    //    oPayment.CreditCards.VoucherNum = "1111";
                                    //    oPayment.CreditCards.Add();
                                    //    Singleton.traceService("Payment Credit");
                                    //}
                                    //else
                                    {
                                        oPayment.CashAccount = strValues[0].ToString();
                                        oPayment.CashSum = Convert.ToDouble(dr["PayAmount"].ToString());
                                        //Singleton.traceService("Payment Cash");
                                    }
                                }
                            }

                            //Singleton.traceService("Payment Adding");
                            try
                            {
                                int intPayment = oPayment.Add();
                                if (intPayment == 0)
                                {
                                    //Singleton.traceService("Payment Successs");
                                    if (oCompany.InTransaction)
                                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                                    Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.ARCreditMemo.ToString(), strDkey, intDocNum.ToString(), 1, "", "Armada_Sync Completed Sucessfully", strLogger);
                                }
                                else
                                {
                                    //Singleton.traceService(intPayment.ToString());
                                    Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.ARCreditMemo.ToString(), "0", "0", 0, intPayment.ToString(), oCompany.GetLastErrorDescription().Replace("'", ""), strLogger);
                                    if (oCompany.InTransaction)
                                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (oCompany.InTransaction)
                                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                                Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.ARCreditMemo.ToString(), "0", "0", 0, "0", ex.Message.ToString(), strLogger);
                            }
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.ARCreditMemo.ToString(), "0", "0", 0, "0", ex.Message.ToString(), strLogger);
                if (oCompany.InTransaction)
                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                throw ex;
            }
            finally
            {
                oHeader = null;
                oDetails = null;
                oDataSet = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oInvoiceR);
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
