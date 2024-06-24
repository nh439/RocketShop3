using LanguageExt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RocketShop.Database.Model.Identity;
using RocketShop.Framework.Extension;
using RocketShop.Identity.Configuration;
using RocketShop.Identity.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web;

namespace RocketShop.Identity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;
        readonly IConfiguration _configuration;
        readonly IHttpContextAccessor _accessor;

        public HomeController(ILogger<HomeController> logger,
            UserManager<User> user,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            IHttpContextAccessor accessor)
        {
            _logger = logger;
            _userManager = user;
            _signInManager = signInManager;
            _configuration = configuration;
            _accessor = accessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return Ok(200);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Login(string? returnUrl)
        {
            var authenicate = HttpContext.User.Identity?.IsAuthenticated;
            if (authenicate.IsTrue())
                return Redirect("LoggedIn?state=Login_Successful");
            ViewBag.ReturnUrl = returnUrl;  
            return View();
        }
            
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username,string password,string? returnUrl)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user.IsNull())
                return Redirect("LoggedIn?state=Username_Invalid");
            var res = await _signInManager.CheckPasswordSignInAsync(user!,password,true);
            if (res.Succeeded)
            {
                await _signInManager.SignInWithClaimsAsync(user!,null ,SetClaims(user!));
                return Redirect("LoggedIn?state=Login_Successful");
            }
            return Redirect("LoggedIn?state=Error");
        }
        [HttpGet]
        public IActionResult GoogleLogin(string? returnUrl)
        {

            var oidc = _configuration.GetSection("OauthConfiguration").Get<OauthConfiguration>();
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
        }
        [HttpPost]
        public async Task<IActionResult> External(string? returnUrl,string? id_token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(id_token);
            var email = jwtSecurityToken.Claims.Find(x => x.Type == "email").FirstOrDefault().Value;
            if (email.IsNull())
                return Redirect("LoggedIn?state=Username_Invalid");
            var user = await _userManager.FindByEmailAsync(email);
            if (user.IsNull())
                return Redirect("LoggedIn?state=Access_Diened");
            var claims = SetClaims(user!);
            await _signInManager.SignInWithClaimsAsync(user!,null,claims);
            return Redirect("LoggedIn?state=Login_Successful");
        }
        [HttpGet]
        public IActionResult LoggedIn(string? state)
        {
            ViewBag.State = state;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
        List<Claim> SetClaims(User user)
        {
            List<Claim> claims = new List<Claim>();
            var newClaim = new
            {
                sub = user.Id,
                name = user.UserName,
                email = user.Email,
                surname = user.Surname,
                firstname = user.Firstname,
                active = !user.Resigned
            };       
            foreach (var claim in newClaim.ToDictionaryStringObject())
            {
                if (claim.Value == null)
                    continue;
                var nc = new Claim(claim.Key, claim.Value.ToString());
                claims.Add(nc);
            }
            return claims;
        }
    }
}
