using LanguageExt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Extension
{
    public static class PostgreSqlExtension
    {
        public static async Task<Either<Exception,long>> CopyFromBinary<T>(
            this IDbConnection connection,
            string tableName,
            string columnToInsert,
            IEnumerable<T> dataToCopy,
            Func<T, object[]> tranformToDataRow)
        {
            if (connection is not Npgsql.NpgsqlConnection npgsqlConnection)
                throw new ArgumentException("Connection must be NpgsqlConnection");
            
            long returnValue = 0;
            try
            {
                await npgsqlConnection.OpenAsync();
                await using var writer = npgsqlConnection.BeginBinaryImport($"COPY {tableName} ({columnToInsert}) FROM STDIN (FORMAT BINARY)");
                foreach (var data in dataToCopy)
                {
                    writer.StartRow();
                    writer.WriteRow(tranformToDataRow(data));
                }
                returnValue = (long)(await writer.CompleteAsync());
            }
            catch(Exception ex)
            {
                return ex;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    await npgsqlConnection.CloseAsync();
            }
            return returnValue;
        }

        public static async Task<Either<Exception, long>> CopyFromCSV(
            this IDbConnection connection,
            string tableName,
            string columnToInsert,
            string csvData
            )
        {
            if (connection is not Npgsql.NpgsqlConnection npgsqlConnection)
                throw new ArgumentException("Connection must be NpgsqlConnection");
            
            long returnValue = 0;
            try
            {
                await npgsqlConnection.OpenAsync();
                using var writer = npgsqlConnection.BeginTextImport($"COPY {tableName} ({columnToInsert}) FROM STDIN CSV");
                foreach (var line in csvData.Split('\n'))
                {
                    await writer.WriteLineAsync(line);
                    returnValue++;
                }
                writer.Close();
            }
            catch(Exception ex)
            {
                return ex;
            }
            finally
            {
                if(connection.State != ConnectionState.Closed)
                    await npgsqlConnection.CloseAsync();
            }
            return returnValue;
        }
    }
}
