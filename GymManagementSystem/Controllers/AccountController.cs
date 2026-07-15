using GymManagementSystem.BLL.ViewModels.AccountViewModels;
using GymManagementSystem.Controllers;
using GymManagementSystem.DAL.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers
{
    public class AccountController : Controller
    {

        private readonly SignInManager<ApplicationUser> _signInManager;


        public AccountController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {

            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,
                model.RememberMe, lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account Locked, please try again later.");
                return View(model);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid Email or Password.");
                return View(model);
            }


            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }


            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        //1. remove cookie and sign out user
        //2. logged => logged out
        //3. redirect to login page

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // said to browser to remove cookie and sign out user
            return RedirectToAction(nameof(Login)); // redirect to login page
        }


        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
