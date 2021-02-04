using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RetailClientApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var Log4NetRepository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
			log4net.Config.XmlConfigurator.Configure(Log4NetRepository, new FileInfo("log4net.config"));
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>().ConfigureLogging((hostingContent, logging) =>
					{
						logging.AddLog4Net();
						logging.SetMinimumLevel(LogLevel.Debug);
					});
				});
	}
}
