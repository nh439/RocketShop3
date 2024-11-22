﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Extension
{
    public static class HttpContextAccessorExtension
    {
        public static string? GetCurrentUserId(this IHttpContextAccessor accessor)=>
            accessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

        public static string? GetCurrentEmail(this IHttpContextAccessor accessor) =>
            accessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Email)!.Value;
    }
}
