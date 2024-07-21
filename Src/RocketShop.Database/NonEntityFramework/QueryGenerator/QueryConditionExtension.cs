using LanguageExt;
using LanguageExt.Pretty;
using RocketShop.Framework.Extension;
using System;
using System.Collections;
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
        public static (string,Dictionary<string,object>) CompiledConditionWithParameterizedMode(this QueryCondition queryCondition)
        {
            var inCond = new[] { SqlOperator.In, SqlOperator.NotIn }.Contains(queryCondition.Operator);
            var nullCond = new[] { SqlOperator.Null, SqlOperator.NotNull }.Contains(queryCondition.Operator);
            var betweenCond = new[] { SqlOperator.Between, SqlOperator.NotBetween }.Contains(queryCondition.Operator);
            string compileId =Guid.NewGuid().ToString().Replace("-",string.Empty);
            Dictionary<string, object> data = new Dictionary<string, object>();
            if (inCond)
            {
                int itemId = 0;               
                queryCondition.ValueIn.HasDataAndForEach(it =>
                {
                    data.Add($"{compileId}_{itemId}", it);
                    itemId++;
                });
                var sql = $"\"{queryCondition.ColumnName}\" {queryCondition.Operator.GetOperator()} ({string.Join(",",data.Select(s=>$"@{s.Key}"))})";
                return (sql, data);
            }
               
            if (nullCond)
                return ($"\"{queryCondition.ColumnName}\" {queryCondition.Operator.GetOperator()}", new Dictionary<string, object>());
            if (betweenCond)
            {
                var sql = $"\"{queryCondition.ColumnName}\" {queryCondition.Operator.GetOperator()} @{compileId}__min and @{compileId}__max ";
                data.Add($"{compileId}__min", queryCondition.ValueMin);
                data.Add($"{compileId}__max", queryCondition.ValueMax);
                return (sql, data);
            }
            if(queryCondition.Operator == SqlOperator.WhereSub)
            {
                var queryresult = CompiledConditionWithParameterizedMode(queryCondition.queryStore.conditions!);
                return ( $"({queryresult.Item1})",queryresult.Item2);
            }
            data.Add(compileId, queryCondition.Value);
            return ($"\"{queryCondition.ColumnName}\" {queryCondition.Operator.GetOperator()} @{compileId}",data);

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

         public static (string,Dictionary<string,object>) CompiledConditionWithParameterizedMode(this IEnumerable<QueryCondition> queryCondition)
        {
            List<string> conditions = new List<string>();
            bool firstCond = true;
            Dictionary<string, object> data = new Dictionary<string, object>(); 
            queryCondition.HasDataAndForEach(f => {
                string c = string.Empty;
                if (!firstCond)
                    c += $" {(f.RelatedOrCondition ? "or" : "and")} ";
                firstCond = false;
                var cmp = CompiledConditionWithParameterizedMode(f);
                c += cmp.Item1;
                conditions.Add(c);
                cmp.Item2.HasDataAndForEach(p => data.Add(p.Key, p.Value));
            });
            return (string.Join(" ", conditions),data);
        }


       

        static string CompiledValue(object value)
        {
            if (value.GetType() == typeof(string) || value.GetType() == typeof(DateTime))
                return $"'{value}'";
            return value.ToString()!;
        }
    }
}

