using Microsoft.Extensions.DependencyInjection;
using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Domain.Calendar;
using Synergy.StandardApps.Domain.Notes;
using Synergy.StandardApps.EntityForms.Alarm;
using Synergy.StandardApps.EntityForms.Alarm.Converters;
using Synergy.StandardApps.EntityForms.Calendar;
using Synergy.StandardApps.EntityForms.Calendar.Converters;
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
                .AddTransient<IConverter<Note, NoteForm>, NoteToNoteFormConverter>()
                .AddTransient<IConverter<AlarmRecord, AlarmForm>, AlarmRecordToAlarmFormConverter>()
                .AddTransient<IConverter<CalendarEvent, CalendarEventForm>, CalendarEventToCalendarEventFormConverter>();

            return services;
        }
    }
}
