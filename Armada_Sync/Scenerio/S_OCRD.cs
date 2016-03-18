using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;

namespace Armada_Sync.Scenerio
{
    public class S_OCRD : IArmada_Sync 
    {
        #region "Construtor"
        public S_OCRD()
        { }
        #endregion

        #region "Buson_Sync Members"

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
                Singleton.objSqlDataAccess.UpdateLog(strKey, TransScenerio.Customer.ToString() , "0","0", 0, "0", ex.Message.ToString(),strLogger);                
            }
        }

        void IArmada_Sync.Add(string sKey, SAPbobsCOM.Company oCompany, string strLogger, string strWareHouse, string[] strValues, Hashtable htCCdet)
        {
            DataSet oDS_S_OCRD = null;
            SAPbobsCOM.BusinessPartners oPartner = (SAPbobsCOM.BusinessPartners)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);

            string str_S_OCRD = "Exec Armada_Service_S_OCRD_s '" + sKey + "'";
            oDS_S_OCRD = Armada_Sync.Singleton.objSqlDataAccess.ExecuteDataSet(str_S_OCRD, strLogger);

            try
            {
                if (oDS_S_OCRD == null && oDS_S_OCRD.Tables.Count == 0)
                {
                    return;
                }
                else
                {
                    if ((oDS_S_OCRD.Tables[0] != null) && (oDS_S_OCRD.Tables[0].Rows.Count > 0))
                    {
                        int retVal;

                        if (!oPartner.GetByKey(oDS_S_OCRD.Tables[0].Rows[0]["Code"].ToString().Trim()))
                        {
                            oPartner.CardCode = oDS_S_OCRD.Tables[0].Rows[0]["Code"].ToString().Trim();
                            oPartner.CardName = oDS_S_OCRD.Tables[0].Rows[0]["Name"].ToString().Trim();
                            oPartner.CardType = SAPbobsCOM.BoCardTypes.cCustomer;
                            oPartner.Cellular = oDS_S_OCRD.Tables[0].Rows[0]["Phone"].ToString().Trim();
                            oPartner.Currency = "##";
                            oPartner.EmailAddress = oDS_S_OCRD.Tables[0].Rows[0]["Email"].ToString().Trim();
                            oPartner.Fax = oDS_S_OCRD.Tables[0].Rows[0]["Fax"].ToString().Trim();

                            int intLine = 0;
                            oPartner.Addresses.SetCurrentLine(intLine);
                            oPartner.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo;
                            oPartner.Addresses.AddressName = oDS_S_OCRD.Tables[0].Rows[0]["Name"].ToString().Trim();
                            oPartner.Addresses.Street = oDS_S_OCRD.Tables[0].Rows[0]["Address"].ToString().Trim();
                            oPartner.Addresses.Block = oDS_S_OCRD.Tables[0].Rows[0]["Area"].ToString().Trim();
                            oPartner.Addresses.ZipCode = oDS_S_OCRD.Tables[0].Rows[0]["ZipCode"].ToString().Trim();
                            oPartner.Addresses.City = oDS_S_OCRD.Tables[0].Rows[0]["City"].ToString().Trim();
                            oPartner.Addresses.Country = oDS_S_OCRD.Tables[0].Rows[0]["Country"].ToString();
                            //oPartner.Addresses.State = oDS_S_OCRD.Tables[0].Rows[0]["Name"].ToString();
                            oPartner.Addresses.Add();

                            retVal = oPartner.Add();

                            if (retVal != 0)
                            {
                                Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.Customer.ToString(), "0", "0", 0, oCompany.GetLastErrorCode().ToString(), oCompany.GetLastErrorDescription().Replace("'", ""), strLogger);
                            }
                            else
                            {
                                string strDkey;
                                oCompany.GetNewObjectCode(out strDkey);
                                Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.Customer.ToString(), strDkey, strDkey, 1, "", "Armada_Sync Completed Sucessfully", strLogger);
                            }
                        }
                        else
                        {
                            Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.Customer.ToString(), "","" , 1, "", "Armada_Sync Completed Sucessfully(Customer Already Exist)", strLogger);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Singleton.objSqlDataAccess.UpdateLog(sKey, TransScenerio.Customer.ToString(), "0", "0", 0, "0", ex.Message.ToString(), strLogger);
                throw ex;
            }
            finally
            {
                oDS_S_OCRD = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oPartner);
            }
        }        

        #endregion


        public void Add(string strKey, SAPbobsCOM.Company oCompany_S, SAPbobsCOM.Company oCompany_D, string strLogger, string strFromWare, string strToWare, string[] strValues)
        {
            throw new NotImplementedException();
        }


        public void Sync(string strKey, TransType eTrnType, SAPbobsCOM.Company oCompany_S, SAPbobsCOM.Company oCompany_D, string strLogger, string strFromWare, string strToWare, string[] strValues)
        {
            throw new NotImplementedException();
        }
    }
}
