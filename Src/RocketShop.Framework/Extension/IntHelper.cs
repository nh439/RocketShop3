using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Extension
{
    public static class IntHelper
    {
        #region Not Nullable
        public static bool Eq(this int x, int y) =>
            x == y;

        public static bool NotEq(this int x, int y) =>
            !Eq(x, y);

        public static bool Ge(this int x, int y) =>
            x > y;

        public static bool GeOrEq(this int x, int y) =>
            x >= y;

        public static bool Le(this int x, int y) =>
            x < y;

        public static bool LeOrEq(this int x, int y) =>
            x <= y;

        public static int FindDistance(this int x, int y) =>
            (int)Math.Abs((x) - (y));
        #endregion
        #region Nullable
        public static bool Eq(this int? x, int? y)=>
            x==y;

        public static bool NotEq(this int? x, int? y)=>
            !Eq(x, y);

        public static bool Ge(this int? x, int? y) =>
            x > y;

        public static bool GeOrEq(this int? x, int? y) =>
            x >= y;

        public static bool Le(this int? x, int? y) =>
            x < y;

        public static bool LeOrEq(this int? x, int? y) =>
            x <= y;

        public static int FindDistance(this int? x, int? y) =>
            (int)Math.Abs((x??0) - (y??0));
        #endregion
    }
}
