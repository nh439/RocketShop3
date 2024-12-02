using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Function
{
    public static class StringFunction
    {
        private static readonly char[] _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

        public static string GenerateRandomString(int length)
        {
            if (length <= 0) throw new ArgumentException("Length must be greater than 0", nameof(length));

            var random = new Random();
            return new string(Enumerable.Repeat(_chars, length)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray());
        }
    }
}
