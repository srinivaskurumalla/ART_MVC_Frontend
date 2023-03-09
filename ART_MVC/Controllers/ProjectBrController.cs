using ART_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ART_MVC.Controllers
{
    public class ProjectBrController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProjectBrController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<ProjectViewModel> projectViewModels = new();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("ProjectsBR/GetAllProjectBRs");

                if (result.IsSuccessStatusCode)
                {
                    projectViewModels = await result.Content.ReadAsAsync<List<ProjectViewModel>>();
                }
            }
            return View(projectViewModels);
        }
    }
}
