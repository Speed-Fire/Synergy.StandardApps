using Microsoft.Extensions.DependencyInjection;
using Synergy.StandardApps.Calendar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Calendar.Extensions
{
    public static class DIExtensions
    {

        public static IServiceCollection RegisterCalendarUI(this IServiceCollection services)
        {
            services
                .AddTransient<CalendarVM>()
                .AddTransient<CalendarPage>();

            return services;
        }
    }
}
