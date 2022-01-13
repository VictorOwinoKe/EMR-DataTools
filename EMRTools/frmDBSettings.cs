using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.Win32;

namespace EMRTools
{
    public partial class frmDBSettings : Form
    {
        public frmDBSettings()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string constring = "Server = " + txtServer.Text + "; port = " + txtPort.Text + "; database = " + txtDB.Text + "; UID = " + txtLogin.Text + "; password = " + txtPassword.Text;

                MySqlConnection conn = new MySqlConnection(constring);
                conn.Open();
                conn.Close();

                string versionDependent = Application.UserAppDataRegistry.Name;
                string versionIndependent = versionDependent.Substring(0, versionDependent.LastIndexOf("\\"));
                Registry.SetValue(versionIndependent, "ConnectionString", constring);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
