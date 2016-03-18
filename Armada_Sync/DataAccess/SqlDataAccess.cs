using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Armada_Sync.DataAccess
{
    public class SqlDataAccess
    {
        private SqlDataAdapter oSqlAdap = null;
        private DataSet Ds = null;
        private SqlCommand oCommand = null;
        private string err = string.Empty;

        public DataSet ExecuteDataSet(string str, string strKey)
        {
            DataSet functionReturnValue = null;
            //System.Configuration.ConfigurationManager.AppSettings[strKey];
            string ConnectionString = String.Format(System.Configuration.ConfigurationManager.AppSettings["Logger"].ToString(), strKey);
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            Ds = new DataSet();
            try
            {
                myConnection.Open();
                oSqlAdap = new SqlDataAdapter(str, myConnection);
                oSqlAdap.Fill(Ds, "T_Temp");
                functionReturnValue = Ds;
            }
            catch (Exception ex)
            {
                err = ex.ToString();
                myConnection.Close();
            }
            finally
            {
                myConnection.Close();
                myConnection = null;
                oSqlAdap = null;
            }
            return functionReturnValue;
        }

        public DataTable ExecuteReader(string str, string strKey)
        {
            DataTable functionReturnValue = null;
            //string ConnectionString = System.Configuration.ConfigurationManager.AppSettings[strKey];            
            string ConnectionString = String.Format(System.Configuration.ConfigurationManager.AppSettings["Logger"].ToString(), strKey);//System.Configuration.ConfigurationManager.AppSettings[strKey];
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            Ds = new DataSet();
            try
            {
                myConnection.Open();
                oSqlAdap = new SqlDataAdapter(str, myConnection);
                oSqlAdap.Fill(Ds, "T_Temp");
                functionReturnValue = Ds.Tables["T_Temp"];
            }
            catch (Exception ex)
            {
                err = ex.ToString();
                myConnection.Close();
            }
            finally
            {
                myConnection.Close();
                myConnection = null;
                oSqlAdap = null;
            }
            return functionReturnValue;
        }

        public void ExecuteNonQuery(string str, string strKey)
        {
            //string ConnectionString = System.Configuration.ConfigurationManager.AppSettings[strKey];
            string ConnectionString = String.Format(System.Configuration.ConfigurationManager.AppSettings["Logger"].ToString(), strKey);//System.Configuration.ConfigurationManager.AppSettings[strKey];
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                oCommand = new SqlCommand(str, myConnection);
                oCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                err = ex.ToString();
                myConnection.Close();
            }
            finally
            {
                myConnection.Close();
                myConnection = null;
                oCommand = null;
            }
        }

        public int ExecuteScalar(string str, string strKey)
        {
            int functionReturnValue = 0;
            //string ConnectionString = System.Configuration.ConfigurationManager.AppSettings[strKey];            
            string ConnectionString = String.Format(System.Configuration.ConfigurationManager.AppSettings["Logger"].ToString(), strKey);//System.Configuration.ConfigurationManager.AppSettings[strKey];
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                oCommand = new SqlCommand(str, myConnection);
                functionReturnValue = Convert.ToInt32(oCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                err = ex.ToString();
            }
            finally
            {
                myConnection.Close();
                myConnection = null;
                oCommand = null;
            }
            return functionReturnValue;
        }

        public string ExecuteScalar_String(string str, string strKey)
        {
            string functionReturnValue = "";
            //System.Configuration.ConfigurationManager.AppSettings[strKey];
            string ConnectionString = String.Format(System.Configuration.ConfigurationManager.AppSettings["Logger"].ToString(), strKey);
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                oCommand = new SqlCommand(str, myConnection);
                functionReturnValue = (oCommand.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                err = ex.ToString();
            }
            finally
            {
                myConnection.Close();
                myConnection = null;
                oCommand = null;
            }
            return functionReturnValue;
        }

        public void UpdateLog(string strSKey, string Scenerio, string strDKey, string strDNo, int intStatus, string strErrCode, string strRemarks, string strLogger)
        {
            try
            {
                string strQry = "Exec Armada_Service_TxnLog_U '" + strSKey + "','" + Scenerio + "','" + strDKey + "','" + strDNo + "','" + intStatus + "','" + strErrCode + "','" + strRemarks.Replace("'", "") + "'";
                traceService(strQry);
                ExecuteNonQuery(strQry, strLogger);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public  void traceService(string content)
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


        public DataSet ExecuteDataSet(string strQuery)
        {
            string ConnectionString = System.Configuration.ConfigurationManager.AppSettings["Logger"];
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            DataSet _retVal = new DataSet();
            try
            {
                myConnection.Open();
                oCommand = new SqlCommand();
                if (myConnection.State == ConnectionState.Open)
                {
                    oCommand.Connection = myConnection;
                    oCommand.CommandText = strQuery;
                    oCommand.CommandType = CommandType.StoredProcedure;
                    oSqlAdap = new SqlDataAdapter(oCommand);
                    oSqlAdap.Fill(_retVal);
                }
                else
                {
                    throw new Exception("");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                myConnection.Close();
                oCommand = null;
                oSqlAdap = null;
            }
            return _retVal;
        }

        public DataTable ExecuteDataTable(string strQuery)
        {
            string ConnectionString = System.Configuration.ConfigurationManager.AppSettings["Logger"];
            SqlConnection myConnection = new SqlConnection(ConnectionString);
            DataTable _retVal = null;
            DataSet oDataSet = new DataSet();
            try
            {
                myConnection.Open();
                oCommand = new SqlCommand();
                if (myConnection.State == ConnectionState.Open)
                {
                    oCommand.Connection = myConnection;
                    oCommand.CommandText = strQuery;
                    oCommand.CommandType = CommandType.StoredProcedure;
                    oSqlAdap = new SqlDataAdapter(oCommand);
                    oSqlAdap.Fill(oDataSet);
                }
                else
                {
                    throw new Exception("");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                myConnection.Close();
                oCommand = null;
                oSqlAdap = null;
            }
            return _retVal = oDataSet.Tables[0];
        }

        //public void traceService(string content)
        //{
        //    try
        //    {
        //        string strFile = @"\Armada_Service_" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt";
        //        string strPath = System.Windows.Forms.Application.StartupPath.ToString() + strFile;
        //        if (!File.Exists(strPath))
        //        {
        //            File.Create(strPath);
        //        }
        //        FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.Write);
        //        StreamWriter sw = new StreamWriter(fs);
        //        sw.BaseStream.Seek(0, SeekOrigin.End);
        //        sw.WriteLine(content);
        //        sw.Flush();
        //        sw.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        traceService(ex.Message);
        //    }
        //}

        public DataSet ExecuteDataSet(string strQuery, SqlParameter[] oParameters, string strKey)
        {
            //string ConnectionString = System.Configuration.ConfigurationManager.AppSettings["Logger"];
            //System.Configuration.ConfigurationManager.AppSettings[strKey];
            string ConnectionString = String.Format(System.Configuration.ConfigurationManager.AppSettings["Logger"].ToString(), strKey);

            SqlConnection myConnection = new SqlConnection(ConnectionString);
            DataSet _retVal = new DataSet();
            try
            {
                myConnection.Open();
                oCommand = new SqlCommand();
                if (myConnection.State == ConnectionState.Open)
                {
                    oCommand.Connection = myConnection;
                    oCommand.CommandText = strQuery;
                    oCommand.CommandType = CommandType.StoredProcedure;
                    oCommand.Parameters.AddRange(oParameters);
                    oSqlAdap = new SqlDataAdapter(oCommand);
                    oSqlAdap.Fill(_retVal);
                }
                else
                {
                    throw new Exception("");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                myConnection.Close();
                oCommand = null;
                oSqlAdap = null;
            }
            return _retVal;
        }
    }
}


//public int ExecuteNonQuery(string strQuery, SqlParameter[] oParameters)
//{
//    int _retVal = 0;
//    string ConnectionString = System.Configuration.ConfigurationManager.AppSettings["Logger"];                
//    SqlConnection myConnection = new SqlConnection(ConnectionString);
//    try
//    {   
//        myConnection.Open();
//        oCommand = new SqlCommand();
//        if (myConnection.State == ConnectionState.Open)
//        {
//            oCommand.Connection = myConnection;
//            oCommand.CommandText = strQuery;
//            oCommand.CommandType = CommandType.StoredProcedure;
//            oCommand.Parameters.AddRange(oParameters);
//            _retVal = oCommand.ExecuteNonQuery();
//        }
//        else
//        {
//            throw new Exception("");
//        }
//    }
//    catch (Exception)
//    {
//        throw;
//    }
//    finally
//    {
//        myConnection.Close();
//        myConnection = null;
//        oCommand = null;
//    }
//    return _retVal;
//}

//public DataTable ExecuteDataTable(string strQuery, SqlParameter[] oParameters)
//{
//    string ConnectionString = System.Configuration.ConfigurationManager.AppSettings["Logger"];
//    SqlConnection myConnection = new SqlConnection(ConnectionString);
//    DataTable _retVal = null;
//    DataSet oDataSet = new DataSet();
//    try
//    {   
//        myConnection.Open();
//        oCommand = new SqlCommand();
//        if (myConnection.State == ConnectionState.Open)
//        {
//            oCommand.Connection = myConnection;
//            oCommand.CommandText = strQuery;
//            oCommand.CommandType = CommandType.StoredProcedure;
//            oCommand.Parameters.AddRange(oParameters);
//            oSqlAdap = new SqlDataAdapter(oCommand);
//            oSqlAdap.Fill(oDataSet);
//        }
//        else
//        {
//            throw new Exception("");
//        }
//    }
//    catch (Exception)
//    {
//        throw;
//    }
//    finally
//    {
//        myConnection.Close();
//        oCommand = null;
//        oSqlAdap = null;
//    }
//    return _retVal = oDataSet.Tables[0];
//}

