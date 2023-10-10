using Microsoft.Extensions.DependencyInjection;
using Synergy.StandardApps.Background.Notifications;
using Synergy.StandardApps.Background.Workers;
using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Domain.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Background.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection RegisterBackground(this IServiceCollection services)
        {
            services
                .RegisterAlarmBackground()
                .RegisterCalendarBackground()
                .RegisterNoteCleanerBackground();

            return services;
        }

        public static IServiceCollection RegisterAlarmBackground(this IServiceCollection services)
        {
            services
                .AddHostedService<BackgroundAlarmService>()
                .AddTransient<INotifier<AlarmRecord>, AlarmToastNotifier>();

            return services;
        }

        public static IServiceCollection RegisterCalendarBackground(this IServiceCollection services)
        {
            services
                .AddHostedService<BackgroundCalendarService>()
                .AddTransient<INotifier<CalendarEvent>, CalendarEventToastNotifier>();

            return services;
        }

        public static IServiceCollection RegisterNoteCleanerBackground(this  IServiceCollection services)
        {
            services
                .AddHostedService<BackgroundNoteCleanerService>();

            return services;
        }
    }
}
