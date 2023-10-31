using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.EntityForms.Notes;
using Synergy.StandardApps.Notes.Messages;
using Synergy.WPF.Navigation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Synergy.StandardApps.Notes.ViewModels.ChangeNoteVMs
{
    public class NoteVM :
        ViewModel,
        IRecipient<NoteUpdatedMessage>
    {
        public event Action UpdateStart;
        public event Action UpdateEnd;

        private long _id = 0;
        public long Id => _id;

        private string _title = "";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _description = "";
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private Color _color = Colors.Yellow;
        public Color Color
        {
            get => _color;
            set=>SetProperty(ref _color, value);
        }

        public NoteVM(NoteForm form)
        {
            _id = form.Id;
            Title = form.Name;
            Description = form.Description;
            Color = form.GetColor();
        }

        #region Messages

        void IRecipient<NoteUpdatedMessage>.Receive(NoteUpdatedMessage message)
        {
            if (message.Value.Id != _id)
                return;

            UpdateStart?.Invoke();

            Title = message.Value.Name;
            Description = message.Value.Description;
            Color = message.Value.GetColor();

            UpdateEnd?.Invoke();
        }

        #endregion

        #region Commands

        private RelayCommand? loadedCommand;
        public ICommand LoadedCommand => loadedCommand ??
            (loadedCommand = new(Loaded));
        private void Loaded()
        {
            IsActive = true;
        }

        private RelayCommand? unloadedCommand;
        public ICommand UnloadedCommand => unloadedCommand ??
            (unloadedCommand = new(Unloaded));
        private void Unloaded()
        {
            IsActive = false;
        }

        #endregion
    }
}
