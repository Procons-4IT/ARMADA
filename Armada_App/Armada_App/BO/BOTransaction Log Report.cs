using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Armada_Sync.DataAccess;
using BAL;

namespace Armada_App
{
    public class BOTransation_Log_Report : IBusinessLogic<DOTransaction_Log_Report>
    {
        const string SELECT = "Armada_Service_TxnLogReport_S";

        #region IBusinessLogic<Transaction log Report> Members
        public DataSet List(String Scenario, String FromReqdate, String ResponseDate, String ChbFailed)
        {
            DataSet _retVal = null;
            SqlDataAccess oDataWrapper = null;
            string strInterface = System.Configuration.ConfigurationManager.AppSettings["InterDB"].ToString();
            try
            {
                oDataWrapper = new SqlDataAccess();
                SqlParameter[] oParameter = new SqlParameter[4];
                oParameter[0] = new SqlParameter("@Scenario", SqlDbType.VarChar);
                oParameter[0].Value = Scenario;
                oParameter[1] = new SqlParameter("@ReqDateF", SqlDbType.Date);
                oParameter[1].Value = Convert.ToDateTime(FromReqdate.ToString());
                oParameter[2] = new SqlParameter("@ReqDateT", SqlDbType.Date);
                oParameter[2].Value = Convert.ToDateTime(ResponseDate.ToString());
                oParameter[3] = new SqlParameter("@Failed", SqlDbType.Int);
                if (ChbFailed.ToString() != "False")
                {
                    oParameter[3].Value = "0";
                }
                else
                {
                    oParameter[3].Value = "1";
                }
                _retVal = oDataWrapper.ExecuteDataSet(SELECT, oParameter, strInterface);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                oDataWrapper = null;
            }
            return _retVal;
        }
        #endregion
    }
}
