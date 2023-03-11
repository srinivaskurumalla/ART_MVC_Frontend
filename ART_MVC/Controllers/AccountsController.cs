using ART_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace ART_MVC.Controllers
{
    public class AccountsController : Controller
    {

        private readonly IConfiguration _configuration;
        public AccountsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult SignUp()
        {
          
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm] SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();

                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);


                   /* SignUpViewModel emp = new SignUpViewModel
                    {
                        EmpName = model.EmpName,
                        Password = model.Password,
                        Email = model.Email,
                      


                    };*/

                    var result = await client.PostAsJsonAsync("Accounts/Register", model);
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Login");
                    }
                    ModelState.AddModelError("", "UserName or Email already exist please try with different");

                }
            }
            SignUpViewModel emp1 = new SignUpViewModel
            {
                Name = model.Name,
                Password = model.Password,
                Email = model.Email,
              

            };
          
            return View(emp1);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Accounts/Login", login);

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string token = await result.Content.ReadAsAsync<string>();
                        HttpContext.Session.SetString("token", token);

                        string email = login.Email;
                        HttpContext.Session.SetString("empEmail", email);

                        return RedirectToAction("Index", "ProjectBr");
                        // TempData["UserName"] = login.Username;
                      
                    }
                    ModelState.AddModelError("", "Invalid Username or Password");
                }
            }
            TempData["Alert"] = "Invalid login credentials. Please try again.";
            return View(login);
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("token");
            HttpContext.Session.Remove("empEmail");
            return RedirectToAction("Index", "Home");
        }
    }
}
