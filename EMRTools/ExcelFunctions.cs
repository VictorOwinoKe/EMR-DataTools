using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.IO;
using System.Data;

namespace EMRTools
{
    public class ExcelFunctions
    {
        public void ExportToExcel(DataTable dt)
        {
            var rowCount = dt.Rows.Count;
            string finalFileNameWithPath = string.Empty;

            string myDocsPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            finalFileNameWithPath = myDocsPath + "\\EMR Reports\\Data_export_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + ".xlsx";

            //Delete existing file with same file name.
            if (File.Exists(finalFileNameWithPath))
                File.Delete(finalFileNameWithPath);

            var newFile = new FileInfo(finalFileNameWithPath);
            newFile.Directory.Create();
            using (var package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Data");

                worksheet.Cells["A1"].LoadFromDataTable(dt, true, TableStyles.Medium2);
                var dateColumns = from DataColumn d in dt.Columns
                                  where d.DataType == typeof(DateTime) || d.ColumnName.Contains("Date")
                                  select d.Ordinal + 1;

                foreach (var dc in dateColumns)
                {
                    worksheet.Cells[2, dc, rowCount + 1, dc].Style.Numberformat.Format = "dd-MMM-yyyy";
                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                package.Save();
                System.Diagnostics.Process.Start(finalFileNameWithPath);
            }
        }

        public void ExportToExcel_mysqlFormat(DataTable dt)
        {
            var rowCount = dt.Rows.Count;
            string finalFileNameWithPath = string.Empty;

            string myDocsPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            finalFileNameWithPath = myDocsPath + "\\EMR Reports\\Data_export_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + ".xlsx";

            //Delete existing file with same file name.
            if (File.Exists(finalFileNameWithPath))
                File.Delete(finalFileNameWithPath);

            var newFile = new FileInfo(finalFileNameWithPath);
            newFile.Directory.Create();
            using (var package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Data");

                worksheet.Cells["A1"].LoadFromDataTable(dt, true, TableStyles.Medium2);

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                package.Save();
                System.Diagnostics.Process.Start(finalFileNameWithPath);
            }
        }
    }
}
