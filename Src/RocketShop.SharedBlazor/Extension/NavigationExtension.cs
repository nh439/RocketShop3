using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RocketShop.Framework.Extension;

namespace RocketShop.SharedBlazor.Extension
{
    public static class NavigationExtension
    {
        public static string? GetValueFromUrlQuery(this NavigationManager navigationManager, string urlQuery,string? valueIfCannotParse = null)
        {
            var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(urlQuery, out var value))
                return value;
            return valueIfCannotParse;
        }
        public static int GetValueFromUrlQueryAsInt32(this NavigationManager navigationManager, string urlQuery,int valueIfCannotParse = -1)
        {
            var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(urlQuery, out var value))
            {
                if (value.IsNull()) return valueIfCannotParse;
                string rawReturn = value!;
                return rawReturn.ToInt(valueIfCannotParse);
            }            
            return valueIfCannotParse;
        }
         public static long GetValueFromUrlQueryAsInt64(this NavigationManager navigationManager, string urlQuery,long valueIfCannotParse = -1)
        {
            var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(urlQuery, out var value))
            {
                if (value.IsNull()) return valueIfCannotParse;
                string rawReturn = value!;
                return rawReturn.ToLong(valueIfCannotParse);
            }            
            return valueIfCannotParse;
        }
        public static int? GetValueFromUrlQueryAsNullableInt32(this NavigationManager navigationManager, string urlQuery)
        {
            var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(urlQuery, out var value))
            {
                if (value.IsNull()) return null;
                string rawReturn = value!;
                return rawReturn.ToNullableInt();
            }            
            return null;
        }
         public static long? GetValueFromUrlQueryAsNullableInt64(this NavigationManager navigationManager, string urlQuery)
        {
            var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(urlQuery, out var value))
            {
                if (value.IsNull()) return null;
                string rawReturn = value!;
                return rawReturn.ToNullableInt();
            }            
            return null;
        }


    }
}
