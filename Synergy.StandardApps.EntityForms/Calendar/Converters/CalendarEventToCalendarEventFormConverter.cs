using Synergy.StandardApps.Domain.Calendar;
using Synergy.StandardApps.Utility.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.EntityForms.Calendar.Converters
{
    internal class CalendarEventToCalendarEventFormConverter : IConverter<CalendarEvent, CalendarEventForm>
    {
        public CalendarEventForm Convert(CalendarEvent entity)
        {
            return new()
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Color = entity.Color,
                Month = entity.Month,
                Day = entity.Day,
            };
        }
    }
}
