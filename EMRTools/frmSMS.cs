using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using GsmComm.PduConverter;
using GsmComm.GsmCommunication;
using System.Management;
using System.Threading;

namespace EMRTools
{
    public partial class frmSMS : Form
    {
        GsmCommMain comm;
        private delegate void ConnectedHandler(bool connected);

        public frmSMS()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sQuery = "";
            DataTable dt;

            if (cboList.SelectedIndex == 1) //appointment list
            {
                sQuery = @"select a.unique_patient_no
                        , a.patient_clinic_number
                        , concat(ifnull(a.given_name, ''), ' ', ifnull(a.middle_name, ''), ' ', ifnull(a.family_name, '')) as Name
                        , TIMESTAMPDIFF(YEAR, a.DOB, '" + dateTimePicker2.Text + @"') AS age
                        , a.Gender
                        , a.phone_number as Phone
                        , b.latest_tca as Appointment_date
                        from kenyaemr_etl.etl_patient_demographics a
                        inner
                        join kenyaemr_etl.etl_current_in_care b on a.patient_id = b.patient_id
                        where b.latest_tca between '" + dateTimePicker1.Text + @"' and '" + dateTimePicker2.Text + @"'
                        and length(a.phone_number) > 1";
            }

            MySqlConnection conn = new MySqlConnection(UtilityFunctions.GetConnString());
            MySqlCommand command = new MySqlCommand(sQuery, conn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataSet dataset = new DataSet();

            try
            {
                adapter.Fill(dataset);
                dt = dataset.Tables[0];
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SQL query error");
            }
        }

        private void btnLoadExcel_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx";
                openFileDialog1.Title = "Select the list";

                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    dataGridView1.DataSource = null;
                    DataTable dt = GetDataTableFromExcel(openFileDialog1.FileName);
                    dataGridView1.DataSource = dt;
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tabPageSendSMS)
            {
                try
                {
                    string phone = dataGridView1.Rows[0].Cells["phone"].Value.ToString();
                }
                catch
                {
                    MessageBox.Show("Please ensure your list has PHONE column, and that you have at least one record", "Missing details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tabControl1.SelectedTab = tabPageRecipients;
                    return;
                }

                lblRecipients.Text = dataGridView1.Rows.Count.ToString() + " recipients";
            }
        }

        private void radioModem_CheckedChanged(object sender, EventArgs e)
        {
            if (radioModem.Checked)
            {
                cboModem.Visible = true;
                btnModem.Visible = true;
                lblCredit.Visible = false;
                lblCreditLabel.Visible = true;

                lblCreditLabel.Text = "Select modem:";
            }
        }

        private void radioWeb_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (radioWeb.Checked)
            {
                cboModem.Visible = false;
                btnModem.Visible = false;
                lblCredit.Visible = true;
                lblCreditLabel.Visible = true;

                lblCreditLabel.Text = "SMS Credit bal:";
                DisplayWebServiceSMSBalance();
            }

