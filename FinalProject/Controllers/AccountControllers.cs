using FinalProject.Models;
using FinalProject.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet("/admin/Account/Login")]
        public IActionResult AdminLoginRedirect()
        {
            return Redirect("/Account/Login");
        }

        [AllowAnonymous]
        [HttpGet("/admin/Account/AccessDenied")]
        public IActionResult AdminAccessDeniedRedirect()
        {
            return Redirect("/Account/AccessDenied");
        }

        [AllowAnonymous]
        [HttpGet("/Account/Login")]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Appointments", new { area = "AdminPanel" });
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost("/Account/Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email və ya şifrə yanlışdır.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email və ya şifrə yanlışdır.");
                return View(model);
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            var isDoctor = await _userManager.IsInRoleAsync(user, "Doctor");

            if (isAdmin || isDoctor)
            {
                if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Appointments", new { area = "AdminPanel" });
            }

            await _signInManager.SignOutAsync();
            ModelState.AddModelError(string.Empty, "Bu panelə giriş icazəniz yoxdur.");
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet("/Account/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        [HttpPost("/Account/Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Account/Login");
        }

      
    }
}