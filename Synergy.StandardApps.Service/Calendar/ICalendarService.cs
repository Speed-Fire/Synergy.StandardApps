using Synergy.StandardApps.Domain.Responses;
using Synergy.StandardApps.EntityForms.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Service.Calendar
{
    public interface ICalendarService
    {
        Task<IResponse<CalendarEventForm>> CreateEvent(CalendarEventCreationForm form);
        Task<IResponse<CalendarEventForm>> UpdateEvent(CalendarEventCreationForm form, long id);
        Task<IResponse<IEnumerable<CalendarEventForm>>> GetEvents(int month);
        Task<IResponse<bool>> DeleteEvent(long id);
    }
}
