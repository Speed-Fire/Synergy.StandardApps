using Microsoft.Extensions.DependencyInjection;
using Synergy.StandardApps.Background.Notifications;
using Synergy.StandardApps.Background.Workers;
using Synergy.StandardApps.Domain.Alarm;
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
                .AddTransient<INotifier<AlarmRecord>, AlarmToastNotifier>();

            services
                .RegisterAlarmBackground();

            return services;
        }

        public static IServiceCollection RegisterAlarmBackground(this IServiceCollection services)
        {
            services
                .AddHostedService<BackgroundAlarmService>();

            return services;
        }
    }
}
