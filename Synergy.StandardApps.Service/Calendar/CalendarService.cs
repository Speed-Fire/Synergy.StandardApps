using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Synergy.StandardApps.Background.Messages.Calendar;
using Synergy.StandardApps.DAL.Repositories;
using Synergy.StandardApps.Domain.Calendar;
using Synergy.StandardApps.Domain.Notes;
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
            try
            {
                if (form.HasErrors)
                    throw new("Invalid form!");

                var existing = await _calendarRepository
                    .GetAll()
                    .Where(e => e.Day == form.Day && e.Month == form.Month)
                    .FirstOrDefaultAsync();

                if (existing != null)
                    throw new($"Calendar event for date {form.Day}.{form.Month}.yyyy already exist!");

                var ev = new CalendarEvent()
                {
                    Title = form.Title,
                    Description = form.Description,
                    Day = form.Day,
                    Month = form.Month,
                    Color = form.ColorNum
                };

                await _calendarRepository.Create(ev);

                WeakReferenceMessenger.Default
                    .Send(new AddCalendarEventMessage(ev));

                return ResponseFactory.OK(_converter.Convert(ev));
            }
            catch (Exception ex)
            {
                return ResponseFactory.BadResponse<CalendarEventForm>(ex);
            }
        }

        public async Task<IResponse<CalendarEventForm>> UpdateEvent(CalendarEventCreationForm form, long id)
        {
            try
            {
                if (form.HasErrors)
                    throw new("Invalid form!");

                var ev = await _calendarRepository
                    .GetAll()
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (ev is null)
                    throw new("Invalid id!");

                ev.Title = form.Title;
                ev.Description = form.Description;
                ev.Day = form.Day;
                ev.Month = form.Month;
                ev.Color = form.ColorNum;

                ev = await _calendarRepository.Update(ev);

                WeakReferenceMessenger.Default
                    .Send(new AddCalendarEventMessage(ev));

                return ResponseFactory.OK(_converter.Convert(ev));
            }
            catch (Exception ex)
            {
                return ResponseFactory.BadResponse<CalendarEventForm>(ex);
            }
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
            try
            {
                var ev = await _calendarRepository
                    .GetAll()
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (ev is null)
                {
                    return ResponseFactory.BadResponse<bool>(Domain.Enums.ErrorCode.NotFound);
                }

                await _calendarRepository.Delete(ev);

                WeakReferenceMessenger.Default
                    .Send(new DeleteCalendarEventMessage(ev.Id));

                return ResponseFactory.OK(true);
            }
            catch (Exception ex)
            {
                return ResponseFactory.BadResponse<bool>(ex);
            }
        }
    }
}
