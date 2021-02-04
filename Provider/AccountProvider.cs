using RetailClientApp.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailClientApp.Provider
{
	public class AccountProvider : IAccountProvider
	{
		/// <summary>
		/// takes the Customer ID and connect with Account API
		/// </summary>
		/// <param name="customerId"></param>
		/// <returns>customer's accounts</returns>
		public async Task<HttpResponseMessage> getCustomerAccounts(int customerId)
		{
			using (HttpClient client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://52.224.198.245");
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
				var response = await client.GetAsync("api/Account/getCustomerAccounts/" + customerId);
				return response;
			}
		}

		/// <summary>
		/// takes account Id and connects with Account API
		/// </summary>
		/// <param name="AccountId"></param>
		/// <returns>Details of Account</returns>
		public async Task<HttpResponseMessage> getAccount(int AccountId)
		{
			using (HttpClient client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://52.224.198.245");
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
				var response = await client.GetAsync("api/Account/getAccount/" + AccountId);
				return response;
			}
		}

		/// <summary>
		/// It gets all the account statements from Account API
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<HttpResponseMessage> GetAccountStatement(GetAccountStatementViewModel model)
		{
			using (HttpClient client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://52.224.198.245");
				var response = await client.PostAsJsonAsync("api/Account/getAccountStatement", new { AccountId = model.Id, from_date = model.StartDate, to_date = model.EndDate });
				return response;
			}
		}
	}
}