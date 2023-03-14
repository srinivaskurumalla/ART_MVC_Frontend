using ART_MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ART_MVC.Controllers
{
    public class MasterBRController : Controller
    {
        private readonly IConfiguration _configuration;

        public MasterBRController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<MasterViewModel> masterViewModels = new();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("MasterBR/GetAllMasterBRs");

                if (result.IsSuccessStatusCode)
                {
                    masterViewModels = await result.Content.ReadAsAsync<List<MasterViewModel>>();
                }

            }
            return View(masterViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            MasterViewModel masterViewModel = new();
            List<ProjectViewModel> projectViewModels = new();
            SignUpViewModel signUpViewModel = new();
            string empEmail = HttpContext.Session.GetString("empEmail");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("ProjectsBR/GetAllProjectBRs");
                var loggedInEmp = await client.GetAsync($"Accounts/GetEmpId/{empEmail}");

                if (result.IsSuccessStatusCode)
                {
                    projectViewModels = await result.Content.ReadAsAsync<List<ProjectViewModel>>();
                    signUpViewModel = await loggedInEmp.Content.ReadAsAsync<SignUpViewModel>();

                }
            }
            masterViewModel.ProjectViewModels = projectViewModels;
            masterViewModel.EmployeeId = signUpViewModel.Id;

            return View(masterViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MasterViewModel masterViewModel)
        {

            List<ProjectViewModel> projectViewModels = new();
            SignUpViewModel signUpViewModel = new();
            string empEmail = HttpContext.Session.GetString("empEmail");

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);

                    if (masterViewModel.L1_Eval_Date != null && masterViewModel.L1_Eval_Date < masterViewModel.ScreeningDate)
                    {
                        ModelState.AddModelError("", "L1_Eval_Date must be grater than Screening Date");

                    }
                    else if (masterViewModel.Client_Eval_Date != null && masterViewModel.Client_Eval_Date < masterViewModel.L1_Eval_Date)
                    {
                        ModelState.AddModelError("", "Client_Eval_Date must be grater than L1_Eval_Date");

                    }
                    else if (masterViewModel.Manager_Eval_Date != null && masterViewModel.Manager_Eval_Date < masterViewModel.Client_Eval_Date)
                    {
                        ModelState.AddModelError("", "Manager_Eval_Date must be grater than Client_Eval_Date");
                    }

                    else
                    {




                        var result = await client.PostAsJsonAsync("MasterBR/CreateMasterBR", masterViewModel);
                        var projects1 = await client.GetAsync("ProjectsBR/GetAllProjectBRs");
                        projectViewModels = await projects1.Content.ReadAsAsync<List<ProjectViewModel>>();

                        var loggedInEmp = await client.GetAsync($"Accounts/GetEmpId/{empEmail}");

                        signUpViewModel = await loggedInEmp.Content.ReadAsAsync<SignUpViewModel>();

                        if (result.StatusCode == System.Net.HttpStatusCode.Created)
                        {
                            return RedirectToAction("Index", "MasterBR");

                        }
                    }

                    var projects = await client.GetAsync("ProjectsBR/GetAllProjectBRs");
                    projectViewModels = await projects.Content.ReadAsAsync<List<ProjectViewModel>>();

                }

            }
                MasterViewModel masterView = new MasterViewModel
                {
                    ProjectViewModels = projectViewModels,
                    EmployeeId = signUpViewModel.Id

                };

                // return View(obj);

                return View(masterView);
            }

            [HttpGet]
            public async Task<IActionResult> Details(int id)
            {
                MasterViewModel masterViewModel = new();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"MasterBR/GetMasterBRById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        masterViewModel = await result.Content.ReadAsAsync<MasterViewModel>();
                        // var cityNameResult = await client.GetAsync($"Cities/GetcityById/{house.CityId}");

                        // var cityName = await cityNameResult.Content.ReadAsAsync<CityViewModel>();

                        // house.CityName = cityName.CityName;
                    }

                    /*  house.HouseId = id;
                      string houseId = id.ToString();
                      HttpContext.Session.SetString("houseId", houseId);
      */
                }
                return View(masterViewModel);
            }


            [HttpGet]
            public async Task<IActionResult> Edit(int id)
            {
                // List<ProjectViewModel> projectViewModels = new();

                if (ModelState.IsValid)
                {
                    MasterViewModel masterViewModel = new();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                        var result = await client.GetAsync($"MasterBR/GetMasterBRById/{id}");
                        var projects = await client.GetAsync("ProjectsBR/GetAllProjectBRs");
                        if (result.IsSuccessStatusCode)
                        {
                            masterViewModel = await result.Content.ReadAsAsync<MasterViewModel>();

                            //  movie.Genres = await this.GetGenres();
                            masterViewModel.ProjectViewModels = await projects.Content.ReadAsAsync<List<ProjectViewModel>>();
                            masterViewModel.CandidateId = masterViewModel.CandidateId;
                            masterViewModel.EmployeeId = masterViewModel.EmployeeId;
                            masterViewModel.EmployeeName = masterViewModel.EmployeeName;
                            return View(masterViewModel);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Candidate doesn't exists");
                        }
                    }
                }
                return View();
            }

        [HttpPost]
        public async Task<IActionResult> Edit(MasterViewModel masterViewModel)
        {
            string empEmail = HttpContext.Session.GetString("empEmail");

            if (empEmail != null)
            {


                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        // client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                        client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                        var result = await client.PutAsJsonAsync($"MasterBR/UpdateMasterBR/{masterViewModel.Id}", masterViewModel);
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
                return View(masterViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
        }

            [HttpGet]
            public async Task<IActionResult> Delete(int id)
            {
                if (ModelState.IsValid)
                {
                    MasterViewModel masterViewModel = null;
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                        var result = await client.GetAsync($"MasterBR/GetMasterBRById/{id}");
                        if (result.IsSuccessStatusCode)
                        {
                            masterViewModel = await result.Content.ReadAsAsync<MasterViewModel>();
                            //  movie.Genres = await this.GetGenres();
                            return View(masterViewModel);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Candidate details doesn't exists");
                        }
                    }
                }
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Delete(MasterViewModel masterViewModel)
            {
                using (var client = new HttpClient())
                {
                    // client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.DeleteAsync($"MasterBR/DeleteMasterBR/{masterViewModel.Id}");
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Login", "Accounts");
                    }
                }
            }
        }
    }
