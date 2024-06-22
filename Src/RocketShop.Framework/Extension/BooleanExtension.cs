namespace RocketShop.Framework.Extension
{
    public static class BooleanExtension
    {
        public static bool IsTrue(this bool? value) =>
            value.HasValue && value.Value;

        public static bool IsFalse(this bool? value) =>
            value.HasValue && !value.Value;

        public static bool IsNull(this bool? value) =>
            !value.HasValue;

        public static bool IsNotNull(this bool? value) =>
            value.HasValue;

        public static bool ToBoolean(this string? value) =>
            bool.TryParse(value, out bool result) ? result : false;
    }
}
