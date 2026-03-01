using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
                new("dr.xuraman-qaribova", "Dr. Xuraman Qaribova", "Ginekoloq", 17, "HTCcliniva hospital", "/assets/images/doctor1.jpg"),
                new("dr.ceyran-imameliyeva", "Dr. Ceyran İmaməliyeva", "Ginekoloq", 9, "4 Saylı Qadın Məsləhətxanası", "/assets/images/doctor2.jpg"),
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
                PhotoUrl = "/assets/images/doctor1.jpg"
            };

            return View(vm);
        }
    }
}

