using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Extension
{
    public static class DateTimeExtensions
    {
        // Convert datetime to UNIX time
        public static long ToUnixTime(this DateTime dateTime)
        {
            DateTimeOffset dto = new DateTimeOffset(dateTime.ToUniversalTime());
            return dto.ToUnixTimeSeconds();
        }

        // Convert datetime to UNIX time including miliseconds
        public static long ToUnixTimeMilliSeconds(this DateTime dateTime)
        {
            DateTimeOffset dto = new DateTimeOffset(dateTime.ToUniversalTime());
            return dto.ToUnixTimeMilliseconds();
        }

        public static string ToDateAndTimeFormat(this DateTime dateTime) =>
            dateTime.ToString("yyyy-MM-dd HH:mm:ss");

        public static string ToLocalDateAndTimeFormat(this DateTime dateTime) =>
          dateTime.Kind != DateTimeKind.Local ? 
            dateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") :
            dateTime.ToString("yyyy-MM-dd HH:mm:ss");

        public static string ToLocalDateAndTimeLongFormat(this DateTime dateTime) =>
          dateTime.Kind != DateTimeKind.Local ? 
            dateTime.ToLocalTime().ToString("dd MMMM yyyy HH:mm:ss") :
            dateTime.ToString("dd MMMM yyyy HH:mm:ss");

    }
}
