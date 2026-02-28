using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class ArticlesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
