using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.EntityFrameworkCore;
using Synergy.StandardApps.DAL.DbContexts;
using Synergy.StandardApps.DAL.Extensions;
using Synergy.StandardApps.Worker.Extensions;
using Synergy.StandardApps.Worker.Services;
using Synergy.StandardApps.Worker.Workers;
using Microsoft.Toolkit.Uwp.Notifications;

namespace Synergy.StandardApps.Worker
{
    public class Program
    {
        internal static WebApplication App { get; private set; }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                ContentRootPath = WindowsServiceHelpers.IsWindowsService() ?
                    AppContext.BaseDirectory : default
            });

            // Additional configuration is required to successfully run gRPC on macOS.
            // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

            builder.Host.UseWindowsService(options =>
            {
                options.ServiceName = "Synergy.StandardApps.Service";
            });

            // Add services to the container.
            builder.Services.AddGrpc();

            // Add reflection services
            builder.Services.AddGrpcReflection();

            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Synergy");
            Directory.CreateDirectory(dir);
            dir = Path.Combine(dir, "StandardApps");
            Directory.CreateDirectory(dir);
            dir = Path.Combine(dir, "standardapps.sqlite");

            var conStr = $"Data Source={dir};";
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(conStr);
            });

            builder.Services
                .RegisterConverters()
                .RegisterRepositories()
                .RegisterNotifiers();

            builder.Services.AddHostedService<BackgroundAlarmService>();

            App = builder.Build();

            // Configure the HTTP request pipeline.
            App.MapGrpcService<AlarmAdderService>();
            App.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            App.MapGrpcReflectionService();

            App.Run();

            ToastNotificationManagerCompat.History.Clear();
        }
    }
}