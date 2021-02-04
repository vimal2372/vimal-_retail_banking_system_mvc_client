using Microsoft.AspNetCore.Mvc;
using RetailClientApp.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailClientApp.Repository
{
	public interface ICustomerRepository
	{
		IActionResult CreateCustomer(CreateCutomerViewModel model);
	}
}
