using CommunityToolkit.Mvvm.Input;
using Synergy.StandardApps.EntityForms.Calendar;
using Synergy.StandardApps.Service.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Synergy.StandardApps.Calendar.ViewModels.ChangeCalendarEventVMs
{
    public class UpdateCalendarEventVM : ChangeCalendarEventVM
    {
        public UpdateCalendarEventVM(ICalendarService calendarService, CalendarEventForm form) 
            : base(calendarService, form)
        {
        }

        private AsyncRelayCommand? save;
        public override ICommand Save => save ??
            (save = new AsyncRelayCommand(SaveAsync));

        public AsyncRelayCommand? delete;
        public override ICommand Delete => delete ??
            (delete = new AsyncRelayCommand(DeleteAsync));

        private async Task SaveAsync()
        {

        }

        private async Task DeleteAsync()
        {

        }
    }
}
