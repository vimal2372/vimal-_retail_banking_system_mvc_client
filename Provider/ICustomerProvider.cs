using Microsoft.AspNetCore.Mvc;
using RetailClientApp.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailClientApp.Provider
{
	public interface ICustomerProvider
	{
		Task<HttpResponseMessage> CreateCustomer(CreateCutomerViewModel model);
		Task<HttpResponseMessage> GetCustomerDetails(int id);
		Task<HttpResponseMessage> GetCustomers();
	}
}
