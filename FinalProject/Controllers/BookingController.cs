using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class BookingController : Controller
    {
        [HttpGet]
        public IActionResult Create(string slug)
        {
            var vm = new BookingCreateVM
            {
                DoctorSlug = slug,
                DoctorName = "Dr. Xuraman Qaribova"
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(string slug, BookingCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                model.DoctorSlug = slug;
                return View(model);
            }

            return RedirectToAction(nameof(Success));
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}

