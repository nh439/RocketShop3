using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.NonEntityFramework.QueryGenerator
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class QueryStore
    {
        public string TableName { get; set; }
        public List<QueryCondition>? conditions { get; set; }
        readonly IDbConnection connection;
        public QueryStore(IDbConnection connection,
            string tableName)
        {
            this.connection = connection;
            this.TableName = tableName;
        }

        public QueryStore Where(string column, object value)
        {
            AddNormalCond(column, SqlOperator.Equal, value);
            return this;
        }
        public QueryStore OrWhere(string column, object value)
        {
            AddNormalCond(column, SqlOperator.Equal, value, true);
            return this;
        }
        public QueryStore Where(string column, SqlOperator @operator, object value)
        {
            AddNormalCond(column, @operator, value);
            return this;
        }
        public QueryStore OrWhere(string column, SqlOperator @operator, object value)
        {
            AddNormalCond(column, @operator, value, true);
            return this;
        }

        void AddNormalCond(string column,
            SqlOperator @operator,
            object value,
            bool Or = false) =>
             conditions.SafeAdd(new QueryCondition
             {
                 ColumnName = column,
                 Value = value,
                 Operator = @operator,
                 RelatedOrCondition = Or
             });
    }
    public class QueryCondition
    {
        public SqlOperator Operator { get; set; }
        public object? Value { get; set; }
        public string ColumnName { get; set; }
        public bool RelatedOrCondition { get; set; } = false;
        public object[]? ValueIn { get; set; }
        public object? ValueMin { get; set; }
        public object? ValueMax { get; set; }
    }
    public enum SqlOperator
    {
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        In,
        NotIn,
        Between,
        NotBetween
    }
    public class OrderBy
    {
        public string Column { get; set; }
        public bool Desc { get; set; }
        public string GeneratePartial() => $" \"{Column}\" {(Desc ? "desc" : "asc")}";
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
