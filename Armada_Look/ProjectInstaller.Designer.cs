namespace Buson_Look
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Buson_Lookup_ProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.Buson_Lookup_Installer = new System.ServiceProcess.ServiceInstaller();
            // 
            // Buson_Lookup_ProcessInstaller
            // 
            this.Buson_Lookup_ProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.Buson_Lookup_ProcessInstaller.Password = null;
            this.Buson_Lookup_ProcessInstaller.Username = null;
            // 
            // Buson_Lookup_Installer
            // 
            this.Buson_Lookup_Installer.Description = "Buson_Lookup";
            this.Buson_Lookup_Installer.DisplayName = "Buson_Lookup";
            this.Buson_Lookup_Installer.ServiceName = "BusOn_Lookup";
            this.Buson_Lookup_Installer.ServicesDependedOn = new string[] {
        "MSSQLSERVER",
        "TAO_NT_Naming_Service",
        "B1LicenseService"};
            this.Buson_Lookup_Installer.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.Buson_Lookup_ProcessInstaller,
            this.Buson_Lookup_Installer});

        }

        #endregion

        public System.ServiceProcess.ServiceProcessInstaller Buson_Lookup_ProcessInstaller;
        public System.ServiceProcess.ServiceInstaller Buson_Lookup_Installer;
    }
}