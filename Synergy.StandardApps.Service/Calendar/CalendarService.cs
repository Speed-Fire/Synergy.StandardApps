using Microsoft.EntityFrameworkCore;
using Synergy.StandardApps.DAL.Repositories;
using Synergy.StandardApps.Domain.Calendar;
using Synergy.StandardApps.Domain.Responses;
using Synergy.StandardApps.EntityForms.Calendar;
using Synergy.StandardApps.Utility.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Service.Calendar
{
    internal class CalendarService : ICalendarService
    {
        private readonly IRepository<CalendarEvent> _calendarRepository;
        private readonly IConverter<CalendarEvent, CalendarEventForm> _converter;

        public CalendarService(IRepository<CalendarEvent> calendarRepository,
            IConverter<CalendarEvent, CalendarEventForm> converter)
        {
            _calendarRepository = calendarRepository;
            _converter = converter;
        }

        public async Task<IResponse<CalendarEventForm>> CreateEvent(CalendarEventCreationForm form)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<CalendarEventForm>> UpdateEvent(CalendarEventCreationForm form, long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<IEnumerable<CalendarEventForm>>> GetEvents(int month)
        {
            try
            {
                var events = await _calendarRepository
                    .GetAll()
                    .Where(e => e.Month == month)
                    .Select(e => _converter.Convert(e))
                    .ToListAsync();

                return ResponseFactory.OK<IEnumerable<CalendarEventForm>>(events);
            }
            catch (Exception ex)
            {
                return ResponseFactory.BadResponse<IEnumerable<CalendarEventForm>>(ex);
            }
        }

        public async Task<IResponse<bool>> DeleteEvent(long id)
        {
            throw new NotImplementedException();
        }
    }
}
