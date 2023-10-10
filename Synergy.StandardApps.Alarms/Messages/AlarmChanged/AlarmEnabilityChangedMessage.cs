using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Alarms.Messages.AlarmChanged
{
    public class AlarmEnabilityChangedMessage : ValueChangedMessage<Tuple<long, bool>>
    {
        public AlarmEnabilityChangedMessage(Tuple<long, bool> value) : base(value)
        {
        }
    }
}
