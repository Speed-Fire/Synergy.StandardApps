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

namespace Synergy.StandardApps.Notes.ViewModels
{
    internal class CreateNoteVM : ObservableObject
    {
        private readonly INoteService _noteService;

        private NoteCreationForm protoNote;
        public NoteCreationForm ProtoNote
        {
            get => protoNote;
            set => SetProperty(ref protoNote, value);
        }

        internal CreateNoteVM(INoteService noteService)
        {
            _noteService = noteService;

            protoNote = new();
        }

        private RelayCommand? createNote;
        public RelayCommand CreateNote => createNote ??
            (createNote = new RelayCommand(async () =>
            {
                var res = await _noteService.CreateNote(ProtoNote);

                if(res.StatusCode == Domain.Enums.StatusCode.Error)
                {

                    return;
                }

                WeakReferenceMessenger.Default.Send(new NoteCreatedMessage(res.Data));
                WeakReferenceMessenger.Default.Send(new NoteNavigateMessage(null));
            }, () => { return !ProtoNote.HasErrors; }));
    }
}
