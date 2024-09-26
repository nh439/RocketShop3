using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RocketShop.AuditService.Helper
{
    public static class MessageHelper
    {
        public static string[] GetUserIdFromMessage(string message)
        {
            string pattern = @"<([^>]+)>";

            // Find matches
            MatchCollection matches = Regex.Matches(message, pattern);
            return matches.Select(x=>x.Value).ToArray();
        }
    }
}
