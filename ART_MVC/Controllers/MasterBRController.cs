using ART_MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("ProjectsBR/GetAllProjectBRs");

                if (result.IsSuccessStatusCode)
                {
                    projectViewModels = await result.Content.ReadAsAsync<List<ProjectViewModel>>();
                }
            }
            masterViewModel.ProjectViewModels= projectViewModels;

            return View(masterViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MasterViewModel masterViewModel)
        {
          
            List<ProjectViewModel> projectViewModels = new();
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                   
                    var result = await client.PostAsJsonAsync("MasterBR/CreateMasterBR", masterViewModel);
                    var projects = await client.GetAsync("ProjectsBR/GetAllProjectBRs");
                    projectViewModels = await projects.Content.ReadAsAsync<List<ProjectViewModel>>();
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index", "MasterBR");

                    }
                }
            }
            MasterViewModel masterView = new MasterViewModel
            {
                ProjectViewModels = projectViewModels
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
                     var projects =  await client.GetAsync("ProjectsBR/GetAllProjectBRs");
                    if (result.IsSuccessStatusCode)
                    {
                        masterViewModel = await result.Content.ReadAsAsync<MasterViewModel>();

                        //  movie.Genres = await this.GetGenres();
                        masterViewModel.ProjectViewModels = await projects.Content.ReadAsAsync<List<ProjectViewModel>>();
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
