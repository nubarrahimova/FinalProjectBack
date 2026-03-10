using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Admin,Doctor")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalAppointments = await _context.Appointments.CountAsync();
            var newAppointments = await _context.Appointments.CountAsync(x => x.Status == "New");
            var approvedAppointments = await _context.Appointments.CountAsync(x => x.Status == "Approved");
            var rejectedAppointments = await _context.Appointments.CountAsync(x => x.Status == "Rejected");

            var totalArticles = await _context.Articles.CountAsync();
            var totalDoctors = await _context.Doctors.CountAsync();

            ViewBag.TotalAppointments = totalAppointments;
            ViewBag.NewAppointments = newAppointments;
            ViewBag.ApprovedAppointments = approvedAppointments;
            ViewBag.RejectedAppointments = rejectedAppointments;
            ViewBag.TotalArticles = totalArticles;
            ViewBag.TotalDoctors = totalDoctors;

            return View();
        }
    }
}