using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RetailClientApp.Models.ViewModel;
using RetailClientApp.Models;

using Microsoft.AspNetCore.Http;
using RetailClientApp.Provider;

namespace RetailClientApp.Controllers
{
	public class CustomerController : Controller
	{
		private readonly ICustomerProvider _provider;
		private log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(AccountController));

		public CustomerController(ICustomerProvider provider)
		{
			this._provider = provider;
		}
		/// <summary>
		/// returns default view for customer's welcome
		/// </summary>
		/// <returns></returns>
		public IActionResult Index()
		{
			return View();
		}
		/// <summary>
		/// view for new customer creation
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Create()
		{
			if (HttpContext.Session.GetString("IsEmployee") == null)
			{
				return RedirectToAction("Login", "Authentication");
			}else if (HttpContext.Session.GetString("IsEmployee") == "False")
			{
				return RedirectToAction("UnAuthorized", "Authentication");
			}
			else
			{
				return View();
			}
		}
		/// <summary>
		/// this actually creates the customer
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost,ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateCutomerViewModel model)
		{
			if (HttpContext.Session.GetString("IsEmployee") == null)
			{
				return RedirectToAction("Login", "Authentication");
			}
			else
			{
				if (!ModelState.IsValid)
					return View(model);

				CreateCustomerSuccessViewModel createSuccess = new CreateCustomerSuccessViewModel();
				try
				{

					var response = await _provider.CreateCustomer(model);
					if (response.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var jsoncontent = await response.Content.ReadAsStringAsync();
						createSuccess = JsonConvert.DeserializeObject<CreateCustomerSuccessViewModel>(jsoncontent);
						return View("CreateSuccess", createSuccess);
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
					{
						ModelState.AddModelError("", "Having server issue while adding record");
						return View(model);
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
					{
						ModelState.AddModelError("", "Username already present with ID :" + model.CustomerId);
						return View(model);
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
					{
						ModelState.AddModelError("", "Invalid model states");
						return View(model);
					}
				}catch(Exception ex)
				{
					_logger.Error("Exception Occured as : " + ex.Message);
				}
				return View(model);
			}
		}
		/// <summary>
		/// Shows success view when customer is created
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		public IActionResult CreateSuccess()
		{
			CreateCustomerSuccessViewModel model = new CreateCustomerSuccessViewModel();
			return View(model);
		}

		/// <summary>
		/// Get soecific customer details by ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> GetCustomerDetails(int id)
		{
			if (HttpContext.Session.GetString("IsEmployee") == null)
			{
				return RedirectToAction("Login", "Authentication");
			}
			else
			{
				Customer customer = new Customer();
				try
				{
					var response = await _provider.GetCustomerDetails(id);
					if (response.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var JsonContent = await response.Content.ReadAsStringAsync();
						customer = JsonConvert.DeserializeObject<Customer>(JsonContent);
						return View(customer);
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
					{
						ViewBag.Message = "No any record Found! Bad Request";
						return View(customer);
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
					{
						ViewBag.Message = "Having server issue while adding record";
						return View(customer);
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
					{
						ViewBag.Message = "No record found in DB for ID :"+id;
						return View(customer);
					}
				}catch(Exception ex)
				{
					_logger.Warn("Exception occured as :"+ex.Message);
				}
				return View(customer);
			}
		}
		/// <summary>
		/// get All customers from the Customer API
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> GetCustomer()
		{
			if (HttpContext.Session.GetString("IsEmployee") == null)
			{
				return RedirectToAction("Login", "Authentication");
			}
			else
			{
				List<Customer> customers = new List<Customer>();
				try 
				{
					var response =await _provider.GetCustomers();
					if (response.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var JsonContent = await response.Content.ReadAsStringAsync();
						customers = JsonConvert.DeserializeObject<List<Customer>>(JsonContent);
						return View(customers);
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
					{
						ViewBag.Message = "Having server issue while adding record";
						return View(customers);
					}
				
				}catch(Exception ex)
				{
					_logger.Error("Exceptions Occured as :" + ex.Message);
				}
				return View(customers);
			}
		}
	}
}
