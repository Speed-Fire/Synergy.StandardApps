using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Calendar.Messages;
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
    public class CalendarVM : ObservableRecipient
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
        }

        #region Commands

        private AsyncRelayCommand? pageLoaded;
        public ICommand PageLoaded => pageLoaded ??
            (pageLoaded = new AsyncRelayCommand(PageLoadedAsync));

        private RelayCommand<string>? changeCalendarEvent;
        public ICommand ChangeCalendarEvent => changeCalendarEvent ??
            (changeCalendarEvent = new RelayCommand<string>(_day =>
            {
                var day = int.Parse(_day);
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
                FillEvents(_events.Data);
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
