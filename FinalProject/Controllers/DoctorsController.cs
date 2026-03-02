using FinalProject.Controllers;
using FinalProject.Data;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

private readonly AppDbContext _context;

public DoctorsController(AppDbContext context)
{
    _context = context;
}

namespace FinalProject.Controllers
{
    public class DoctorsController : Controller
    {
        [HttpGet("/doctors")]
        public IActionResult Index()
        {
            var vm = new DoctorsIndexVM
            {
                CityTitle = "Bakıda həkimlər",
                Doctors = new List<DoctorCardVM>
            {
                new("dr.xuraman-qaribova", "Dr. Xuraman Qaribova", "Ginekoloq", 17, "HTCcliniva hospital", "/assets/images/xuraman-qaribova.jpg"),
                new("dr.ceyran-imameliyeva", "Dr. Ceyran İmaməliyeva", "Ginekoloq", 9, "4 Saylı Qadın Məsləhətxanası", "/assets/images/ceyran-imamaliyeva.jpg"),
            }
            };

            return View(vm);
        }

        public IActionResult Details(string slug)
        {
            var vm = new DoctorDetailsVM
            {
                Slug = slug,
                FullName = "Dr. Xuraman Qaribova",
                Speciality = "Ginekoloq",
                ExperienceYears = 17,
                Clinic = "HTCcliniva hospital",
                PhotoUrl = "/assets/images/xuraman-qaribova.jpg"
            };

            return View(vm);
        }
    }
}

