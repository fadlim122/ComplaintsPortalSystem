namespace ComplaintPortalSystem.Dashboard
{
    partial class ComplaintByStatus
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
            DevExpress.DataAccess.EntityFramework.EFConnectionParameters efConnectionParameters1 = new DevExpress.DataAccess.EntityFramework.EFConnectionParameters();
            DevExpress.DashboardCommon.Dimension dimension1 = new DevExpress.DashboardCommon.Dimension();
            DevExpress.DashboardCommon.Measure measure1 = new DevExpress.DashboardCommon.Measure();
            DevExpress.DashboardCommon.DashboardLayoutGroup dashboardLayoutGroup1 = new DevExpress.DashboardCommon.DashboardLayoutGroup();
            DevExpress.DashboardCommon.DashboardLayoutItem dashboardLayoutItem1 = new DevExpress.DashboardCommon.DashboardLayoutItem();
            this.dashboardEFDataSource1 = new DevExpress.DashboardCommon.DashboardEFDataSource();
            this.dashboardObjectDataSource1 = new DevExpress.DashboardCommon.DashboardObjectDataSource();
            this.pieDashboardItem1 = new DevExpress.DashboardCommon.PieDashboardItem();
            ((System.ComponentModel.ISupportInitialize)(this.dashboardEFDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dashboardObjectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pieDashboardItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(dimension1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(measure1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // dashboardEFDataSource1
            // 
            this.dashboardEFDataSource1.ComponentName = "dashboardEFDataSource1";
            efConnectionParameters1.ConnectionString = "Data Source=DESKTOP-BLLCV3S;Initial Catalog=ComplaintPortalDB;User ID=sa;Password" +
    "=sa123!@#;MultipleActiveResultSets=True;Application Name=EntityFramework";
            efConnectionParameters1.ConnectionStringName = "";
            efConnectionParameters1.Source = typeof(ComplaintPortalSystem.Models.ComplaintPortalDBEntities);
            this.dashboardEFDataSource1.ConnectionParameters = efConnectionParameters1;
            this.dashboardEFDataSource1.Name = "EF Data Source 1";
            // 
            // dashboardObjectDataSource1
            // 
            this.dashboardObjectDataSource1.ComponentName = "dashboardObjectDataSource1";
            this.dashboardObjectDataSource1.DataSource = typeof(ComplaintPortalSystem.Models.Complaint);
            this.dashboardObjectDataSource1.Name = "Object Data Source 1";
            // 
            // pieDashboardItem1
            // 
            dimension1.DataMember = "Status";
            this.pieDashboardItem1.Arguments.AddRange(new DevExpress.DashboardCommon.Dimension[] {
            dimension1});
            this.pieDashboardItem1.ComponentName = "pieDashboardItem1";
            measure1.DataMember = "ID";
            measure1.SummaryType = DevExpress.DashboardCommon.SummaryType.Count;
            this.pieDashboardItem1.DataItemRepository.Clear();
            this.pieDashboardItem1.DataItemRepository.Add(dimension1, "DataItem0");
            this.pieDashboardItem1.DataItemRepository.Add(measure1, "DataItem1");
            this.pieDashboardItem1.DataMember = "Complaints";
            this.pieDashboardItem1.DataSource = this.dashboardEFDataSource1;
            this.pieDashboardItem1.InteractivityOptions.IgnoreMasterFilters = false;
            this.pieDashboardItem1.Name = "Pies 1";
            this.pieDashboardItem1.ShowCaption = true;
            this.pieDashboardItem1.ShowPieCaptions = false;
            this.pieDashboardItem1.Values.AddRange(new DevExpress.DashboardCommon.Measure[] {
            measure1});
            // 
            // ComplaintByStatus
            // 
            this.DataSources.AddRange(new DevExpress.DashboardCommon.IDashboardDataSource[] {
            this.dashboardObjectDataSource1,
            this.dashboardEFDataSource1});
            this.Items.AddRange(new DevExpress.DashboardCommon.DashboardItem[] {
            this.pieDashboardItem1});
            dashboardLayoutItem1.DashboardItem = this.pieDashboardItem1;
            dashboardLayoutItem1.Weight = 50D;
            dashboardLayoutGroup1.ChildNodes.AddRange(new DevExpress.DashboardCommon.DashboardLayoutNode[] {
            dashboardLayoutItem1});
            dashboardLayoutGroup1.DashboardItem = null;
            dashboardLayoutGroup1.Weight = 100D;
            this.LayoutRoot = dashboardLayoutGroup1;
            this.Title.Text = "Dashboard";
            ((System.ComponentModel.ISupportInitialize)(this.dashboardEFDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dashboardObjectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(dimension1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(measure1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pieDashboardItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.DashboardCommon.DashboardObjectDataSource dashboardObjectDataSource1;
        private DevExpress.DashboardCommon.DashboardEFDataSource dashboardEFDataSource1;
        private DevExpress.DashboardCommon.PieDashboardItem pieDashboardItem1;
    }
}
