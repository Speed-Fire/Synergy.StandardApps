using CommunityToolkit.Mvvm.Messaging.Messages;
using Synergy.StandardApps.EntityForms.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Notes.Messages
{
    internal class NoteCreatedMessage : ValueChangedMessage<NoteForm>
    {
        public NoteCreatedMessage(NoteForm value) : base(value)
        {
        }
    }
}
