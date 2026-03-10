using FinalProject.Data;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels;
using FinalProject.ViewModels.Doctors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index(string? search, int? specialtyId, string? sortBy)
        {
            var query = _context.Doctors
                .Include(d => d.Speciality)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var normalizedSearch = search.Trim().ToLower();

                query = query.Where(d =>
                    d.FullName.ToLower().Contains(normalizedSearch) ||
                    (d.Clinic != null && d.Clinic.ToLower().Contains(normalizedSearch)) ||
                    (d.Speciality != null && d.Speciality.Name.ToLower().Contains(normalizedSearch)));
            }

            if (specialtyId.HasValue)
            {
                query = query.Where(d => d.SpecialityId == specialtyId.Value);
            }

            query = sortBy switch
            {
                "experience_desc" => query.OrderByDescending(d => d.ExperienceYears).ThenBy(d => d.FullName),
                "experience_asc" => query.OrderBy(d => d.ExperienceYears).ThenBy(d => d.FullName),
                "name_desc" => query.OrderByDescending(d => d.FullName),
                _ => query.OrderBy(d => d.FullName)
            };

            var doctorsFromDb = await query
                .Select(d => new DoctorCardVM(
                    d.Id,
                    d.Slug,
                    d.FullName,
                    d.Speciality != null ? d.Speciality.Name : "",
                    d.ExperienceYears,
                    d.Clinic ?? "",
                    d.PhotoUrl ?? ""
                ))
                .ToListAsync();

            var specialities = await _context.Specialities
                .OrderBy(s => s.Name)
                .ToListAsync();

            var vm = new DoctorsIndexVM
            {
                CityTitle = "Bakıda həkimlər",
                Doctors = doctorsFromDb,
                Search = search,
                SpecialtyId = specialtyId,
                SortBy = sortBy,
                Specialities = specialities
            };

            return View(vm);
        }

        [HttpGet("/doctors/{slug}")]
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
                PhotoUrl = doctor.PhotoUrl ?? "",

                LastPeriodDate = null,
                CycleLength = 28,
                PeriodLength = 5,

                Schedule = new List<ScheduleItemVM>
        {
            new() { Day = "Bazar ertəsi", Hours = "15:00 – 17:00" },
            new() { Day = "Çərşənbə axşamı", Hours = "15:00 – 17:00" },
            new() { Day = "Çərşənbə", Hours = "15:00 – 17:00" },
            new() { Day = "Cümə axşamı", Hours = "15:00 – 17:00" },
            new() { Day = "Cümə", Hours = "15:00 – 17:00" },
            new() { Day = "Şənbə", Hours = "15:00 – 17:00" },
            new() { Day = "Bazar", Hours = "Qəbul yoxdur" }
        },

                Services = new List<string>
        {
            "Hamiləlik təqibi",
            "Ginekoloji xəstəliklərin müalicəsi",
            "Ginekoloji əməliyyatlar",
            "Təbii doğuş, Qeysər",
            "Vaginizm müalicəsi"
        },

                Biography = new List<string>
        {
            "2000–2008-ci illərdə Tümen Dövlət Tibb Akademiyasında təhsil almışam.",
            "2008–2017-ci illərdə Tümen vilayətində doğum evində və qadın məsləhətxanasında həkim kimi çalışmışam.",
            "2017-ci ildən bu günə qədər Bakı şəhərində peşə fəaliyyətimi davam etdirirəm."
        }
            };

            return View(vm);
        }
    }
}

