using ClosedXML.Excel;
using LanguageExt;
using RocketShop.Framework.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.SharedService.Scoped
{
    public interface IExportExcelServices
    {
        Either<Exception, byte[]> ExportExcel(DataTable table);
        Either<Exception, byte[]> ExportExcel(DataSet set);
    }
    public class ExportExcelServices(Serilog.ILogger logger) : BaseServices("Export Excel Service", logger), IExportExcelServices
    {
        public Either<Exception, byte[]> ExportExcel(DataTable table) =>
            InvokeService(() =>
            {
                byte[] data;
                using (XLWorkbook wb = new XLWorkbook())
                {
                    if (string.IsNullOrEmpty(table.TableName))
                        table.TableName = "Sheet1";
                    var sheet = wb.AddWorksheet(table, table.TableName);
                    sheet.Columns().AdjustToContents();
                    sheet.Cells().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        data = stream.ToArray();
                    }
                }
                return data;
            });

        public Either<Exception, byte[]> ExportExcel(DataSet set) =>
            InvokeService(() =>
            {
                byte[] data;
                //    string filename = $"{set.Namespace} {DateTime.Now.ToString("dddd dd MMMM yyyy HH:mm:ss", new System.Globalization.CultureInfo("th-TH"))}";
                string filename = set.Namespace;
                using (XLWorkbook wb = new XLWorkbook())
                {
                    int i = 1;
                    foreach (System.Data.DataTable table in set.Tables)
                    {
                        if (string.IsNullOrEmpty(table.TableName))
                            table.TableName = $"Sheet{i}";
                        var sheet = wb.AddWorksheet(table, table.TableName);
                        sheet.Columns().AdjustToContents();
                        sheet.Cells().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        i++;
                    }
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        data = stream.ToArray();
                    }
                }
                return data;
            });
    }
}
