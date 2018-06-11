using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication_MultiTenant.AuthHelpers;
using WebApplication_MultiTenant.Models;

namespace WebApplication_MultiTenant.Controllers
{
    public class AuthController : Controller
    {
        private readonly IJwtTokenGenerator tokenGenerator;

        public AuthController(IJwtTokenGenerator tokenGenerator)
        {
            this.tokenGenerator = tokenGenerator;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var tokenWithClaims = this.tokenGenerator.GenerateAccessTokenIfIdentityConfirmed(loginModel.Username, loginModel.Password);
            //loginModel.Username == "aman@yahoo.com" && loginModel.Password == "test123"

            if (!string.IsNullOrWhiteSpace(tokenWithClaims.AccessToken))
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, tokenWithClaims.ClaimsPrincipal);
                // I am having to do this with cookie based JWT token auth otherwise no cookies are
                // sent out and login effectively fails.
                //HttpContext.Response.Cookies.Append("AccessToken", tokenWithClaims.AccessToken);
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public static ClaimsPrincipal Create(string userName)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, userName));
            claimsPrincipal.AddIdentity(new ClaimsIdentity(claims, "Password", ClaimTypes.Name, "Recipient"));

            return claimsPrincipal;
        }
    }
}