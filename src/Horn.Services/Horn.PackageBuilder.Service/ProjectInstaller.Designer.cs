namespace Horn.PackageBuilder.Service
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
            this.packageProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // packageProcessInstaller
            // 
            this.packageProcessInstaller.Account = System.ServiceProcess.ServiceAccount.NetworkService;
            this.packageProcessInstaller.Password = null;
            this.packageProcessInstaller.Username = null;
            // 
            // serviceInstaller
            // 
            this.serviceInstaller.ServiceName = "PackageBuilderService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.packageProcessInstaller,
            this.serviceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller packageProcessInstaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller;
    }
}