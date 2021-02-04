using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RetailClientApp.Models.ViewModel
{
	public class CreateCustomerSuccessViewModel
	{
		[Display(Name ="Customer ID")]
		public string Message { get; set; }
		public int CustomerId { get; set; }

	}
}
