using FinalProject.Data;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{

    public class DoctorsController : Controller
    {
     
    private readonly AppDbContext _context;

    public DoctorsController(AppDbContext context)
    {
        _context = context;
    }

        [HttpGet("/doctors")]
        public async Task<IActionResult> Index()
        {
            var doctorsFromDb = await _context.Doctors
                .Include(d => d.Speciality)
                .Select(d => new DoctorCardVM(
                    d.Slug,
                    d.FullName,
                    d.Speciality != null ? d.Speciality.Name : "",
                    d.ExperienceYears,
                    d.Clinic ?? "",
                    d.PhotoUrl ?? ""
                ))
                .ToListAsync();

            var vm = new DoctorsIndexVM
            {
                CityTitle = "Bakıda həkimlər",
                Doctors = doctorsFromDb
            };

            return View(vm);
        }
        [HttpGet("doctors/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Speciality)
                .FirstOrDefaultAsync(d => d.Slug == slug);

            if (doctor == null)
                return NotFound();

            var vm = new DoctorDetailsVM
            {
                Slug = doctor.Slug,
                FullName = doctor.FullName,
                Speciality = doctor.Speciality != null ? doctor.Speciality.Name : "",
                ExperienceYears = doctor.ExperienceYears,
                Clinic = doctor.Clinic ?? "",
                PhotoUrl = doctor.PhotoUrl ?? ""
            };

            return View(vm);
        }
    }
}

