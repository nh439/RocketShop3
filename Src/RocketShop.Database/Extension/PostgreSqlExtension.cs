using DocumentFormat.OpenXml.Spreadsheet;
using LanguageExt;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Extension
{
    public static class PostgreSqlExtension
    {
        /// <summary>
        /// Copies binary data into a database table asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the data to copy.</typeparam>
        /// <param name="connection">The database connection to use.</param>
        /// <param name="tableName">The name of the table to insert data into.</param>
        /// <param name="columnToInsert">The column in which the data will be inserted.</param>
        /// <param name="dataToCopy">The collection of data items to copy.</param>
        /// <param name="tranformToDataRow">A function that transforms each data item into an array of objects representing a row.</param>
        /// <param name="logSql">Determines whether SQL statements should be logged. Defaults to <c>false</c>.</param>
        /// <param name="logData">Determines whether data being copied should be logged. Defaults to <c>false</c>.</param>
        /// <returns>
        /// An <see cref="Either{Exception, long}"/> representing either an exception if the operation fails 
        /// or the number of rows successfully copied.
        /// </returns>
        public static async Task<Either<Exception,long>> CopyFromBinary<T>(
            this IDbConnection connection,
            string tableName,
            string columnToInsert,
            IEnumerable<T> dataToCopy,
            Func<T, object[]> tranformToDataRow,
            bool logSql = false,
            bool logData = false)
        {
            if (connection is not Npgsql.NpgsqlConnection npgsqlConnection)
                throw new ArgumentException("Connection must be NpgsqlConnection");
            
            long returnValue = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                if(npgsqlConnection.State != ConnectionState.Open)
                    await npgsqlConnection.OpenAsync();
                var sql = $"COPY \"{tableName}\" ({columnToInsert}) FROM STDIN (FORMAT BINARY)";
                if (logSql.Or(logData))
                {
                    Console.WriteLine($"Begin COPY FROM BINARY at {DateTime.Now.ToDateAndTimeFormat()}");
                    if(logSql)
                        Console.WriteLine($"{DateTime.Now.ToDateAndTimeFormat()} Executing : {sql}");
                }
                   
                await using var writer = npgsqlConnection.BeginBinaryImport(sql);
                foreach (var data in dataToCopy)
                {
                    object?[] row = tranformToDataRow(data);
                    writer.WriteRow(row);
                    if(logData)
                        Console.WriteLine($"{DateTime.Now.ToDateAndTimeFormat()} Data : ({string.Join(",", row)})");
                }
                returnValue = (long)(await writer.CompleteAsync());
                if (logSql.Or(logData))   
                    Console.WriteLine($" COPY FROM BINARY COMPLETED at {DateTime.Now.ToDateAndTimeFormat()} Duration {(sw.ElapsedMilliseconds/1000).ToString("N2")} ");
                
            }
            catch(Exception ex)
            {
                if (logSql.Or(logData))
                    Console.WriteLine($" COPY FROM BINARY FAILED at {DateTime.Now.ToDateAndTimeFormat()} Duration {(sw.ElapsedMilliseconds / 1000).ToString("N2")} \n Cause {ex.Message} ");
                return ex;
            }
            finally
            {
                sw.Stop();
                if (connection.State != ConnectionState.Closed)
                    await npgsqlConnection.CloseAsync();
            }
            return returnValue;
        }

        /// <summary>
        /// Copies data from a CSV string into a database table asynchronously.
        /// </summary>
        /// <param name="connection">The database connection to use.</param>
        /// <param name="tableName">The name of the table to insert data into.</param>
        /// <param name="columnToInsert">The column in which the data will be inserted.</param>
        /// <param name="csvData">The CSV-formatted string containing the data to be copied.</param>
        /// <param name="logSql">Determines whether SQL statements should be logged. Defaults to <c>false</c>.</param>
        /// <param name="logData">Determines whether data being copied should be logged. Defaults to <c>false</c>.</param>
        /// <returns>
        /// An <see cref="Either{Exception, long}"/> representing either an exception if the operation fails 
        /// or the number of rows successfully copied.
        /// </returns>
        public static async Task<Either<Exception, long>> CopyFromCSV(
            this IDbConnection connection,
            string tableName,
            string columnToInsert,
            string csvData,
            bool logSql = false,
            bool logData = false
            )
        {
            if (connection is not Npgsql.NpgsqlConnection npgsqlConnection)
                throw new ArgumentException("Connection must be NpgsqlConnection");
            
            long returnValue = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                if (npgsqlConnection.State != ConnectionState.Open)
                    await npgsqlConnection.OpenAsync();
                var sql = $"COPY \"{tableName}\" ({columnToInsert}) FROM STDIN CSV";
                if (logSql.Or(logData))
                {
                    Console.WriteLine($"Begin COPY FROM CSV at {DateTime.Now.ToDateAndTimeFormat()}");
                    if (logSql)
                        Console.WriteLine($"{DateTime.Now.ToDateAndTimeFormat()} Executing : {sql}");
                }
                using var writer = npgsqlConnection.BeginTextImport(sql);
                foreach (var line in csvData.Split('\n'))
                {
                    if (logData)
                        Console.WriteLine($"{DateTime.Now.ToDateAndTimeFormat()} Data : ({line})");
                    await writer.WriteLineAsync(line);
                    returnValue++;
                   
                }
                writer.Close();
                if (logSql.Or(logData))
                    Console.WriteLine($" COPY FROM CSV COMPLETED at {DateTime.Now.ToDateAndTimeFormat()} Duration {(sw.ElapsedMilliseconds / 1000).ToString("N2")} Seconds");
            }
            catch(Exception ex)
            {
                if (logSql.Or(logData))
                    Console.WriteLine($" COPY FROM CSV FAILED at {DateTime.Now.ToDateAndTimeFormat()} Duration {(sw.ElapsedMilliseconds / 1000).ToString("N2")} Seconds \n Cause {ex.Message}");
                return ex;
            }
            finally
            {
                sw.Stop();
                if (connection.State != ConnectionState.Closed)
                    await npgsqlConnection.CloseAsync();
            }
            return returnValue;
        }
    }
}
