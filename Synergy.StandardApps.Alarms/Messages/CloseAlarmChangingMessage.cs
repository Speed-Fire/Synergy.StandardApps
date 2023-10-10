using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Alarms.Messages
{
    internal class CloseAlarmChangingMessage : ValueChangedMessage<object?>
    {
        public CloseAlarmChangingMessage(object? value) : base(value)
        {
        }
    }
}
