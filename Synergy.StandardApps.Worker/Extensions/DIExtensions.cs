using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Utility.Converters;
using Synergy.StandardApps.Worker.Utility.Converters;

namespace Synergy.StandardApps.Worker.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection RegisterConverters(this IServiceCollection services)
        {
            services
                .AddTransient<IConverter<AlarmRequest, AlarmRecord>, AlarmRecordConverter>();

            return services;
        }
    }
}
