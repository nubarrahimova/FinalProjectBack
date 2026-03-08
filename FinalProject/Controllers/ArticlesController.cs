using FinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using FinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly AppDbContext _context;

        public ArticlesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _context.Articles
                .Where(x => x.IsPublished)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(articles);
        }

        [Route("Articles/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var article = await _context.Articles
                .FirstOrDefaultAsync(x => x.IsPublished && x.Slug == slug);

            if (article == null)
                return NotFound();

            return View("Detail", article);
        }
    }
}