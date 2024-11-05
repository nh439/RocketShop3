using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RocketShop.Framework.Extension
{
    public static class ObjectExtension
    {
        public static string? FromObjectToBase64(this object? obj)
        {
            if(obj.IsNull()) return null;
            return Convert.ToBase64String(
                Encoding.UTF32.GetBytes(JsonSerializer.Serialize(obj))
                );
        }
    }
}
