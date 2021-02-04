using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RetailClientApp.Models.ViewModel
{
	public class GetAccountStatementViewModel
	{
		public int Id { get; set; }
		[Display(Name="From Date")]
		public DateTime StartDate { get; set; }
		[Display(Name ="To Date")]
		public DateTime EndDate { get; set; }
	}

}
