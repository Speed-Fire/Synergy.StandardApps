using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Calendar.Messages;
using Synergy.StandardApps.Service.Calendar;
using Synergy.WPF.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Synergy.StandardApps.Calendar.ViewModels.ChangeCalendarEventVMs
{
    public class CreateCalendarEventVM : ChangeCalendarEventVM
    {
        public CreateCalendarEventVM(ICalendarService calendarService, int day, int month)
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

            if(res.StatusCode == Domain.Enums.StatusCode.Error)
            {
                await NotifyingGrid.ShowNotificationAsync("MainGrid",
                        "Error", "Specified name is already taken!",
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
