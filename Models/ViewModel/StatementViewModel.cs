using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RetailClientApp.Models.ViewModel
{
	public class StatementViewModel
	{
		[Display(Name ="Statement ID")]
		public int StatementId { get; set; }
		[Display(Name ="Account ID")]
		public int AccountId { get; set; }
		[Display(Name = "Date")]
		public DateTime date { get; set; }
		[Display(Name = "Chq/RefNo")]
		public string refno { get; set; }
		[Display(Name ="Value Date")]
		public DateTime ValueDate { get; set; }
		[Display(Name ="Withdraw")]
		public int Withdrawal { get; set; }
		public int Deposit { get; set; }
		[Display(Name ="Closing Balance")]
		public int ClosingBalance { get; set; }
	}
}
