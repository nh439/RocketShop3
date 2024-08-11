using LanguageExt;
using System.Collections;
using System.Text;
using System.Text.Json;

namespace RocketShop.Framework.Extension
{
    public static class StringExtension
    {
        public static bool HasMessage(this string? str) =>
            !string.IsNullOrEmpty(str);

        public static string ToBase64<T>(this T item) =>
            Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(item)));

        public static Option<T> FromBase64<T>(this string base64){
            var arr = Convert.FromBase64String(base64);
            string result = Encoding.UTF8.GetString(arr, 0, arr.Length);
            return JsonSerializer.Deserialize<T>(result);
        }

        public static bool IsNullOrEmpty(this string? s)  => string.IsNullOrEmpty(s);

        public static async Task<bool> HasMessageAsync(this Task<string?> s) => (await s).HasMessage();
        public static async Task<bool> IsNullOrEmptyAsync(this Task<string?> s) => (await s).IsNullOrEmpty();



    }
}
