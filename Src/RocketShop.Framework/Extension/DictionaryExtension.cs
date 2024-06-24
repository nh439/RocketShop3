using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RocketShop.Framework.Extension
{
    public static class DictionaryExtension
    {
        public static IDictionary<string, object>? ToDictionaryStringObject<T>(this T item) =>
          JsonSerializer.Deserialize<Dictionary<string,object>>((JsonSerializer.Serialize(item)));
    }
}
