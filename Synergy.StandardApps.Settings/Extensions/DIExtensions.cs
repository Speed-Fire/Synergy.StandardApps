using Microsoft.Extensions.DependencyInjection;
using Synergy.StandardApps.Settings.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Settings.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection RegisterSettingsUI(this IServiceCollection services)
        {
            services
                .AddSingleton<SettingsVM>();

            return services;
        }
    }
}
