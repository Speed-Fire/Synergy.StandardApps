using CommunityToolkit.Mvvm.Messaging.Messages;
using Synergy.StandardApps.Domain.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Background.Messages.Calendar
{
    public class AddCalendarEventMessage : ValueChangedMessage<CalendarEvent>
    {
        public AddCalendarEventMessage(CalendarEvent value) : base(value)
        {
        }
    }
}
