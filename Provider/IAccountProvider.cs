using RetailClientApp.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailClientApp.Provider
{
	public interface IAccountProvider
	{
		Task<HttpResponseMessage> getCustomerAccounts(int customerId);
		Task<HttpResponseMessage> getAccount(int AccountId);
		Task<HttpResponseMessage> GetAccountStatement(GetAccountStatementViewModel model);

	}
}
