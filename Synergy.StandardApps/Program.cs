using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Synergy.StandardApps.Background.Extensions;
using Synergy.StandardApps.DAL.DbContexts;
using Synergy.StandardApps.Extensions;
using Synergy.StandardApps.Service.Extensions;
using Synergy.StandardApps.Settings;
using Synergy.StandardApps.Utility.Misc;
using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace Synergy.StandardApps
{
    public static class Program
    {
        public static IHost AppHost;

        [STAThread]
        public static void Main(params string[] args)
		{
			string appGuid = "6E0C4219-4E85-433E-AA53-2616D3601EAD";

			using var mutex = new Mutex(false, @"Global\" + appGuid);

			if (!mutex.WaitOne(0, false))
			{
				MessageBox.Show("Instance is already running!");
				return;
			}

			StartApp(args);
		}

		private static void StartApp(string[] args)
		{
			// App building
			var builder = Host.CreateDefaultBuilder(args);
			
			builder.ConfigureServices(services =>
			{
				var path = Properties.GetPropertiesDirectory("standardapps.sqlite");

				var connStr = $"Data Source={path};";
				services.AddDbContext<AppDbContext>(options =>
				{
					options.UseSqlite(connStr);
				});

				services
					.RegisterStandardAppsServices()
					.RegisterStandardAppsUI();

			});

			AppHost = builder.Build();

			AppHost.RunAsync();
			var app = AppHost.Services.GetRequiredService<App>();
			app.Run();

			var cleaner = new Cleaner();
			cleaner.CleanNotifications();
		}
	}
}
