using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Com.Danliris.Service.Auth.Lib.Helpers
{
    public static class Excel
    {
        public static MemoryStream CreateExcel(List<KeyValuePair<DataTable, String>> dtSourceList, bool styling = false)
        {
            ExcelPackage package = new ExcelPackage();
            foreach (KeyValuePair<DataTable, String> item in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(item.Value);
                sheet.Cells["A1"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }
    }
}
