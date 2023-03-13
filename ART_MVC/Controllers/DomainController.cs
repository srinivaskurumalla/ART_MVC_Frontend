using ART_MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.Intrinsics.Arm;

namespace ART_MVC.Controllers
{
    public class DomainController : Controller
    {
        private readonly IConfiguration _configuration;

        public DomainController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            List<DomainViewModel> domainViewModels = new();
            ProjectViewModel projectViewModel = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("Domains/GetAllDomains");
                var project = await client.GetAsync($"ProjectsBR/GetProjectBRById/{id}");

                if (result.IsSuccessStatusCode)
                {
                    domainViewModels = await result.Content.ReadAsAsync<List<DomainViewModel>>();
                    domainViewModels = domainViewModels.Where(d => d.ProjectFkId== id).ToList();
                    projectViewModel = await project.Content.ReadAsAsync<ProjectViewModel>();
                    ViewBag.projectName = projectViewModel.ProjectName;
                }

               
            }
            return View(domainViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int projectId)
        {
           // @(ViewContext.RouteData.Values["id"].ToInt();
            DomainViewModel domainViewModel = new();
            List<ProjectViewModel> projectViewModels = new();
            List<AccountViewModel> accountViewModels = new();
            SignUpViewModel signUpViewModel = new();
            ProjectViewModel projectViewModel = new();
            string empEmail = HttpContext.Session.GetString("empEmail");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("ProjectsBR/GetAllProjectBRs");
                var allAccounts = await client.GetAsync("AccountsBR/GetAllAccBRs");

                var loggedInEmp = await client.GetAsync($"Accounts/GetEmpId/{empEmail}");

                var project = await client.GetAsync($"ProjectsBR/GetProjectBRById/{projectId}");


                if (result.IsSuccessStatusCode)
                {
                    projectViewModels = await result.Content.ReadAsAsync<List<ProjectViewModel>>();
                    signUpViewModel = await loggedInEmp.Content.ReadAsAsync<SignUpViewModel>();
                    accountViewModels = await allAccounts.Content.ReadAsAsync<List<AccountViewModel>>();
                    projectViewModel = await project.Content.ReadAsAsync<ProjectViewModel>();

                }
            }
            domainViewModel.ProjectViewModels = projectViewModels;
            domainViewModel.AccountViewModels = accountViewModels;
            domainViewModel.EmployeeId = signUpViewModel.Id;
            domainViewModel.ProjectFkId = projectId;
            domainViewModel.ProjectName = projectViewModel.ProjectName;


            return View(domainViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DomainViewModel domainViewModel)
        {

            List<ProjectViewModel> projectViewModels = new();
            List<AccountViewModel> accountViewModels = new();
            SignUpViewModel signUpViewModel = new();
            string empEmail = HttpContext.Session.GetString("empEmail");

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())

                {
                     domainViewModel.Age = (int)(DateTime.Now - domainViewModel.ApprovedDate).TotalDays;

                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);

                    var result = await client.PostAsJsonAsync("Domains/CreateDomain", domainViewModel);
                    var projects = await client.GetAsync("ProjectsBR/GetAllProjectBRs");
                    var allAccounts = await client.GetAsync("AccountsBR/GetAllAccBRs");
                    var loggedInEmp = await client.GetAsync($"Accounts/GetEmpId/{empEmail}");

                    projectViewModels = await projects.Content.ReadAsAsync<List<ProjectViewModel>>();
                    signUpViewModel = await loggedInEmp.Content.ReadAsAsync<SignUpViewModel>();
                    accountViewModels = await allAccounts.Content.ReadAsAsync<List<AccountViewModel>>();
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index", "ProjectBr");

                    }
                }
            }
            DomainViewModel domainView = new DomainViewModel
            {
                ProjectViewModels = projectViewModels,
                AccountViewModels= accountViewModels,
                SignUpViewModel = signUpViewModel
            };

            // return View(obj);

            return View(domainView);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            DomainViewModel domainViewModel = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Domains/GetDomainById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    domainViewModel = await result.Content.ReadAsAsync<DomainViewModel>();
                    // var cityNameResult = await client.GetAsync($"Cities/GetcityById/{house.CityId}");

                    // var cityName = await cityNameResult.Content.ReadAsAsync<CityViewModel>();

                    // house.CityName = cityName.CityName;
                }
                domainViewModel.Age = (int)(DateTime.Now - domainViewModel.ApprovedDate).TotalDays;
                /*  house.HouseId = id;
                  string houseId = id.ToString();
                  HttpContext.Session.SetString("houseId", houseId);,.L"
                O
  */
            }
            return View(domainViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // List<ProjectViewModel> projectViewModels = new();

            if (ModelState.IsValid)
            {
                DomainViewModel domainViewModel = new();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Domains/GetDomainById/{id}");
                   // var projects = await client.GetAsync("ProjectsBR/GetAllProjectBRs");
                    if (result.IsSuccessStatusCode)
                    {
                        domainViewModel = await result.Content.ReadAsAsync<DomainViewModel>();

                        //  movie.Genres = await this.GetGenres();
                        //  masterViewModel.ProjectViewModels = await projects.Content.ReadAsAsync<List<ProjectViewModel>>();
                        domainViewModel.Age = (int)(DateTime.Now - domainViewModel.ApprovedDate).TotalDays;

                        domainViewModel.ProjectName = domainViewModel.ProjectName;
                        domainViewModel.ProjectFkId= domainViewModel.ProjectFkId;
                        domainViewModel.EmployeeId = domainViewModel.EmployeeId;
                        return View(domainViewModel);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Domain doesn't exists");
                    }

                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DomainViewModel domainViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                  

                    // client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    domainViewModel.Age = (int)(DateTime.Now - domainViewModel.ApprovedDate).TotalDays;

                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"Domains/UpdateDomain/{domainViewModel.Id}", domainViewModel);
                    if (result.StatusCode == System.Net.HttpStatusCode.OK && domainViewModel.Age >= 0)
                    {
                        return RedirectToAction("Index","ProjectBr");
                    }
                    else if(domainViewModel.Age < 0)
                    {
                        ModelState.AddModelError("", "Approval date must be today or past.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Server error..Please try again later.");

                    }
                }
            }
            //MovieViewModel viewModel = new MovieViewModel
            //{
            //    Genres = await this.GetGenres()
            //};
            return View(domainViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                DomainViewModel domainViewModel = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Domains/GetDomainById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        domainViewModel = await result.Content.ReadAsAsync<DomainViewModel>();
                        //  movie.Genres = await this.GetGenres();
                        return View(domainViewModel);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Domain details doesn't exists");
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DomainViewModel domainViewModel)
        {
            using (var client = new HttpClient())
            {
                // client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Domains/DeleteDomain/{domainViewModel.Id}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index","ProjectBr");
                }
                else
                {
                    return RedirectToAction("Login", "Accounts");
                }
            }
        }
    }
}
