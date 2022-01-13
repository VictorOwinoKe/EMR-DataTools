using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace EMRTools
{
    public partial class frmExecuteQuery : Form
    {
        DataTable queryResult;

        public frmExecuteQuery()
        {
            InitializeComponent();
        }

        private void frmExecuteQuery_Load(object sender, EventArgs e)
        {
            txtQuery.Focus();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (e.TabPage == tabPageExecute)
            {
                MySqlConnection conn = new MySqlConnection(UtilityFunctions.GetConnString());
                MySqlCommand command = new MySqlCommand(txtQuery.Text, conn);
                command.CommandTimeout = 0;
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataSet dataset = new DataSet();

                try
                {
                    adapter.Fill(dataset);
                    queryResult = dataset.Tables[0];
                    dataGridView1.DataSource = queryResult;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "SQL query error.");
                }
            }

            this.Cursor = DefaultCursor;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ExcelFunctions func = new ExcelFunctions();
            func.ExportToExcel(queryResult);
            this.Cursor = Cursors.Default;
        }

        private void txtQuery_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {  
                ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
                MenuItem menuItem = new MenuItem("Cut");
                menuItem.Click += new EventHandler(CutAction);
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem("Copy");
                menuItem.Click += new EventHandler(CopyAction);
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem("Paste");
                menuItem.Click += new EventHandler(PasteAction);
                contextMenu.MenuItems.Add(menuItem);

                txtQuery.ContextMenu = contextMenu;
            }
        }

        void CutAction(object sender, EventArgs e)
        {
            txtQuery.Cut();
        }

        void CopyAction(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Rtf, txtQuery.SelectedRtf);
            Clipboard.Clear();
        }

        void PasteAction(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText(TextDataFormat.Rtf))
            {
                txtQuery.SelectedRtf = Clipboard.GetData(DataFormats.Rtf).ToString();
            }
        } 
    }
}
