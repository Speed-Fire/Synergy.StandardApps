using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.EntityForms.Notes;
using Synergy.StandardApps.Notes.Messages;
using Synergy.StandardApps.Service.Notes;
using Synergy.WPF.Navigation.Services.Local;
using Synergy.WPF.Navigation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace Synergy.StandardApps.Notes.ViewModels.ChangeNoteVMs
{
    public abstract class ChangeNoteVM : ViewModel
    {
        protected readonly INoteService _noteService;

        public abstract NoteCreationForm ProtoNote { get; protected set; }

        private bool isUpdatingMode = false;
        public bool IsUpdatingMode
        {
            get => isUpdatingMode;
            set => SetProperty(ref isUpdatingMode, value);
        }

        public ChangeNoteVM(INoteService noteService)
        {
            _noteService = noteService;
        }

        public abstract ICommand? SaveCommand { get; }
        public abstract ICommand? DeleteCommand { get; }

        private RelayCommand? goBackCommand;
        public ICommand? GoBackCommand => goBackCommand ??
            (goBackCommand = new RelayCommand(() =>
            {
                WeakReferenceMessenger.Default.Send(new CloseNoteChangingMessage(null));
            }));

        private RelayCommand? viewLoadedCommand;
        public ICommand ViewLoadedCommand => viewLoadedCommand ??
            (viewLoadedCommand = new RelayCommand(() =>
            {
                IsActive = true;
            }));

        private RelayCommand? viewUnloadedCommand;
        public ICommand ViewUnloadedCommand => viewUnloadedCommand ??
            (viewUnloadedCommand = new RelayCommand(() =>
            {
                IsActive = false;
            }));

		private RelayCommand<Color>? setColor;
		public ICommand SetColor => setColor ??
			(setColor = new RelayCommand<Color>(color =>
			{
				ProtoNote.Color = color;
			}));
	}
}
