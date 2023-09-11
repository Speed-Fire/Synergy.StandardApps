using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Synergy.StandardApps.Notes.Messages
{
    internal class NoteNavigateMessage : ValueChangedMessage<Page?>
    {
        public NoteNavigateMessage(Page? value) : base(value)
        {
        }
    }
}
