using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class DoctorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
