using System.Text;
using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels.AdminPanel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ArticlesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ArticlesController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
            return View(new ArticleFormVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleFormVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var slug = GenerateSlug(model.Title);

            var slugExists = await _context.Articles.AnyAsync(x => x.Slug == slug);
            if (slugExists)
            {
                slug = $"{slug}-{DateTime.Now:yyyyMMddHHmmss}";
            }

            string? imageUrl = null;

            if (model.CoverImageFile != null)
            {
                imageUrl = await SaveImageAsync(model.CoverImageFile);
            }

            var article = new Article
            {
                Title = model.Title,
                Slug = slug,
                Summary = model.Summary,
                Content = model.Content,
                CoverImageUrl = imageUrl,
                CreatedAt = DateTime.Now,
                IsPublished = model.IsPublished
            };

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Məqalə uğurla əlavə olundu.";
            return RedirectToAction(nameof(Success));
        }
  
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
                return NotFound();

            var vm = new ArticleFormVM
            {
                Id = article.Id,
                Title = article.Title,
                Summary = article.Summary,
                Content = article.Content,
                ExistingImageUrl = article.CoverImageUrl,
                IsPublished = article.IsPublished
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ArticleFormVM model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
                return NotFound();

            article.Title = model.Title;
            article.Summary = model.Summary;
            article.Content = model.Content;
            article.IsPublished = model.IsPublished;

            var newSlug = GenerateSlug(model.Title);
            var slugExists = await _context.Articles.AnyAsync(x => x.Slug == newSlug && x.Id != id);
            article.Slug = slugExists ? $"{newSlug}-{id}" : newSlug;

            if (model.CoverImageFile != null)
            {
                article.CoverImageUrl = await SaveImageAsync(model.CoverImageFile);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Məqalə uğurla yeniləndi.";
            return RedirectToAction(nameof(Success));
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

            TempData["SuccessMessage"] = "Məqalə uğurla silindi.";
            return RedirectToAction(nameof(Success));
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "images", "articles");
            Directory.CreateDirectory(uploadsFolder);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/assets/images/articles/{fileName}";
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