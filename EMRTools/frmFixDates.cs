using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GenericParsing;

namespace EMRTools
{
    public partial class frmFixDates : Form
    {
        DataTable dt;
        DataTable dtCloned;

        public frmFixDates()
        {
            InitializeComponent();
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "CSV File|*.csv";
                openFileDialog1.Title = "Select the VL Results file";

                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (GenericParserAdapter parser = new GenericParserAdapter(openFileDialog1.FileName))
                    {
                        DataSet ds = new DataSet();

                        //Load the data
                        ds = parser.GetDataSet();
                        dt = ds.Tables[0];

                        //Format the data
                        foreach (DataColumn dc in dt.Columns)
                        {
                            dc.ColumnName = dt.Rows[0][dc].ToString();
                        }
                        dt.Rows[0].Delete();

                        //Display the data
                        dgvResults.DataSource = dt;
                    }
                }

                btnFixExcel.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFixExcel_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            //update values
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (dt.Columns[j].ColumnName.ToLower().Contains("date") || dt.Columns[j].ColumnName.Contains("DOB"))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i][j] = FixDateFormat(dt.Rows[i][j].ToString());
                    }
                }
            }

            dtCloned = dt.Clone();

            foreach (DataColumn col in dtCloned.Columns)
            {
                if (col.ColumnName.ToLower().Contains("date") || col.ColumnName.Contains("DOB"))
                {
                    col.DataType = typeof(DateTime);
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                dtCloned.ImportRow(row);
            }

            dgvResults.DataSource = dtCloned;

            btnFixExcel.Enabled = false;

            this.Cursor = Cursors.Default;
        }

        private string FixDateFormat(string rawdate)
        {
            try
            {
                string[] dateparts = rawdate.Split('/');
                return dateparts[0] + "-" + GetMonthInWords(Convert.ToInt16(dateparts[1])) + "-" + dateparts[2];
            }
            catch
            {
                return null;
            }
        }

        private string GetMonthInWords(int no)
        {
            string mnth = "";

            if (no == 1)
            {
                mnth = "Jan";
            }
            else if (no == 2)
            {
                mnth = "Feb";
            }
            else if (no == 3)
            {
                mnth = "Mar";
            }
            else if (no == 4)
            {
                mnth = "Apr";
            }
            else if (no == 5)
            {
                mnth = "May";
            }
            else if (no == 6)
            {
                mnth = "Jun";
            }
            else if (no == 7)
            {
                mnth = "Jul";
            }
            else if (no == 8)
            {
                mnth = "Aug";
            }
            else if (no == 9)
            {
                mnth = "Sep";
            }
            else if (no == 10)
            {
                mnth = "Oct";
            }
            else if (no == 11)
            {
                mnth = "Nov";
            }
            else if (no == 12)
            {
                mnth = "Dec";
            }

            return mnth;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ExcelFunctions func = new ExcelFunctions();
            func.ExportToExcel(dtCloned);
            this.Cursor = Cursors.Default;
        }
    }
}
