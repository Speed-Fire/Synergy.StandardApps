using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Domain.Notes;
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
        IRecipient<NoteDeletedMessage>,
        IRecipient<CloseNoteChangingMessage>,
        IRecipient<RightSidePanelClosedMessage>
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

        #endregion

        public NotesVM(INoteService noteService,
            INavigationService navigationService,
            ILocalNavigationService localNavigationService)
        {
            _noteService = noteService;
            _navigationService = navigationService;
            LocalNavigationService = localNavigationService;

            notes = new();

            IsActive = true;
        }

        #region Messages

        #region CRUD

        void IRecipient<NoteCreatedMessage>.Receive(NoteCreatedMessage message)
        {
            Notes.Add(message.Value);
        }

        void IRecipient<NoteUpdatedMessage>.Receive(NoteUpdatedMessage message)
        {
            Notes.Remove(Notes.First(n => n.Id == message.Value.Id));
            Notes.Add(message.Value);
        }

        void IRecipient<NoteDeletedMessage>.Receive(NoteDeletedMessage message)
        {
            Notes.Remove(Notes.First(n => n.Id == message.Value));
        }

        #endregion

        void IRecipient<RightSidePanelClosedMessage>.Receive(RightSidePanelClosedMessage message)
        {
            if (_localNavigationService.CurrentView is null ||
                _localNavigationService.CurrentView.GetType() != typeof(UpdateNoteVM))
            {
                _localNavigationService.NavigateTo<UpdateNoteVM>(_noteService);
            }

            WeakReferenceMessenger.Default
                .Send(new OpenNoteEditMessage(null));
        }

        void IRecipient<CloseNoteChangingMessage>.Receive(CloseNoteChangingMessage message)
        {
            WeakReferenceMessenger.Default
                .Send(new SetRightSidePanelVisibilityMessage(false));
        }

        #endregion

        #region Commands

        private AsyncRelayCommand? loadNotesCommand;
        public ICommand LoadNotesCommand => loadNotesCommand ??
            (loadNotesCommand = new AsyncRelayCommand(LoadNotesAsync));

        private RelayCommand? openNoteCreationCommand;
        public ICommand OpenNoteCreationCommand => openNoteCreationCommand ??
            (openNoteCreationCommand = new RelayCommand(() =>
            {
                _localNavigationService
                    .NavigateTo<CreateNoteVM>(_noteService);
                WeakReferenceMessenger.Default
                    .Send(new SetRightSidePanelVisibilityMessage(true));
            }));

        private RelayCommand<long>? openNoteEditCommand;
        public ICommand OpenNoteEditCommand => openNoteEditCommand ??
            (openNoteEditCommand = new RelayCommand<long>(id =>
            {
                var note = notes.FirstOrDefault(x => x.Id == id);
                if (note is null)
                    return;

                WeakReferenceMessenger.Default
                    .Send(new OpenNoteEditMessage(note));
                WeakReferenceMessenger.Default
                    .Send(new SetRightSidePanelVisibilityMessage(true));
            }));

        public async Task LoadNotesAsync()
        {
            notes.Clear();

            var _notes = await _noteService.GetNotes();

            if (_notes.StatusCode == Domain.Enums.StatusCode.Error)
                return;

            foreach (var note in _notes.Data)
            {
                notes.Add(note);
            }

            _localNavigationService
                    .NavigateTo<UpdateNoteVM>(_noteService);

            WeakReferenceMessenger.Default
                    .Send(new NotesLoadedMessage(notes));
        }

        #endregion
    }
}
