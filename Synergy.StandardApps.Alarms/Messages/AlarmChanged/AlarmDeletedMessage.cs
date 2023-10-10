using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Alarms.Messages.AlarmChanged
{
    public class AlarmDeletedMessage : ValueChangedMessage<long>
    {
        public AlarmDeletedMessage(long value) : base(value)
        {
        }
    }
}
