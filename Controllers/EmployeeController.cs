using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace RetailClientApp.Controllers
{
	public class EmployeeController : Controller
	{
		/// <summary>
		/// ye to default hi h
		/// </summary>
		/// <returns></returns>
		public IActionResult Index()
		{
			if (HttpContext.Session.GetString("IsEmployee") == null)
			{
				return RedirectToAction("Login", "Authentication");
			}
			else if(HttpContext.Session.GetString("IsEmployee") == "False")
			{
				return RedirectToAction("UnAuthorized", "Authentication");
			}else{
				return View();
			}
		}
	}
}
