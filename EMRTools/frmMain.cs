using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Reflection;

namespace EMRTools
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void databaseConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDBSettings settings = new frmDBSettings();
            settings.ShowDialog();
        }

        private void webBulkSMSSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSMSSettings settings = new frmSMSSettings();
            settings.ShowDialog();
        }

        private void btnSMS_Click(object sender, EventArgs e)
        {
            frmSMS sms = new frmSMS();
            sms.ShowDialog();
        }

        private void btnFixDates_Click(object sender, EventArgs e)
        {
            frmFixDates fixdates = new frmFixDates();
            fixdates.ShowDialog();
        }

        private void btnStandardQueries_Click(object sender, EventArgs e)
        {
            frmStandardQueries queries = new frmStandardQueries();
            queries.ShowDialog();
        }

        private void btnExecuteSQL_Click(object sender, EventArgs e)
        {
            frmExecuteQuery execquery = new frmExecuteQuery();
            execquery.ShowDialog();
        }

        private void btnQueryBuilder_Click(object sender, EventArgs e)
        {
            frmQueryBuilder builder = new frmQueryBuilder();
            builder.ShowDialog();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            toolStripStatusVersion.Text = "Version "+Assembly.GetExecutingAssembly().GetName().Version.ToString();

            string constring = UtilityFunctions.GetConnString();

            if (constring.Length == 0)
            {
                frmDBSettings settings = new frmDBSettings();
                settings.ShowDialog();
            }
            else
            {
                try
                {
                    MySqlConnection conn = new MySqlConnection(constring);
                    conn.Open();
                    conn.Close();

                    toolStripDBConnection.Text = "Connected to Database";
                    toolStripDBConnection.ForeColor = Color.DarkGreen;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Database Connection error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    toolStripDBConnection.Text = "Not Connected to Database";
                    toolStripDBConnection.ForeColor = Color.Red;
                }
            }
        }
    }
}
