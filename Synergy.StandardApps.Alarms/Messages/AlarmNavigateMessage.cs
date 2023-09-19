using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Synergy.StandardApps.Alarms.Messages
{
    internal class AlarmNavigateMessage : ValueChangedMessage<Page?>
    {
        public AlarmNavigateMessage(Page? value) : base(value)
        {
        }
    }
}
