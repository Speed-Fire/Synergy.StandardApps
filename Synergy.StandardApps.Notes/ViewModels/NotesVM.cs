using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.EntityForms.Notes;
using Synergy.StandardApps.Notes.Messages;
using Synergy.StandardApps.Notes.SubPages;
using Synergy.StandardApps.Service.Notes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Synergy.StandardApps.Notes.ViewModels
{
    public class NotesVM : ObservableRecipient, IRecipient<NoteCreatedMessage>
    {
        private readonly INoteService _noteService;

        private ObservableCollection<NoteForm> notes;
        public ObservableCollection<NoteForm> Notes
        {
            get => notes;
            set => SetProperty(ref notes, value);
        }

        private bool isListEnabled;
        public bool IsListEnabled
        {
            get => isListEnabled;
            set => SetProperty(ref isListEnabled, value);
        }

        public NotesVM(INoteService noteService)
        {
            _noteService = noteService;

            notes = new();
            IsListEnabled = true;

            IsActive = true;

            LoadNotesAsync();
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

        void IRecipient<NoteCreatedMessage>.Receive(NoteCreatedMessage message)
        {
            IsListEnabled = true;

            if (message.Value is null)
                return;

            Notes.Add(message.Value);
        }

        #region Commands

        private RelayCommand? openNoteCreation;
        public RelayCommand OpenNoteCreation => openNoteCreation ??
            (openNoteCreation = new RelayCommand(() =>
            {
                IsListEnabled = false;
                //WeakReferenceMessenger.Default
                //    .Send(new NoteNavigateMessage(new Uri("", UriKind.RelativeOrAbsolute)));
                WeakReferenceMessenger.Default
                    .Send(new NoteNavigateMessage(new CreateNotePage(_noteService)));
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
