using Dapper;
using LanguageExt;
using LanguageExt.ClassInstances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using RocketShop.Database.Domain;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RocketShop.Database.Extension
{
    public static class DBSetExtension
    {
        public static IQueryable<T> UsePaging<T>(this IQueryable<T> query, int? page, int per = 10) =>
            page.HasValue ?
            query.Skip((page.Value - 1) * per)
            .Take(per) : query;

        public static async Task<int> GetLastpageAsync<T>(this IQueryable<T> queryable, int per = 10)
        {
            var count = await queryable.CountAsync();
            return (int)Math.Ceiling((decimal)count / (decimal)per);
        }

        public static async Task<int> GetLastpageAsync<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate, int per = 10)
        {
            var count = await queryable.CountAsync(predicate);
            return (int)Math.Ceiling((decimal)count / (decimal)per);
        }


        public static async Task<ScrollingPageResult<T>> FetchScrollingPageAsync<T>(this IDbConnection connection,
            string tableName,
            IEnumerable<string>? selectedColumn = null,
            string? rawCondition = null,
            object? parameter = null,
            string? orderByCol = null,
            bool asc = true,
            int Limit = 10,
            IDbTransaction? transaction = null,
            int timeOut = 100
            )
        {
            var selectStatement = selectedColumn.HasDataAndTranformData(
                x => string.Join(",", $"\"{x}\""),
                () => "*");
            var whereStatement = rawCondition.If(x => x.HasMessage(), x => $"where {x}", x => string.Empty);
            var orderByStatement = orderByCol.If(x => x.HasMessage(), x => $"order by \"{x}\" {(asc ? "asc" : "desc")} ", s => string.Empty);
            var sql = @$"select {selectStatement ?? "*"} from ""{tableName}"" {whereStatement} {orderByStatement} limit {(Limit + 1)}";
            var result = await connection.QueryAsync<T>(sql, parameter, transaction, timeOut);
            var count = result.Count();
            Option<NextPageToken> next = count > Limit ? new NextPageToken(Limit,
                Limit,
                tableName,
                selectedColumn,
                rawCondition,
                orderByCol,
                parameter,
                asc) : null;
            return new ScrollingPageResult<T>(result.Take(Limit),
                Limit,
                result.IsNotNull(),
                next.IsSome,
                next.IsSome ? next.Extract().ToBase64() : null);
        }

        public static async Task<ScrollingPageResult<T>> FromNextTokenAsync<T>(this IDbConnection connection,
            string nextToken,
            IDbTransaction? transaction = null,
            int timeOut = 100)
        {
            var token = nextToken.FromBase64<NextPageToken>();
            if (token.IsNone)
                throw new InvalidCastException("Token Invalid");
            var extracted = token.Extract();
            var selectStatement = extracted!.selectedCol.HasDataAndTranformData(
                x => string.Join(",", $"\"{x}\""),
                () => "*");

            var whereStatement = extracted.rawCondition.If(x => x.HasMessage(), x => $"where {x}", x => string.Empty);
            var orderByStatement = extracted.OrderBy.If(x => x.HasMessage(), x => $"order by \"{x}\" {(extracted.asc ? "asc" : "desc")} ", s => string.Empty);
            var sql = @$"select {selectStatement ?? "*"} ""{extracted.TableName}"" {whereStatement} {orderByStatement} limit {(extracted.Limit + 1)} offset {extracted.Offset}";
            var result = await connection.QueryAsync<T>(sql, extracted.Parameter, transaction, timeOut);
            var count = result.Count();
            var offset = extracted.Limit + extracted.Offset;
            Option<NextPageToken> next = new NextPageToken(offset,
                extracted.Limit,
                extracted.TableName,
                extracted.selectedCol,
                extracted.rawCondition,
                extracted.OrderBy,
                extracted.Parameter,
                extracted.asc);
            return new ScrollingPageResult<T>(result,
                extracted.Limit,
                result.IsNotNull(),
                next.IsSome,
                next.IsSome ? next.Extract().ToBase64() : null);
        }
    }
}
