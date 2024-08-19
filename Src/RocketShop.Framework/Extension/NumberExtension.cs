using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Extension
{
    public static class NumberExtension
    {
        public static int ToInt(this string? value, int defaultValue = -1) 
            => int.TryParse(value?.Replace(",", string.Empty), out var result) ? result : defaultValue;

        public static long ToLong(this string? value, long defaultValue = -1) 
            => long.TryParse(value?.Replace(",", string.Empty), out var result) ? result : defaultValue;

        public static double ToDouble(this string? value, double defaultValue = -1) 
            => double.TryParse(value?.Replace(",",string.Empty),out var result) ? result : defaultValue;

        public static decimal ToDecimal(this string? value, decimal defaultValue = -1) 
            => decimal.TryParse(value?.Replace(",", string.Empty), out var result) ? result : defaultValue;


        public static int? ToNullableInt(this string? value) 
            => int.TryParse(value?.Replace(",", string.Empty), out var result) ? result : null;

        public static long? ToNullableLong(this string? value) 
            => long.TryParse(value?.Replace(",", string.Empty), out var result) ? result : null;

        public static double? ToNullableDouble(this string? value) 
            => double.TryParse(value?.Replace(",", string.Empty), out var result) ? result : null;

        public static decimal? ToNullableDecimal(this string? value) 
            => decimal.TryParse(value?.Replace(",", string.Empty), out var result) ? result : null;
    }
}
