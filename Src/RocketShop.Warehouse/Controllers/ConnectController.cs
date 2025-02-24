﻿using Microsoft.AspNetCore.Mvc;
using RocketShop.Framework.Extension;
using RocketShop.Warehouse.Services;

namespace RocketShop.Warehouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectController(IAuthenicationService authenicationService) : ControllerBase
    {
        [HttpPost("token")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> CallToken(
            [FromForm] string client_id,
            [FromForm] string? client_secret,
            [FromForm] string application)
        {
            var result = await authenicationService.IssueToken(client_id, client_secret, application);
            return result.IsRight ? Ok(result.GetRight()) : BadRequest("Invalid Request");
        }

        [HttpPost("introspection")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> introspection(
            [FromForm] string token)
        {
            var result = await authenicationService.Introspection(token);
            return result.IsRight ? Ok(result.GetRight()) : BadRequest("Invalid Request");
        }

        [HttpPost("revocation")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Revocation(
            [FromForm] string token)
        {
            var result = await authenicationService.Revocation(token);
            return result.IsRight ? Ok(result.GetRight()) : BadRequest("Invalid Request");
        }
    }
}
