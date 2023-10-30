using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Calendar.Messages;
using Synergy.StandardApps.EntityForms.Calendar;
using Synergy.StandardApps.Service.Calendar;
using Synergy.WPF.Navigation.Services.Local;
using Synergy.WPF.Navigation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Synergy.StandardApps.Calendar.ViewModels.CalendarEvent
{
    public abstract class ChangeCalendarEventVM : ViewModel
    {
        protected readonly ICalendarService _calendarService;
        protected long _id;

        #region Properties

        private CalendarEventCreationForm? form;
        public CalendarEventCreationForm Form
        {
            get => form;
            set => SetProperty(ref form, value);
        }

        private bool isUpdatingMode;
        public bool IsUpdatingMode
        {
            get => isUpdatingMode;
            set => SetProperty(ref isUpdatingMode, value);
        }

        private string? month;
        public string Month
        {
            get => month;
            set => SetProperty(ref month, value);
        }

        #endregion

        #region Constructors

        public ChangeCalendarEventVM(ICalendarService calendarService,
            CalendarEventForm form)
        {
            _calendarService = calendarService;
            IsUpdatingMode = true;

            _id = form.Id;
            Form = new()
            {
                Title = form.Title,
                Description = form.Description,
                Color = form.GetColor(),
                Day = form.Day,
                Month = form.Month,
            };

            SetMonth(Form.Month);
        }

        public ChangeCalendarEventVM(ICalendarService calendarService,
            int day, int month)
        {
            _calendarService = calendarService;
            IsUpdatingMode = false;

            _id = -1;
            Form = new()
            {
                Title = null,
                Description = null,
                Color = Colors.Black,
                Day = day,
                Month = month
            };

            SetMonth(Form.Month);
        }

        #endregion

        #region Commands

        public abstract ICommand Save { get; }
        public abstract ICommand Delete { get; }

        private RelayCommand<Color>? setColor;
        public ICommand SetColor => setColor ??
            (setColor = new RelayCommand<Color>(color =>
            {
                Form.Color = color;
            }));

        private RelayCommand? cancel;
        public ICommand Cancel => cancel ??
            (cancel = new(() =>
            {
                WeakReferenceMessenger.Default
                    .Send(new CloseCalendarEventChangingMessage(null));
            }));

		#endregion

		#region Methods

		private void SetMonth(int val)
        {
            var dt = new DateTime(DateTime.Now.Year, val, 1);
            Month = dt.ToString("MMMM");
        }

        #endregion
    }
}
