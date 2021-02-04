using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RetailClientApp.Controllers
{
	public class HomeController : Controller
	{
		/// <summary>
		/// ye bhi defaul h
		/// </summary>
		/// <returns></returns>
		public IActionResult Index()
		{
			return View();
		}
	}
}
