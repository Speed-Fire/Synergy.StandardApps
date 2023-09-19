using Microsoft.Extensions.DependencyInjection;
using Synergy.StandardApps.Alarms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Alarms.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection RegisterAlarmsUI(this IServiceCollection services)
        {
            services
                .AddTransient<AlarmsVM>()
                .AddTransient<AlarmsPage>();

            return services;
        }
    }
}
