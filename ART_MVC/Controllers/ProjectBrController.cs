using ART_MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ART_MVC.Controllers
{
    public class ProjectBrController : Controller
    {
        private readonly IConfiguration _configuration;

        public string Status { get; set; }
        public ProjectBrController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

      //  [HttpGet]
        public async Task<IActionResult> AllBRs([FromForm] string? projectId, [FromForm] string status = "All")
        {
            Status = status;
            Proj_Acc_Dto proj_Acc_Dto = new();
            List<ProjectViewModel> projectViewModels = new();
            List<AccountViewModel> accountViewModels = new();
           // SignUpViewModel signUpViewModel = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("ProjectsBR/GetAllProjectBRs");
                var allAccounts = await client.GetAsync("AccountsBR/GetAllAccBRs");
               // string empEmail = HttpContext.Session.GetString("empEmail");
               // var loggedInEmp = await client.GetAsync($"Accounts/GetEmpId/{empEmail}");

                if (result.IsSuccessStatusCode)
                {
                    projectViewModels = await result.Content.ReadAsAsync<List<ProjectViewModel>>();
                    accountViewModels = await allAccounts.Content.ReadAsAsync<List<AccountViewModel>>();

                }
              /*  signUpViewModel = await loggedInEmp.Content.ReadAsAsync<SignUpViewModel>();

                if (signUpViewModel == null)
                {
                    return RedirectToAction("Index", "Error");
                }*/
               // projectViewModels = projectViewModels.Where(p => p.EmployeeId == signUpViewModel.Id).ToList();
                proj_Acc_Dto.projectViewModels = projectViewModels;
                proj_Acc_Dto.accountViewModels = accountViewModels;

                if (result.IsSuccessStatusCode && status != "All")
                {
                    proj_Acc_Dto.projectViewModels =  projectViewModels.Where(p => p.ProjectId == projectId).ToList();
                    if(status != null)
                    {
                       // proj_Acc_Dto.projectViewModels = projectViewModels.Where(p => p.Status == status).ToList();

                    }
                }
            }

            Proj_Acc_Dto proj_Acc_Dto1 = new Proj_Acc_Dto
            {
                accountViewModels = proj_Acc_Dto.accountViewModels,
                projectViewModels = proj_Acc_Dto.projectViewModels,
                ProjectId = projectId,
                Values = new List<SelectListItem>
                {
                    new SelectListItem { Value = "All", Text = "All" },
                    new SelectListItem { Value = "OPEN", Text = "OPEN" },
                    new SelectListItem { Value = "CLOSED", Text = "CLOSED" },
                    new SelectListItem { Value = "PENDING", Text = "PENDING" },
                }
                

            };
            return View(proj_Acc_Dto1);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Proj_Acc_Dto proj_Acc_Dto = new();
            List<ProjectViewModel> projectViewModels = new();
            List<AccountViewModel> accountViewModels = new();
            SignUpViewModel signUpViewModel = new();
           List<DomainViewModel> domainViewModels = new();
            //int totalPositionsRerquired = 0;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("ProjectsBR/GetAllProjectBRs");
                var allAccounts = await client.GetAsync("AccountsBR/GetAllAccBRs");
                string empEmail = HttpContext.Session.GetString("empEmail");
                var loggedInEmp = await client.GetAsync($"Accounts/GetEmpId/{empEmail}");
                var allDomains = await client.GetAsync("Domains/GetAllDomains");

                if (result.IsSuccessStatusCode)
                {
                    projectViewModels = await result.Content.ReadAsAsync<List<ProjectViewModel>>();
                    accountViewModels = await allAccounts.Content.ReadAsAsync<List<AccountViewModel>>();
                    domainViewModels = await allDomains.Content.ReadAsAsync<List<DomainViewModel>>();

                }
                signUpViewModel = await loggedInEmp.Content.ReadAsAsync<SignUpViewModel>();

                if(signUpViewModel == null)
                {
                    return RedirectToAction("Index", "Error");
                }
                projectViewModels = projectViewModels.Where(p => p.EmployeeId == signUpViewModel.Id).ToList();

                foreach (ProjectViewModel project in projectViewModels)
                {
                    var matchingDomains = domainViewModels.Where(d => d.ProjectFkId == project.Id && d.No_Of_Positions >= 0).ToList();
                    Console.WriteLine($"Matching domains for project {project.Id}: {matchingDomains.Count}");
                    project.Total_Positions = matchingDomains.Sum(d => d.No_Of_Positions);
                }

                // int total = domWhere(d => d.ProjectFkId == obj.Id).Where(d => d.No_Of_Positions >= 0).Sum(d => d.No_Of_Positions);

                proj_Acc_Dto.projectViewModels= projectViewModels;
               
                proj_Acc_Dto.accountViewModels= accountViewModels;

             
            }
            
            return View(proj_Acc_Dto);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ProjectViewModel projectViewModel = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"ProjectsBR/GetProjectBRById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    projectViewModel = await result.Content.ReadAsAsync<ProjectViewModel>();
                  
                }

               
            }
            return View(projectViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ProjectViewModel projectViewModel = new();
            List<AccountViewModel> accountViewModels = new();
            SignUpViewModel signUpViewModel = new();
            string empEmail = HttpContext.Session.GetString("empEmail");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("AccountsBR/GetAllAccBRs");
                var loggedInEmp = await client.GetAsync($"Accounts/GetEmpId/{empEmail}");

                if (result.IsSuccessStatusCode)
                {
                    accountViewModels = await result.Content.ReadAsAsync<List<AccountViewModel>>();
                    signUpViewModel = await loggedInEmp.Content.ReadAsAsync<SignUpViewModel>();
                }
            }
            projectViewModel.AccountViewModels = accountViewModels;
            projectViewModel.EmployeeId = signUpViewModel.Id;
            projectViewModel.EmployeeName = signUpViewModel.Name;
           // projectViewModel.Age = (int)(DateTime.Now - projectViewModel.ApprovedDate).TotalDays;
          //  projectViewModel.

            return View(projectViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectViewModel projectViewModel)
        {

            List<AccountViewModel> accountViewModels = new();
            if (ModelState.IsValid)
            {
               /// projectViewModel.Age = (int)(DateTime.Now - projectViewModel.ApprovedDate).TotalDays;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);

                    var result = await client.PostAsJsonAsync("ProjectsBR/CreateProjectBR", projectViewModel);
                    var accounts = await client.GetAsync("AccountsBR/GetAllAccBRs");
                    accountViewModels = await accounts.Content.ReadAsAsync<List<AccountViewModel>>();
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index", "ProjectBr");

                    }
                }
            }
            ProjectViewModel projectView = new ProjectViewModel
            {
                AccountViewModels = accountViewModels
            };

            // return View(obj);

            return View(projectView);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // List<ProjectViewModel> projectViewModels = new();

            if (ModelState.IsValid)
            {
                ProjectViewModel projectViewModel = new();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"ProjectsBR/GetProjectBRById/{id}");
                    var accounts = await client.GetAsync("AccountsBR/GetAllAccBRs");
                    if (result.IsSuccessStatusCode)
                    {
                        projectViewModel = await result.Content.ReadAsAsync<ProjectViewModel>();

                        //  movie.Genres = await this.GetGenres();
                        projectViewModel.AccountViewModels = await accounts.Content.ReadAsAsync<List<AccountViewModel>>();
                        return View(projectViewModel);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Project doesn't exists");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Project doesn't exists");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProjectViewModel projectViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    // client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"ProjectsBR/UpdateProjectBR/{projectViewModel.Id}", projectViewModel);
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Server Error. Please try later");
                    }
                }
            }
            //MovieViewModel viewModel = new MovieViewModel
            //{
            //    Genres = await this.GetGenres()
            //};
            return View(projectViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                ProjectViewModel projectViewModel = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"ProjectsBR/GetProjectBRById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        projectViewModel = await result.Content.ReadAsAsync<ProjectViewModel>();
                        //  movie.Genres = await this.GetGenres();
                        return View(projectViewModel);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Project details doesn't exists");
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProjectViewModel projectViewModel)
        {
            using (var client = new HttpClient())
            {
                // client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"ProjectsBR/DeleteProjectBR/{projectViewModel.Id}");

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.projectDeleteId = projectViewModel.Id;
                    return RedirectToAction("ProjectDeleteError", "Error");
                }
            }
        }

    }
}
