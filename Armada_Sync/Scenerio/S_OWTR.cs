using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;

namespace Armada_Sync.Scenerio
{
    public class S_OWTR : IArmada_Sync
    {
        private DataSet oDataSet = null;
        private string strQuery = string.Empty;

        #region "Contructor"

        public S_OWTR()
        {

        }

        #endregion

        #region Armada_Sync Members

        public void Sync(string strKey, TransType eTrnType, SAPbobsCOM.Company oCompany, string strLogger, string strWareHouse, string[] strValues,Hashtable oht)
        {
            throw new NotImplementedException();
        }

        public void Add(string strKey, SAPbobsCOM.Company oCompany, string strLogger, string strWareHouse, string[] strValues, Hashtable oht)
        {
            throw new NotImplementedException();
        }

        public void Sync(string strKey, TransType eTrnType, SAPbobsCOM.Company oCompany_S, SAPbobsCOM.Company oCompany_D, string strLogger, string strFromWare, string strToWare, string[] strValues)
        {
            try
            {
                switch (eTrnType)
                {
                    case TransType.A:
                        ((IArmada_Sync)this).Add(strKey, oCompany_S, oCompany_D, strLogger, strFromWare, strToWare, strValues);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Singleton.objSqlDataAccess.UpdateLog(strKey, TransScenerio.InventoryTransfer.ToString(), "0", "0", 0, "0", ex.Message.ToString(), strLogger);
                //throw;
            }
        }

        public void Add(string strKey, SAPbobsCOM.Company oCompany_S, SAPbobsCOM.Company oCompany_D, string strLogger, string strFromWare, string strToWare, string[] strValues)
        {
            DataTable oHeader = null;
            DataTable oDetails = null;
            int intTStatus = 0;
            int intExitStatus = 0;
            int intEntryStatus = 0;

            SAPbobsCOM.StockTransfer oStockTransfer = (SAPbobsCOM.StockTransfer)oCompany_S.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);
            SAPbobsCOM.Documents oInventoryExit = (SAPbobsCOM.Documents)oCompany_S.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);
            SAPbobsCOM.Documents oInventoryEntry = (SAPbobsCOM.Documents)oCompany_D.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);

