using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Model.WHRequestModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="schemaName">Graph QL Schema</param>
    /// <param name="properties">Graph QL Response Property</param>
    /// <param name="parameters">Graph QL Schema Arg</param>
    public class WarehouseQuery(string schemaName,
        string[] properties,
        List<WarehouseQueryParameter>? parameters = null)
    {
       protected void AddParameter(string name, object value)
        {
            if (parameters is null)
                parameters = new List<WarehouseQueryParameter>();
            parameters.Add(name, value);
        }
            
        string ParseParameter()
        {
            if (!parameters.HasData()) return string.Empty;
            List<string> rawparameters = new List<string>();
            parameters.HasDataAndForEach(p =>
            {
                if (p.GetType() == typeof(string))
                    rawparameters.Add($"{p.key}:\"{p.value}\"");
                else
                    rawparameters.Add($"{p.key}:{p.value}");
            });
            return $"({string.Join(",", rawparameters)})";
        }
        public string ToQueryString() => $@"""
        {schemaName}{ParseParameter()}
{{
{string.Join(Environment.NewLine, properties)}
}}
""";
    }



    public sealed record WarehouseQueryParameter(string key, object value);

    public static class WarehouseQueryParameterExtension
    {
        public static void Add(this List<WarehouseQueryParameter> parameters, string key, object value)
        {
            if (parameters is null)
                parameters = new List<WarehouseQueryParameter>();
            parameters.Add(new WarehouseQueryParameter(key, value));
        }
    }
}
