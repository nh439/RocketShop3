using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrentType = long;
namespace RocketShop.Framework.Extension
{
    public static class TaskLongExtension
    {
        #region Not Nullable
        /// <summary>
        /// Equal Check
        /// </summary>
        /// <param name="x">First Operand</param>
        /// <param name="y">Second Operand</param>
        /// <returns>x == y</returns>
        public static async Task<bool> EqAsync(this Task<CurrentType> x, CurrentType y) =>
           await x == y;
        /// <summary>
        /// Not Equal Check
        /// </summary>
        /// <param name="x">First Operand</param>
        /// <param name="y">Second Operand</param>
        /// <returns>x != y</returns>
        public static async Task<bool> NotEqAsync(this Task<CurrentType> x, CurrentType y) =>
            !(await EqAsync(x, y));
        /// <summary>
        /// Greater Than Check
        /// </summary>
        /// <param name="x">First Operand</param>
        /// <param name="y">Second Operand</param>
        /// <returns>x > y</returns>
        public static async Task<bool> GeAsync(this Task<CurrentType> x, CurrentType y) =>
           await x > y;

        /// <summary>
        /// Greater Than Or Equal Check
        /// </summary>
        /// <param name="x">First Operand</param>
        /// <param name="y">Second Operand</param>
        /// <returns>x >= y</returns>
        public static async Task<bool> GeOrEqAsync(this Task<CurrentType> x, CurrentType y) =>
           await x >= y;
        /// <summary>
        /// Less Than Check
        /// </summary>
        /// <param name="x">First Operand</param>
        /// <param name="y">Second Operand</param>
        /// <returns>x < y</returns>
        public static async Task<bool> LeAsync(this Task<CurrentType> x, CurrentType y) =>
           await x < y;
        /// <summary>
        /// Less Than Or Equal Check
        /// </summary>
        /// <param name="x">First Operand</param>
        /// <param name="y">Second Operand</param>
        /// <returns>x <= y</returns>
        public static async Task<bool> LeOrEqAsync(this Task<CurrentType> x, CurrentType y) =>
           await x <= y;

        /// <summary>
        /// Find Distance Between X ,Y 
        /// </summary>
        /// <param name="x">First Operand</param>
        /// <param name="y">Second Operand</param>
        /// <returns>Math.AbsAsync(Async(x) - Async(y))</returns>
        public static async Task<CurrentType> FindDistanceAsync(this Task<CurrentType> x, CurrentType y) =>
            (CurrentType)Math.Abs((await x) - (y));
        #endregion
        #region Nullable
        public static async Task<bool> EqAsync(this Task<CurrentType?> x, CurrentType? y) =>
           await x == y;

        public static async Task<bool> NotEqAsync(this Task<CurrentType?> x, CurrentType? y) =>
            !(await EqAsync(x, y));

        public static async Task<bool> GeAsync(this Task<CurrentType?> x, CurrentType? y) =>
           await x > y;

        public static async Task<bool> GeOrEqAsync(this Task<CurrentType?> x, CurrentType? y) =>
           await x >= y;

        public static async Task<bool> LeAsync(this Task<CurrentType?> x, CurrentType? y) =>
           await x < y;

        public static async Task<bool> LeOrEqAsync(this Task<CurrentType?> x, CurrentType? y) =>
           await x <= y;

        public static async Task<CurrentType> FindDistanceAsync(this Task<CurrentType?> x, CurrentType? y) =>
            (CurrentType)Math.Abs((await x ?? 0) - (y ?? 0));
        #endregion
    }
}
