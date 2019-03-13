using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTime.Services;
using MovieTime.ViewModels.Account;
using System.Security.Claims;
using System.Threading.Tasks;

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
                await CreateUserIdentity(login.Username);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("Username", "Invalid login");
                return View();
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountCreationViewModel account)
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

                await CreateUserIdentity(account.UserName);

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public async Task CreateUserIdentity(string username)
        {
            var userPrincipal = accountService.GetUserPrincipal(username);
            await HttpContext.SignInAsync(userPrincipal);
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}