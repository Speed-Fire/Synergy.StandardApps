using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Calendar.Messages;
using Synergy.StandardApps.Calendar.SubPages;
using Synergy.StandardApps.Calendar.ViewModels.ChangeCalendarEventVMs;
using Synergy.StandardApps.EntityForms.Calendar;
using Synergy.StandardApps.Service.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Synergy.StandardApps.Calendar.ViewModels
{
    public class CalendarVM : 
        ObservableRecipient,
        IRecipient<CalendarEventCreatedMessage>,
        IRecipient<CalendarEventUpdatedMessage>,
        IRecipient<CalendarEventDeletedMessage>
    {
        #region Fields

        private readonly ICalendarService _calendarService;

        private DateTime _currentDate;
        private List<CalendarEventForm> _calendarEvents;

        #endregion

        #region Properties

        public DateTime CurrentDate
        {
            get => _currentDate;
            set => SetProperty(ref _currentDate, value);
        }

        public IEnumerable<CalendarEventForm> CalendarEvents => _calendarEvents;

        #endregion

        public CalendarVM(ICalendarService calendarService)
        {
            _calendarService = calendarService;

            IsActive = true;

            _calendarEvents = new();
            CurrentDate = DateTime.Now;
        }

        #region Messages

        void IRecipient<CalendarEventCreatedMessage>.Receive(CalendarEventCreatedMessage message)
        {
            _calendarEvents.Add(message.Value);
        }

        void IRecipient<CalendarEventUpdatedMessage>.Receive(CalendarEventUpdatedMessage message)
        {
            var pos = _calendarEvents.FindIndex(ev => ev.Day == message.Value.Day &&
                ev.Month == message.Value.Month);

            _calendarEvents.RemoveAt(pos);
            _calendarEvents.Insert(pos, message.Value);
        }

        void IRecipient<CalendarEventDeletedMessage>.Receive(CalendarEventDeletedMessage message)
        {
            var pos = _calendarEvents.FindIndex(ev => ev.Day == message.Value);

            _calendarEvents.RemoveAt(pos);
        }

        #endregion

        #region Commands

        private AsyncRelayCommand? pageLoaded;
        public ICommand PageLoaded => pageLoaded ??
            (pageLoaded = new AsyncRelayCommand(PageLoadedAsync));

        private RelayCommand<int>? changeCalendarEvent;
        public ICommand ChangeCalendarEvent => changeCalendarEvent ??
            (changeCalendarEvent = new RelayCommand<int>(day =>
            {
                var ev = CalendarEvents.FirstOrDefault(e => e.Day == day);

                if(ev is null)
                {
                    WeakReferenceMessenger.Default
                        .Send(new CalendarNavigateMessage(new ChangeCalendarEventPage(new CreateCalendarEventVM(_calendarService, day, CurrentDate.Month))));
                }
                else
                {
                    WeakReferenceMessenger.Default
                        .Send(new CalendarNavigateMessage(new ChangeCalendarEventPage(new UpdateCalendarEventVM(_calendarService, ev))));
                }
            }));

        private RelayCommand<int>? openCalendarEvent;
        public ICommand OpenCalendarEvent => openCalendarEvent ??
            (openCalendarEvent = new RelayCommand<int>(day =>
            {
                var ev = _calendarEvents.FirstOrDefault(e => e.Day == day);

                if (ev is null)
                    return;

                WeakReferenceMessenger.Default
                    .Send(new OpenCalendarEventMessage(ev));
            }));

        private AsyncRelayCommand? loadNextMonth;
        public ICommand LoadNextMonth => loadNextMonth ??
            (loadNextMonth = new AsyncRelayCommand(LoadNextMonthAsync));

        private AsyncRelayCommand? loadPreviousMonth;
        public ICommand LoadPreviousMonth => loadPreviousMonth ??
            (loadPreviousMonth = new AsyncRelayCommand(LoadPreviousMonthAsync));


        #region Async methods

        private async Task LoadNextMonthAsync()
        {
            CurrentDate = CurrentDate.AddMonths(1);

            await LoadEvents();
        }

        private async Task LoadPreviousMonthAsync()
        {
            CurrentDate = CurrentDate.AddMonths(-1);

            await LoadEvents();
        }

        private async Task PageLoadedAsync()
        {
            await LoadEvents();
        }

        #endregion

        #endregion

        #region Methods

        private async Task LoadEvents()
        {
            var _events = await _calendarService.GetEvents(CurrentDate.Month);

            if (_events.StatusCode == Domain.Enums.StatusCode.OK)
                FillEvents(_events.Data?.OrderBy(x => x.Day));
            else
                FillEvents(new List<CalendarEventForm>());

            WeakReferenceMessenger.Default
                .Send(new MonthLoadedMessage(null));
        }

        private void FillEvents(IEnumerable<CalendarEventForm> events)
        {
            _calendarEvents.Clear();

            foreach(var ev in events)
            {
                _calendarEvents.Add(ev);
            }
        }

        #endregion
    }
}
