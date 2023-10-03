using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.EntityForms.Notes;
using Synergy.StandardApps.Notes.Messages;
using Synergy.StandardApps.Notes.ViewModels.ChangeNoteVMs;
using Synergy.StandardApps.Service.Notes;
using Synergy.WPF.Navigation.Services;
using Synergy.WPF.Navigation.Services.Local;
using Synergy.WPF.Navigation.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Synergy.StandardApps.Notes.ViewModels
{
    public class NotesVM : 
        ViewModel,
        IRecipient<NoteCreatedMessage>,
        IRecipient<NoteUpdatedMessage>,
        IRecipient<OpenNoteEditMessage>,
        IRecipient<NoteChangingCanceledMessage>
    {
        private readonly INoteService _noteService;
        private readonly INavigationService _navigationService;
        private ILocalNavigationService _localNavigationService;

        #region Properties

        public ILocalNavigationService LocalNavigationService
        {
            get => _localNavigationService;
            set => SetProperty(ref _localNavigationService, value);
        }

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

        #endregion

        public NotesVM(INoteService noteService,
            INavigationService navigationService,
            ILocalNavigationService localNavigationService)
        {
            _noteService = noteService;
            _navigationService = navigationService;
            LocalNavigationService = localNavigationService;

            notes = new();
            IsListDisabled = false;

            IsActive = true;
        }

        #region Messages

        void IRecipient<NoteCreatedMessage>.Receive(NoteCreatedMessage message)
        {
            IsListDisabled = false;

            if (message.Value is null)
                return;

            Notes.Add(message.Value);

            OpenNote(selectedItem);
        }

        void IRecipient<NoteUpdatedMessage>.Receive(NoteUpdatedMessage message)
        {
            IsListDisabled = false;

            if (message.Value is null)
                return;

            var old = Notes.First(n => n.Id == message.Value.Id);
            var oldPos = Notes.IndexOf(old);

            Notes.RemoveAt(oldPos);
            Notes.Insert(oldPos, message.Value);

            SelectedItem = message.Value;

            OpenNote(SelectedItem);
        }

        void IRecipient<OpenNoteEditMessage>.Receive(OpenNoteEditMessage message)
        {
            IsListDisabled = true;

            _localNavigationService
                    .NavigateTo<UpdateNoteVM>(_noteService, SelectedItem);
        }

        void IRecipient<NoteChangingCanceledMessage>.Receive(NoteChangingCanceledMessage message)
        {
            IsListDisabled = false;

            OpenNote(SelectedItem);
        }

        #endregion

        #region Commands

        private AsyncRelayCommand? loadNotesCommand;
        public ICommand LoadNotesCommand => loadNotesCommand ??
            (loadNotesCommand = new AsyncRelayCommand(LoadNotesAsync));

        private RelayCommand? openNoteCreationCommand;
        public RelayCommand OpenNoteCreationCommand => openNoteCreationCommand ??
            (openNoteCreationCommand = new RelayCommand(() =>
            {
                IsListDisabled = true;

                _localNavigationService
                    .NavigateTo<CreateNoteVM>(_noteService);
            }));

        private RelayCommand<NoteForm>? openNoteCommand;
        public RelayCommand<NoteForm> OpenNoteCommand => openNoteCommand ??
            (openNoteCommand = new RelayCommand<NoteForm>(note =>
            {
                if (note is null)
                    note = SelectedItem;

                OpenNote(note);
            }));

        private AsyncRelayCommand<NoteForm>? deleteNoteCommand;
        public ICommand DeleteNoteCommand => deleteNoteCommand ??
            (deleteNoteCommand = new AsyncRelayCommand<NoteForm>(DeleteNoteAsync));

        public async Task LoadNotesAsync()
        {
            var _notes = await _noteService.GetNotes();

            if (_notes.StatusCode == Domain.Enums.StatusCode.Error)
                return;

            foreach (var note in _notes.Data)
            {
                notes.Add(note);
            }

            OpenNote(null);
        }

        private async Task DeleteNoteAsync(NoteForm note)
        {
            if (note is null)
                return;

            var res = await _noteService.DeleteNote(note.Id);

            if (res.StatusCode == Domain.Enums.StatusCode.Error)
            {

                return;
            }

            Notes.Remove(note);
        }

        #endregion

        #region Methods

        private void OpenNote(NoteForm? note)
        {
            if(LocalNavigationService.CurrentView is not NoteViewVM)
            {
                _localNavigationService
                    .NavigateTo<NoteViewVM>();
            }

            WeakReferenceMessenger.Default.Send(new OpenNoteMessage(note));
        }

        #endregion
    }
}
