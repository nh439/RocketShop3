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

        public static SqlResult Compiled(this QueryStore store)
        {
            string sql = string.Empty;
            var columns = store.SelectedColumns.HasDataAndTranformData(
                s => string.Join(",", $"\"{s}\""),
                () => "*");
            sql = $"select {columns} from \"{store.TableName}\" ";
            Dictionary<string, object> data = new Dictionary<string, object>();
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

        public static IEnumerable<T> Fetch<T>(this QueryStore store, IDbTransaction? transaction = null, int commandTimeout = 30)
        {
            var compiledResult = store.Compiled();
            return store.connection.Query<T>(compiledResult.Sql, compiledResult.Parameters, transaction, commandTimeout: commandTimeout);
        }

        public static async Task<IEnumerable<T>> FetchAsync<T>(this QueryStore store, IDbTransaction? transaction = null, int commandTimeout = 30)
        {
            var compiledResult = await store.CompiledAsync();
            return await store.connection.QueryAsync<T>(compiledResult.Sql, compiledResult.Parameters, transaction, commandTimeout: commandTimeout);
        }

        public static List<T> ToList<T>(this QueryStore store, IDbTransaction? transaction = null, int commandTimeout = 30) =>
            store.Fetch<T>().ToList();

        public static async Task<List<T>> ToListAsync<T>(this QueryStore store, IDbTransaction? transaction = null, int commandTimeout = 30) =>
          (await store.FetchAsync<T>()).ToList();

         public static T[] ToArray<T>(this QueryStore store, IDbTransaction? transaction = null, int commandTimeout = 30) =>
            store.Fetch<T>().ToArray();

        public static async Task<T[]> ToArrayAsync<T>(this QueryStore store, IDbTransaction? transaction = null, int commandTimeout = 30) =>
          (await store.FetchAsync<T>()).ToArray();

        public static Option<T> FetchOne<T>(this QueryStore store, IDbTransaction? transaction = null, int commandTimeout = 30)
        {
            var compiledResult = store.Compiled();
            return store.connection.QueryFirst<T>(compiledResult.Sql, compiledResult.Parameters, transaction, commandTimeout: commandTimeout);
        }

        public static async Task< Option<T>> FetchOneAsync<T>(this QueryStore store, IDbTransaction? transaction = null, int commandTimeout = 30)
        {
            var compiledResult = await store.CompiledAsync();
            return await store.connection.QueryFirstAsync<T>(compiledResult.Sql, compiledResult.Parameters, transaction, commandTimeout: commandTimeout);
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
