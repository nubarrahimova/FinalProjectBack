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

    }
}