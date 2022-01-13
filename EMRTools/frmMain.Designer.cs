namespace EMRTools
{
    partial class frmMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSMS = new System.Windows.Forms.Button();
            this.btnFixDates = new System.Windows.Forms.Button();
            this.btnStandardQueries = new System.Windows.Forms.Button();
            this.btnExecuteSQL = new System.Windows.Forms.Button();
            this.btnQueryBuilder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webBulkSMSSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripDBConnection = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(955, 419);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnSMS);
            this.flowLayoutPanel1.Controls.Add(this.btnFixDates);
            this.flowLayoutPanel1.Controls.Add(this.btnStandardQueries);
            this.flowLayoutPanel1.Controls.Add(this.btnExecuteSQL);
            this.flowLayoutPanel1.Controls.Add(this.btnQueryBuilder);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(5, 47);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(945, 367);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnSMS
            // 
            this.btnSMS.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSMS.Image = ((System.Drawing.Image)(resources.GetObject("btnSMS.Image")));
            this.btnSMS.Location = new System.Drawing.Point(3, 3);
            this.btnSMS.Name = "btnSMS";
            this.btnSMS.Size = new System.Drawing.Size(275, 180);
            this.btnSMS.TabIndex = 0;
            this.btnSMS.Text = "Send SMS";
            this.btnSMS.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSMS.UseVisualStyleBackColor = true;
            this.btnSMS.Click += new System.EventHandler(this.btnSMS_Click);
            // 
            // btnFixDates
            // 
            this.btnFixDates.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFixDates.Image = ((System.Drawing.Image)(resources.GetObject("btnFixDates.Image")));
            this.btnFixDates.Location = new System.Drawing.Point(284, 3);
            this.btnFixDates.Name = "btnFixDates";
            this.btnFixDates.Size = new System.Drawing.Size(275, 180);
            this.btnFixDates.TabIndex = 1;
            this.btnFixDates.Text = "Fix KenyaEMR Dates";
            this.btnFixDates.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnFixDates.UseVisualStyleBackColor = true;
            this.btnFixDates.Click += new System.EventHandler(this.btnFixDates_Click);
            // 
            // btnStandardQueries
            // 
            this.btnStandardQueries.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStandardQueries.Image = ((System.Drawing.Image)(resources.GetObject("btnStandardQueries.Image")));
            this.btnStandardQueries.Location = new System.Drawing.Point(565, 3);
            this.btnStandardQueries.Name = "btnStandardQueries";
            this.btnStandardQueries.Size = new System.Drawing.Size(275, 180);
            this.btnStandardQueries.TabIndex = 2;
            this.btnStandardQueries.Text = "Standard Queries";
            this.btnStandardQueries.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStandardQueries.UseVisualStyleBackColor = true;
            this.btnStandardQueries.Click += new System.EventHandler(this.btnStandardQueries_Click);
            // 
            // btnExecuteSQL
            // 
            this.btnExecuteSQL.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExecuteSQL.Image = ((System.Drawing.Image)(resources.GetObject("btnExecuteSQL.Image")));
            this.btnExecuteSQL.Location = new System.Drawing.Point(3, 189);
            this.btnExecuteSQL.Name = "btnExecuteSQL";
            this.btnExecuteSQL.Size = new System.Drawing.Size(275, 180);
            this.btnExecuteSQL.TabIndex = 3;
            this.btnExecuteSQL.Text = "Execute SQL Query";
            this.btnExecuteSQL.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExecuteSQL.UseVisualStyleBackColor = true;
            this.btnExecuteSQL.Click += new System.EventHandler(this.btnExecuteSQL_Click);
            // 
            // btnQueryBuilder
            // 
            this.btnQueryBuilder.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQueryBuilder.Image = ((System.Drawing.Image)(resources.GetObject("btnQueryBuilder.Image")));
            this.btnQueryBuilder.Location = new System.Drawing.Point(284, 189);
            this.btnQueryBuilder.Name = "btnQueryBuilder";
            this.btnQueryBuilder.Size = new System.Drawing.Size(275, 180);
            this.btnQueryBuilder.TabIndex = 4;
            this.btnQueryBuilder.Text = "Query Builder";
            this.btnQueryBuilder.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnQueryBuilder.UseVisualStyleBackColor = true;
            this.btnQueryBuilder.Click += new System.EventHandler(this.btnQueryBuilder_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "EMR Tools";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(955, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.databaseConnectionToolStripMenuItem,
            this.webBulkSMSSettingsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // databaseConnectionToolStripMenuItem
            // 
            this.databaseConnectionToolStripMenuItem.Name = "databaseConnectionToolStripMenuItem";
            this.databaseConnectionToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.databaseConnectionToolStripMenuItem.Text = "Database Connection";
            this.databaseConnectionToolStripMenuItem.Click += new System.EventHandler(this.databaseConnectionToolStripMenuItem_Click);
            // 
            // webBulkSMSSettingsToolStripMenuItem
            // 
            this.webBulkSMSSettingsToolStripMenuItem.Name = "webBulkSMSSettingsToolStripMenuItem";
            this.webBulkSMSSettingsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.webBulkSMSSettingsToolStripMenuItem.Text = "Web Bulk SMS Settings";
            this.webBulkSMSSettingsToolStripMenuItem.Click += new System.EventHandler(this.webBulkSMSSettingsToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDBConnection,
            this.toolStripStatusLabel1,
            this.toolStripStatusVersion});
            this.statusStrip1.Location = new System.Drawing.Point(0, 443);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(955, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripDBConnection
            // 
            this.toolStripDBConnection.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripDBConnection.Name = "toolStripDBConnection";
            this.toolStripDBConnection.Size = new System.Drawing.Size(100, 17);
            this.toolStripDBConnection.Text = "DB Connection";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel1.Text = "|";
            // 
            // toolStripStatusVersion
            // 
            this.toolStripStatusVersion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusVersion.Name = "toolStripStatusVersion";
            this.toolStripStatusVersion.Size = new System.Drawing.Size(54, 17);
            this.toolStripStatusVersion.Text = "Version";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 465);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EMR Tools";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnSMS;
        private System.Windows.Forms.Button btnFixDates;
        private System.Windows.Forms.Button btnStandardQueries;
        private System.Windows.Forms.Button btnExecuteSQL;
        private System.Windows.Forms.Button btnQueryBuilder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseConnectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem webBulkSMSSettingsToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripDBConnection;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusVersion;
    }
}