using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Calendar.Messages
{
    internal class SetRightSidePanelVisibilityMessage : ValueChangedMessage<bool>
    {
        public SetRightSidePanelVisibilityMessage(bool value) : base(value)
        {
        }
    }
}
