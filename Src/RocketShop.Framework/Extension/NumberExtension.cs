using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Extension
{
    /// <summary>
    /// Provides extension methods for converting strings to numeric types and checking numeric types.
    /// </summary>
    public static class NumberExtension
    {
        /// <summary>
        /// Converts the string representation of a number to an integer. 
        /// Returns a default value if the conversion fails.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="defaultValue">The default value to return if conversion fails.</param>
        /// <returns>The converted integer or the default value.</returns>
        public static int ToInt(this string? value, int defaultValue = -1)
            => int.TryParse(value?.Replace(",", string.Empty), out var result) ? result : defaultValue;

        /// <summary>
        /// Converts the string representation of a number to a long integer. 
        /// Returns a default value if the conversion fails.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="defaultValue">The default value to return if conversion fails.</param>
        /// <returns>The converted long integer or the default value.</returns>
        public static long ToLong(this string? value, long defaultValue = -1)
            => long.TryParse(value?.Replace(",", string.Empty), out var result) ? result : defaultValue;

        /// <summary>
        /// Converts the string representation of a number to a double. 
        /// Returns a default value if the conversion fails.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="defaultValue">The default value to return if conversion fails.</param>
        /// <returns>The converted double or the default value.</returns>
        public static double ToDouble(this string? value, double defaultValue = -1)
            => double.TryParse(value?.Replace(",", string.Empty), out var result) ? result : defaultValue;

        /// <summary>
        /// Converts the string representation of a number to a decimal. 
        /// Returns a default value if the conversion fails.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="defaultValue">The default value to return if conversion fails.</param>
        /// <returns>The converted decimal or the default value.</returns>
        public static decimal ToDecimal(this string? value, decimal defaultValue = -1)
            => decimal.TryParse(value?.Replace(",", string.Empty), out var result) ? result : defaultValue;

        /// <summary>
        /// Converts the string representation of a number to a nullable integer. 
        /// Returns null if the conversion fails.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <returns>The converted integer or null.</returns>
        public static int? ToNullableInt(this string? value)
            => int.TryParse(value?.Replace(",", string.Empty), out var result) ? result : null;

        /// <summary>
        /// Converts the string representation of a number to a nullable long integer. 
        /// Returns null if the conversion fails.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <returns>The converted long integer or null.</returns>
        public static long? ToNullableLong(this string? value)
            => long.TryParse(value?.Replace(",", string.Empty), out var result) ? result : null;

        /// <summary>
        /// Converts the string representation of a number to a nullable double. 
        /// Returns null if the conversion fails.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <returns>The converted double or null.</returns>
        public static double? ToNullableDouble(this string? value)
            => double.TryParse(value?.Replace(",", string.Empty), out var result) ? result : null;

        /// <summary>
        /// Converts the string representation of a number to a nullable decimal. 
        /// Returns null if the conversion fails.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <returns>The converted decimal or null.</returns>
        public static decimal? ToNullableDecimal(this string? value)
            => decimal.TryParse(value?.Replace(",", string.Empty), out var result) ? result : null;

        /// <summary>
        /// Determines whether the specified object is of a numeric type.
        /// </summary>
        /// <param name="o">The object to check.</param>
        /// <returns>True if the object is a numeric type; otherwise, false.</returns>
        public static bool IsNumericType(this object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }

}
