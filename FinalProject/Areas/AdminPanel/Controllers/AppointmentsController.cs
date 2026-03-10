using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProject.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Admin,Doctor")]
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _context;
        private const int PageSize = 8;

        public AppointmentsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? status, string? search, int page = 1)
        {
            var query = _context.Appointments
                .Include(x => x.Doctor)
                .AsQueryable();

            if (User.IsInRole("Doctor"))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var currentDoctor = await _context.Doctors
                    .FirstOrDefaultAsync(x => x.AppUserId == currentUserId);

                if (currentDoctor == null)
                {
                    return Forbid();
                }

                query = query.Where(x => x.DoctorId == currentDoctor.Id);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(x => x.Status == status);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.FirstName.Contains(search) ||
                    x.LastName.Contains(search) ||
                    x.Phone.Contains(search));
            }

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            if (totalPages == 0)
            {
                totalPages = 1;
            }

            if (page < 1)
            {
                page = 1;
            }

            if (page > totalPages)
            {
                page = totalPages;
            }

            var appointments = await query
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.SelectedStatus = status;
            ViewBag.Search = search;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.PageSize = PageSize;

            return View(appointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, int page = 1, string? status = null, string? search = null)
        {
            if (!await CanAccessAppointmentAsync(id))
            {
                return Forbid();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = "Approved";
            await _context.SaveChangesAsync();

            TempData["ToastMessage"] = "Randevu uğurla təsdiqləndi.";
            TempData["ToastType"] = "success";

            return RedirectToAction(nameof(Index), new { page, status, search });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, int page = 1, string? status = null, string? search = null)
        {
            if (!await CanAccessAppointmentAsync(id))
            {
                return Forbid();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = "Rejected";
            await _context.SaveChangesAsync();

            TempData["ToastMessage"] = "Randevu rədd edildi.";
            TempData["ToastType"] = "warning";

            return RedirectToAction(nameof(Index), new { page, status, search });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int page = 1, string? status = null, string? search = null)
        {
            if (!await CanAccessAppointmentAsync(id))
            {
                return Forbid();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            TempData["ToastMessage"] = "Randevu silindi.";
            TempData["ToastType"] = "info";

            return RedirectToAction(nameof(Index), new { page, status, search });
        }

        private async Task<bool> CanAccessAppointmentAsync(int appointmentId)
        {
            if (User.IsInRole("Admin"))
            {
                return true;
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return false;
            }

            var currentDoctor = await _context.Doctors
                .FirstOrDefaultAsync(x => x.AppUserId == currentUserId);

            if (currentDoctor == null)
            {
                return false;
            }

            return await _context.Appointments
                .AnyAsync(x => x.Id == appointmentId && x.DoctorId == currentDoctor.Id);
        }
    }
}