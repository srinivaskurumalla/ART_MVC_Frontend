using Microsoft.AspNetCore.Mvc;

namespace ART_MVC.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
