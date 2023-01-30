using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using API.RulesEngine.Logging;

namespace Api.RulesEngine
{
	public class Program
	{
		public static void Main(string[] args)
		{

            Logger.CreateSerilogConfiguration();


            try
			{
				Log.Information("Starting up");
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Application Start-up failed");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
				Host.CreateDefaultBuilder(args)
						.UseServiceProviderFactory(new AutofacServiceProviderFactory())
						.UseSerilog()
						.ConfigureWebHostDefaults(webBuilder =>
						{
							webBuilder.UseStartup<Startup>();
						});
	}
}
