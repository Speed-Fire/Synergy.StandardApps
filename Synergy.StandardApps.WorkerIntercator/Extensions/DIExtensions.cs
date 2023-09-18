using Microsoft.Extensions.DependencyInjection;
using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Utility.Converters;
using Synergy.StandardApps.Worker;
using Synergy.StandardApps.WorkerInteractor.Converters;
using Synergy.StandardApps.WorkerInteractor.Interactors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.WorkerInteractor.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection RegisterWorkerIntercator(this IServiceCollection services)
        {
            services
                .RegisterIntercatorConverters()
                .RegisterInteractors();

            return services;
        }

        public static IServiceCollection RegisterIntercatorConverters(this IServiceCollection services)
        {
            services
                .AddTransient<IConverter<AlarmRecord, AlarmRequest>, AlarmRecordToRequestConverter>();

            return services;
        }

        public static IServiceCollection RegisterInteractors(this IServiceCollection services)
        {
            services
                .AddTransient<IServiceInteractor<AlarmRecord>, AlarmServiceInteractor>();

            return services;
        }
    }
}
