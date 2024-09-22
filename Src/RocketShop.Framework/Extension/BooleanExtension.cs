namespace RocketShop.Framework.Extension
{
    public static class BooleanExtension
    {
        /// <summary>
        /// Checks if the nullable boolean is true.
        /// </summary>
        /// <param name="value">A nullable boolean value.</param>
        /// <returns>True if the value is true, otherwise false.</returns>
        public static bool IsTrue(this bool? value) =>
            value.HasValue && value.Value;

        /// <summary>
        /// Checks if the nullable boolean is false.
        /// </summary>
        /// <param name="value">A nullable boolean value.</param>
        /// <returns>True if the value is false, otherwise false.</returns>
        public static bool IsFalse(this bool? value) =>
            value.HasValue && !value.Value;

        /// <summary>
        /// Checks if the nullable boolean is null.
        /// </summary>
        /// <param name="value">A nullable boolean value.</param>
        /// <returns>True if the value is null, otherwise false.</returns>
        public static bool IsNull(this bool? value) =>
            !value.HasValue;

        /// <summary>
        /// Checks if the nullable boolean is not null.
        /// </summary>
        /// <param name="value">A nullable boolean value.</param>
        /// <returns>True if the value is not null, otherwise false.</returns>
        public static bool IsNotNull(this bool? value) =>
            value.HasValue;

        /// <summary>
        /// Converts a string to a boolean.
        /// </summary>
        /// <param name="value">A string representing a boolean value.</param>
        /// <returns>True if the string is successfully parsed as a boolean, otherwise false.</returns>
        public static bool ToBoolean(this string? value) =>
            bool.TryParse(value, out bool result) ? result : false;

        /// <summary>
        /// Performs an XOR (exclusive OR) operation between two boolean values.
        /// </summary>
        /// <param name="firstValue">The first boolean value.</param>
        /// <param name="comparer">The second boolean value to compare with.</param>
        /// <returns>True if one value is true and the other is false, otherwise false.</returns>
        public static bool XOR(this bool firstValue, bool comparer) =>
            firstValue != comparer;

        /// <summary>
        /// Performs an XNOR (exclusive NOR) operation between two boolean values.
        /// </summary>
        /// <param name="firstValue">The first boolean value.</param>
        /// <param name="comparer">The second boolean value to compare with.</param>
        /// <returns>True if both values are the same (both true or both false), otherwise false.</returns>
        public static bool XNOR(this bool firstValue, bool comparer) =>
            firstValue == comparer;
    }
}
