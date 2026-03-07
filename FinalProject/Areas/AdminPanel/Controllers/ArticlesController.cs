using System.Text;
using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ArticlesController : Controller
    {
        private readonly AppDbContext _context;

        public ArticlesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _context.Articles
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Article());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article model)
        {
            model.Slug = GenerateSlug(model.Title);
            ModelState.Remove("Slug");

            if (!ModelState.IsValid)
                return View(model);

            model.CreatedAt = DateTime.Now;

            var slugExists = await _context.Articles.AnyAsync(x => x.Slug == model.Slug);
            if (slugExists)
            {
                model.Slug = $"{model.Slug}-{DateTime.Now:yyyyMMddHHmmss}";
            }

            _context.Articles.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
                return NotFound();

            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Article model)
        {
            if (id != model.Id)
                return NotFound();

            ModelState.Remove("Slug");

            if (!ModelState.IsValid)
                return View(model);

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
                return NotFound();

            article.Title = model.Title;
            article.Summary = model.Summary;
            article.Content = model.Content;
            article.CoverImageUrl = model.CoverImageUrl;
            article.IsPublished = model.IsPublished;

            var newSlug = GenerateSlug(model.Title);
            var slugExists = await _context.Articles.AnyAsync(x => x.Slug == newSlug && x.Id != id);

            article.Slug = slugExists ? $"{newSlug}-{id}" : newSlug;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
                return NotFound();

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private static string GenerateSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            text = text.Trim().ToLowerInvariant();
            var sb = new StringBuilder();

            foreach (var ch in text)
            {
                if (char.IsLetterOrDigit(ch))
                    sb.Append(ch);
                else if (char.IsWhiteSpace(ch) || ch == '-' || ch == '_')
                    sb.Append('-');
            }

            var result = sb.ToString();

            while (result.Contains("--"))
                result = result.Replace("--", "-");

            return result.Trim('-');
        }
    }
}