            this.Cursor = Cursors.Default;
        }

        public DataTable GetContentAsDataTable(DataGridView dgv, bool IgnoreHideColumns)
        {
            try
            {
                if (dgv.ColumnCount == 0) return null;
                DataTable dtSource = new DataTable();
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (IgnoreHideColumns & !col.Visible) continue;
                    if (col.Name == string.Empty) continue;
                    dtSource.Columns.Add(col.Name, col.ValueType);
                    dtSource.Columns[col.Name].Caption = col.HeaderText;
                }
                if (dtSource.Columns.Count == 0) return null;
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    try
                    {
                        DataRow drNewRow = dtSource.NewRow();
                        foreach (DataColumn col in dtSource.Columns)
                        {
                            drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value;
                        }
                        dtSource.Rows.Add(drNewRow);
                    }
                    catch { }
                }
                return dtSource;
            }
            catch { return null; }
        }

        private void SendSMSWebUsingService(DataTable dt, string sMessage)
        {
            string textMsg = sMessage;
            string phoneNo = string.Empty;
            string patientname = "";
            string[] sPhoneNos = { };

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                textMsg = sMessage;
                phoneNo = string.Empty;

                try
                {
                    sPhoneNos = dt.Rows[i]["Phone"].ToString().Split('/');
                    patientname = "";
                    try
                    {
                        patientname = dt.Rows[i]["name"].ToString();
                    }
                    catch { }
                }
                catch
                {

                }

                for (int j = 0; j < sPhoneNos.Length; j++)
                {
                    phoneNo = sPhoneNos[j].Trim();

                    try
                    {
                        AfricasTalkingGateway gateway = new AfricasTalkingGateway(UtilityFunctions.GetSMSUsername(), UtilityFunctions.GetSMSAPIKey());
                        dynamic results = gateway.sendMessage(phoneNo, sMessage);

                        lstMsgLogs.Invoke(new MethodInvoker(delegate
                        {
                            lstMsgLogs.Items.Add(phoneNo + " " + patientname + " - " + results[0]["status"]);
                        }));
                    }
                    catch (Exception e)
                    {
                        lstMsgLogs.Invoke(new MethodInvoker(delegate
                        {
                            lstMsgLogs.Items.Add(phoneNo + " :" + patientname + " FAILED. Error: " + e.Message);
                        }));
                    }
                }
            }

            this.Invoke(new MethodInvoker(delegate
            {
                DisplayWebServiceSMSBalance();
                this.Cursor = Cursors.Default;
                btnSend.Text = "Send";
                btnSend.Enabled = true;
            }));
        }

        public void DisplayWebServiceSMSBalance()
        {
            string bal = "";

            dynamic response;

            try
            {
                AfricasTalkingGateway gateway = new AfricasTalkingGateway(UtilityFunctions.GetSMSUsername(), UtilityFunctions.GetSMSAPIKey());
                response = gateway.getUserData();
                bal = response["balance"];
            }
            catch (Exception ex)
            {
                bal = ex.Message;
            }

            lblCredit.Text = bal;
        }

        private void SendSMSusingModem(DataTable dt, string sMessage)
        {
            string textMsg = sMessage;
            string phoneNo = string.Empty;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                textMsg = sMessage;
                phoneNo = string.Empty;
                string[] sPhoneNos = dt.Rows[i]["phone"].ToString().Split('/');
                string patientname = "";
                try
                {
                    patientname = dt.Rows[i]["name"].ToString();
                }
                catch { }

                for (int j = 0; j < sPhoneNos.Length; j++)
                {
                    try
                    {
                        phoneNo = "+254" + sPhoneNos[j].Trim().Substring(1);
                        if (textMsg.Length <= 160)
                        {
                            SmsSubmitPdu pdu = new SmsSubmitPdu(textMsg, phoneNo, "");
                            comm.SendMessage(pdu);
                            pdu.RequestStatusReport = true;
                        }
                        else if (textMsg.Length > 160 && textMsg.Length < 310)
                        {
                            SmsSubmitPdu pdu = new SmsSubmitPdu("1/2: " + textMsg.Substring(0, 155), phoneNo, "");
                            pdu.RequestStatusReport = true;
                            comm.SendMessage(pdu);
                            pdu = new SmsSubmitPdu("2/2: " + textMsg.Substring(155), phoneNo, "");
                            pdu.RequestStatusReport = true;
                            comm.SendMessage(pdu);
                        }
                        else if (textMsg.Length >= 310 && textMsg.Length < 450)
                        {
                            SmsSubmitPdu pdu = new SmsSubmitPdu("1/3: " + textMsg.Substring(0, 155), phoneNo, "");
                            pdu.RequestStatusReport = true;
                            comm.SendMessage(pdu);
                            pdu = new SmsSubmitPdu("2/3: " + textMsg.Substring(155, 155), phoneNo, "");
                            pdu.RequestStatusReport = true;
                            comm.SendMessage(pdu);
                            pdu = new SmsSubmitPdu("3/3: " + textMsg.Substring(311), phoneNo, "");
                            pdu.RequestStatusReport = true;
                            comm.SendMessage(pdu);
                        }
                        else
                        {
                            SmsSubmitPdu pdu = new SmsSubmitPdu("1/3: " + textMsg.Substring(0, 155), phoneNo, "");
                            pdu.RequestStatusReport = true;
                            comm.SendMessage(pdu);
                            pdu = new SmsSubmitPdu("2/3: " + textMsg.Substring(155, 155), phoneNo, "");
                            pdu.RequestStatusReport = true;
                            comm.SendMessage(pdu);
                            pdu = new SmsSubmitPdu("3/3: " + textMsg.Substring(311, 155), phoneNo, "");
                            pdu.RequestStatusReport = true;
                            comm.SendMessage(pdu);
                        }

                        lstMsgLogs.Invoke(new MethodInvoker(delegate
                        {
                            lstMsgLogs.Items.Add(phoneNo + " :" + patientname + " SUCCESSFUL");
                        }));
                    }
                    catch (Exception ex)
                    {
                        lstMsgLogs.Invoke(new MethodInvoker(delegate
                        {
                            lstMsgLogs.Items.Add(phoneNo + " :" + patientname + " FAILED. Error: " + ex.Message);
                        }));
                    }
                }
            }

            this.Invoke(new MethodInvoker(delegate
            {
                this.Cursor = Cursors.Default;
                btnSend.Text = "Send";
                btnSend.Enabled = true;
            }));
        }

        private void btnModem_Click(object sender, EventArgs e)
        {
            comm = null;
            int port = 3;
            int baudrate = 19200;
            int timeout = 2000;

            cboModem.Items.Clear();
            cboModem.Text = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_POTSModem");
            foreach (ManagementObject modem in searcher.Get())
            {
                try
                {
                    port = (int)Convert.ToSingle(modem.GetPropertyValue("AttachedTo").ToString().Substring(3));
                    Cursor.Current = Cursors.WaitCursor;
                    comm = new GsmCommMain(port, baudrate, timeout);
                    Cursor.Current = Cursors.WaitCursor;
                    comm.Open();
                    Cursor.Current = Cursors.Default;
                    if (comm.IsOpen()) cboModem.Items.Add(modem.GetPropertyValue("Name"));
                    comm.Close();
                }
                catch
                { }
                finally
                { if (comm.IsOpen()) comm.Close(); }
            }
            if (cboModem.Items.Count == 1) cboModem.SelectedIndex = 0;
            else
                MessageBox.Show("No active modems found in this computer");
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (radioModem.Checked && cboModem.Text == string.Empty)
            {
                MessageBox.Show("Please select a modem", "Modem not selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboModem.Focus();
                return;
            }

            if (radioWeb.Checked && lblCredit.Text.Contains("KES") == false)
            {
                MessageBox.Show("SMS settings are missing. Please configure the Bulk SMS Settings on the settings section", "SMS Settings missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtSMS.Text == string.Empty)
            {
                MessageBox.Show("Please enter a message", "Message missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSMS.Focus();
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            btnSend.Text = "Sending...";
            btnSend.Enabled = false;

            DataTable data = GetContentAsDataTable(dataGridView1, true);
            string smsMessage = txtSMS.Text;

            if (radioModem.Checked)
            {
                Thread smsThread = new Thread(() => SendSMSusingModem(data, smsMessage));
                smsThread.Start();
            }
            else
            {
                Thread smsThread = new Thread(() => SendSMSWebUsingService(data, smsMessage));
                smsThread.Start();
            }
        }

        private void frmSMS_Load(object sender, EventArgs e)
        {
            cboList.SelectedIndex = 0;
            radioModem.Checked = true;
        }

        private void cboModem_SelectedIndexChanged(object sender, EventArgs e)
        {
            int port = 3;
            int baudrate = 19200;
            int timeout = 2000;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_POTSModem");
            foreach (ManagementObject modem in searcher.Get())
            {
                if (modem.GetPropertyValue("Name").ToString().Trim().ToLower() == cboModem.Text.Trim().ToLower())
                {
                    port = (int)Convert.ToSingle(modem.GetPropertyValue("AttachedTo").ToString().Substring(3)); ;
                }

            }

            Cursor.Current = Cursors.WaitCursor;
            comm = new GsmCommMain(port, baudrate, timeout);
            Cursor.Current = Cursors.Default;
            comm.PhoneConnected += new EventHandler(comm_PhoneConnected);
            comm.PhoneDisconnected += new EventHandler(comm_PhoneDisconnected);

            bool retry;
            do
            {
                retry = false;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (!comm.IsOpen()) comm.Open();
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception)
                {
                    if (MessageBox.Show(this, "Unable to open this modem on port: " + port, "Error",
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
                        retry = true;
                    else
                    {
                        return;
                    }
                }
            }
            while (retry);
        }

        private void comm_PhoneConnected(object sender, EventArgs e)
        {
            this.Invoke(new ConnectedHandler(OnPhoneConnectionChange), new object[] { true });
        }

        private void OnPhoneConnectionChange(bool connected)
        {
            //lblNotConnected.Visible = !connected;
        }

        private void comm_PhoneDisconnected(object sender, EventArgs e)
        {
            try
            {
                this.Invoke(new ConnectedHandler(OnPhoneConnectionChange), new object[] { false });
            }
            catch
            {

            }
        }

        private void frmSMS_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (comm != null)
            {
                if (comm.IsConnected() || comm.IsOpen())
                {
                    comm.Close();
                }
            }
        }
    }
}
