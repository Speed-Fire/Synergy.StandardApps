using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.EntityForms.Notes;
using Synergy.StandardApps.Notes.Messages;
using Synergy.StandardApps.Resources;
using Synergy.StandardApps.Service.Notes;
using Synergy.WPF.Common.Controls;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Synergy.StandardApps.Notes.ViewModels.ChangeNoteVMs
{
    public class CreateNoteVM : ChangeNoteVM
    {
        private NoteCreationForm protoNote;
        public override NoteCreationForm ProtoNote
        {
            get => protoNote;
            protected set => SetProperty(ref protoNote, value);
        }

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
                    if (!ExceptionTranslator.Instance.TryGetValue(res.Error, out string message))
                        message = res.Error?.Message ?? "Internal error.";

					await NotifyingGrid.ShowNotificationAsync("MainGrid",
						Application.Current.TryFindResource("Strings.Error") as string,
                        message,
                        System.Windows.MessageBoxButton.OK);

                    return;
                }

                WeakReferenceMessenger.Default.Send(new NoteCreatedMessage(res.Data));
                WeakReferenceMessenger.Default
                    .Send(new CloseNoteChangingMessage(null));
            }));

        private RelayCommand? deleteCommand;
        public override ICommand? DeleteCommand => deleteCommand ??
            (deleteCommand = new(() => { }));
    }
}
