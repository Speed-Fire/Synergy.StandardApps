using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.EntityForms.Notes;
using Synergy.StandardApps.Notes.Messages;
using Synergy.StandardApps.Resources;
using Synergy.StandardApps.Service.Calendar;
using Synergy.StandardApps.Service.Notes;
using Synergy.WPF.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Synergy.StandardApps.Notes.ViewModels.ChangeNoteVMs
{
    public class UpdateNoteVM : 
        ChangeNoteVM,
        IRecipient<OpenNoteEditMessage>
    {
        private long id;

        private NoteCreationForm protoNote;
        public override NoteCreationForm ProtoNote 
        {
            get => protoNote;
            protected set => SetProperty(ref protoNote, value);
        }

        public UpdateNoteVM(INoteService noteService) :
            base(noteService)
        {
            IsUpdatingMode = true;

            protoNote = new()
            {
                Name = "",
                Description = ""
            };

            id = -1;
        }

        #region Commands

        private AsyncRelayCommand? saveCommand;
        public override ICommand? SaveCommand => saveCommand ??
            (saveCommand = new AsyncRelayCommand(UpdateNoteAsync));

        private AsyncRelayCommand? deleteCommand;
        public override ICommand? DeleteCommand => deleteCommand ??
            (deleteCommand = new AsyncRelayCommand(DeleteNoteAsync));

        private async Task UpdateNoteAsync()
        {
            if (ProtoNote.HasErrors)
                return;

            var res = await _noteService.UpdateNote(ProtoNote, id);
            if (res.StatusCode == Domain.Enums.StatusCode.Error)
            {
				if (!ExceptionTranslator.Instance.TryGetValue(res.Error, out string message))
					message = res.Error?.Message ?? "Internal error.";

				await NotifyingGrid.ShowNotificationAsync("MainGrid",
					Application.Current.TryFindResource("Strings.Error") as string,
                    message,
                    System.Windows.MessageBoxButton.OK);

                return;
            }

            if (res.Data != null)
                WeakReferenceMessenger.Default.Send(new NoteUpdatedMessage(res.Data));
            WeakReferenceMessenger.Default
                .Send(new CloseNoteChangingMessage(null));
        }

        private async Task DeleteNoteAsync()
        {
            var res = await _noteService.DeleteNote(id);

            if (res.StatusCode == Domain.Enums.StatusCode.Error)
            {
				if (!ExceptionTranslator.Instance.TryGetValue(res.Error, out string message))
					message = res.Error?.Message ?? "Internal error.";

				await NotifyingGrid.ShowNotificationAsync("MainGrid",
						Application.Current.TryFindResource("Strings.Error") as string,
                        message,
                        System.Windows.MessageBoxButton.OK);

                return;
            }

            WeakReferenceMessenger.Default
                .Send(new NoteDeletedMessage(id));
            WeakReferenceMessenger.Default
                .Send(new CloseNoteChangingMessage(null));
        }

        #endregion

        void IRecipient<OpenNoteEditMessage>.Receive(OpenNoteEditMessage message)
        {
            ProtoNote = new()
            {
                Name = message.Value?.Name ?? "",
                Description = message.Value?.Description ?? "",
            };

            id = message.Value?.Id ?? 0;
        }
    }
}
