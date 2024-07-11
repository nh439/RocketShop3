using LanguageExt;
using LanguageExt.Pretty;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RocketShop.Database.NonEntityFramework.QueryGenerator
{
    public static class QueryConditionExtension
    {
        public static string CompiledCondition(this QueryCondition queryCondition)
        {
            var inCond = new[] { SqlOperator.In, SqlOperator.NotIn }.Contains(queryCondition.Operator);
            var nullCond = new[] { SqlOperator.Null, SqlOperator.NotNull }.Contains(queryCondition.Operator);
            var betweenCond = new[] { SqlOperator.Between, SqlOperator.NotBetween }.Contains(queryCondition.Operator);
            if (inCond)
                return $"\"{queryCondition.ColumnName}\" {queryCondition.Operator.GetOperator()} ({string.Join(",",queryCondition.ValueIn!.Select(s=> CompiledValue(s)))})";
            if (nullCond)
                return $"\"{queryCondition.ColumnName}\" {queryCondition.Operator.GetOperator()}";
            if(betweenCond) 
                return $"\"{queryCondition.ColumnName}\" {queryCondition.Operator.GetOperator()} {CompiledValue(queryCondition.ValueMin!)} and {CompiledValue(queryCondition.ValueMin!)}";
            return $"\"{queryCondition.ColumnName}\" {queryCondition.Operator.GetOperator()} {CompiledValue(queryCondition.Value!)}";

        }
        public static string CompiledCondition(this IEnumerable<QueryCondition> queryCondition)
        {
            List<string> conditions = new List<string>();
            bool firstCond = true;
            queryCondition.HasDataAndForEach(f => {
                string c = string.Empty;
                if (!firstCond)
                    c += $" {(f.RelatedOrCondition ? "or" : "and")} ";
                firstCond = false;
                c += CompiledCondition(f);
                conditions.Add(c);
            });
            return string.Join(" ", conditions);
        }
        static string CompiledValue(object value)
        {
            if (value.GetType() == typeof(string) || value.GetType() == typeof(DateTime))
                return $"'{value}'";
            return value.ToString()!;
        }
    }
}

