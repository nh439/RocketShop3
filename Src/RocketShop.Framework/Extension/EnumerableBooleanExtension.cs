using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Extension
{
    public static class EnumerableBooleanExtension
    {
        /// <summary>
        /// Checks if all elements in the enumerable are true.
        /// </summary>
        /// <param name="enumerable">An enumerable of boolean values.</param>
        /// <returns>True if all elements are true, otherwise false.</returns>
        public static bool AllTrue(this IEnumerable<bool> enumerable)
        {
            bool result = true;
            foreach (var item in enumerable) { 
            result = result && item;
                if (!result) 
                     return false;
            }
            return result;
        }
            

        /// <summary>
        /// Checks if all elements in the enumerable are false.
        /// </summary>
        /// <param name="enumerable">An enumerable of boolean values.</param>
        /// <returns>True if all elements are false, otherwise false.</returns>
        public static bool AllFalse(this IEnumerable<bool> enumerable)
        {
            bool result = false;
            foreach (var item in enumerable) {
                result = result && item;
                if(result) return false;
            }
            return result;
        }

        /// <summary>
        /// Checks if any element in the enumerable is true.
        /// </summary>
        /// <param name="enumerable">An enumerable of boolean values.</param>
        /// <returns>True if any element is true, otherwise false.</returns>
        public static bool SomeTrue(this IEnumerable<bool> enumerable)
        {
            bool result = true;
            foreach (var item in enumerable)
            {
                result = result || item;
                if (result)
                    return true;
            }
            return result;
        }
           

        /// <summary>
        /// Checks if any element in the enumerable is false.
        /// </summary>
        /// <param name="enumerable">An enumerable of boolean values.</param>
        /// <returns>True if any element is false, otherwise false.</returns>
        public static bool SomeFalse(this IEnumerable<bool> enumerable)
        {
            bool result = true;
            foreach (var item in enumerable)
            {
                result = result || item;
                if (!result)
                    return true;
            }
            return result;
        }

        /// <summary>
        /// Performs a logical AND operation between the first value and all values in the comparer array.
        /// </summary>
        /// <param name="firstValue">The first boolean value.</param>
        /// <param name="comparer">An array of boolean values to compare against.</param>
        /// <returns>True if the first value and all comparer values are true, otherwise false.</returns>
        public static bool And(this bool firstValue,params bool[] comparer)
        {
            var result = firstValue;
            if (!firstValue) return false;
            foreach(var item in comparer)
            {
                result = result && item;
                if (!result) return false;
            }
            return result;
        }

        /// <summary>
        /// Performs a logical OR operation between the first value and any value in the comparer array.
        /// </summary>
        /// <param name="firstValue">The first boolean value.</param>
        /// <param name="comparer">An array of boolean values to compare against.</param>
        /// <returns>True if the first value or any comparer value is true, otherwise false.</returns>
        public static bool Or(this bool firstValue,params bool[] comparer)
        {
            var result = firstValue;
            if (result) return true;
            foreach(var item in comparer)
            {
                result |= item;
                if(result)
                    return true;
            }
            return result;
        }

        /// <summary>
        /// Performs a NOR (not OR) operation between the first boolean value and the comparer array.
        /// </summary>
        /// <param name="firstValue">The first boolean value.</param>
        /// <param name="comparer">An array of boolean values to compare against.</param>
        /// <returns>True if neither the first value nor any of the comparer values are true, otherwise false.</returns>
        public static bool Nor(this bool firstValue, params bool[] comparer) =>
            !firstValue.Or(comparer);

        /// <summary>
        /// Performs a NAND (not AND) operation between the first boolean value and the comparer array.
        /// </summary>
        /// <param name="firstValue">The first boolean value.</param>
        /// <param name="comparer">An array of boolean values to compare against.</param>
        /// <returns>True if not all values, including the first value and comparer values, are true, otherwise false.</returns>
        public static bool Nand(this bool firstValue, params bool[] comparer)=> 
            !firstValue.And(comparer);
    }

}
