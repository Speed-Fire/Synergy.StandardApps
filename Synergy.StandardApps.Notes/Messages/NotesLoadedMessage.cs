using CommunityToolkit.Mvvm.Messaging.Messages;
using Synergy.StandardApps.EntityForms.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Notes.Messages
{
    internal class NotesLoadedMessage : ValueChangedMessage<IEnumerable<NoteForm>>
    {
        public NotesLoadedMessage(IEnumerable<NoteForm> value) : base(value)
        {
        }
    }
}