            try
            {
                string str_S_OWTR = "Exec Armada_Service_S_OWTR_s '" + strKey + "'";
                oDataSet = Singleton.objSqlDataAccess.ExecuteDataSet(str_S_OWTR, strLogger);

                if (oDataSet == null && oDataSet.Tables.Count == 0)
                    return;
                else
                {
                    Singleton.traceService("Has Record");

                    oHeader = oDataSet.Tables[0];
                    oDetails = oDataSet.Tables[1];

                    if (oHeader != null && oHeader.Rows.Count > 0)
                    {
                        if (oCompany_S.CompanyDB == oCompany_D.CompanyDB)
                        {
                            Singleton.traceService("Same Company...So Transfer");
                            oStockTransfer.DocDate = Convert.ToDateTime(oHeader.Rows[0]["TrnDate"].ToString());
                            oStockTransfer.TaxDate = Convert.ToDateTime(oHeader.Rows[0]["TrnDate"].ToString());
                            oStockTransfer.FromWarehouse = strFromWare;
                            oStockTransfer.Comments = oHeader.Rows[0]["Remarks"].ToString();
                            oStockTransfer.UserFields.Fields.Item("U_Z_TrnNum").Value = oHeader.Rows[0]["TrnNum"].ToString();

                            Singleton.traceService("Set Header");
                            Singleton.traceService(strFromWare);
                            Singleton.traceService(strToWare);
                            if (oDetails.Rows.Count > 0)
                            {
                                foreach (DataRow dr in oDetails.Rows)
                                {
                                    oStockTransfer.Lines.ItemCode = dr["ItemCode"].ToString().Trim();
                                    oStockTransfer.Lines.Quantity = Convert.ToDouble(dr["Qty"].ToString());
                                    //oStockTransfer.Lines.FromWarehouseCode = strFromWare;
                                    oStockTransfer.Lines.WarehouseCode = strToWare;
                                    oStockTransfer.Lines.Add();
                                }
                                Singleton.traceService("Set Details");
                            }
                            Singleton.traceService("Adding");
                            intTStatus = oStockTransfer.Add();
                            Singleton.traceService(intTStatus.ToString());
                            if (intTStatus != 0)
                            {
                                Singleton.objSqlDataAccess.UpdateLog(strKey, TransScenerio.InventoryTransfer.ToString(), "0", "0", 0, oCompany_S.GetLastErrorCode().ToString(), oCompany_S.GetLastErrorDescription().Replace("'", ""), strLogger);
                            }
                            else
                            {
                                string strDkey;
                                int intDocNum = 0;
                                oCompany_S.GetNewObjectCode(out strDkey);
                                if (oStockTransfer.GetByKey(Convert.ToInt32(strDkey)))
                                {
                                    intDocNum = oStockTransfer.DocNum;
                                }
                                Singleton.objSqlDataAccess.UpdateLog(strKey, TransScenerio.InventoryTransfer.ToString(), strDkey, intDocNum.ToString(), 1, "", "Armada_Sync Completed Sucessfully", strLogger);
                            }
                        }
                        else
                        {
                            //Singleton.traceService("Inventory Exit Started" + oCompany_S.CompanyDB.ToString());
                            //Singleton.traceService(strFromWare);
                            //Singleton.traceService(strToWare);

                            oInventoryExit.DocDate = Convert.ToDateTime(oHeader.Rows[0]["TrnDate"].ToString());
                            oInventoryExit.DocDueDate = Convert.ToDateTime(oHeader.Rows[0]["TrnDate"].ToString());
                            oInventoryExit.Comments  = oHeader.Rows[0]["Remarks"].ToString();
                            oInventoryExit.UserFields.Fields.Item("U_Z_TrnNum").Value = oHeader.Rows[0]["TrnNum"].ToString();
                            //oInventoryExit.Reference2 = oHeader.Rows[0]["Ref2"].ToString();
                            //Singleton.traceService("Set Header");
                            if (oDetails.Rows.Count > 0)
                            {
                                foreach (DataRow dr in oDetails.Rows)
                                {
                                    oInventoryExit.Lines.ItemCode = dr["ItemCode"].ToString();
                                    oInventoryExit.Lines.ItemDescription = dr["ItemDsc"].ToString();
                                    oInventoryExit.Lines.Quantity = Convert.ToDouble(dr["Qty"].ToString());
                                    //oInventoryExit.Lines.ShipDate = Convert.ToDateTime(dr["ShipDate"].ToString());
                                    //oInventoryExit.Lines.UnitPrice = 0;
                                    oInventoryExit.Lines.WarehouseCode = strFromWare;
                                    oInventoryExit.Lines.AccountCode = strValues[1];
                                    oInventoryExit.Lines.Add();
                                }
                            }
                            //Singleton.traceService("Set Details");

                            oCompany_S.StartTransaction();
                            //Singleton.traceService("Adding");
                            intExitStatus = oInventoryExit.Add();
                            //Singleton.traceService("Added");
                            if (intExitStatus != 0)
                            {
                                //Singleton.traceService(intExitStatus.ToString());
                                Singleton.objSqlDataAccess.UpdateLog(strKey, TransScenerio.InventoryTransfer.ToString(), "0", "0", 0, intExitStatus.ToString(), oCompany_S.GetLastErrorDescription().Replace("'", ""), strLogger);
                                if (oCompany_S.InTransaction)
                                    oCompany_S.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                            }
                            else
                            {
                                string strDkey;
                                int intDocNum = 0;
                                oCompany_S.GetNewObjectCode(out strDkey);
                                if (oInventoryExit.GetByKey(Convert.ToInt32(strDkey)))
                                {
                                    intDocNum = oInventoryExit.DocNum;
                                }


                                //Singleton.traceService("Exit Success");
                                //Singleton.traceService(intDocNum.ToString());
                                //Singleton.traceService("Setting Entry Header");
                                oInventoryEntry.DocDate = Convert.ToDateTime(oHeader.Rows[0]["TrnDate"].ToString());
                                oInventoryEntry.DocDueDate = Convert.ToDateTime(oHeader.Rows[0]["TrnDate"].ToString());
                                oInventoryEntry.Comments = oHeader.Rows[0]["Remarks"].ToString();
                                //oInventoryEntry.PaymentGroupCode = Convert.ToInt16(strValues[0]);
                                // oInventoryEntry.Reference2 = oHeader.Rows[0]["Ref2"].ToString();
                                oInventoryEntry.UserFields.Fields.Item("U_Z_TrnNum").Value = oHeader.Rows[0]["TrnNum"].ToString();

                                if (oDetails.Rows.Count > 0)
                                {
                                    //Singleton.traceService("Setting Entry Detials");
                                    foreach (DataRow dr in oDetails.Rows)
                                    {
                                        oInventoryEntry.Lines.ItemCode = dr["ItemCode"].ToString();
                                        oInventoryEntry.Lines.ItemDescription = dr["ItemDsc"].ToString();
                                        oInventoryEntry.Lines.Quantity = Convert.ToDouble(dr["Qty"].ToString());
                                        // oInventoryEntry.Lines.ShipDate = Convert.ToDateTime(dr["ShipDate"].ToString());
                                        oInventoryEntry.Lines.UnitPrice = Convert.ToDouble(dr["LCPrice"].ToString()); ;
                                        oInventoryEntry.Lines.WarehouseCode = strToWare;
                                        oInventoryEntry.Lines.AccountCode = strValues[2];
                                        oInventoryEntry.Lines.Add();
                                    }
                                }

                                oCompany_D.StartTransaction();
                                //Singleton.traceService("Adding Entry");
                                intEntryStatus = oInventoryEntry.Add();

                                if (intEntryStatus != 0)
                                {
                                    //Singleton.traceService(intEntryStatus.ToString());
                                    Singleton.objSqlDataAccess.UpdateLog(strKey, TransScenerio.InventoryTransfer.ToString(), "0", "0", 0, intEntryStatus.ToString(), oCompany_D.GetLastErrorDescription().Replace("'", ""), strLogger);
                                    if (oCompany_S.InTransaction)
                                        oCompany_S.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                                    if (oCompany_D.InTransaction)
                                        oCompany_D.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                                }
                                else
                                {

                                    if (oCompany_S.InTransaction)
                                        oCompany_S.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                                    if (oCompany_D.InTransaction)
                                        oCompany_D.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                                    Singleton.objSqlDataAccess.UpdateLog(strKey, TransScenerio.InventoryTransfer.ToString(), strDkey, intDocNum.ToString(), 1, "", "Armada_Sync Completed Sucessfully", strLogger);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (oCompany_S.InTransaction)
                    oCompany_S.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                if (oCompany_D.InTransaction)
                    oCompany_D.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                //Singleton.objSqlDataAccess.UpdateLog(strKey, TransScenerio.InventoryTransfer.ToString(), "0", "0", 0, "0", ex.Message.ToString(), strLogger);
                throw ex;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oStockTransfer);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oInventoryExit);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oInventoryEntry);
            }
        }

        #endregion

    }
}



