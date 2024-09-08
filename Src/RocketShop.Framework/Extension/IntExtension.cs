using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrentType = int;
namespace RocketShop.Framework.Extension
{
    public static class IntExtension
    {
        #region Not Nullable
        /// <summary>
        /// Determines whether two instances of <see cref="CurrentType"/> are equal by comparing their values.
        /// </summary>
        /// <param name="x">The first instance of <see cref="CurrentType"/> to compare.</param>
        /// <param name="y">The second instance of <see cref="CurrentType"/> to compare.</param>
        /// <returns><c>true</c> if the two instances are equal; otherwise, <c>false</c>.</returns>
        public static bool Eq(this CurrentType x, CurrentType y) =>
            x == y;
        /// <summary>
        /// Determines whether two instances of <see cref="CurrentType"/> are not equal by comparing their values.
        /// </summary>
        /// <param name="x">The first instance of <see cref="CurrentType"/> to compare.</param>
        /// <param name="y">The second instance of <see cref="CurrentType"/> to compare.</param>
        /// <returns><c>true</c> if the two instances are not equal; otherwise, <c>false</c>.</returns>
        public static bool NotEq(this CurrentType x, CurrentType y) =>
            !Eq(x, y);
        /// <summary>
        /// Determines whether one instance of <see cref="CurrentType"/> is greater than to another instance.
        /// </summary>
        /// <param name="x">The first instance of <see cref="CurrentType"/> to compare.</param>
        /// <param name="y">The second instance of <see cref="CurrentType"/> to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is greater than to <paramref name="y"/>; otherwise, <c>false</c>.</returns>
        public static bool Ge(this CurrentType x, CurrentType y) =>
            x > y;

        /// <summary>
        /// Determines whether one instance of <see cref="CurrentType"/> is greater than or equal to another instance.
        /// </summary>
        /// <param name="x">The first instance of <see cref="CurrentType"/> to compare.</param>
        /// <param name="y">The second instance of <see cref="CurrentType"/> to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is greater than or equal to <paramref name="y"/>; otherwise, <c>false</c>.</returns>
        public static bool GeOrEq(this CurrentType x, CurrentType y) =>
            x >= y;
        /// <summary>
        /// Determines whether one instance of <see cref="CurrentType"/> is less than to another instance.
        /// </summary>
        /// <param name="x">The first instance of <see cref="CurrentType"/> to compare.</param>
        /// <param name="y">The second instance of <see cref="CurrentType"/> to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is less than to <paramref name="y"/>; otherwise, <c>false</c>.</returns>
        public static bool Le(this CurrentType x, CurrentType y) =>
            x < y;
        /// <summary>
        /// Determines whether one instance of <see cref="CurrentType"/> is less than or equal to another instance.
        /// </summary>
        /// <param name="x">The first instance of <see cref="CurrentType"/> to compare.</param>
        /// <param name="y">The second instance of <see cref="CurrentType"/> to compare.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is less than or equal to <paramref name="y"/>; otherwise, <c>false</c>.</returns>
        public static bool LeOrEq(this CurrentType x, CurrentType y) =>
            x <= y;

        /// <summary>
        /// Calculates the distance between two instances of <see cref="CurrentType"/>.
        /// </summary>
        /// <param name="x">The first instance of <see cref="CurrentType"/> to use in the distance calculation.</param>
        /// <param name="y">The second instance of <see cref="CurrentType"/> to use in the distance calculation.</param>
        /// <returns>A <see cref="CurrentType"/> representing the distance between <paramref name="x"/> and <paramref name="y"/>.</returns>
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
