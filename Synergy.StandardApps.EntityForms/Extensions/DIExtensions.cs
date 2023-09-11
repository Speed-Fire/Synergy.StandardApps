using Microsoft.Extensions.DependencyInjection;
using Synergy.StandardApps.Domain.Notes;
using Synergy.StandardApps.EntityForms.Notes;
using Synergy.StandardApps.EntityForms.Notes.Converters;
using Synergy.StandardApps.Utility.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.EntityForms.Extensions
{
    public static class DIExtensions
    {
        public static IServiceCollection RegisterEntityFormsConverters(this IServiceCollection services)
        {
            services
                .AddTransient<IConverter<Note, NoteForm>, NoteToNoteFormConverter>();

            return services;
        }
    }
}
