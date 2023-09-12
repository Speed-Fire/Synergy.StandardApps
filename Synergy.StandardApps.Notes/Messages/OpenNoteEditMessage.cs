using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Notes.Messages
{
    internal class OpenNoteEditMessage : ValueChangedMessage<object?>
    {
        public OpenNoteEditMessage(object? value) : base(value)
        {
        }
    }
}
