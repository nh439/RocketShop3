﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Extension
{
    public static class LongHelper
    {
        #region Not Nullable
        public static bool Eq(this long x, long y) =>
            x == y;

        public static bool NotEq(this long x, long y) =>
            !Eq(x, y);

        public static bool Ge(this long x, long y) =>
            x > y;

        public static bool GeOrEq(this long x, long y) =>
            x >= y;

        public static bool Le(this long x, long y) =>
            x < y;

        public static bool LeOrEq(this long x, long y) =>
            x <= y;

        public static long FindDistance(this long x, long y) =>
            (long)Math.Abs((x) - (y));
        #endregion
        #region Nullable
        public static bool Eq(this long? x, long? y)=>
            x==y;

        public static bool NotEq(this long? x, long? y)=>
            !Eq(x, y);

        public static bool Ge(this long? x, long? y) =>
            x > y;

        public static bool GeOrEq(this long? x, long? y) =>
            x >= y;

        public static bool Le(this long? x, long? y) =>
            x < y;

        public static bool LeOrEq(this long? x, long? y) =>
            x <= y;

        public static long FindDistance(this long? x, long? y) =>
            (long)Math.Abs((x??0) - (y??0));

        public static bool IsNullOrZero(this long? x) =>
           x == null || x.Eq(0);

        public static bool IsNotNullAndUpperZero(this long? x) =>
            x.Ge(0);

        public static bool IsNotNullAndBelowZero(this long? x) =>
            x.Le(0);

        public static long Inverted(this long x) =>
            x * -1;
        #endregion
    }
}
