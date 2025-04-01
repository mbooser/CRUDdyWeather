using AspNetCore.LdapAuthentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRUDdyWeather.Controllers
{
    public class ldapAuth : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await HttpContext.AuthenticateAsync(); //Incomplete Function, Missing Parameters
            if (result.Succeeded)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            return View("LoginFailed");
        }
    }
}
