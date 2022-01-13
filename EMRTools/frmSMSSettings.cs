using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace EMRTools
{
    public partial class frmSMSSettings : Form
    {
        public frmSMSSettings()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtSMSKey.Text.Length == 0 || txtSMSUsername.Text.Length == 0)
            {
                MessageBox.Show("Please enter the SMS username and Key");
                return;
            }

            try
            {
                string versionDependent = Application.UserAppDataRegistry.Name;
                string versionIndependent = versionDependent.Substring(0, versionDependent.LastIndexOf("\\"));
                Registry.SetValue(versionIndependent, "sms_username", txtSMSUsername.Text.Trim());
                Registry.SetValue(versionIndependent, "sms_apikey", txtSMSKey.Text.Trim());

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
