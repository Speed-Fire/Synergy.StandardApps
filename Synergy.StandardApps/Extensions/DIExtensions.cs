using Microsoft.Extensions.DependencyInjection;
using Synergy.StandardApps.Alarms.Extensions;
using Synergy.StandardApps.Calendar.Extensions;
using Synergy.StandardApps.Notes.Extensions;
using Synergy.StandardApps.ViewModels;
using Synergy.WPF.Common.Extensions;
using Synergy.WPF.Navigation.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection RegisterStandardAppsUI(this IServiceCollection services)
        {
            services
                .RegisterCommonUILibraries()
                .RegisterAlarmsUI()
                .RegisterCalendarUI()
                .RegisterNotes();

            services.AddSingleton<App>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainVM>();

            return services;
        }

        public static IServiceCollection RegisterCommonUILibraries(this IServiceCollection services)
        {
            services
                .RegisterSynergyWPFCommon()
                .RegisterNavigation();

            return services;
        }
    }
}
