using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.NonEntityFramework.QueryGenerator
{
    public sealed record SqlResult(string Sql, Dictionary<string, object>? Parameters)
    {
        public override string ToString()
        {
            string returnValues = Sql;
            Parameters.HasDataAndForEach(f =>
            {
                if (f.Value.GetType() == typeof(string))
                    returnValues = returnValues.Replace($"@{f.Key}", $"'{f.Value}'");
                else
                    returnValues = returnValues.Replace($"@{f.Key}", $"{f.Value}");
            });
            return returnValues;
        }
    }
}
