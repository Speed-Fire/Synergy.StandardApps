using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Background.Messages.Calendar
{
    public class DeleteCalendarEventMessage : ValueChangedMessage<long>
    {
        public DeleteCalendarEventMessage(long value) : base(value)
        {
        }
    }
}
