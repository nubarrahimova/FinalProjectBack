using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Admin")]
    [Route("admin/questions")]
    public class QuestionsController : Controller
    {
        private readonly AppDbContext _context;

        public QuestionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            ViewBag.UnreadQuestionCount = await _context.AdminQuestions.CountAsync(x => !x.IsRead);

            var questions = await _context.AdminQuestions
                .OrderBy(x => x.IsRead)
                .ThenByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(questions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.UnreadQuestionCount = await _context.AdminQuestions.CountAsync(x => !x.IsRead);

            var question = await _context.AdminQuestions
                .FirstOrDefaultAsync(x => x.Id == id);

            if (question == null)
                return NotFound();

            if (!question.IsRead)
            {
                question.IsRead = true;
                await _context.SaveChangesAsync();

                ViewBag.UnreadQuestionCount = await _context.AdminQuestions.CountAsync(x => !x.IsRead);
            }

            return View(question);
        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _context.AdminQuestions
                .FirstOrDefaultAsync(x => x.Id == id);

            if (question == null)
                return NotFound();

            _context.AdminQuestions.Remove(question);
            await _context.SaveChangesAsync();

            TempData["ToastMessage"] = "Mesaj silindi.";
            TempData["ToastType"] = "success";

            return RedirectToAction(nameof(Index));
        }
    }
}