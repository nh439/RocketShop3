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

        public HomeController(ILogger<HomeController> logger,
            UserManager<User> user,
            SignInManager<User> signInManager,
            IConfiguration configuration)
        {
            _logger = logger;
            _userManager = user;
            _signInManager = signInManager;
            _configuration = configuration;
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
            var res = await _signInManager.PasswordSignInAsync(user!, password,false,false);
            if (res.Succeeded)
            {
                SetClaims(user!);
                return Redirect("LoggedIn?state=Login_Successful");
            }
            return Redirect("LoggedIn?state=Error");
        }
        [HttpGet]
       
        public IActionResult GoogleLogin(string? returnUrl)
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/home/External" }, GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet]
        public async Task<IActionResult> External(string? returnUrl)
        {
            return Redirect("LoggedIn?state=Login_Successful");
        }
        [HttpGet]
        public IActionResult LoggedIn(string? state)
        {
            ViewBag.State = state;
            return View();
        }
        public void SetClaims(User user)
        {
            ClaimsPrincipal principal = new ClaimsPrincipal();
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
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext?.User.AddIdentity(identity);
        }
    }
}
