using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers;

[Route("booking")]
public class BookingController : Controller
{
    private readonly AppDbContext _context;

    public BookingController(AppDbContext context)
    {
        _context = context;
    }

    // GET /booking/{slug}
    [HttpGet("{slug}")]
    public async Task<IActionResult> Create(string slug)
    {
        var doctor = await _context.Doctors
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Slug == slug);

        if (doctor == null) return NotFound();

        var vm = new BookingCreateVM
        {
            DoctorSlug = slug,
            DoctorName = doctor.FullName
        };

        return View(vm);
    }

    // POST /booking/{slug}
    [HttpPost("{slug}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string slug, BookingCreateVM model)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Slug == slug);

        if (doctor == null)
            return NotFound();

        model.DoctorSlug = slug;
        model.DoctorName = doctor.FullName;

        if (!ModelState.IsValid)
            return View(model);

        var appointment = new Appointment
        {
            DoctorId = doctor.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Phone = model.Phone,
            Email = model.Email,
            Date = model.Date!.Value,
            Time = model.Time,
            Comment = model.Comment,
            Status = "New"
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Success));
    }

    // GET /booking/success
    [HttpGet("success")]
    public IActionResult Success()
    {
        return View();
    }
}