///void IBuson_Sync.Add(int intDocEntry, string sKey, SAPbobsCOM.Company oCompany)
//{
//    DataTable oHeader = null;
//    DataTable oDetails = null;
//    DataTable oBatch = null;
//    SAPbobsCOM.Documents oInventoryEntry = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);

//    try
//    {
//        strQuery = "Exec Buson_Service_S_PRR_V_IGR_s '" + sKey + "','A'";
//        oDataSet = Singleton.objSqlDataAccess.ExecuteDataSet(strQuery, "Logger");

//        if (oDataSet == null && oDataSet.Tables.Count == 0)
//            return;
//        else
//        {
//            oHeader = oDataSet.Tables[0];
//            oDetails = oDataSet.Tables[1];
//            oBatch = oDataSet.Tables[2];

//            if (oHeader != null && oHeader.Rows.Count > 0)
//            {
//                //Header Table
//                oInventoryEntry.DocDate = Convert.ToDateTime(oHeader.Rows[0]["DocDate"].ToString());
//                oInventoryEntry.DocDueDate = Convert.ToDateTime(oHeader.Rows[0]["DocDueDate"].ToString());
//                oInventoryEntry.Reference2 = oHeader.Rows[0]["Ref2"].ToString();

//                if (oDetails.Rows.Count > 0)
//                {
//                    foreach (DataRow dr in oDetails.Rows)
//                    {
//                        oInventoryEntry.Lines.ItemCode = dr["ItemCode"].ToString();
//                        oInventoryEntry.Lines.ItemDescription = dr["Dscription"].ToString();
//                        oInventoryEntry.Lines.Quantity = Convert.ToDouble(dr["Quantity"].ToString());
//                        if (!string.IsNullOrEmpty(dr["ShipDate"].ToString()))
//                            oInventoryEntry.Lines.ShipDate = Convert.ToDateTime(dr["ShipDate"].ToString());

