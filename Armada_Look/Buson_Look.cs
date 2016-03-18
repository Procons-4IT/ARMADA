using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace Buson_Look
{
    partial class BusOn_Lookup : ServiceBase
    {
        private ServiceController service;
        Timer tmrReset = new Timer();

        public BusOn_Lookup()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            service = new ServiceController("Buson_Service");
            tmrReset.Interval = 5000;
            tmrReset.Enabled = true;
            tmrReset.Elapsed += new ElapsedEventHandler(tmrReset_Elapsed);          
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            tmrReset.Stop();    
        }

        private void tmrReset_Elapsed(object source, ElapsedEventArgs e)
        {
            try
            {
                service.Refresh();
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    service.Start();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
