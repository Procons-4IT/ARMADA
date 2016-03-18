using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using SAPbobsCOM;

namespace Armada_Sync.Scenerio
{
    public class S_OPDN : IArmada_Sync
    {
        private DataSet oDataSet = null;
        private string strQuery = string.Empty;

        #region "Construtor"
        public S_OPDN()
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
                        ((IArmada_Sync)this).Add(strKey, oCompany, strLogger, strWareHouse,strValues,htCCdet);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Singleton.objSqlDataAccess.UpdateLog(strKey, TransScenerio.GRPO.ToString(), "0", "0", 0, "0", ex.Message.ToString(), strLogger);
                //throw;
            }
        }

        void IArmada_Sync.Add(string sKey, SAPbobsCOM.Company oCompany, string strLogger, string strWareHouse, string[] strValues, Hashtable htCCdet)
        {
            //Singleton.traceService("Has Record2");
            DataTable oHeader = null;
            DataTable oDetails = null;
            SAPbobsCOM.Documents oGRPO = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);

            try
            {
                string str_S_OPDN = "Exec Armada_Service_S_OPDN_s '" + sKey + "'";
                oDataSet = Singleton.objSqlDataAccess.ExecuteDataSet(str_S_OPDN, strLogger);
                
                if (oDataSet == null && oDataSet.Tables.Count == 0)
                    return;
                else
                {
                    oHeader = oDataSet.Tables[0];
                    oDetails = oDataSet.Tables[1];

                    if (oHeader != null && oHeader.Rows.Count > 0)
                    {
                        //Singleton.traceService("Has Record");
                        oGRPO.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;

                        //Header Table
                        oGRPO.CardCode = oHeader.Rows[0]["VCode"].ToString();
                        oGRPO.CardName = oHeader.Rows[0]["VName"].ToString();
                        oGRPO.DocDate = Convert.ToDateTime(oHeader.Rows[0]["DocDate"].ToString());
                        oGRPO.NumAtCard = oHeader.Rows[0]["DocNum"].ToString();
                        oGRPO.DocCurrency = oHeader.Rows[0]["DocCur"].ToString();
                        oGRPO.Comments = oHeader.Rows[0]["Remarks"].ToString();
                        oGRPO.DiscountPercent = Convert.ToDouble(oHeader.Rows[0]["DiscPrct"].ToString());
                        oGRPO.UserFields.Fields.Item("U_Z_TrnNum").Value = oHeader.Rows[0]["DocNum"].ToString();

                        if (oDetails.Rows.Count > 0)
                        {
                            foreach (DataRow dr in oDetails.Rows)
                            {
                                oGRPO.Lines.ItemCode = dr["ItemCode"].ToString();
                                oGRPO.Lines.ItemDescription = dr["ItemDesc"].ToString();
                                oGRPO.Lines.Quantity = Convert.ToDouble(dr["Qty"].ToString());
                                oGRPO.Lines.UnitPrice = Convert.ToDouble(dr["UnitPrice"].ToString());
                                oGRPO.Lines.BaseType = 18;
                                oGRPO.Lines.BaseEntry = Convert.ToInt32(dr["BaseEntry"].ToString());
                                oGRPO.Lines.BaseLine = Convert.ToInt32(dr["BaseLine"].ToString());
                                oGRPO.Lines.WarehouseCode = strWareHouse;
                                oGRPO.Lines.Add();
                            }
                        }
                        //oInvoice.DocTotal = Convert.ToDouble(oHeader.Rows[0]["DocTotal"].ToString());

                        int intError = oGRPO.Add();
                        if (intError != 0)
                        {
                            //Singleton.traceService(intError.ToString());
                            Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.GRPO.ToString(), "0", "0", 0, intError.ToString(), oCompany.GetLastErrorDescription().Replace("'", ""), strLogger);   
                                                                             
                        }
                        else
                        {
                            //Singleton.traceService("Success");
                            string strDkey;
                            int intDocNum = 0;
                            oCompany.GetNewObjectCode(out strDkey);
                            if (oGRPO.GetByKey(Convert.ToInt32(strDkey)))
                            {
                                intDocNum = oGRPO.DocNum;
                            }

                            //Singleton.traceService("Payment Successs");
                            Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.GRPO.ToString(), strDkey, intDocNum.ToString(), 1, "", "Armada_Sync Completed Sucessfully", strLogger);                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.ARCreditMemo.ToString(), "0", "0", 0, "0", ex.Message.ToString(), strLogger);                
                throw ex;
            }
            finally
            {
                oHeader = null;
                oDetails = null;
                oDataSet = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oGRPO);
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
