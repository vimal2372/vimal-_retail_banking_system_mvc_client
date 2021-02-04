using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RetailClientApp.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using RetailClientApp.Provider;

namespace RetailClientApp.Controllers
{
	public class AccountController : Controller
	{
		private log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(AccountController));
		private readonly IAccountProvider _accountProvider;
		private readonly ITransactionProvider _transactionProvider;
		public AccountController(IAccountProvider accountProvider,ITransactionProvider transactionProvider)
		{
			this._accountProvider = accountProvider;
			this._transactionProvider = transactionProvider;
		}
		/// <summary>
		/// Id is customer id
		/// </summary>
		/// <param name="id"></param>
		/// <returns>It will return list of account associated with customer id</returns>
		public async Task<IActionResult> GetCustomerAccount(int id)
		{
			if (HttpContext.Session.GetString("IsEmployee") == null)
			{
				return RedirectToAction("Login", "Authentication");
			}
			else
			{
				List<AccountViewModel> accountViews = new List<AccountViewModel>();
				try 
				{
					var response = await _accountProvider.getCustomerAccounts(id);
					if (response.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var JsonContent = await response.Content.ReadAsStringAsync();
						accountViews = JsonConvert.DeserializeObject<List<AccountViewModel>>(JsonContent);
						return View(accountViews);
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
					{
						ViewBag.Message = "Invalid Customer ID";
						return View(accountViews);
					}
					else if(response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
					{
						ViewBag.Message = "Internal Server Error! Please try again later";
						return View(accountViews);
					}
				}
				catch(Exception ex)
				{
					_logger.Warn("Exceptions Occured as " + ex.Message);
				}
				return View(accountViews);
			}
		}
		/// <summary>
		/// Here id is account id
		/// </summary>
		/// <param name="id"></param>
		/// <returns>AccountViewModel which contains two properties id and balance</returns>
		[HttpGet]
		public async Task<IActionResult> GetAccount(int id)
		{
			if (HttpContext.Session.GetString("IsEmployee") == null)
			{
				return RedirectToAction("Login", "Authentication");
			}
			else
			{
				AccountViewModel model = new AccountViewModel();
				try
				{
					var response = await _accountProvider.getAccount(id);
					if (response.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var JsonContent = await response.Content.ReadAsStringAsync();
						model = JsonConvert.DeserializeObject<AccountViewModel>(JsonContent);
						return View(model);
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
					{
						ViewBag.Message = "Having server issue while adding record";
						return View(model);
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
					{
						ViewBag.Message = "Internal Server Error! Please try again later";
						return View(model);
					}
				}
				catch(Exception ex)
				{
					_logger.Error("Exceptions Occured as " + ex.Message);
				}
				return View(model);
			}
		}

		/// <summary>
		/// Here id is account ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns>IT will return account statement within given date range</returns>
		[HttpGet]
		public IActionResult GetAccountStatementView(int id)
		{
			if (HttpContext.Session.GetString("IsEmployee") == null)
			{
				return RedirectToAction("Login", "Authentication");
			}
			else
			{
				GetAccountStatementViewModel model = new GetAccountStatementViewModel() { Id = id };
				return View(model);
			}
		}

		/// <summary>
		/// It takes GetAccountStatementViewModel and gets the account statement from Account API
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> GetAccountStatement(GetAccountStatementViewModel model)
		{
			if (HttpContext.Session.GetString("IsEmployee") == null)
			{
				return RedirectToAction("Login", "Authentication");
			}
			else
			{
				List<StatementViewModel> statementViews = new List<StatementViewModel>();
				try
				{
					var response = await _accountProvider.GetAccountStatement(model);
					if (response.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var JsonContent = await response.Content.ReadAsStringAsync();
						statementViews = JsonConvert.DeserializeObject<List<StatementViewModel>>(JsonContent);
						return View(statementViews);
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
					{
						ViewBag.Message = "Having server issue while adding record";
						return View(statementViews);
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
					{
						ViewBag.Message = "Internal Server Error! Please try again later";
						return View(statementViews);
					}
				}
				catch(Exception ex)
				{
					_logger.Error("Exceptions Occured as " + ex.Message);
				}
				return View(statementViews);
			}
		}

		/// <summary>
		/// Here Id is account ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns>returns view where user can enter amount to be deposit</returns>
		[HttpGet]
		public IActionResult Deposit(int id)
		{
			if (HttpContext.Session.GetString("IsEmployee") == null)
			{
				return RedirectToAction("Login", "Authentication");
			}
			else
			{
				CreateDepositWithdrawViewModel model = new CreateDepositWithdrawViewModel() { AccountId = id };
				return View(model);
			}
		}
		[HttpPost,ValidateAntiForgeryToken]
		public async Task<IActionResult> Deposit(CreateDepositWithdrawViewModel model)
		{
			if(!ModelState.IsValid)
				return View(model);

			TransactionStatusViewModel transactionStatus = new TransactionStatusViewModel();
			try 
			{
				var response = await _transactionProvider.Deposit(model);
				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var jsoncontent = await response.Content.ReadAsStringAsync();
					transactionStatus = JsonConvert.DeserializeObject<TransactionStatusViewModel>(jsoncontent);
					return View("TransactionStatus", transactionStatus);
				}
				else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
				{
					ModelState.AddModelError("", "Having server issue while adding record");
					return View(model);
				}
				else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				{
					ViewBag.Message = "Internal Server Error! Please try again later";
					return View(model);
				}
			}
			catch(Exception ex)
			{
				_logger.Error("Exceptions Occured as " + ex.Message);
			}
			ModelState.AddModelError("", "Having some unexpected error while processing transaction");
			return View(model);
		}

		/// <summary>
		/// Here id is Account ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns>returns view where user can enter amount to be withdrawn from given account no</returns>
		[HttpGet]
		public IActionResult Withdraw(int id)
		{
			if (HttpContext.Session.GetString("IsEmployee") == null)
			{
				return RedirectToAction("Login", "Authentication");
			}
			else
			{
				CreateDepositWithdrawViewModel model = new CreateDepositWithdrawViewModel() { AccountId = id };
				return View(model);
			}
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Withdraw(CreateDepositWithdrawViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);
			TransactionStatusViewModel transactionStatus = new TransactionStatusViewModel();
			try
			{
				var response = await _transactionProvider.Withdraw(model);
				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var jsoncontent = await response.Content.ReadAsStringAsync();
					transactionStatus = JsonConvert.DeserializeObject<TransactionStatusViewModel>(jsoncontent);
					return View("TransactionStatus", transactionStatus);
				}
				else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
				{
					ModelState.AddModelError("", "Transaction Denied,because of overdraft amount!");
					return View(model);
				}
				else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				{
					ViewBag.Message = "Internal Server Error! Please try again later";
					return View(model);
				}
			}
			catch(Exception ex)
			{
				_logger.Error("Exceptions Occured as " + ex.Message);
			}
			ModelState.AddModelError("", "Having some unexpected error while processing transaction");
			return View(model);
		}
		
		/// <summary>
		/// Takes no paramerter
		/// </summary>
		/// <returns>Return transaction view weather it is Deposit/Withdraw</returns>
		/// 
		[HttpGet]
		public IActionResult TransferView(int id)
		{
			if (HttpContext.Session.GetString("IsEmployee") == null)
			{
				return RedirectToAction("Login", "Authentication");
			}
			else
			{
				GetTransferViewModel model = new GetTransferViewModel()
				{
					Source_AccountId = id
				};
				return View(model);
			}
		}

		/// <summary>
		/// It takes TransferViewModel for the transfer of money from one account to another
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost,ValidateAntiForgeryToken]
		public async Task<IActionResult> TransferView(GetTransferViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			TransactionStatusViewModel transactionStatus = new TransactionStatusViewModel();
			try
			{
				var response = await _transactionProvider.Transfer(model);
				if (response.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var jsoncontent = await response.Content.ReadAsStringAsync();
					transactionStatus = JsonConvert.DeserializeObject<TransactionStatusViewModel>(jsoncontent);
					return View("TransactionStatus", transactionStatus);
				}
				else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
				{
					ModelState.AddModelError("", "Transaction Denied,because of overdraft amount!");
					return View("TransferView",model);
				}
				else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				{
					ViewBag.Message = "Internal Server Error! Please try again later";
					return View("TransferView",model);
				}
			}
			catch(Exception ex)
			{
				_logger.Error("Exceptions Occured as " + ex.Message);
			}
			ModelState.AddModelError("", "Having some unexpected error while processing transaction");
			return View(model);
		}

		/// <summary>
		/// it gives transaction details of the customer by getting it's ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> GetTransactions(int id)
		{
			if (HttpContext.Session.GetString("IsEmployee") == null)
			{
				return RedirectToAction("Login", "Authentication");
			}
			else
			{
				List<TransactionHistoryViewModel> model = new List<TransactionHistoryViewModel>();
				try
				{
					var response = await _transactionProvider.GetTransactions(id);
					if (response.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var JsonContent = await response.Content.ReadAsStringAsync();
						model = JsonConvert.DeserializeObject<List<TransactionHistoryViewModel>>(JsonContent);
						return View(model);
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
					{
						ViewBag.Message = "Having server issue while adding record";
						return View(model);
					}
					else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
					{
						ViewBag.Message = "Internal Server Error! Please try again later";
						return View(model);
					}
				}
				catch(Exception ex)
				{
					_logger.Error("Exceptions Occured as " + ex.Message);
				}
				return View(model);
			}
		}
		/// <summary>
		/// Shows the transaction status after every transaction done
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult TransactionStatus()
		{
			TransactionStatusViewModel model = new TransactionStatusViewModel();
			return View(model);
		}
	}
}
