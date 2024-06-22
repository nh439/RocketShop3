using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using RocketShop.Database.Domain;

namespace RocketShop.Database.NonEntityFramework
{
    public static class SqlExecution
    {
        public static async Task<int> ExecuteSqlAsync(this IDbConnection connection,
           string sql,
           object? param = null,
           IDbTransaction? transaction = null,
           int? timeout = 100) =>
          await connection.ExecuteAsync(sql, param, transaction, timeout);


        public static async Task<IEnumerable<T>> SelectSqlAsync<T>(this IDbConnection connection,
            string sql,
            object? @params = null,
            IDbTransaction? transaction = null,
            int? timeout = null) =>
            await connection.QueryAsync<T>(sql, @params, transaction, timeout);

        public static int ExecuteSql(this IDbConnection connection,
           string sql,
           object? param = null,
           IDbTransaction? transaction = null,
           int? timeout = 100) =>
           connection.Execute(sql, param, transaction, timeout);


        public static IEnumerable<T> SelectSql<T>(this IDbConnection connection,
            string sql,
            object? @params = null,
            IDbTransaction? transaction = null,
            int? timeout = null) =>
            connection.Query<T>(sql, @params, transaction, true, timeout);

        // Spefic Database
        public static DbCommand SetCommand(this IDbConnection connection,
            string sql,
            CommandType type = CommandType.Text)
        {
            DbCommand command = (connection as DbConnection)!.CreateCommand();
            command.CommandText = sql;
            command.CommandType = type;
            return command;
        }

        public static DbCommand AddParameters(this DbCommand command, params QueryParameter[] parameters)
        {
            foreach (var parameter in parameters)
            {
                var param = command.CreateParameter();
                param.ParameterName = parameter.paramName;
                param.Value = parameter.value;
                command.Parameters.Add(param);
            }

            return command;
        }
        public static DbCommand MapParameters<T>(this DbCommand command, T inputobject)
        {
            var prop = typeof(T).GetProperties();
            if (inputobject != null)
                return command;
            foreach (var param in prop)
            {
                string paramName = param.Name;
                object? value = param.GetValue(inputobject)!;
                if (value != null)
                {
                    var para = command.CreateParameter();
                    para.ParameterName = paramName.StartsWith("@") ? paramName : $"@{paramName}";
                    para.Value = value;
                    command.Parameters.Add(para);
                }
            }
            return command;
        }
        public static async Task<IEnumerable<T>> ReadAsync<T>(this DbCommand command, Func<DbDataReader, T> Operation)
        {
            var reader = await command.ExecuteReaderAsync();
            List<T> returnValue = new List<T>();
            while (await reader.ReadAsync())
            {
                var data = Operation(reader);
                returnValue.Add(data);
            }
            reader.Close();
            return returnValue;
        }

        public static async Task<T> ExecuteAsync<T>(this DbCommand command,
            Func<int, T> Operation) =>
            Operation(await command.ExecuteNonQueryAsync());
    }
}
