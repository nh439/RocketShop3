using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RocketShop.Framework.Extension;

namespace RocketShop.Database.NonEntityFramework.QueryGenerator
{
    public static class QueryStoreExtension
    {
        public static QueryStore Where(this QueryStore query, string column, object value)
        {
            query.AddNormalCond(column, SqlOperator.Equal, value);
            return query;
        }
        public static QueryStore OrWhere(this QueryStore query, string column, object value)
        {
            query.AddNormalCond(column, SqlOperator.Equal, value, true);
            return query;
        }
        public static QueryStore Where(this QueryStore query, string column, SqlOperator @operator, object value)
        {
            query.AddNormalCond(column, @operator, value);
            return query;
        }
        public static QueryStore OrWhere(this QueryStore query, string column, SqlOperator @operator, object value)
        {
            query.AddNormalCond(column, @operator, value, true);
            return query;
        }
        public static QueryStore Where(this QueryStore query,Func<QueryStore,QueryStore> callback)
        {
            var st = new QueryStore(query.connection,query.TableName);
            st = callback(st);
            query.conditions!.Add(new QueryCondition
            {
                queryStore = st,
                RelatedOrCondition = false,
                Operator = SqlOperator.WhereSub
            });
            return query;
        }
        public static QueryStore OrWhere(this QueryStore query,Func<QueryStore,QueryStore> callback)
        {
            var st = new QueryStore(query.connection,query.TableName);
            st = callback(st);
            query.conditions!.Add(new QueryCondition
            {
                queryStore = st,
                RelatedOrCondition = true,
                Operator = SqlOperator.WhereSub
            });
            return query;
        }

        public static QueryStore WhereIn<T>(this QueryStore query, string column,IEnumerable<T> values)
        {
            query.conditions.SafeAdd(new QueryCondition
            {
                ColumnName = column,
                Operator = SqlOperator.In,
                RelatedOrCondition = false,
                ValueIn = values.Select(s=> s as object)!
            });
            return query;
        }
        public static QueryStore WhereNotIn(this QueryStore query, string column, params object[] values)
        {
            query.conditions.SafeAdd(new QueryCondition
            {
                ColumnName = column,
                Operator = SqlOperator.NotIn,
                RelatedOrCondition = false,
                ValueIn = values
            });
            return query;
        }
        public static QueryStore OrWhereIn(this QueryStore query, string column, params object[] values)
        {
            query.conditions.SafeAdd(new QueryCondition
            {
                ColumnName = column,
                Operator = SqlOperator.In,
                RelatedOrCondition = true,
                ValueIn = values
            });
            return query;
        }
        public static QueryStore OrWhereNotIn(this QueryStore query, string column, params object[] values)
        {
            query.conditions.SafeAdd(new QueryCondition
            {
                ColumnName = column,
                Operator = SqlOperator.NotIn,
                RelatedOrCondition = true,
                ValueIn = values
            });
            return query;
        }

        public static QueryStore WhereBetween(this QueryStore query, string column, object minValue, object maxValue)
        {
            query.conditions.SafeAdd(new QueryCondition
            {
                ColumnName = column,
                Operator = SqlOperator.Between,
                RelatedOrCondition = false,
                ValueMin = minValue,
                ValueMax = maxValue
            });
            return query;
        }
        public static QueryStore WhereNotBetween(this QueryStore query, string column, object minValue, object maxValue)
        {
            query.conditions.SafeAdd(new QueryCondition
            {
                ColumnName = column,
                Operator = SqlOperator.NotBetween,
                RelatedOrCondition = false,
                ValueMin = minValue,
                ValueMax = maxValue
            });
            return query;
        }
        public static QueryStore OrWhereBetween(this QueryStore query, string column, object minValue, object maxValue)
        {
            query.conditions.SafeAdd(new QueryCondition
            {
                ColumnName = column,
                Operator = SqlOperator.Between,
                RelatedOrCondition = true,
                ValueMin = minValue,
                ValueMax = maxValue
            });
            return query;
        }
        public static QueryStore OrWhereNotBetween(this QueryStore query, string column, object minValue, object maxValue)
        {
            query.conditions.SafeAdd(new QueryCondition
            {
                ColumnName = column,
                Operator = SqlOperator.NotBetween,
                RelatedOrCondition = true,
                ValueMin = minValue,
                ValueMax = maxValue
            });
            return query;
        }

        public static QueryStore OrderBy(this QueryStore query,string column, bool desc = false)
        {
            query.orderBies.SafeAdd(new NonEntityFramework.QueryGenerator.OrderBy
            {
                Column = column,
                Desc = desc
            });
            return query;
        }
        public static QueryStore UsePaging(this QueryStore query, int page, int per = 10)
        {
            query.QueryPaging = new QueryPaging(page, per);
            return query;
        }

        public static QueryStore Select(this QueryStore query, params string[] columns)
        {
            query.SelectedColumns.AddRange(columns);
            return query;
        }
        public static QueryStore WhereNull(this QueryStore query, string column)
        {
            query.conditions!.Add(new QueryCondition
            {
                ColumnName = column,
                Operator = SqlOperator.Null
            });
            return query;
        }
        public static QueryStore OrWhereNull(this QueryStore query, string column)
        {
            query.conditions!.Add(new QueryCondition
            {
                ColumnName = column,
                Operator = SqlOperator.Null,
                RelatedOrCondition = true
            });
            return query;
        }
        public static QueryStore WhereNotNull(this QueryStore query, string column)
        {
           query.conditions!.Add(new QueryCondition
            {
                ColumnName = column,
                Operator = SqlOperator.NotNull
            });
            return query;
        }
        public static QueryStore OrWhereNotNull(this QueryStore query, string column)
        {
            query.conditions!.Add(new QueryCondition
            {
                ColumnName = column,
                Operator = SqlOperator.NotNull,
                RelatedOrCondition = true
            });
            return query;
        }
    }
}
