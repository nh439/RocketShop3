using LanguageExt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using RocketShop.Identity.Configuration;
using RocketShop.Identity.IdentityBaseServices;
using RocketShop.Identity.Models;
using RocketShop.Identity.Service;
using System.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RocketShop.Identity.Controllers
{
    public class HomeController(ILogger<HomeController> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            IRoleAndPermissionService roleAndPermissionService,
            IdentityContext identityContext) : IdentityControllerServices(logger)
    {

        public IActionResult Index() =>
            InvokeControllerService(() =>
            {
                return View();
            });

        public IActionResult Privacy() =>
            InvokeControllerService(() => View());


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            InvokeControllerService(() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }));

        public IActionResult Login(string? returnUrl, string? err) =>
            InvokeControllerService<IActionResult>(() =>
            {
                var authenicate = HttpContext.User.Identity?.IsAuthenticated;
                if (authenicate.IsTrue())
                {
                    var token = BuildToken();
                    return Redirect(returnUrl.HasMessage() ? $"{returnUrl}?id_token={token}" : "/");
                }
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.Err = err;
                return View();
            });


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl) =>
            await InvokeControllerServiceAsync(
                async () =>
                {
                    var user = await userManager.FindByNameAsync(username);
                    if (user.IsNull())
                        return Redirect($"Login?{(returnUrl.HasMessage() ? $"returnUrl={returnUrl}&" : string.Empty)}err=Username Invalid");
                    if (user!.Resigned)
                        return Redirect("AccessDeined?mode=2");
                    var res = await signInManager.CheckPasswordSignInAsync(user!, password, true);
                    if (res.Succeeded)
                    {
                        var userAccessFailed = await userManager.GetAccessFailedCountAsync(user);
                        if (userAccessFailed >= 3)
                            return Redirect($"AccessDeined?mode=3");
                        await userManager.ResetAccessFailedCountAsync(user); 
                        await userManager.UpdateAsync(user);
                        var roles = await roleAndPermissionService.GetRoleByUserId(user.Id);
                        var permissions = await roleAndPermissionService.GetAuthoriozedPermissionList(user.Id);
                        var roleNames = roles.GetRight()?.Select(s => s.RoleName);
                        await signInManager.SignInWithClaimsAsync(user!, null, SetClaims(user!,roleNames,permissions.GetRight()));
                        user.LastLoggedIn = DateTime.UtcNow;
                        await userManager.UpdateAsync(user);
                        var token = BuildToken();
                        return Redirect(returnUrl.HasMessage() ? $"{returnUrl}?id_token={token}" : "/");
                    }
                    if(res.IsLockedOut)
                        return Redirect($"AccessDeined?mode=3");
                    else
                        await userManager.AccessFailedAsync(user);

                    return Redirect($"Login?&{(returnUrl.HasMessage() ? $"returnUrl={returnUrl}&" : string.Empty)}err=Password Incorrect");
                }
                );

        [HttpGet]
        public IActionResult GoogleLogin(string? returnUrl) =>
            InvokeControllerService(() =>
            {
                if (returnUrl.HasMessage())
                    HttpContext.Response.Cookies.Append("redirect", returnUrl!);
                var oidc = configuration.GetSection("OauthConfiguration").Get<OauthConfiguration>();
                string url = @$"{oidc.Authority}/o/oauth2/v2/auth?
client_id={oidc.ClientId}&
redirect_uri={oidc.RedirectUrl}&
response_type=id_token&
scope=openid%20profile%20email&
response_mode=form_post&
nonce=638548327190926054.OTczMmM2MmUtZmI0Ny00ODJjLWFjYzItNGE0MGZkY2JiZjRmMzM1ZDY3OGYtMzkyNC00NTkwLTgzM2MtODAzODM5Nzk5MTI1&
state=CfDJ8E87PjxRz2hHkHQUruOJ7QF04EiUqk8jLe-6Hid7OS0WfKS2-3wGgOpUf0WXOPnkFTOXMGGU5znEr1U-8ZNCh32OfGfM28slIuDj7pz1m1Vir_Q51f1pTMvHvfK1YR6QzKZO0SrnW72-0wuHIdP9iF_xv4QpS5EBCcS7w0zW098HWpf6LO87kHZ2i3pqGaTDrbQCplQeFcBHNnM9YE6Y7MJtkfXs_dOvkld-YmbGwZzi8q39N5zL_q2zgRVI6Ql2jslN3BRTXtv7dsZA3kyyhdKwPcRoRn3yoqhHWBD5lhtVsfxPYU00p58QGF_E513jQw&
x-client-SKU=ID_NET8_0&
x-client-ver=7.1.2.0";
                url = url.Replace(Environment.NewLine, string.Empty);
                return Redirect(url);
            });

        [HttpPost]
        public async Task<IActionResult> External(string? id_token) =>
            await InvokeControllerServiceAsync(async () =>
            {
                Option<string> redirect = HttpContext.Request.Cookies.Where(s => s.Key == "redirect").Select(s => s.Value).FirstOrDefault();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(id_token);
                var iss = jwtSecurityToken.Issuer;
                var sub = jwtSecurityToken.Subject;
                var user = await identityContext.Users.FirstOrDefaultAsync(
                    x => x.ProviderName == iss &&
                    x.ProviderKey == sub
                );
                if (user.IsNull())
                {
                    var email = jwtSecurityToken.Claims.Find(x => x.Type == "email").FirstOrDefault()!.Value;
                    if (email.IsNull())
                        return Redirect(($"Login?{(redirect.IsSome ? $"returnUrl={redirect.Extract()!}&" : string.Empty)}err=Username Invalid"));
                    user = await userManager.FindByEmailAsync(email);
                    if (user.IsNull())
                        return Redirect("AccessDeined");
                    if (user!.Resigned)
                        return Redirect("AccessDeined?mode=2");
                    await identityContext.Users.Where(x => x.Email == email)
                    .ExecuteUpdateAsync(s =>
                    s.SetProperty(c => c.ProviderName, iss)
                    .SetProperty(c => c.ProviderKey, sub));
                }
                var roles = await roleAndPermissionService.GetRoleByUserId(user!.Id);
                var permissions = await roleAndPermissionService.GetAuthoriozedPermissionList(user.Id);
                var roleNames = roles.GetRight()?.Select(s => s.RoleName);
                var claims = SetClaims(user!,roleNames,permissions.GetRight());
                await signInManager.SignInWithClaimsAsync(user!, null, claims);
                user.LastLoggedIn= DateTime.UtcNow;
                await userManager.UpdateAsync(user);
                var token = BuildToken();
                if (redirect.IsSome)
                    return Redirect(redirect.Extract().Tranform(r => $"{r}?id_token={token}")!);
                return Redirect($"/");
            });

        [HttpGet]
        public IActionResult LoggedIn(string? state) =>
            InvokeControllerService(() =>
            {
                ViewBag.State = state;
                return View();
            });

        [HttpGet]
        public async Task<IActionResult> Logout() =>
            await InvokeControllerServiceAsync(async () =>
            {
                await signInManager.SignOutAsync();
                return Redirect("/");
            });

        List<Claim> SetClaims(User user, IEnumerable<string>? roles, IEnumerable<string> ?permissions)
        {
            List<Claim> claims = new List<Claim>();
            var newClaim = new
            {
                sub = user.Id,
                name = user.UserName,
                email = user.Email,
                surname = user.Surname,
                firstname = user.Firstname,
                active = !user.Resigned,
                exp = DateTime.UtcNow.AddHours(10).ToUnixTime()
            };
            foreach (var claim in newClaim.ToDictionaryStringObject()!)
            {
                if (claim.Value == null)
                    continue;
                var nc = new Claim(claim.Key, claim.Value.ToString()!);
                claims.Add(nc);
            }
            roles.HasDataAndForEach(role =>    
                    claims.Add(new Claim("Role", role)));
            permissions.HasDataAndForEach(permission =>
                    claims.Add(new Claim("Permission", permission)));

            return claims;
        }
        [HttpGet]
        public IActionResult AccessDeined(int? mode) =>
            View("./Views/Home/AccessFailed.cshtml", mode);

        public string BuildToken()
        {
            var claims = HttpContext.User.Claims;
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
        [Authorize]
        public IActionResult AuthSample() => View();
    }
}
