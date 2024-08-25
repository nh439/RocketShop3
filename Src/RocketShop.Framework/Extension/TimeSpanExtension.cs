using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Extension
{
    public static class TimeSpanExtension
    {
        public static string ToRemainingFormat(this TimeSpan? value)
        {
            if (value.IsNull()) return "0 sec";
            var val = value!.Value;
            if (val.Days > 0)
                return $"{Math.Floor(val.TotalDays)} days {val.Hours} hrs {val.Minutes} mins {val.Seconds} secs";
            if (val.TotalHours > 0)
                return $"{val.Hours} hrs {val.Minutes} mins {val.Seconds} secs";
            if (val.TotalMinutes > 0)
                return $"{val.Minutes} mins {val.Seconds} secs";
            return $"{val.TotalSeconds} secs";

        }
    }
}
