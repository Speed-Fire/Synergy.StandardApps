using Microsoft.Extensions.DependencyInjection;
using Synergy.StandardApps.Notes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Notes.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection RegisterNotes(this IServiceCollection services)
        {
            services
                .AddTransient<NotesVM>()
                .AddTransient<NotesPage>();

            return services;
        }
    }
}
