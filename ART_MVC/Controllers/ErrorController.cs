using Microsoft.AspNetCore.Mvc;

namespace ART_MVC.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProjectDeleteError()
        {
            int? projectId =  @ViewBag.projectDeleteId;
            return View(projectId);
        }
    }
}
