using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Alarms.Messages
{
    internal class RightSidePanelClosedMessage : ValueChangedMessage<object?>
    {
        public RightSidePanelClosedMessage(object? value) : base(value)
        {
        }
    }
}
