using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Synergy.StandardApps.Background.Extensions;
using Synergy.StandardApps.DAL.Extensions;
using Synergy.StandardApps.EntityForms.Extensions;
using Synergy.StandardApps.Service.Alarm;
using Synergy.StandardApps.Service.Calendar;
using Synergy.StandardApps.Service.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Service.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection RegisterStandardAppsServices(this IServiceCollection services)
        {
            services
                .RegisterNoteServices()
                .RegisterCalendarServices()
                .RegisterAlarmServices()
                .RegisterBackground()
                .RegisterEntityFormsConverters()
                .RegisterRepositories();

            return services;
        }

        public static IServiceCollection RegisterNoteServices(this IServiceCollection services)
        {
            services
                .AddTransient<INoteService, NoteService>();

            return services;
        }

        public static IServiceCollection RegisterAlarmServices(this IServiceCollection services)
        {
            services
                .AddTransient<IAlarmService, AlarmService>();

            return services;
        }

        public static IServiceCollection RegisterCalendarServices(this IServiceCollection services)
        {
            services
                .AddTransient<ICalendarService, CalendarService>();

            return services;
        }
    }
}
