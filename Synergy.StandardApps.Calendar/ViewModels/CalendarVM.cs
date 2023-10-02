using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Calendar.Messages;
using Synergy.StandardApps.Calendar.ViewModels.CalendarEvent;
using Synergy.StandardApps.EntityForms.Calendar;
using Synergy.StandardApps.Service.Calendar;
using Synergy.WPF.Navigation.Services;
using Synergy.WPF.Navigation.Services.Local;
using Synergy.WPF.Navigation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Synergy.StandardApps.Calendar.ViewModels
{
    public class CalendarVM : 
        ViewModel,
        IRecipient<CalendarEventCreatedMessage>,
        IRecipient<CalendarEventUpdatedMessage>,
        IRecipient<CalendarEventDeletedMessage>,
        IRecipient<CloseCalendarEventChangingMessage>,
        IRecipient<RightSidePanelClosedMessage>
    {
        #region Fields

        private readonly ICalendarService _calendarService;
        private readonly INavigationService _navigationService;
        private ILocalNavigationService _localNavigationService;

        private DateTime _currentDate;
        private readonly List<CalendarEventForm> _calendarEvents;

        #endregion

        #region Properties

        public ILocalNavigationService LocalNavigationService
        {
            get => _localNavigationService;
            set => SetProperty(ref _localNavigationService, value);
        }

        public DateTime CurrentDate
        {
            get => _currentDate;
            set => SetProperty(ref _currentDate, value);
        }

        #endregion

        #region Constructor

        public CalendarVM(ICalendarService calendarService,
            INavigationService navigationService,
            ILocalNavigationService localNavigationService)
        {
            _calendarService = calendarService;
            _navigationService = navigationService;
            _localNavigationService = localNavigationService;

            IsActive = true;

            _calendarEvents = new();
        }

        #endregion

        #region Messages

        #region CalendarEvent CRUD

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

        void IRecipient<CloseCalendarEventChangingMessage>.Receive(CloseCalendarEventChangingMessage message)
        {
            WeakReferenceMessenger.Default
                .Send(new SetRightSidePanelVisibilityMessage(false));
        }

        void IRecipient<RightSidePanelClosedMessage>.Receive(RightSidePanelClosedMessage message)
        {
            if (_localNavigationService.CurrentView is null ||
                _localNavigationService.CurrentView.GetType() != typeof(CalendarEventVM))
            {
                _localNavigationService.NavigateTo<CalendarEventVM>();
            }

            WeakReferenceMessenger.Default
                    .Send(new OpenCalendarEventMessage(null));
        }

        #endregion

        #region Commands

        private AsyncRelayCommand? viewLoaded;
        public ICommand ViewLoaded => viewLoaded ??
            (viewLoaded = new AsyncRelayCommand(ViewLoadedAsync));

        private RelayCommand<int>? changeCalendarEvent;
        public ICommand ChangeCalendarEvent => changeCalendarEvent ??
            (changeCalendarEvent = new RelayCommand<int>(day =>
            {
                var ev = _calendarEvents.FirstOrDefault(e => e.Day == day);

                if(ev is null)
                {
                    _localNavigationService
                        .NavigateTo<CreateCalendarEventVM>(_calendarService, day, CurrentDate.Month);
                }
                else
                {
                    _localNavigationService
                        .NavigateTo<UpdateCalendarEventVM>(_calendarService, ev);
                }

                WeakReferenceMessenger.Default
                    .Send(new SetRightSidePanelVisibilityMessage(true));
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

                WeakReferenceMessenger.Default
                    .Send(new SetRightSidePanelVisibilityMessage(true));
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

        private async Task ViewLoadedAsync()
        {
            CurrentDate = DateTime.Now;

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
                .Send(new MonthLoadedMessage(Tuple
                    .Create((IEnumerable<CalendarEventForm>)_calendarEvents, CurrentDate)));
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
