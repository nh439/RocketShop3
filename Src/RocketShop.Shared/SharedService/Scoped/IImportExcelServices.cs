using ClosedXML.Excel;
using LanguageExt;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.SharedService.Scoped
{
    public interface IImportExcelServices
    {
        Either<Exception, DataTable> ReadExcel(byte[] data);
        Task<Either<Exception, DataTable>> ReadExcelAsync(byte[] data);
    }
    public class ImportExcelServices(Serilog.ILogger logger) : BaseServices("Import Excel Services",logger), IImportExcelServices
    {
        public Either<Exception, DataTable> ReadExcel(byte[] data) =>
            InvokeService(() =>
            {
                DataTable result = new DataTable();
                MemoryStream stream = new MemoryStream(data);
                var workbook = new XLWorkbook(stream);
                var sheet = workbook.Worksheet(1);
                bool firstRow = true;
                foreach (var row in sheet.Rows())
                {
                    if (firstRow)
                    {
                        foreach (var cell in row.Cells())
                        {
                            result.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                        continue;
                    }
                    List<string> values = new List<string>();
                    foreach (var cell in row.Cells())
                    {
                        values.Add(cell.Value.ToString());
                    }
                    result.Rows.Add(values.ToArray());
                }
                stream.Close();
                return result;
            });

        public async Task< Either<Exception, DataTable>> ReadExcelAsync(byte[] data) =>
            await InvokeServiceAsync<DataTable>(async () => { 
               var result = await Task.Factory.StartNew(() => ReadExcel(data));
                if (result.IsLeft)
                    throw result.GetLeft()!;
                return result.GetRight()!;
            });
    }
}
