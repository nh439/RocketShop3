namespace RocketShop.Framework.Extension
{
    public static class StringExtension
    {
        public static bool HasMessage(this string? str) =>
            !string.IsNullOrEmpty(str);
    }
}
