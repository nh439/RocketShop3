using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrentType = double;
namespace RocketShop.Framework.Extension
{
    public static class DoubleHelper
    {
        #region Not Nullable
        public static bool Eq(this CurrentType x, CurrentType y) =>
            x == y;

        public static bool NotEq(this CurrentType x, CurrentType y) =>
            !Eq(x, y);

        public static bool Ge(this CurrentType x, CurrentType y) =>
            x > y;

        public static bool GeOrEq(this CurrentType x, CurrentType y) =>
            x >= y;

        public static bool Le(this CurrentType x, CurrentType y) =>
            x < y;

        public static bool LeOrEq(this CurrentType x, CurrentType y) =>
            x <= y;

        public static CurrentType FindDistance(this CurrentType x, CurrentType y) =>
            (CurrentType)Math.Abs((x) - (y));
        #endregion
        #region Nullable
        public static bool Eq(this CurrentType? x, CurrentType? y) =>
            x == y;

        public static bool NotEq(this CurrentType? x, CurrentType? y) =>
            !Eq(x, y);

        public static bool Ge(this CurrentType? x, CurrentType? y) =>
            x > y;

        public static bool GeOrEq(this CurrentType? x, CurrentType? y) =>
            x >= y;

        public static bool Le(this CurrentType? x, CurrentType? y) =>
            x < y;

        public static bool LeOrEq(this CurrentType? x, CurrentType? y) =>
            x <= y;

        public static CurrentType FindDistance(this CurrentType? x, CurrentType? y) =>
            (CurrentType)Math.Abs((x ?? 0) - (y ?? 0));
        #endregion
    }
}
