using Microsoft.Extensions.DependencyInjection;
using Synergy.StandardApps.DAL.Repositories;
using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Domain.Calendar;
using Synergy.StandardApps.Domain.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.DAL.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services
                .AddScoped<IRepository<Note>, NotesRepository>()
                .AddScoped<IRepository<CalendarEvent>, CalendarRepository>()
                .AddScoped<IRepository<AlarmRecord>, AlarmRepository>();

            return services;
        }
    }
}
