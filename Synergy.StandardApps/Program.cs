using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Synergy.StandardApps.DAL.DbContexts;
using Synergy.StandardApps.DAL.Extensions;
using Synergy.StandardApps.EntityForms.Extensions;
using Synergy.StandardApps.Notes.Extensions;
using Synergy.StandardApps.Service.Extensions;
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
            var builder = Host.CreateDefaultBuilder(args);

            builder.ConfigureServices(services =>
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                path = Path.Combine(path, "Synergy", "StandardApps", "standardAppsDb.sqlite");

                var connStr = $"Data Source={path};";

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlite(connStr);
                });

                services
                    .RegisterSynergyWPFCommon()
                    .RegisterRepositories()
                    .RegisterEntityFormsConverters()
                    .RegisterNoteServies()
                    .RegisterNotes();

                services.AddSingleton<App>();
                services.AddSingleton<MainWindow>();
            });

            AppHost = builder.Build();

            var app = AppHost.Services.GetRequiredService<App>();
            app.Run();
        }
    }
}
