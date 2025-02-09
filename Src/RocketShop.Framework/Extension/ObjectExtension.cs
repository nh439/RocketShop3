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

        /// <summary>
        /// Compares two objects at a binary level to determine if they are equal.
        /// </summary>
        /// <param name="obj">The first object to compare.</param>
        /// <param name="comparer">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are binary equivalent; otherwise, <c>false</c>.</returns>
        public static bool BinaryEquals(this object? obj, object? comparer)
        {
            if (obj.IsNull() && comparer.IsNull())
                return true;
            if (obj.IsNull() || comparer.IsNull())
                return false;
            return Convert.ToBase64String(Encoding.UTF32.GetBytes(JsonSerializer.Serialize(obj)))
                == Convert.ToBase64String(Encoding.UTF32.GetBytes(JsonSerializer.Serialize(comparer)));
        }

        /// <summary>
        /// Determines whether the specified object contains a property with the given name.
        /// </summary>
        /// <param name="obj">The object to check for the property.</param>
        /// <param name="propertyName">The name of the property to look for.</param>
        /// <returns><c>true</c> if the property exists; otherwise, <c>false</c>.</returns>
        public static bool HasProperty(this object? obj, string propertyName)
        {
            if (obj.IsNull())
                return false;
            return obj!.GetType().GetProperty(propertyName) != null;
        }

        /// <summary>
        /// Retrieves the property names of the given object.
        /// </summary>
        /// <param name="obj">The object whose property names are to be retrieved.</param>
        /// <param name="includedDoubleqout">Determines whether property names should be enclosed in double quotes.</param>
        /// <param name="skipColumns">An optional collection of property names to exclude from the result.</param>
        /// <returns>An IEnumerable of property names as strings.</returns>
        public static IEnumerable<string> GetPropertiesName(this object? obj,bool includedDoubleqout = false,IEnumerable<string>? skipColumns =null)
        {
            if (obj.IsNull())
                return new List<string>();
            var objType = obj!.GetType().GetProperties();
            if(skipColumns.HasData())
                objType = objType.Where(s =>!skipColumns!.Contains(s.Name)).ToArray();
            if (includedDoubleqout)
                return objType.Select(s => $"\"{s.Name}\"");
            return objType.Select(s => s.Name);
        }

        /// <summary>
        /// Merges a collection of strings into a single string, separated by the specified separator.
        /// </summary>
        /// <param name="strings">The collection of strings to merge.</param>
        /// <param name="separator">The separator to use between strings. Defaults to ";".</param>
        /// <returns>A single concatenated string with elements separated by the specified separator.</returns>
        public static string Merged(this IEnumerable<string> strings, string separator =";") => string.Join(separator, strings);

        /// <summary>
        /// Converts an object's properties into an array of values.
        /// </summary>
        /// <param name="obj">The object to extract property values from.</param>
        /// <param name="skipProperties">
        /// A comma-separated string of property names to exclude from the result. Defaults to null.
        /// </param>
        /// <returns>An array of property values from the object.</returns>
        public static object?[] ToObjectRows(this object? obj,string? skipProperties = null)
        {
            List<string> skipProp = new();
            if (skipProperties.IsNotNull())
                skipProp = skipProperties!.Split(',').ToList();
            if (obj.IsNull())
                return Array.Empty<object>();
            var properties = obj!.GetType().GetProperties();
            return skipProp.HasData() ? properties.Where(x=>!skipProp.Contains(x.Name)).Select(s => s.GetValue(obj)).ToArray() : properties.Select(s => s.GetValue(obj)).ToArray();
        }
    }
}
