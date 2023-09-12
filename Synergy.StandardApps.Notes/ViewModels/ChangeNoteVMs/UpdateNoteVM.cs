using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.EntityForms.Notes;
using Synergy.StandardApps.Notes.Messages;
using Synergy.StandardApps.Service.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Synergy.StandardApps.Notes.ViewModels.ChangeNoteVMs
{
    internal class UpdateNoteVM : ChangeNoteVM
    {
        private readonly long id;

        private NoteCreationForm protoNote;
        public override NoteCreationForm ProtoNote 
        {
            get => protoNote;
            //set => SetProperty(ref protoNote, value);
        }

        private string created;
        public override string Created
        {
            get => created;
            //set => SetProperty(ref created, value);
        }

        public UpdateNoteVM(NoteForm form, INoteService noteService) :
            base(noteService)
        {
            protoNote = new()
            {
                Name = form.Name,
                Description = form.Description,
            };

            id = form.Id;
            created = form.Created.ToString();
        }

        private AsyncRelayCommand? save;
        public override ICommand? Save => save ??
            (save = new AsyncRelayCommand(async () =>
            {

            }));
    }
}
