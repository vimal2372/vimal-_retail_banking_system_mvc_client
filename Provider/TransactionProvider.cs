using RetailClientApp.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailClientApp.Provider
{
	public class TransactionProvider : ITransactionProvider
	{
		/// <summary>
		/// Deposits money to someone's account
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<HttpResponseMessage> Deposit(CreateDepositWithdrawViewModel model)
		{
			using (HttpClient client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://40.76.130.186");
				var response = await client.PostAsJsonAsync("api/transaction/deposit", new { AccountId = model.AccountId, amount = model.Amount });
				return response;
			}
		}
		/// <summary>
		/// Gets all the transactions done by customer
		/// </summary>
		/// <param name="CustomerId"></param>
		/// <returns></returns>
		public async Task<HttpResponseMessage> GetTransactions(int CustomerId)
		{
			using (HttpClient client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://40.76.130.186");
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/Json"));
				var response = await client.GetAsync("api/transaction/gettransactions/" + CustomerId);
				return response;
			}
		}
		/// <summary>
		/// Transfers money from one account to another
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<HttpResponseMessage> Transfer(GetTransferViewModel model)
		{
			using (HttpClient client = new HttpClient())
			{

				client.BaseAddress = new Uri("http://40.76.130.186");
				var response = await client.PostAsJsonAsync("api/transaction/transfer", new { Source_AccountId = model.Source_AccountId, Target_AccountId = model.Target_AccountId, amount = model.amount });
				return response;
			}
		}
		/// <summary>
		/// Withdraws money from an account
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<HttpResponseMessage> Withdraw(CreateDepositWithdrawViewModel model)
		{
			using (HttpClient client = new HttpClient())
			{
				client.BaseAddress = new Uri("http://40.76.130.186");
				var response = await client.PostAsJsonAsync("api/transaction/withdraw", new { AccountId = model.AccountId, amount = model.Amount });
				return response;
			}
		}
	}
}
