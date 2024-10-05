using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RocketShop.Identity.Helper
{
    public static class TokenHelper
    {
        public static string BuildToken(HttpContext context)
        {
            var claims = context.User.Claims;
            string secretKey = "828db15b-65f0-4492-a1eb-a89afb182e30"; // Replace with a strong, secure key
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwtHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = signingCredentials,
                Expires = DateTime.Now.AddHours(10)
            };

            var securityToken = jwtHandler.CreateToken(tokenDescriptor);
            return jwtHandler.WriteToken(securityToken);

        }
    }
}