//                        oInventoryEntry.Lines.UnitPrice = 0;
//                        //oInventoryEntry.Lines.Price = Convert.ToDouble(dr["Price"].ToString());
//                        //oInventoryEntry.Lines.Rate = Convert.ToDouble(dr["Rate"].ToString());

//                        oInventoryEntry.Lines.WarehouseCode = dr["WhsCode"].ToString();
//                        oInventoryEntry.Lines.UserFields.Fields.Item("U_FiniWidth").Value = dr["U_FiniWidth"].ToString();

//                        var o = from b in oBatch.AsEnumerable()
//                                where b.Field<int>("BaseLinNum") == Convert.ToInt16(dr["LineNum"].ToString())
//                                select b;

//                        foreach (DataRow item in o)
//                        {
//                            if (item["MnfSerial"].ToString() == "")
//                                return;                                    

//                            SAPbobsCOM.BatchNumbers oBatchNumber = oInventoryEntry.Lines.BatchNumbers;
//                            oBatchNumber.BatchNumber = item["BatchNum"].ToString();
//                            oBatchNumber.ManufacturerSerialNumber = item["MnfSerial"].ToString();
//                            oBatchNumber.Quantity = Convert.ToDouble(item["Quantity"].ToString());

//                            //oBatchNumber.UserFields.Fields.Item("U_LotPre").Value = item["U_LotPre"].ToString();
//                            //oBatchNumber.UserFields.Fields.Item("U_Baleno").Value = item["U_Baleno"].ToString();
//                            //oBatchNumber.UserFields.Fields.Item("U_ProNo").Value = item["U_ProNo"].ToString();
//                            //oBatchNumber.UserFields.Fields.Item("U_SalesNo").Value = item["U_SalesNo"].ToString();
//                            //oBatchNumber.UserFields.Fields.Item("U_Customer").Value = item["U_Customer"].ToString();
//                            //oBatchNumber.UserFields.Fields.Item("U_Shade").Value = item["U_Shade"].ToString();

//                            //oBatchNumber.UserFields.Fields.Item("U_MTKGNO").Value = item["U_REPROCESS"].ToString();
//                            //oBatchNumber.UserFields.Fields.Item("U_RELot").Value = item["U_REIND"].ToString();
//                            //oBatchNumber.UserFields.Fields.Item("U_REPro").Value = item["U_REPro"].ToString();
//                            //oBatchNumber.UserFields.Fields.Item("U_BaleNoNew").Value = item["U_BaleNoNew"].ToString();
//                            //oBatchNumber.UserFields.Fields.Item("U_Flag").Value = item["U_Flag"].ToString();
//                            //oBatchNumber.UserFields.Fields.Item("U_CON").Value = item["U_CON"].ToString();
//                            //oBatchNumber.UserFields.Fields.Item("U_BUYER").Value = item["U_BUYER"].ToString();

//                            oBatchNumber.Add();
//                        }
//                        oInventoryEntry.Lines.Add();
//                    }
//                }

//                oInventoryEntry.Comments = oHeader.Rows[0]["Comments"].ToString();

//                int intError = oInventoryEntry.Add();
//                if (intError != 0)
//                {
//                    Singleton.objSqlDataAccess.UpdateLog(intDocEntry, "0", 0, oCompany.GetLastErrorCode().ToString(), oCompany.GetLastErrorDescription());
//                }
//                else
//                {
//                    string strDkey;
//                    oCompany.GetNewObjectCode(out strDkey);
//                    Singleton.objSqlDataAccess.UpdateLog(intDocEntry, strDkey, 1, "", "Buson_Sync Completed Sucessfully");

//                    strQuery = "Exec Buson_Service_S_PRR_V_IGR_Batch_u '" + sKey + "','" + strDkey + "'";
//                    Singleton.objSqlDataAccess.ExecuteNonQuery(strQuery, "Logger");
//                }
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        throw ex;
//    }
//    finally
//    {
//        oHeader = null;
//        oDetails = null;
//        oDataSet = null;
//        System.Runtime.InteropServices.Marshal.ReleaseComObject(oInventoryEntry);
//    }
//}        