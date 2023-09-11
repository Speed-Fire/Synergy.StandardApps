using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Notes.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Notes.ViewModels
{
    internal class NoteViewVM : ObservableRecipient, IRecipient<OpenNoteMessage>
    {
        private string name = "";
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string description = "";
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public void Receive(OpenNoteMessage message)
        {
            if (message.Value is null)
            {
                Name = "";
                Description = "";
            }
            else
            {
                Name = message.Value.Name;
                Description = message.Value.Description;
            }
        }
    }
}
