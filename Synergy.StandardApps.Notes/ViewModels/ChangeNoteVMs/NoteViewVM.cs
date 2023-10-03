using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Notes.Messages;
using Synergy.WPF.Navigation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Notes.ViewModels.ChangeNoteVMs
{
    internal class NoteViewVM : 
        ViewModel,
        IRecipient<OpenNoteMessage>
    {
        private long _id = 0;

        #region Properties

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

        private string created = "";
        public string Created
        {
            get => created;
            set => SetProperty(ref created, value);
        }

        #endregion

        public NoteViewVM()
        {
            IsActive = true;
        }

        private RelayCommand? openNoteEditCommand;
        public RelayCommand OpenNoteEditCommand => openNoteEditCommand ??
            (openNoteEditCommand = new RelayCommand(() =>
            {
                WeakReferenceMessenger.Default.Send(new OpenNoteEditMessage(null));
            }));

        public void Receive(OpenNoteMessage message)
        {
            if (message.Value is null)
            {
                Name = "";
                Description = "";
                Created = "";
                _id = 0;
            }
            else
            {
                Name = message.Value.Name;
                Description = message.Value.Description;
                Created = message.Value.Created.ToString();
                _id = message.Value.Id;
            }
        }
    }
}
