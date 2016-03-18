using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace Armada_SendMail
{
    public class clsTransMail
    {
        SqlConnection objConn;
        SqlCommand objCmd;
        SqlDataAdapter objSDP;
        DataTable oHeaderDT;
        DataTable oChildDT;
        SmtpClient SmtpServer = new SmtpClient();
        MailMessage mail = new MailMessage();
        string mailServer;
        string mailPort;
        string mailId;
        string mailUser;
        string mailPwd;
        string mailSSL;
        string mailBody;
        //string mailSubject;
        string mailName;
       // string strCardName;
        string TOID;
        String CCID;


        public void LoadMailDetails(string strInterface)
        {
            try
            {
                DataSet ds;
                string ConnectionString = String.Format(System.Configuration.ConfigurationManager.AppSettings["Logger"].ToString(), strInterface);
                objConn = new SqlConnection(ConnectionString);
                objConn.Open();
                string strQry = "Exec Armada_LoadDailyTransaction";
                objCmd = new SqlCommand(strQry, objConn);
                objSDP = new SqlDataAdapter(objCmd);
                ds = new DataSet();
                objSDP.Fill(ds);
                if (ds != null && ds.Tables.Count > 1)
                {
                    oHeaderDT = ds.Tables[0];
                    oChildDT = ds.Tables[1];
                    objConn.Close();
                }
                else
                {
                    objConn.Close();
                }
                GetMailDetails();
                if (oHeaderDT.Rows.Count > 0)
                {
                  SendMails(oHeaderDT, oChildDT,strInterface );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
         }

        private void GetMailDetails()
        {
            try
            { 
                /*
                DataSet ds;
                System.Data.DataTable oDT = new System.Data.DataTable();
               
                objConn = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["Logger"].ToString());
                objConn.Open();
                string strQry = "Select * From OUSR Where USER_CODE = '" + System.Configuration.ConfigurationManager.AppSettings["SapUser"].ToString() + "'";
                objCmd = new SqlCommand(strQry, objConn);
                objSDP = new SqlDataAdapter(objCmd);
                ds = new DataSet();
                objSDP.Fill(ds);
                if (ds != null && ds.Tables.Count > 0)
                {
                    oDT = ds.Tables[0];
                    objConn.Close();
                }
                else
                {
                    objConn.Close();
                }

                if (oDT.Rows.Count > 0)
                {
                    mailServer = oDT.Rows[0]["U_SMTPSERVER"].ToString();
                    mailPort = oDT.Rows[0]["U_SMTPPORT"].ToString();
                    mailId = oDT.Rows[0]["U_MAILID"].ToString();
                    mailUser = oDT.Rows[0]["U_SMTPUSER"].ToString();
                    mailPwd = oDT.Rows[0]["U_SMTPPWD"].ToString();
                    mailSSL = oDT.Rows[0]["U_SSL"].ToString();
                    mailBody = oDT.Rows[0]["U_MAILBODY"].ToString();
                    mailName = oDT.Rows[0]["U_MAILNAME"].ToString();
                }*/

                mailServer = System.Configuration.ConfigurationManager.AppSettings["mailServer"].ToString();
                mailPort = System.Configuration.ConfigurationManager.AppSettings["mailPort"].ToString();
                mailId = System.Configuration.ConfigurationManager.AppSettings["mailId"].ToString();
                mailUser = System.Configuration.ConfigurationManager.AppSettings["mailUser"].ToString();
                mailPwd = System.Configuration.ConfigurationManager.AppSettings["mailPwd"].ToString();
                mailSSL = System.Configuration.ConfigurationManager.AppSettings["mailSSL"].ToString();
                mailBody = System.Configuration.ConfigurationManager.AppSettings["mailBody"].ToString();
                mailName = System.Configuration.ConfigurationManager.AppSettings["mailName"].ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendMails(DataTable oHeader, DataTable oChild,string strInterface)
        {
            TOID = oHeader.Rows[0]["ToId"].ToString();
            CCID = oHeader.Rows[0]["CCId"].ToString();
            SendHTMLMail(TOID, CCID, strInterface);
        }

        private void SendHTMLMail(string toId, string ccId, string strInterface)
        {
            try
            {
                string attachmentPath = System.Windows.Forms.Application.StartupPath + "\\Armada-Holding.gif";
                Attachment inline = new Attachment(attachmentPath);
                inline.ContentDisposition.Inline = true;
                inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                //inline.ContentId = contentID
                inline.ContentType.MediaType = "image/gif";
                inline.ContentType.Name = Path.GetFileName(attachmentPath);

                SmtpServer.Credentials = new System.Net.NetworkCredential(mailId, mailPwd);
                SmtpServer.Port = Convert.ToInt16(mailPort);
                SmtpServer.EnableSsl = Convert.ToBoolean(mailSSL);
                SmtpServer.Host = mailServer;
                mail = new MailMessage();
                mail.From = new MailAddress(mailId, mailName);
                mail.To.Add(toId);
                mail.CC.Add(ccId);

                mail.Subject = "Armada Service - Daily Transaction" + " - " + System.DateTime.Now.ToShortDateString();
                mail.IsBodyHtml = true;
                mail.Body = BuildHtmBody();
                mail.Priority = MailPriority.High;
                mail.Attachments.Add(inline);
                SmtpServer.Send(mail);
                releaseObject(mail);
                releaseObject(SmtpServer);

                string strQuery = "INSERT INTO Z_OMTX(TransDate,Status) Values (GetDate(),1)" ;
                ExecuteNonQuery(strQuery, strInterface);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExecuteNonQuery(string str, string strKey)
        {
               SqlCommand oCommand = null;
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
                //err = ex.ToString();
                myConnection.Close();
            }
            finally
            {
                myConnection.Close();
                myConnection = null;
                oCommand = null;
            }
        }

        private string BuildHtmBody()
        {
            try
            {
                string oHTML = null;                
                string HTemplate = System.Windows.Forms.Application.StartupPath  + "\\ARTemplate.htm";
                oHTML = GetFileContents(HTemplate);

                oHTML = oHTML.Replace("$$Matrix$$", SKSMatrix());

                return oHTML;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetFileContents(string FullPath)
        {
	        string strContents = null;
	        StreamReader objReader = default(StreamReader);
	        try 
            {
		        objReader = new StreamReader(FullPath);
		        strContents = objReader.ReadToEnd();
		        objReader.Close();
		        return strContents;
	        } 
            catch (Exception Ex) 
            {
                throw Ex;
	        }
        }

        private string SKSMatrix()
        {
            try
            {
                string DTemplate = System.Windows.Forms.Application.StartupPath + "\\ARMatrixTemplate.htm";
                string oFileContent = GetFileContents(DTemplate);
                string oMatStr = "";
                for (int I = 0; I <= oChildDT.Rows.Count - 1; I++)
                { 
                        string oHTML = oFileContent;
                        if (oChildDT.Rows[I]["Scenario"].ToString() != null)
                        {
                            oHTML = oHTML.Replace("$$Scenario$$", oChildDT.Rows[I]["Scenario"].ToString());
                        }
                        else
                        {
                            oHTML = oHTML.Replace("$$Scenario$$", "");
                        }                       
                        if (oChildDT.Rows[I]["NoOfTransaction"].ToString() != null)
                        {
                            oHTML = oHTML.Replace("$$NoOfTransaction$$", oChildDT.Rows[I]["NoOfTransaction"].ToString());
                        }
                        else
                        {
                            oHTML = oHTML.Replace("$$NoOfTransaction$$", "");
                        }
                        if (oChildDT.Rows[I]["Success"].ToString() != null)
                        {
                            oHTML = oHTML.Replace("$$Success$$", oChildDT.Rows[I]["Success"].ToString());
                        }
                        else
                        {
                            oHTML = oHTML.Replace("$$Success$$", "");
                        }
                        if (oChildDT.Rows[I]["Failed"].ToString() != null)
                        {
                            oHTML = oHTML.Replace("$$Failed$$", oChildDT.Rows[I]["Failed"].ToString());
                        }
                        else
                        {
                            oHTML = oHTML.Replace("$$Failed$$", "");
                        }
                        oMatStr = oMatStr + oHTML;
                }
                return oMatStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void releaseObject(Object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
