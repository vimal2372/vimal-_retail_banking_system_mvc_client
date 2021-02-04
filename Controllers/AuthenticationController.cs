using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RetailClientApp.Models.ViewModel;

namespace RetailClientApp.Controllers
{
	public class AuthenticationController : Controller
	{
        /// <summary>
        /// gives view for the login
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns>login view</returns>
		[HttpGet]
		public IActionResult Login(string returnUrl=null)
		{
			this.ViewData["returnUrl"] = returnUrl;
			return View();
		}
        /// <summary>
        /// It does the authentication for the Login
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
		[HttpPost,ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model,string returnUrl="")
		{
			if(!ModelState.IsValid)
				return View(model);
            using (HttpClient client = new HttpClient())
			{
                client.BaseAddress = new Uri("http://52.224.149.18");
                var jsonstring = JsonConvert.SerializeObject(model);
                var obj = new StringContent(jsonstring, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Auth/UserLogin", obj);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContext.Session.SetString("IsEmployee",model.IsEmployee.ToString());
                    HttpContext.Session.SetString("Username",model.Username);
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    if (model.IsEmployee)
					{
                        return RedirectToAction("Index", "Employee");
					}
                    else
					{
                        return RedirectToAction("Index", "Customer");
					}
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    ModelState.AddModelError("", "Bad credential.");
                    return View(model);
                }
                ViewBag.Message = "Unable to process your request at this moments! Sorry.";
                return View(model);
            }

        }
        /// <summary>
        /// Logging out 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("IsEmployee") != null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Authentication");
            }
            return View();
        }
        /// <summary>
        /// redirects to the view when unauthorized user tries to call 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UnAuthorized()
		{
            return View();
		}
    }
}
