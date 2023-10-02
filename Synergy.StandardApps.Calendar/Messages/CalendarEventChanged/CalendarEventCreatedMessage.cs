using CommunityToolkit.Mvvm.Messaging.Messages;
using Synergy.StandardApps.EntityForms.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Calendar.Messages
{
    internal class CalendarEventCreatedMessage : ValueChangedMessage<CalendarEventForm>
    {
        public CalendarEventCreatedMessage(CalendarEventForm value) : base(value)
        {
        }
    }
}
