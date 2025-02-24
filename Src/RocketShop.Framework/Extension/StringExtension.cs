﻿using LanguageExt;
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

        public static string IfNull(this string? s, string messageIfNull) =>
            s.HasMessage() ? s! : messageIfNull;

        public static string SealMiddleCharacters(this string input)
        {
            if (input.Length <= 2)
                return input;

            int middleLength = input.Length - 4;
            string sealedPart = new string('X', middleLength);
            return input.Substring(0, 2) + sealedPart + input.Substring(input.Length - 2);
        }
        public static bool InCaseSensitiveEquals(this string? input,string? comparer)
        {
            if(input == null && comparer == null)
            {
                return true;
            }
            return input?.ToLower() == comparer?.ToLower();
        } 
    }



}

