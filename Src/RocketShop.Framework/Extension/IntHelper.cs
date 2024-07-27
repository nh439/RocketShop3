using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrentType = int;
namespace RocketShop.Framework.Extension
{
    public static class IntHelper
    {
        #region Not Nullable
        /// <summary>
        /// Equal Check
        /// </summary>
        /// <param name="x">First Operand</param>
        /// <param name="y">Second Operand</param>
        /// <returns>x == y</returns>
        public static bool Eq(this CurrentType x, CurrentType y) =>
            x == y;
        /// <summary>
        /// Not Equal Check
        /// </summary>
        /// <param name="x">First Operand</param>
        /// <param name="y">Second Operand</param>
        /// <returns>x != y</returns>
        public static bool NotEq(this CurrentType x, CurrentType y) =>
            !Eq(x, y);
        /// <summary>
        /// Greater Than Check
        /// </summary>
        /// <param name="x">First Operand</param>
        /// <param name="y">Second Operand</param>
        /// <returns>x > y</returns>
        public static bool Ge(this CurrentType x, CurrentType y) =>
            x > y;

        /// <summary>
        /// Greater Than Or Equal Check
        /// </summary>
        /// <param name="x">First Operand</param>
        /// <param name="y">Second Operand</param>
        /// <returns>x >= y</returns>
        public static bool GeOrEq(this CurrentType x, CurrentType y) =>
            x >= y;
        /// <summary>
        /// Less Than Check
        /// </summary>
        /// <param name="x">First Operand</param>
        /// <param name="y">Second Operand</param>
        /// <returns>x < y</returns>
        public static bool Le(this CurrentType x, CurrentType y) =>
            x < y;
        /// <summary>
        /// Less Than Or Equal Check
        /// </summary>
        /// <param name="x">First Operand</param>
        /// <param name="y">Second Operand</param>
        /// <returns>x <= y</returns>
        public static bool LeOrEq(this CurrentType x, CurrentType y) =>
            x <= y;

        /// <summary>
        /// Find Distance Between X ,Y 
        /// </summary>
        /// <param name="x">First Operand</param>
        /// <param name="y">Second Operand</param>
        /// <returns>Math.Abs((x) - (y))</returns>
        public static CurrentType FindDistance(this CurrentType x, CurrentType y) =>
            (CurrentType)Math.Abs((x) - (y));
        #endregion
        #region Nullable
        public static bool Eq(this CurrentType? x, CurrentType? y)=>
            x==y;

        public static bool NotEq(this CurrentType? x, CurrentType? y)=>
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
            (CurrentType)Math.Abs((x??0) - (y??0));
        #endregion
    }
}
