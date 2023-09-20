using CommunityToolkit.Mvvm.Messaging.Messages;
using Synergy.StandardApps.EntityForms.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Alarms.Messages
{
    internal class AlarmCreatedMessage : ValueChangedMessage<AlarmForm>
    {
        public AlarmCreatedMessage(AlarmForm value) : base(value)
        {
        }
    }
}
