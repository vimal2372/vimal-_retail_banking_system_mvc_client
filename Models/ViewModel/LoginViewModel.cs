﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailClientApp.Models.ViewModel
{
	public class LoginViewModel
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public bool IsEmployee { get; set; }
	}
}
