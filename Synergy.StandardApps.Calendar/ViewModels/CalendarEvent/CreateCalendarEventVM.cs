using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Calendar.Messages;
using Synergy.StandardApps.Resources;
using Synergy.StandardApps.Service.Calendar;
using Synergy.WPF.Common.Controls;
using Synergy.WPF.Navigation.Services.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Synergy.StandardApps.Calendar.ViewModels.CalendarEvent
{
    public class CreateCalendarEventVM : ChangeCalendarEventVM
    {
        public CreateCalendarEventVM(ICalendarService calendarService,
            int day, int month)
            : base(calendarService, day, month)
        {
        }

        private AsyncRelayCommand? save;
        public override ICommand Save => save ??
            (save = new AsyncRelayCommand(SaveAsync));

        private AsyncRelayCommand? delete;
        public override ICommand Delete => delete ??
            (delete = new AsyncRelayCommand(DeleteAsync));

        private async Task SaveAsync()
        {
            var res = await _calendarService.CreateEvent(Form);

			if (res.StatusCode == Domain.Enums.StatusCode.Error)
			{
				if (!ExceptionTranslator.Instance.TryGetValue(res.Error, out string message))
					message = res.Error?.Message ?? "Internal error.";

				await NotifyingGrid.ShowNotificationAsync("MainGrid",
					Application.Current.TryFindResource("Strings.Error") as string,
					message,
					System.Windows.MessageBoxButton.OK);

				return;
			}

			WeakReferenceMessenger.Default
                .Send(new CalendarEventCreatedMessage(res.Data));
            WeakReferenceMessenger.Default
                .Send(new CloseCalendarEventChangingMessage(null));
        }

        private async Task DeleteAsync()
        {
            
        }
    }
}
