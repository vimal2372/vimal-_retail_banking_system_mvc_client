using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient.Memcached;
using Newtonsoft.Json;
using RetailClientApp.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailClientApp.Provider
{
	public class CustomerProvider : ICustomerProvider
	{
		/// <summary>
		/// Creates customer 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<HttpResponseMessage> CreateCustomer(CreateCutomerViewModel model)
		{
			using (HttpClient client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://52.255.224.155");
				var jsonstring = JsonConvert.SerializeObject(model);
				var obj = new StringContent(jsonstring, System.Text.Encoding.UTF8, "application/json");
				var response = await client.PostAsync("api/Customer/createCustomer", obj);
				return response;
			}
		}
		/// <summary>
		/// gets the customer details from Customer API 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<HttpResponseMessage> GetCustomerDetails(int id)
		{
			using (HttpClient client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://52.255.224.155");
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
				var response = await client.GetAsync("api/Customer/getCustomerDetails/" + id);
				return response;
			}
		}

		/// <summary>
		/// Gets all the customers from the Customer API
		/// </summary>
		/// <returns></returns>
		public async Task<HttpResponseMessage> GetCustomers()
		{
			using (HttpClient client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://52.255.224.155");
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
				var response = await client.GetAsync("api/Customer/Get");
				return response;
			}
		}
	}
}
