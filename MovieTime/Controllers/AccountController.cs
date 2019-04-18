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
        private AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (_accountService.IsLoginValid(login))
            {
                await CreateUserIdentityAsync(login.Username);
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
            if (_accountService.IsUsernameTaken(account.UserName))
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
                _accountService.CreateAccount(account);
                await CreateUserIdentityAsync(account.UserName);

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public async Task CreateUserIdentityAsync(string username)
        {
            var userPrincipal = _accountService.GetUserPrincipal(username);
            await HttpContext.SignInAsync(userPrincipal);
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}