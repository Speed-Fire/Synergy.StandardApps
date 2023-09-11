using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        public static IServiceCollection RegisterNoteServies(this IServiceCollection services)
        {
            services
                .AddTransient<INoteService, NoteService>();

            return services;
        }
    }
}
