using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Calendar.Messages;
using Synergy.StandardApps.EntityForms.Calendar;
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
        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get => _currentDate;
            set => SetProperty(ref _currentDate, value);
        }

        private List<CalendarEventForm> _calendarEvents;
        public IEnumerable<CalendarEventForm> CalendarEvents => _calendarEvents;

        public CalendarVM()
        {
            IsActive = true;

            _calendarEvents = new();
        }

        #region Commands

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
            WeakReferenceMessenger.Default
                .Send(new MonthLoadedMessage(null));
        }

        private async Task LoadPreviousMonthAsync()
        {
            WeakReferenceMessenger.Default
                .Send(new MonthLoadedMessage(null));
        }

        #endregion

        #endregion
    }
}
