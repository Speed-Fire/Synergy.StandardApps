using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Synergy.StandardApps.Misc;
using Synergy.StandardApps.EntityForms.Alarm;
using Synergy.StandardApps.Service.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Synergy.StandardApps.Domain.Enums;
using Synergy.WPF.Navigation.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Alarms.Messages;

namespace Synergy.StandardApps.Alarms.ViewModels.ChangeAlarmVMs
{
    public abstract class ChangeAlarmVM : ViewModel
    {
        protected readonly IAlarmService _alarmService;

        public abstract AlarmCreationForm Form { get; protected set; }

        public IEnumerable<DayOfWeek> AlarmedDays => Form.GetAlarmedDays();

        #region Properties

        private bool isUpdatingMode = false;
        public bool IsUpdatingMode
        {
            get => isUpdatingMode;
            set => SetProperty(ref isUpdatingMode, value);
        }

        private List<AlarmSound> sounds;
        public List<AlarmSound> Sounds
        {
            get => sounds;
            set => SetProperty(ref sounds, value);
        }

        #endregion

        protected ChangeAlarmVM(IAlarmService alarmService)
        {
            _alarmService = alarmService;

            Sounds = new(Enum.GetValues<AlarmSound>());
        }

        #region Commands

        #region Changing

        public abstract ICommand SaveCommand { get; }
        public abstract ICommand DeleteCommand { get; }

        #endregion

        #region Navigation

        private RelayCommand? goBackCommand;
        public ICommand GoBackCommand => goBackCommand ??
            (goBackCommand = new(() =>
            {
                WeakReferenceMessenger.Default.Send(new CloseAlarmChangingMessage(null));
            }));

        #endregion

        #region Settings

        private RelayCommand? enableAllDaysCommand;
        public ICommand EnableAllDaysCommand => enableAllDaysCommand ??
            (enableAllDaysCommand = new(() =>
            {
                Form.Monday = Form.Tuesday =
                Form.Wednesday = Form.Thursday =
                Form.Friday = Form.Saturday =
                Form.Sunday = true;
            }));

        private RelayCommand? playAlarmSoundCommand;
        public ICommand PlayAlarmSoundCommand => playAlarmSoundCommand ??
            (playAlarmSoundCommand = new RelayCommand(() =>
            {
                AlarmSoundPlayer.PlaySound(Form.Sound);
            }, () => { return Form.IsSoundEnabled; }));

        #endregion

        #region Loading

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

        #endregion

        #endregion
    }
}
