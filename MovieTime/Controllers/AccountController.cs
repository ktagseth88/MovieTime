using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieTime.ViewModels.Account;
using MovieTime.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace MovieTime.Controllers
{
    public class AccountController : Controller
    {
        private AccountService accountService = new AccountService();

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (accountService.IsLoginValid(login))
            {
                // Create the identity from the user info
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, login.Username, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, login.Username));
                identity.AddClaim(new Claim(ClaimTypes.Name, login.Username));

                // Authenticate using the identity
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = true });
            }
            else
            {
                ModelState.AddModelError("Username", "Invalid login");
                return View();
            }
            return null;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AccountCreationViewModel account)
        {
            if (accountService.IsUsernameTaken(account.UserName))
            {
                ModelState.AddModelError("Username", "Username already taken");
            }
            if (account.Password != account.PasswordValidation)
            {
                ModelState.AddModelError("Password", "Passwords must match");
                ModelState.AddModelError("PasswordValidation", "Passwords must match");
            }
            if (ModelState.IsValid)
            {
                accountService.CreateAccount(account);
                return RedirectToAction("Login", new LoginViewModel { Username = account.UserName, Password = account.Password });
            }

            return View();
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}