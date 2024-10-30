using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Npgsql;
using RocketShop.Warehouse.Admin.Model;
using System.Data;

namespace RocketShop.Warehouse.Admin.Repository
{
    public class CollectionRepository
    {
        public async Task<FlexibleDataReport> CallData(string collectionName,int?page,int? per,NpgsqlConnection warehouseConnection)
        {
            FlexibleDataReport returnValue = new FlexibleDataReport();
            if(warehouseConnection.State != ConnectionState.Open)
                await warehouseConnection.OpenAsync();
            var cmd = warehouseConnection.CreateCommand();
            string query = $@"select * from ""{collectionName}"" ";
            if (page.HasValue)
            {
                query += " limit @limit offset @offset";
                var offsetParam = cmd.CreateParameter();
                offsetParam.ParameterName = "@offset";
                offsetParam.Value = (page.Value -1)*(per ?? 50);

                var limitParam = cmd.CreateParameter();
                limitParam.ParameterName = "@limit";
                limitParam.Value = (per ?? 50);

                cmd.Parameters.Add(offsetParam);
                cmd.Parameters.Add(limitParam);

            }
            await using var reader = await cmd.ExecuteReaderAsync();
            var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToArray();
            returnValue.Columns = columns;
            returnValue.Data = new List<Dictionary<string, object>>();
            Dictionary<string,object> keyValuePairs = new Dictionary<string, object>();
            while (await reader.ReadAsync())
            {
                keyValuePairs = new Dictionary<string, object>();
                foreach (var column in columns)
                {
                    keyValuePairs.Add(column, reader[column]);
                }
                returnValue.Data.Add(keyValuePairs);
            }

            cmd.Parameters.Clear();
            cmd.CommandText = $"select count(*) from \"{collectionName}\"";
            returnValue.TotalCount = (long)(await cmd.ExecuteScalarAsync())!;

            returnValue.CurrentPage = page ?? 1;
            returnValue.TotalPages = (int)Math.Ceiling((decimal)returnValue.TotalCount / (decimal)(per ?? 50));
            return returnValue;
        }
    }
}
