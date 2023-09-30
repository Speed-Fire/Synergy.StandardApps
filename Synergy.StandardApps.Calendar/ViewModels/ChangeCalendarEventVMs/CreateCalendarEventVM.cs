using CommunityToolkit.Mvvm.Input;
using Synergy.StandardApps.Service.Calendar;
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

        public override ICommand Delete => throw new NotImplementedException();

        private async Task SaveAsync()
        {

        }
    }
}
