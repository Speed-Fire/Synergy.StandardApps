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
    public abstract class ChangeNoteVM : ObservableObject
    {
        protected readonly INoteService _noteService;

        public abstract NoteCreationForm ProtoNote { get; }
        public abstract string Created { get; }

        public ChangeNoteVM(INoteService noteService)
        {
            _noteService = noteService;
        }

        public abstract ICommand? Save { get; }

        private RelayCommand? goBack;
        public ICommand? GoBack => goBack ??
            (goBack = new RelayCommand(() =>
            {
                WeakReferenceMessenger.Default.Send(new NoteNavigateMessage(null));
                WeakReferenceMessenger.Default.Send(new NoteChangingCanceledMessage(null));
            }));
    }
}
