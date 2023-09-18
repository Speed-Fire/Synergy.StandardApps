using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Synergy.StandardApps.DAL.DbContexts;
using Synergy.StandardApps.DAL.Extensions;
using Synergy.StandardApps.EntityForms.Extensions;
using Synergy.StandardApps.Notes.Extensions;
using Synergy.StandardApps.Service.Extensions;
using Synergy.StandardApps.WorkerInteractor;
using Synergy.StandardApps.WorkerInteractor.Extensions;
using Synergy.WPF.Common.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps
{
    public static class Program
    {
        public static IHost AppHost;

        [STAThread]
        public static void Main(params string[] args)
        {
            // Windows service setup
            try
            {
                StandardAppsServiceSetuper.Setup();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);

                return;
            }


            // App building
            var builder = Host.CreateDefaultBuilder(args);

            builder.ConfigureServices(services =>
            {
                var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Synergy");
                Directory.CreateDirectory(dir);
                dir = Path.Combine(dir, "StandardApps");
                Directory.CreateDirectory(dir);
                dir = Path.Combine(dir, "standardapps.sqlite");

                var connStr = $"Data Source={dir};";
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlite(connStr);
                });

                services
                    .RegisterSynergyWPFCommon()
                    .RegisterRepositories()
                    .RegisterEntityFormsConverters()
                    .RegisterNoteServies()
                    .RegisterNotes()
                    .RegisterWorkerIntercator();

                services.AddSingleton<App>();
                services.AddSingleton<MainWindow>();
            });

            AppHost = builder.Build();

            var app = AppHost.Services.GetRequiredService<App>();
            app.Run();
        }
    }
}
