using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.EntityForms.Notes;
using Synergy.StandardApps.Notes.Messages;
using Synergy.StandardApps.Notes.SubPages;
using Synergy.StandardApps.Notes.ViewModels.ChangeNoteVMs;
using Synergy.StandardApps.Service.Notes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Synergy.StandardApps.Notes.ViewModels
{
    public class NotesVM : 
        ObservableRecipient,
        IRecipient<NoteCreatedMessage>,
        IRecipient<OpenNoteEditMessage>,
        IRecipient<NoteChangingCanceledMessage>
    {
        private readonly INoteService _noteService;

        private ObservableCollection<NoteForm> notes;
        public ObservableCollection<NoteForm> Notes
        {
            get => notes;
            set => SetProperty(ref notes, value);
        }

        private NoteForm? selectedItem;
        public NoteForm? SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        private bool isListDisabled;
        public bool IsListDisabled
        {
            get => isListDisabled;
            set => SetProperty(ref isListDisabled, value);
        }

        public NotesVM(INoteService noteService)
        {
            _noteService = noteService;

            notes = new();
            IsListDisabled = false;

            IsActive = true;

            //LoadNotesAsync();
        }

        public async Task LoadNotesAsync()
        {
            var _notes = await _noteService.GetNotes();

            if (_notes.StatusCode == Domain.Enums.StatusCode.Error)
                return;

            foreach (var note in _notes.Data)
            {
                notes.Add(note);
            }
        }

        #region Messages

        void IRecipient<NoteCreatedMessage>.Receive(NoteCreatedMessage message)
        {
            IsListDisabled = false;

            if (message.Value is null)
                return;

            Notes.Add(message.Value);
        }

        void IRecipient<OpenNoteEditMessage>.Receive(OpenNoteEditMessage message)
        {
            IsListDisabled = true;

            WeakReferenceMessenger.Default
                    .Send(new NoteNavigateMessage(new ChangeNotePage(new UpdateNoteVM(SelectedItem, _noteService))));
        }

        void IRecipient<NoteChangingCanceledMessage>.Receive(NoteChangingCanceledMessage message)
        {
            IsListDisabled = false;
        }

        #endregion

        #region Commands

        private AsyncRelayCommand? loadNotes;
        public ICommand LoadNotes => loadNotes ??
            (loadNotes = new AsyncRelayCommand(LoadNotesAsync));

        private RelayCommand? openNoteCreation;
        public RelayCommand OpenNoteCreation => openNoteCreation ??
            (openNoteCreation = new RelayCommand(() =>
            {
                IsListDisabled = true;
                WeakReferenceMessenger.Default
                    .Send(new NoteNavigateMessage(new ChangeNotePage(new CreateNoteVM(_noteService))));
            }));

        private RelayCommand<NoteForm>? openNote;
        public RelayCommand<NoteForm> OpenNote => openNote ??
            (openNote = new RelayCommand<NoteForm>(note =>
            {
                WeakReferenceMessenger.Default.Send(new OpenNoteMessage(note));
            }));

        private RelayCommand<NoteForm>? deleteNote;
        public RelayCommand<NoteForm> DeleteNote => deleteNote ??
            (deleteNote = new RelayCommand<NoteForm>(async note =>
            {
                if (note is null)
                    return;

                var res = await _noteService.DeleteNote(note.Id);

                if(res.StatusCode == Domain.Enums.StatusCode.Error)
                {

                    return;
                }

                Notes.Remove(note);
                WeakReferenceMessenger.Default.Send(new OpenNoteMessage(null));
            }));

        #endregion
    }
}
