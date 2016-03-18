using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Armada_App
{
    public class DOTransaction_Log_Report
    {
        #region "Declaration"
        private string _Scenario;
        private DateTime _FromReqdate;
        private DateTime _ResponseDate;
        #endregion

        #region"Property"
        public string Scenario
        {
            get { return _Scenario; }
            set { _Scenario = value; }
        }
        public DateTime FromReqdate
        {
            get { return _FromReqdate; }
            set { _FromReqdate = value; }
        }
        public DateTime ResponseDate
        {
            get { return _ResponseDate; }
            set { _ResponseDate = value; }
        }
        #endregion
    }
}
