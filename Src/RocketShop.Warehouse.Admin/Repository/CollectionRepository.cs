using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Npgsql;
using RocketShop.Framework.Extension;
using RocketShop.Warehouse.Admin.Model;
using System.Data;
using System.Text.RegularExpressions;

namespace RocketShop.Warehouse.Admin.Repository
{
    public class CollectionRepository
    {
        public async Task<FlexibleDataReport> CallData(string collectionName, int? page, int? per, NpgsqlConnection warehouseConnection)
        {
            FlexibleDataReport returnValue = new FlexibleDataReport();
            if (warehouseConnection.State != ConnectionState.Open)
                await warehouseConnection.OpenAsync();
            var cmd = warehouseConnection.CreateCommand();
            string query = $@"select * from ""{collectionName}"" ";
            if (page.HasValue)
            {
                query += " limit @limit offset @offset";
                var offsetParam = cmd.CreateParameter();
                offsetParam.ParameterName = "@offset";
                offsetParam.Value = (page.Value - 1) * (per ?? 50);

                var limitParam = cmd.CreateParameter();
                limitParam.ParameterName = "@limit";
                limitParam.Value = (per ?? 50);

                cmd.Parameters.Add(offsetParam);
                cmd.Parameters.Add(limitParam);

            }
            cmd.CommandText = query;
            await using var reader = await cmd.ExecuteReaderAsync();
            var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToArray();
            returnValue.Columns = columns;
            returnValue.Data = await ReadStreamingAsync(reader);
            cmd.Parameters.Clear();
            cmd.CommandText = $"select count(*) from \"{collectionName}\"";
            returnValue.TotalCount = (long)(await cmd.ExecuteScalarAsync())!;

            returnValue.CurrentPage = page ?? 1;
            returnValue.TotalPages = (int)Math.Ceiling((decimal)returnValue.TotalCount / (decimal)(per ?? 50));
            await warehouseConnection.CloseAsync();
            return returnValue;
        }

        public async Task<FlexibleDataReport> CallDataWithCondition(string collectionName, string where, int? page, int? per, NpgsqlConnection warehouseConnection)
        {
            var hasInjection = ContainsInjection(where);
            if (hasInjection)
                throw new AccessViolationException("Query Injection Detected");
            FlexibleDataReport returnValue = new FlexibleDataReport();
            if (warehouseConnection.State != ConnectionState.Open)
                await warehouseConnection.OpenAsync();
            var cmd = warehouseConnection.CreateCommand();

            string query = where.ToLower().StartsWith("order by") ? $@"select * from ""{collectionName}"" {where}" : $@"select * from ""{collectionName}"" where {where}";
            if (page.HasValue)
            {
                query += " limit @limit offset @offset";
                var offsetParam = cmd.CreateParameter();
                offsetParam.ParameterName = "@offset";
                offsetParam.Value = (page.Value - 1) * (per ?? 50);

                var limitParam = cmd.CreateParameter();
                limitParam.ParameterName = "@limit";
                limitParam.Value = (per ?? 50);

                cmd.Parameters.Add(offsetParam);
                cmd.Parameters.Add(limitParam);

            }
            cmd.CommandText = query;
            await using var reader = await cmd.ExecuteReaderAsync();
            var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToArray();
            returnValue.Columns = columns;
            returnValue.Data = await ReadStreamingAsync(reader);

            await reader.CloseAsync();
            cmd.Parameters.Clear();
            cmd.CommandText = where.IsNullOrEmpty().Or(where.ToLower().StartsWith("order by")) ? $"select count(*) from \"{collectionName}\"" : $"select count(*) from \"{collectionName}\" where {where}";
            returnValue.TotalCount = (long)(await cmd.ExecuteScalarAsync())!;

            returnValue.CurrentPage = page ?? 1;
            returnValue.TotalPages = (int)Math.Ceiling((decimal)returnValue.TotalCount / (decimal)(per ?? 50));
            await warehouseConnection.CloseAsync();
            return returnValue;
        }

        async Task<List<Dictionary<string, object>>> ReadStreamingAsync(NpgsqlDataReader reader)
        {
            List<Dictionary<string, object>> returnValue = new List<Dictionary<string, object>>();
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToArray();
            while (await reader.ReadAsync())
            {
                keyValuePairs = new Dictionary<string, object>();
                foreach (var column in columns)
                {
                    keyValuePairs.Add(column, reader[column]);
                }
                returnValue.Add(keyValuePairs);
            }
            reader.Close();
            return returnValue;
        }

        static bool ContainsInjection(string query)
        {
            // Regular expression to detect "1=1" or "'[ValueA]'='[ValueA]'"
            string pattern = @"(\b1\s*=\s*1\b)|('[^']+'\s*=\s*'[^']+')";

            // Check if the query contains a match
            return Regex.IsMatch(query, pattern, RegexOptions.IgnoreCase);
        }
    }
}
