using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Armada_Sync.Scenerio;
using Armada_Sync.DataAccess;
using System.IO;

namespace Armada_Sync
{
    public sealed class Singleton
    {
        #region "Declaration"

        private static SqlDataAccess oSqlDataAccess = null;
        private static S_OCRD oS_OCRD = null;
        private static S_OINV oS_OINV = null;
        private static S_ORIN oS_ORIN = null;
        private static S_OWTR oS_OWTR = null;
        private static S_OPDN oS_OPDN = null;

        #endregion

        #region "Construtor"
        private Singleton() { }
        #endregion

        #region "Property"

        public static SqlDataAccess objSqlDataAccess
        {
            get
            {
                if (oSqlDataAccess == null)
                {
                    oSqlDataAccess = new SqlDataAccess();
                }
                return oSqlDataAccess;
            }
        }
      
        public static S_OCRD obj_S_OCRD
        {
            get
            {
                if (oS_OCRD == null)
                {
                    oS_OCRD = new S_OCRD();
                }
                return oS_OCRD;
            }
        }

        public static S_OINV obj_S_OINV
        {
            get
            {
                if (oS_OINV == null)
                {
                    oS_OINV = new S_OINV();
                }
                return oS_OINV;
            }
        }

        public static S_ORIN obj_S_ORIN
        {
            get
            {
                if (oS_ORIN == null)
                {
                    oS_ORIN = new S_ORIN();
                }
                return oS_ORIN;
            }
        }

        public static S_OWTR obj_S_OWTR
        {
            get
            {
                if (oS_OWTR == null)
                {
                    oS_OWTR = new S_OWTR();
                }
                return oS_OWTR;
            }
        }

        public static S_OPDN obj_S_OPDN
        {
            get
            {
                if (oS_OPDN == null)
                {
                    oS_OPDN = new S_OPDN();
                }
                return oS_OPDN;
            }
        }

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

        #endregion
    }
}
