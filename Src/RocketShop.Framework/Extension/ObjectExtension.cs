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
    }
}
