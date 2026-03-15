using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class AdminQuestionsController : Controller
    {
        private readonly AppDbContext _context;

        public AdminQuestionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("/AdminQuestions/Send")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(AskAdminCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Zəhmət olmasa bütün vacib sahələri düzgün doldurun.";
                TempData["ToastType"] = "error";
                return Redirect("/");
            }

            var entity = new AdminQuestion
            {
                UserType = model.UserType,
                FullName = model.FullName,
                Phone = model.Phone,
                Email = model.Email,
                Subject = model.Subject,
                Message = model.Message,
                CreatedAt = DateTime.Now,
                IsRead = false
            };

            _context.AdminQuestions.Add(entity);
            await _context.SaveChangesAsync();

            return Redirect("/AdminQuestions/Success");
        }

        [HttpGet("/AdminQuestions/Success")]
        public IActionResult Success()
        {
            return View();
        }
    }
}