using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.EntityFrameworkCore;
using Synergy.StandardApps.DAL.DbContexts;
using Synergy.StandardApps.DAL.Extensions;
using Synergy.StandardApps.Worker.Extensions;
using Synergy.StandardApps.Worker.Services;
using Synergy.StandardApps.Worker.Workers;

namespace Synergy.StandardApps.Worker
{
    public class Program
    {
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

            builder.Host.UseWindowsService();

            // Add services to the container.
            builder.Services.AddGrpc();

            // Add reflection services
            builder.Services.AddGrpcReflection();


            var conStr = "Data Source=";
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(conStr);
            });

            builder.Services
                .RegisterConverters()
                .RegisterRepositories();

            builder.Services.AddHostedService<BackgroundAlarmService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<AlarmAdderService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.MapGrpcReflectionService();

            app.Run();
        }
    }
}