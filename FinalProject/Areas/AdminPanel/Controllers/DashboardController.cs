using System.Security.Claims;
using FinalProject.Areas.AdminPanel.ViewModels;
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
            var model = new DashboardVM();
            var currentYear = DateTime.Now.Year;

            model.CurrentYear = currentYear;
            model.MonthlyLabels = new List<string>
            {
                "Yan", "Fev", "Mar", "Apr", "May", "İyn",
                "İyl", "Avq", "Sen", "Okt", "Noy", "Dek"
            };

            if (User.IsInRole("Admin"))
            {
                model.TotalAppointments = await _context.Appointments.CountAsync();
                model.NewAppointments = await _context.Appointments.CountAsync(x => x.Status == "New");
                model.ApprovedAppointments = await _context.Appointments.CountAsync(x => x.Status == "Approved");
                model.RejectedAppointments = await _context.Appointments.CountAsync(x => x.Status == "Rejected");

                model.TotalArticles = await _context.Articles.CountAsync();
                model.TotalDoctors = await _context.Doctors.CountAsync();

                model.ShowDoctorColumn = true;

                model.RecentAppointments = await _context.Appointments
                    .Include(x => x.Doctor)
                    .OrderByDescending(x => x.Id)
                    .Take(5)
                    .Select(x => new DashboardRecentAppointmentVM
                    {
                        Id = x.Id,
                        FullName = x.FirstName + " " + x.LastName,
                        Phone = x.Phone,
                        Date = x.Date,
                        Time = x.Time,
                        Status = x.Status,
                        DoctorName = x.Doctor != null ? x.Doctor.FullName : null
                    })
                    .ToListAsync();

                var adminMonthlyData = await _context.Appointments
                    .Where(x => x.Date.Year == currentYear)
                    .GroupBy(x => x.Date.Month)
                    .Select(g => new
                    {
                        Month = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                model.MonthlyAppointmentCounts = Enumerable.Range(1, 12)
                    .Select(month => adminMonthlyData.FirstOrDefault(x => x.Month == month)?.Count ?? 0)
                    .ToList();
            }
            else
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    return Forbid();
                }

                var currentDoctor = await _context.Doctors
                    .FirstOrDefaultAsync(x => x.AppUserId == currentUserId);

                if (currentDoctor == null)
                {
                    return Forbid();
                }

                model.TotalAppointments = await _context.Appointments
                    .CountAsync(x => x.DoctorId == currentDoctor.Id);

                model.NewAppointments = await _context.Appointments
                    .CountAsync(x => x.DoctorId == currentDoctor.Id && x.Status == "New");

                model.ApprovedAppointments = await _context.Appointments
                    .CountAsync(x => x.DoctorId == currentDoctor.Id && x.Status == "Approved");

                model.RejectedAppointments = await _context.Appointments
                    .CountAsync(x => x.DoctorId == currentDoctor.Id && x.Status == "Rejected");

                model.TotalArticles = await _context.Articles
                    .CountAsync(x => x.DoctorId == currentDoctor.Id);

                model.TotalDoctors = 1;

                model.ShowDoctorColumn = false;

                model.RecentAppointments = await _context.Appointments
                    .Where(x => x.DoctorId == currentDoctor.Id)
                    .OrderByDescending(x => x.Id)
                    .Take(5)
                    .Select(x => new DashboardRecentAppointmentVM
                    {
                        Id = x.Id,
                        FullName = x.FirstName + " " + x.LastName,
                        Phone = x.Phone,
                        Date = x.Date,
                        Time = x.Time,
                        Status = x.Status,
                        DoctorName = null
                    })
                    .ToListAsync();

                var doctorMonthlyData = await _context.Appointments
                    .Where(x => x.DoctorId == currentDoctor.Id && x.Date.Year == currentYear)
                    .GroupBy(x => x.Date.Month)
                    .Select(g => new
                    {
                        Month = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                model.MonthlyAppointmentCounts = Enumerable.Range(1, 12)
                    .Select(month => doctorMonthlyData.FirstOrDefault(x => x.Month == month)?.Count ?? 0)
                    .ToList();
            }

            return View(model);
        }
    }
}