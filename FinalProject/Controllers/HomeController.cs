using FinalProject.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var homeArticles = await _context.Articles
                .Where(x => x.IsPublished)
                .OrderByDescending(x => x.CreatedAt)
                .Take(3)
                .ToListAsync();

            ViewBag.HomeArticles = homeArticles;

            return View();
        }


        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            var returnUrl = Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Url.Action("Index", "Home");

            return Redirect(returnUrl);
        }
    }
}