using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RetailClientApp.Models.ViewModel
{
	public class GetTransferViewModel
	{
		[Display(Name ="Target Account"),Required]
		public int Target_AccountId { get; set; }
		[Display(Name = "Soucrce Account"),Required]
		public int Source_AccountId { get; set; }
		[Display(Name = "Amount"),Required,Range(1,10000)]
		public int amount { get; set; }
	}
}
