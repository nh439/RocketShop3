using Dapper;
using LanguageExt;
using LanguageExt.ClassInstances.Pred;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.NonEntityFramework.QueryGenerator
{
    public static class QueryGenerator
    {
        public static QueryStore CreateQueryStore(this IDbConnection connection,
            string tableName) => new QueryStore(connection, tableName);

        public static SqlResult Compiled(this QueryStore store,StatementType statementType = StatementType.Select,object? updateValue = null)
        {
            string sql = string.Empty;
            Dictionary<string, object> data = new Dictionary<string, object>();
            var columns = store.SelectedColumns.HasDataAndTranformData(
                s => string.Join(",", s.Select(t=> $"\"{t}\"") ),
                () => "*");
            if(statementType == StatementType.Select ) 
                sql = $"select {columns} from \"{store.TableName}\" ";
            else if(statementType == StatementType.Delete )
                sql = $"delete from \"{store.TableName}\" ";
            else if(statementType==StatementType.Update)
            {
                if (updateValue.IsNull())
                    throw new ArgumentNullException("Update Value Required ON Update Statement Type");
                var updateProp = updateValue!.GetType().GetProperties();
                List<string> setVal = new List<string>();
                int updateIdx = 0;
                foreach (var prop in updateProp) {
                    string paramName = $"up_{updateIdx}";
                    setVal.Add($" \"{prop.Name}\"=@{paramName}");
                    data.Add(paramName, prop.GetValue(updateValue)!);
                    updateIdx++;
                }
                sql = $"update \"{store.TableName}\" set {string.Join(",",setVal)} ";
            }       
            int paramId = 0;
            if (store.conditions.HasData())
            {
                string where = " where ";
                bool firstLoop = true;
                foreach (var cond in store.conditions!)
                {
                    string c = string.Empty;
                    if (!firstLoop)
                        where += $" {(cond.RelatedOrCondition ? "or" : "and")} ";
                    firstLoop = false;
                    if (new[] { SqlOperator.In, SqlOperator.NotIn }.Contains(cond.Operator))
                    {
                        var val = cond.ValueIn;
                        List<string> paset = new List<string>();
                        val.HasDataAndForEach(x =>
                        {
                            string paramName = $"P_{paramId}";
                            paset.Add(paramName);
                            data.Add(paramName, x);
                            paramId++;
                        });
                        var cd = $" \"{cond.ColumnName}\" {cond.Operator.GetOperator()} ({string.Join(",", $"{string.Join(",", paset.Select(s => $"@{s}"))}")}) ";
                        where += cd;
                        continue;
                    }
                    else if (new[] { SqlOperator.Null, SqlOperator.NotNull }.Contains(cond.Operator))
                    {
                        var cd = $" \"{cond.ColumnName}\" {cond.Operator.GetOperator()} ";
                        where += cd;
                        continue;
                    }
                    else if (new[] { SqlOperator.Between, SqlOperator.NotBetween }.Contains(cond.Operator))
                    {
                        var paramNameMin = $"P_{paramId}_Min";
                        var paramNameMax = $"P_{paramId}_Max";
                        var cd = $" \"{cond.ColumnName}\" {cond.Operator.GetOperator()} @{paramNameMin} and @{paramNameMax}";
                        data.Add(paramNameMin, cond.ValueMin!);
                        data.Add(paramNameMax, cond.ValueMax!);
                        where += cd;
                        paramId++;
                        continue;
                    }
                    if(cond.Operator == SqlOperator.WhereSub)
                    {
                        var subQueryObj = cond.queryStore.conditions!.CompiledConditionWithParameterizedMode();
                        where += $" ({subQueryObj.Item1})";
                        subQueryObj.Item2.HasDataAndForEach(d => data.Add(d.Key, d.Value));
                        continue;
                    }
                    string pn = $"P_{paramId}";
                    var ce = $" \"{cond.ColumnName}\" {cond.Operator.GetOperator()} @{pn}";
                    data.Add(pn, cond.Value!);
                    where += ce;
                    paramId++;
                }
                sql += $" {where} ";
            }
            if (store.orderBies.HasData())
            {
                sql += " order by ";
                var dd = store.orderBies.Select(s => $" \"{s.Column}\" {(s.Desc ? "desc" : string.Empty)}");
                var odb = string.Join(",", dd);
                sql += odb;
            }
            if (store.QueryPaging.IsNotNull())
            {

                sql += " limit @limit offset @offset";
                data.Add("limit", store.QueryPaging!.Per);
                data.Add("offset", (store.QueryPaging!.Page - 1) * store.QueryPaging!.Per);
            }
            sql += ";";
            return new SqlResult(sql, data);
        }

        public static async Task<SqlResult> CompiledAsync(this QueryStore store) =>
           await Task.Run(() => store.Compiled());

        /// <summary>
        /// Fetching Data From Database
        /// </summary>
        /// <typeparam name="T">Entity To Fetch</typeparam>
        /// <param name="store">Query Store</param>
        /// <param name="transaction">Db Transaction</param>
        /// <param name="commandTimeout">Connection Maximum Time (Second)</param>
        /// <returns>Enumerable Of Entity</returns>
        public static IEnumerable<T> Fetch<T>(this QueryStore store, IDbTransaction? transaction = null, int commandTimeout = 30)
        {
            var compiledResult = store.Compiled();
            if (store.DebugMode)
                Console.WriteLine($"Executing : {compiledResult.Sql}");
            return store.connection.Query<T>(compiledResult.Sql, compiledResult.Parameters, transaction, commandTimeout: commandTimeout);
        }
        /// <summary>
        /// Fetching Data From Database With Asynchronous Pattern
        /// </summary>
        /// <typeparam name="T">Entity To Fetch</typeparam>
        /// <param name="store">Query Store</param>
        /// <param name="transaction">Db Transaction</param>
        /// <param name="commandTimeout">Connection Maximum Time (Second)</param>
        
        /// <returns>Enumerable Of Entity</returns>
        public static async Task<IEnumerable<T>> FetchAsync<T>(this QueryStore store, IDbTransaction? transaction = null, int commandTimeout = 30)
        {
            var compiledResult = await store.CompiledAsync();
            if (store.DebugMode)
                Console.WriteLine($"Executing : {compiledResult.Sql}");
            return await store.connection.QueryAsync<T>(compiledResult.Sql, compiledResult.Parameters, transaction, commandTimeout: commandTimeout);
        }
        /// <summary>
        /// Fetching Data From Database
        /// </summary>
        /// <typeparam name="T">Entity To Fetch</typeparam>
        /// <param name="store">Query Store</param>
        /// <param name="transaction">Db Transaction</param>
        /// <param name="commandTimeout">Connection Maximum Time (Second)</param>
        
        /// <returns>List Of Entity</returns>
        public static List<T> ToList<T>(this QueryStore store, IDbTransaction? transaction = null, int commandTimeout = 30) =>
            store.Fetch<T>(transaction,commandTimeout).ToList();
        /// <summary>
        /// Fetching Data From Database With Asynchronous Pattern
        /// </summary>
        /// <typeparam name="T">Entity To Fetch</typeparam>
        /// <param name="store">Query Store</param>
        /// <param name="transaction">Db Transaction</param>
        /// <param name="commandTimeout">Connection Maximum Time (Second)</param>
        
        /// <returns>List Of Entity</returns>
        public static async Task<List<T>> ToListAsync<T>(this QueryStore store, IDbTransaction? transaction = null, int commandTimeout = 30) =>
          (await store.FetchAsync<T>(transaction,commandTimeout)).ToList();
        /// <summary>
        /// Fetching Data From Database
        /// </summary>
        /// <typeparam name="T">Entity To Fetch</typeparam>
        /// <param name="store">Query Store</param>
        /// <param name="transaction">Db Transaction</param>
        /// <param name="commandTimeout">Connection Maximum Time (Second)</param>
        
        /// <returns>Array Of Entity</returns>
        public static T[] ToArray<T>(this QueryStore store, IDbTransaction? transaction = null, int commandTimeout = 30) =>
           store.Fetch<T>(transaction,commandTimeout).ToArray();
        /// <summary>
        /// Fetching Data From Database With Asynchronous Pattern
        /// </summary>
        /// <typeparam name="T">Entity To Fetch</typeparam>
        /// <param name="store">Query Store</param>
        /// <param name="transaction">Db Transaction</param>
        /// <param name="commandTimeout">Connection Maximum Time (Second)</param>
        
        /// <returns>Array Of Entity</returns>
        public static async Task<T[]> ToArrayAsync<T>(this QueryStore store, IDbTransaction? transaction = null, int commandTimeout = 30) =>
          (await store.FetchAsync<T>(transaction,commandTimeout)).ToArray();
        /// <summary>
        /// Fetch Item From Database (1 Row Only)
        /// </summary>
        /// <typeparam name="T">Entity To Fetch</typeparam>
        /// <param name="store">Query Store</param>
        /// <param name="transaction">Db Transaction</param>
        /// <param name="commandTimeout">Connection Maximum Time (Second)</param>
        
        /// <returns>Entity</returns>
        public static Option<T> FetchOne<T>(this QueryStore store, IDbTransaction? transaction = null, int commandTimeout = 30, bool debugMode = false)
        {
            var compiledResult = store.Compiled();
            if (store.DebugMode)
                Console.WriteLine($"Executing : {compiledResult.Sql}");
            return store.connection.QueryFirst<T>(compiledResult.Sql, compiledResult.Parameters, transaction, commandTimeout: commandTimeout);
        }
        /// <summary>
        /// Fetch Item From Database (1 Row Only) With Asynchronous Pattern
        /// </summary>
        /// <typeparam name="T">Entity To Fetch</typeparam>
        /// <param name="store">Query Store</param>
        /// <param name="transaction">Db Transaction</param>
        /// <param name="commandTimeout">Connection Maximum Time (Second)</param>
        
        /// <returns>Entity</returns>
        public static async Task<Option<T>> FetchOneAsync<T>(this QueryStore store, IDbTransaction? transaction = null, int commandTimeout = 30)
        {
            var compiledResult = await store.CompiledAsync();
            if (store.DebugMode)
                Console.WriteLine($"Executing : {compiledResult.Sql}");
            return await store.connection.QueryFirstAsync<T>(compiledResult.Sql, compiledResult.Parameters, transaction, commandTimeout: commandTimeout);
        }
        /// <summary>
        /// Insert Row Into Database 
        /// </summary>
        /// <typeparam name="T">Entity To Insert</typeparam>
        /// <param name="store">Query Store</param>
        /// <param name="insertItem">Data To Insert</param>
        /// <param name="transaction">Db Transaction</param>
        /// <param name="commandTimeout">Connection Maximum Time (Second)</param>
        
        /// <returns>Insert Complete Status</returns>
        public static bool Insert<T>(this QueryStore store,
            T insertItem,
            IDbTransaction? transaction = null,
            int commandTimeout = 100, 
            bool debugMode = false)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var columns = properties.Select(s => s.Name);
            string sql = $"insert into \"{store.TableName}\" ({string.Join(",", columns.Select(s => $"\"{s}\""))}) values ({string.Join(",", columns.Select(s => $"@{s}"))})";
            if (store.DebugMode)
                Console.WriteLine($"Executing : {sql}");
            return store.connection.Execute(sql, insertItem, transaction, commandTimeout).Ge(0);
        }
        /// <summary>
        /// Insert Row Into Database With Asynchronous Pattern
        /// </summary>
        /// <typeparam name="T">Entity To Insert</typeparam>
        /// <param name="store">Query Store</param>
        /// <param name="insertItem">Data To Insert</param>
        /// <param name="transaction">Db Transaction</param>
        /// <param name="commandTimeout">Connection Maximum Time (Second)</param>
        
        /// <returns>Insert Complete Status</returns>
        public static async Task<bool> InsertAsync<T>(this QueryStore store,
            T insertItem,
            IDbTransaction? transaction = null,
            int commandTimeout = 100, bool debugMode = false)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var columns = properties.Select(s => s.Name);
            string sql = $"insert into \"{store.TableName}\" ({string.Join(",", columns.Select(s => $"\"{s}\""))}) values ({string.Join(",", columns.Select(s => $"@{s}"))})";
            if (store.DebugMode)
                Console.WriteLine($"Executing : {sql}");
            return (await store.connection.ExecuteAsync(sql, insertItem, transaction, commandTimeout)).Ge(0);
        }
        /// <summary>
        /// Insert Mulitple Item
        /// </summary>
        /// <typeparam name="T">Entity To Insert</typeparam>
        /// <param name="store">Query Store</param>
        /// <param name="insertItem">Data To Insert</param>
        /// <param name="transaction">Db Transaction</param>
        /// <param name="commandTimeout">Connection Maximum Time (Second)</param>
        
        /// <returns>Inserted Rows</returns>
        public static int BulkInsert<T>(this QueryStore store,
            IEnumerable<T> insertItem,
            IDbTransaction? transaction = null,
            int commandTimeout = 100, bool debugMode = false)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var columns = properties.Select(s => s.Name);
            string sql = $"insert into \"{store.TableName}\" ({string.Join(",", columns.Select(s => $"\"{s}\""))}) values ({string.Join(",", columns.Select(s => $"@{s}"))})";
            if (store.DebugMode)
                Console.WriteLine($"Executing : {sql}");
            return store.connection.Execute(sql, insertItem, transaction, commandTimeout);
        }
        /// <summary>
        /// Insert Mulitple Item With Asynchronous Pattern
        /// </summary>
        /// <typeparam name="T">Entity To Insert</typeparam>
        /// <param name="store">Query Store</param>
        /// <param name="insertItem">Data To Insert</param>
        /// <param name="transaction">Db Transaction</param>
        /// <param name="commandTimeout">Connection Maximum Time (Second)</param>
        
        /// <returns>Inserted Rows</returns>
        public static async Task<int> BulkInsertAsync<T>(this QueryStore store,
            IEnumerable<T> insertItem,
            IDbTransaction? transaction = null,
            int commandTimeout = 100, bool debugMode = false)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var columns = properties.Select(s => s.Name);
            string sql = $"insert into \"{store.TableName}\" ({string.Join(",", columns.Select(s => $"\"{s}\""))}) values ({string.Join(",", columns.Select(s => $"@{s}"))})";
            if (store.DebugMode)
                Console.WriteLine($"Executing : {sql}");
            return await store.connection.ExecuteAsync(sql, insertItem, transaction, commandTimeout);
        }
        /// <summary>
        /// Update Item on Database 
        /// </summary>
        /// <param name="store">Query Store</param>
        /// <param name="data">New Data To Update</param>
        /// <param name="transaction">Db Transaction</param>
        /// <param name="commandTimeout">Connection Maximum Time (Second)</param>
        
        /// <returns>Rows Affected</returns>
        public static int Update(this QueryStore store,
            object data,
            IDbTransaction? transaction = null,
            int commandTimeout = 100, 
            bool debugMode = false)
        {
            var sqlRes = store.Compiled(StatementType.Update,data);
            if (store.DebugMode)
                Console.WriteLine($"Executing : {sqlRes.Sql}");
            return store.connection.Execute(sqlRes.Sql, sqlRes.Parameters, transaction, commandTimeout);
        }
        /// <summary>
        /// Update Item on Database With Asynchronous Pattern
        /// </summary>
        /// <param name="store">Query Store</param>
        /// <param name="data">New Data To Update</param>
        /// <param name="transaction">Db Transaction</param>
        /// <param name="commandTimeout">Connection Maximum Time (Second)</param>
        
        /// <returns>Rows Affected</returns>
        public static async Task<int> UpdateAsync(this QueryStore store,
            object data,
            IDbTransaction? transaction = null,
            int commandTimeout = 100,
            bool debugMode = false)
        {
            var sqlRes = store.Compiled(StatementType.Update,data);
            if (store.DebugMode)
                Console.WriteLine($"Executing : {sqlRes.Sql}");
            return await store.connection.ExecuteAsync(sqlRes.Sql, sqlRes.Parameters, transaction, commandTimeout);
        }
        /// <summary>
        /// Delete Item on Database
        /// </summary>
        /// <param name="store">Query Store</param>
        /// <param name="transaction">Db Transaction</param>
        /// <param name="commandTimeout">Connection Maximum Time (Second)</param>
        
        /// <returns>Rows Affected</returns>

        public static int Delete(this QueryStore store,
            IDbTransaction? transaction = null,
            int commandTimeout = 100,
            bool debugMode = false)
        {
            var sqlRes = store.Compiled(StatementType.Delete);
            if (store.DebugMode)
                Console.WriteLine($"Executing : {sqlRes.Sql}");
            return store.connection.Execute(sqlRes.Sql,sqlRes.Parameters, transaction: transaction, commandTimeout: commandTimeout);
        }
        /// <summary>
        /// Delete Item on Database With Asynchronous Pattern
        /// </summary>
        /// <param name="store">Query Store</param>
        /// <param name="transaction">Db Transaction</param>
        /// <param name="commandTimeout">Connection Maximum Time (Second)</param>
        
        /// <returns>Rows Affected</returns>
        public static async Task<int> DeleteAsync(this QueryStore store,
            IDbTransaction? transaction = null,
            int commandTimeout = 100, 
            bool debugMode = false)
        {
            var sqlRes = store.Compiled(StatementType.Delete);
            if (store.DebugMode)
                Console.WriteLine($"Executing : {sqlRes.Sql}");
            return await store.connection.ExecuteAsync(sqlRes.Sql,sqlRes.Parameters, transaction: transaction, commandTimeout: commandTimeout);
        }




        internal static string GetOperator(this SqlOperator @operator)
        {
            switch (@operator)
            {
                case SqlOperator.Equal: return "=";
                case SqlOperator.NotEqual: return "<>";
                case SqlOperator.GreaterThan: return ">";
                case SqlOperator.GreaterThanOrEqual: return ">=";
                case SqlOperator.LessThan: return "<";
                case SqlOperator.LessThanOrEqual: return "<=";
                case SqlOperator.In: return "in";
                case SqlOperator.NotIn: return "not in";
                case SqlOperator.Between: return "between";
                case SqlOperator.NotBetween: return "not between";
                case SqlOperator.Null: return "is null";
                case SqlOperator.NotNull: return "is not null";
            }
            return "=";
        }
    }
}
