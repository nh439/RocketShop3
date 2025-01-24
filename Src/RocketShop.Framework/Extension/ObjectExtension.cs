using LanguageExt;
using RocketShop.Framework.Model;
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
        public static T? FromBase64ToObject<T>(this string? base64)
        {
            if (base64.IsNullOrEmpty()) return default;
            var arr = Convert.FromBase64String(base64!);
            string result = Encoding.UTF32.GetString(arr, 0, arr.Length);
            return JsonSerializer.Deserialize<T>(result);
        }
        public static string[] GetTypeProperties(this object? obj)
        {
            if(obj.IsNull())
                return Array.Empty<string>();
            var properties = obj!.GetType().GetProperties();
            return properties.Select(x=> x.Name ).ToArray();
        }
        public static IEnumerable<ObjectDescription> GetObjectInfo(this object? obj) {
            if (obj.IsNull())
                return new List<ObjectDescription>();
            var properties = obj!.GetType().GetProperties();
            return properties.Select(s => new ObjectDescription
            {
                Name = s.Name,
                Attributes = s.Attributes,
                CanRead = s.CanRead,
                CanWrite = s.CanWrite,
                IsSpecialName = s.IsSpecialName,
                Type = s.PropertyType
            });
        }

        public static bool JsonEquals(this object? obj, object? comparer)
        {
            if (obj.IsNull() && comparer.IsNull())
                return true;
            if (obj.IsNull() || comparer.IsNull())
                return false;
            return JsonSerializer.Serialize(obj).ToLower() == JsonSerializer.Serialize(comparer).ToLower();
        }

        public static bool BinaryEquals(this object? obj, object? comparer)
        {
            if (obj.IsNull() && comparer.IsNull())
                return true;
            if (obj.IsNull() || comparer.IsNull())
                return false;
            return Convert.ToBase64String(Encoding.UTF32.GetBytes(JsonSerializer.Serialize(obj)))
                == Convert.ToBase64String(Encoding.UTF32.GetBytes(JsonSerializer.Serialize(comparer)));
        }

        public static bool HasProperty(this object? obj, string propertyName)
        {
            if (obj.IsNull())
                return false;
            return obj!.GetType().GetProperty(propertyName) != null;
        }
    }
}
