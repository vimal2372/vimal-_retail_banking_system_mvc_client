using RetailClientApp.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailClientApp.Provider
{
	public interface ITransactionProvider
	{
		Task<HttpResponseMessage> Withdraw(CreateDepositWithdrawViewModel model);
		Task<HttpResponseMessage> Deposit(CreateDepositWithdrawViewModel model);
		Task<HttpResponseMessage> Transfer(GetTransferViewModel model);
		Task<HttpResponseMessage> GetTransactions(int CustomerId);
	}
}
