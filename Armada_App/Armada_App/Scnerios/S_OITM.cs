using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Configuration;
using System.Collections;
using System.Threading;

namespace Armada_App
{

    public partial class S_OITM : Form
    {
        DataSet oDS;
        SAPbobsCOM.Company oCompany;
        System.Threading.Thread thread = null;
        DataTable oDtCompany;
        private ArrayList aList_C;
        private Hashtable aList_R;
        delegate void UpdateLabelDelegate(string message);


        public S_OITM()
        {
            InitializeComponent();
        }

        private void S_OITM_Load(object sender, EventArgs e)
        {
            try
            {
                UXUTIL.clsUtilities.setAllControlsThemes(this);
                this.WindowState = FormWindowState.Maximized;
                //this.CheckForIllegalCrossThreadCalls = false;
                oDtCompany = new DataTable();
                oDtCompany.Columns.Add("Company", typeof(string));
                oDtCompany.Columns.Add("Company Name", typeof(string));
                string strMaiDB = System.Configuration.ConfigurationManager.AppSettings["MainDB"].ToString();
                oCompany = (SAPbobsCOM.Company)TransLog.GetCompany(strMaiDB);
                SAPbobsCOM.Recordset oRecordSet;
                oRecordSet = oCompany.GetCompanyList();
                DataRow oDr;
                while (!oRecordSet.EoF)
                {
                    oDr = oDtCompany.NewRow();
                    oDr["Company"] = oRecordSet.Fields.Item(0).Value;
                    oDr["Company Name"] = oRecordSet.Fields.Item(1).Value;
                    oDtCompany.Rows.Add(oDr);
                    oRecordSet.MoveNext();
                }
                BindingSource bs = new BindingSource();
                bs.DataSource = oDtCompany;
                comboBox1.DataSource = oDtCompany;
                comboBox1.DisplayMember = "Company Name";
                comboBox1.ValueMember = "Company";
                comboBox1.SelectedIndex = 0;
                System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
               
            }
            catch (Exception ex)
            {
                TransLog.traceService(" Error : " + ex.Message);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

            if (txtFilePath.Text == string.Empty || txtFilePath.Text == "")
            {
                MessageBox.Show("Please select a file");
            }
            else
            {
                string strProvider = ConfigurationManager.AppSettings["oledbProvider"].ToString();

                bool hasHeaders = true;
                String connString = String.Format(strProvider, txtFilePath.Text, hasHeaders ? "Yes" : "No");


                OleDbConnection oledbConn = new OleDbConnection(connString);
                try
                {
                    // Open connection
                    oledbConn.Open();

                    // Create OleDbCommand object and select data from worksheet Sheet1
                    OleDbCommand cmd = new OleDbCommand("SELECT * FROM " + ConfigurationManager.AppSettings["Sheet"].ToString(), oledbConn);

                    // Create new OleDbDataAdapter 
                    OleDbDataAdapter oleda = new OleDbDataAdapter();

                    oleda.SelectCommand = cmd;

                    // Create a DataSet which will hold the data extracted from the worksheet.
                    oDS = new DataSet();

                    // Fill the DataSet from the data extracted from the worksheet.
                    oleda.Fill(oDS, "OITM");

                    // Bind the data to the GridView
                    dataGridView1.DataSource = oDS.Tables[0].DefaultView;

                    MessageBox.Show("Data Imported Sucessfully...");
                    //txtFilePath.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    TransLog.traceService(" Error : " + ex.Message);
                }
                finally
                {
                    // Close connection
                    oledbConn.Close();
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String sFileName = openFileDialog1.FileName;
                txtFilePath.Text = sFileName;
            }
            else
            {
                MessageBox.Show("Please select a file");
            }
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            try
            {
                //thread = new System.Threading.Thread(import_Products);
                //thread.Start();
                //if (!thread.IsAlive)
                //{
                //    thread.Abort();
                //}

                //int i = 1;
                int intThreadRecords = oDS.Tables[0].Rows.Count/100;
                //int acount = 0;
                Hashtable aListHash = new Hashtable();
                aList_C = new ArrayList(intThreadRecords);

                for (int intRow = 0; intRow < oDS.Tables[0].Rows.Count; intRow++)
                {
                    
                    //acount += 1;

                    if (!aListHash.Contains(oDS.Tables[0].Rows[intRow]["ItemCode"]))
                    {
                        aListHash.Add(oDS.Tables[0].Rows[intRow]["ItemCode"], oDS.Tables[0].Rows[intRow]);
                    }

                    if (aListHash.Count == intThreadRecords)
                    {
                        aList_C.Add(aListHash);
                    }
                    else if (oDS.Tables[0].Rows.Count - 1 == intRow)
                    {
                        aList_C.Add(aListHash);
                    }
                    
                    if (aListHash.Count == intThreadRecords)
                    {
                        aListHash = new Hashtable();
                    }

                }
                string strMaiDB = comboBox1.SelectedValue.ToString();

                //System.Threading.Thread[] ThreadCollections = new System.Threading.Thread[aList_C.Count + 1];

                for (int index = 0; index <= aList_C.Count - 1; index++)
                {
                    Hashtable oHT = (Hashtable)aList_C[index];
                    Multitask oMultiTask = new Multitask(strMaiDB, this, oDS, oHT);
                    thread = new Thread(oMultiTask.import_Products);
                    thread.Priority = ThreadPriority.Highest;
                    thread.IsBackground = true;
                    thread.ApartmentState = System.Threading.ApartmentState.STA;
                    thread.Start();

                    //ThreadCollections[index) = thread;
                }


            }
            catch (Exception ex)
            {
                TransLog.traceService(" Error : " + ex.Message);
            }
        }

       public void UpdateLabel(string message)
        {
            

            if (InvokeRequired)
            {
                Invoke(new UpdateLabelDelegate(UpdateLabel), message);
                return;
            }

            label1.Text = message;
            
        }

        //private void import_Products()
        //{
        //    try
        //    {
        //        System.Reflection.Assembly oItem_R = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "\\Interop.SAPbobsCOM.dll");
        //        string strMaiDB = comboBox1.SelectedValue.ToString();//System.Configuration.ConfigurationManager.AppSettings["MainDB"].ToString();
        //        oCompany = (SAPbobsCOM.Company)TransLog.GetCompany(strMaiDB);
        //        SAPbobsCOM.Items oItem = (SAPbobsCOM.Items)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
        //        foreach (DataRow row in oDS.Tables[0].Rows)
        //        {
        //            try
        //            {
        //                bool blnItemExist = false;
        //                string strItemCode = row["ItemCode"].ToString();
        //                TransLog.traceService(" Product : " + strItemCode);
        //                label1.Text = "Creation Item No : " + " - :" + strItemCode;

        //                if (oItem.GetByKey(row["ItemCode"].ToString()))
        //                {
        //                    blnItemExist = true;
        //                }

        //                foreach (DataColumn column in oDS.Tables[0].Columns)
        //                {
        //                    string DESTF = column.ColumnName;
        //                    if (!DESTF.StartsWith("U_"))
        //                    {
        //                        try
        //                        {
        //                            //switch (oItem_R.GetType("SAPbobsCOM.IItems").GetProperty(DESTF).GetType().Name)
        //                            //{                                            

        //                            //    default:
        //                            //        break;
        //                            //}
        //                            oItem_R.GetType("SAPbobsCOM.IItems").GetProperty(DESTF).SetValue(oItem, row[DESTF].ToString(), null);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            TransLog.traceService(" Error : " + ex.Message + "--> Field  : " + DESTF);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        oItem.UserFields.Fields.Item(DESTF).Value = row[column.ColumnName.ToString()].ToString();
        //                    }
        //                }

        //                int intStatus = 0;
        //                if (!blnItemExist)
        //                {
        //                    intStatus = oItem.Add();
        //                    if (intStatus != 0)
        //                    {
        //                        TransLog.traceService(" Error : " + oCompany.GetLastErrorDescription());
        //                    }

        //                }
        //                else
        //                {
        //                    intStatus = oItem.Update();
        //                    if (intStatus != 0)
        //                    {
        //                        TransLog.traceService(" Error : " + oCompany.GetLastErrorDescription());
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                TransLog.traceService(" Error : " + ex.Message);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TransLog.traceService(" Error : " + ex.Message);
        //    }
        //}
    }

    public partial class Multitask //: Form
    {
        string strCompany_S = string.Empty;
        SAPbobsCOM.Company oCompany = null;
        S_OITM oForm_S;
        DataSet oDS_S;
        Hashtable oHT_S;
        
        public Multitask()
        {

        }

        public Multitask(string strCompany, S_OITM oForm, DataSet oDS, Hashtable oHT)
        {
            strCompany_S = strCompany;
            oForm_S = oForm;
            oDS_S = oDS;
            oHT_S = oHT;
        }

        public void import_Products()
        {
            try
            {
                System.Reflection.Assembly oItem_R = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "\\Interop.SAPbobsCOM.dll");
                string strMaiDB = strCompany_S.ToString();//System.Configuration.ConfigurationManager.AppSettings["MainDB"].ToString();
                oCompany = (SAPbobsCOM.Company)TransLog.GetCompany(strMaiDB);
                SAPbobsCOM.Items oItem = (SAPbobsCOM.Items)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);

                foreach (DictionaryEntry entry in oHT_S)
                {
                    DataRow row = (DataRow)entry.Value;
                    try
                    {
                        bool blnItemExist = false;
                        string strItemCode = row["ItemCode"].ToString();
                        TransLog.traceService(" Product : " + strItemCode);

                        //Thread.Sleep(100);
                        //oForm_S.label1.Text = "Creation Item No : " + " - :" + strItemCode;
                        //Application.DoEvents();

                        //oForm_S.Invoke((MethodInvoker)delegate
                        //{
                        //    oForm_S.label1.Text = strItemCode; // runs on UI thread
                        //});

                        //oForm_S.label1.Invoke(new Action(() => Control.text = ""));
                        if (oItem.GetByKey(row["ItemCode"].ToString()))
                        {
                            blnItemExist = true;
                        }

                        foreach (DataColumn column in oDS_S.Tables[0].Columns)
                        {
                            string DESTF = column.ColumnName;
                            if (!DESTF.StartsWith("U_"))
                            {
                                try
                                {
                                    if (DESTF != "ItemsGroupCode")
                                    {
                                        oItem_R.GetType("SAPbobsCOM.IItems").GetProperty(DESTF).SetValue(oItem, row[DESTF].ToString(), null);
                                    }
                                    else
                                    {
                                        oItem_R.GetType("SAPbobsCOM.IItems").GetProperty(DESTF).SetValue(oItem, Convert.ToInt32(row[DESTF].ToString()), null);
                                    }                                    
                                }
                                catch (Exception ex)
                                {
                                    TransLog.traceService(" Error : " + ex.Message + "--> Field  : " + DESTF);
                                }
                            }
                            else
                            {
                                oItem.UserFields.Fields.Item(DESTF).Value = row[column.ColumnName.ToString()].ToString();
                            }
                        }

                        int intStatus = 0;
                        if (!blnItemExist)
                        {
                            intStatus = oItem.Add();
                            if (intStatus != 0)
                            {
                                TransLog.traceService(" Error : " + oCompany.GetLastErrorDescription());
                            }
                            else
                            {
                                TransLog.traceService(" Success  ");
                            }

                        }
                        else
                        {
                            intStatus = oItem.Update();
                            if (intStatus != 0)
                            {
                                TransLog.traceService(" Error : " + oCompany.GetLastErrorDescription());
                            }
                            else
                            {
                                TransLog.traceService(" Success  ");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TransLog.traceService(" Error : " + ex.Message);
                    }
                }
                               
            }
            catch (Exception ex)
            {
                TransLog.traceService(" Error : " + ex.Message);
            }
        }
    }
}