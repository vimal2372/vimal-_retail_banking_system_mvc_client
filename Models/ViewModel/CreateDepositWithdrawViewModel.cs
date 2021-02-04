using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RetailClientApp.Models.ViewModel
{
	public class CreateDepositWithdrawViewModel
	{
		public int AccountId { get; set; }
		[Display(Name ="Amount"),Required,Range(1,10000)]
		public double Amount { get; set; }
	}
}
