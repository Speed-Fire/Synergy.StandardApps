using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Synergy.StandardApps.Background.Messages.Calendar;
using Synergy.StandardApps.Background.Notifications;
using Synergy.StandardApps.DAL.Repositories;
using Synergy.StandardApps.Domain.Calendar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Synergy.StandardApps.Background.Workers
{
    public class BackgroundCalendarService
        :
        BackgroundService,
        IRecipient<AddCalendarEventMessage>,
        IRecipient<DeleteCalendarEventMessage>
    {
        #region Readonly

        private readonly string SERVICE_NAME = nameof(BackgroundCalendarService);

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackgroundCalendarService> _logger;
        private readonly INotifier<CalendarEvent> _notifier;

        private readonly ConcurrentQueue<CalendarEvent> _calendarEventsToAdd;
        private readonly ConcurrentQueue<long> _calendarEventsToDelete;

        private readonly LinkedList<CalendarEvent> _calendarEvents;

        private readonly int _sleepDelay;

        #endregion

        private DateTime _lastDay;

        public BackgroundCalendarService(IServiceProvider serviceProvider,
            ILogger<BackgroundCalendarService> logger,
            INotifier<CalendarEvent> notifier)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _notifier = notifier;

            _calendarEventsToAdd = new();
            _calendarEventsToDelete = new();
            _calendarEvents = new();

            _lastDay = DateTime.Now.AddDays(-1);

#if DEBUG
            _sleepDelay = 1000 * 5;
#else
            _sleepDelay = 1000 * 60 * 60;
#endif

            WeakReferenceMessenger.Default
                .RegisterAll(this);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var date = DateTime.Now;

                if (_lastDay.AddDays(1).Date.Equals(date.Date))
                {
                    await LoadCalendarEventsFromDb(date);

                    _lastDay = date;
                }

                HandleAddedCalendarEvents(date);
                HandleDeletedCalendarEvents();

                if(_calendarEvents.First is null)
                {
                    await Task.Delay(_sleepDelay, stoppingToken);
                    continue;
                }

                var ev = _calendarEvents.First.Value;
                _calendarEvents.Remove(ev);

                _notifier.Notify(ev);

                LogInformation("CalendarEvent notified.");

                if (_calendarEvents.Count > 0 || !_calendarEventsToAdd.IsEmpty)
                    await Task.Delay(5000, stoppingToken);
            }
        }

        #region Methods

        #region DB

        private async Task LoadCalendarEventsFromDb(DateTime today)
        {
            var _calendarEventRepository = _serviceProvider
                .GetRequiredService<IRepository<CalendarEvent>>();

            _calendarEvents.Clear();

            var calEvents = await _calendarEventRepository
                .GetAll()
                .Where(ce => ce.Day == today.Day && ce.Month == today.Month)
                .ToListAsync();

            foreach(var celEvent in calEvents)
            {
                _calendarEvents.AddLast(celEvent);
            }

            LogInformation($"CalendarEvents loaded for {today:dd.MM.yyyy}.");
        }

        #endregion

        #region CalendarEvent queue changes handlers

        #region Added or updated

        private void HandleAddedCalendarEvents(DateTime today)
        {
            while (!_calendarEventsToAdd.IsEmpty)
            {
                if(_calendarEventsToAdd.TryDequeue(out var calendarEvent))
                {
                    var existing = _calendarEvents
                        .FirstOrDefault(ce => ce.Id == calendarEvent.Id);

                    if(existing is null)
                    {
                        AddCalendarEvent(calendarEvent, today);
                    }
                    else
                    {
                        UpdateCalendarEvent(existing, calendarEvent, today);
                    }
                }
            }
        }

        private void AddCalendarEvent(CalendarEvent calendarEvent, DateTime today)
        {
            if (calendarEvent.Day != today.Day || calendarEvent.Month != today.Month)
                return;

            _calendarEvents.AddLast(calendarEvent);

            LogInformation("CalendarEvent added.");
        }

        private void UpdateCalendarEvent(CalendarEvent existing, CalendarEvent calendarEvent, DateTime today)
        {
            if (calendarEvent.Day != today.Day || calendarEvent.Month != today.Month)
            {
                _calendarEventsToDelete.Enqueue(calendarEvent.Id);
                return;
            }

            existing.Title = calendarEvent.Title;
            existing.Description = calendarEvent.Description;

            LogInformation("CalendarEvent updated.");
        }

        #endregion

        #region Deleted

        private void HandleDeletedCalendarEvents()
        {
            while (!_calendarEventsToDelete.IsEmpty)
            {
                if(_calendarEventsToDelete.TryDequeue(out var id))
                {
                    var existing = _calendarEvents
                        .FirstOrDefault(ce => ce.Id == id);

                    if (existing is null)
                        continue;

                    _calendarEvents.Remove(existing);

                    LogInformation("CalendarEvent deleted.");
                }
            }
        }

        #endregion

        #endregion

        private void LogInformation(string info)
        {
            _logger
                .LogInformation("[{service_name}]: {info}", SERVICE_NAME, info);
        }

        #endregion

        #region Messages

        void IRecipient<AddCalendarEventMessage>.Receive(AddCalendarEventMessage message)
        {
            _calendarEventsToAdd.Enqueue(message.Value);
        }

        void IRecipient<DeleteCalendarEventMessage>.Receive(DeleteCalendarEventMessage message)
        {
            _calendarEventsToDelete.Enqueue(message.Value);
        }

        #endregion
    }
}
