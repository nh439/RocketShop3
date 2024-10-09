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
        internal string TableName { get; set; }
        internal List<QueryCondition>? conditions { get; set; } = new List<QueryCondition>();
        internal List<OrderBy> orderBies { get; set; } = new List<OrderBy>();
        internal QueryPaging? QueryPaging { get; set; }
        internal List<string> SelectedColumns { get; set; } = new List<string>();
        internal readonly IDbConnection connection;
        internal bool DebugMode;
        public QueryStore(IDbConnection connection,
            string tableName,
            bool debugMode = false)
        {
            this.connection = connection;
            this.TableName = tableName;
            this.DebugMode = debugMode;
        }
        /// <summary>
        /// For Open Sql Log To Console(Not Recommend For Production)
        /// </summary>
        public void EnabledDebug() => DebugMode = true;
        /// <summary>
        /// For Don't Sql Log To Console(Recommend For Production)
        /// </summary>
        public void DisableDebug() => DebugMode = false;

        
       internal void AddNormalCond(string column,
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
        public IEnumerable<object>? ValueIn { get; set; }
        public object? ValueMin { get; set; }
        public object? ValueMax { get; set; }
        internal QueryStore queryStore { get; set; }
        public QueryCondition() { }

        public QueryCondition(string column,SqlOperator @operator,object value,bool or = false,QueryStore? store = null)
        {
            ColumnName = column;
            Value = value;
            Operator = @operator;
            RelatedOrCondition= or;
        }
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
        NotBetween,
        Null,
        NotNull,
        WhereSub
    }
    public class OrderBy
    {
        public string Column { get; set; }
        public bool Desc { get; set; }
        public string GeneratePartial() => $" \"{Column}\" {(Desc ? "desc" : "asc")}";
    }
    public class QueryPaging(int page,int per= 10)
    {
        public int Page { get; set; } = page;
        public int Per { get; set; } = per;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
