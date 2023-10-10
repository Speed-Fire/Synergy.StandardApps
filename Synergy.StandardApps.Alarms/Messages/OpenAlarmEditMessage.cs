using CommunityToolkit.Mvvm.Messaging.Messages;
using Synergy.StandardApps.EntityForms.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Alarms.Messages
{
    internal class OpenAlarmEditMessage : ValueChangedMessage<AlarmForm?>
    {
        public OpenAlarmEditMessage(AlarmForm? value) : base(value)
        {
        }
    }
}
