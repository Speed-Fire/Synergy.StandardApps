using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.EntityForms.Notes;
using Synergy.StandardApps.Notes.Messages;
using Synergy.StandardApps.Service.Notes;
using Synergy.WPF.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Synergy.StandardApps.Notes.ViewModels.ChangeNoteVMs
{
    public class CreateNoteVM : ChangeNoteVM
    {
        private NoteCreationForm protoNote;
        public override NoteCreationForm ProtoNote
        {
            get => protoNote;
            //set => SetProperty(ref protoNote, value);
        }

        public override string Created => string.Empty;

        public CreateNoteVM(INoteService noteService) : base(noteService)
        {
            protoNote = new();
        }

        private RelayCommand? saveCommand;
        public override ICommand? SaveCommand => saveCommand ??
            (saveCommand = new RelayCommand(async () =>
            {
                var res = await _noteService.CreateNote(ProtoNote);

                if (res.StatusCode == Domain.Enums.StatusCode.Error)
                {
                    await NotifyingGrid.ShowNotificationAsync("MainGrid",
                        "Error", "Specified name is already taken!",
                        System.Windows.MessageBoxButton.OK);

                    return;
                }

                WeakReferenceMessenger.Default.Send(new NoteCreatedMessage(res.Data));
            }));
    }
}
