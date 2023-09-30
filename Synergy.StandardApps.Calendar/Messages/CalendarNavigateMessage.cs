using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Synergy.StandardApps.Calendar.Messages
{
    internal class CalendarNavigateMessage : ValueChangedMessage<Page?>
    {
        public CalendarNavigateMessage(Page? value) : base(value)
        {
        }
    }
}
