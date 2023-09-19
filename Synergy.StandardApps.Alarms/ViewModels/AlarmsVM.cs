using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Synergy.StandardApps.EntityForms.Alarm;
using Synergy.StandardApps.EntityForms.Notes;
using Synergy.StandardApps.Service.Alarm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Synergy.StandardApps.Alarms.ViewModels
{
    public class AlarmsVM : ObservableRecipient
    {
        private readonly IAlarmService _alarmService;

        private ObservableCollection<AlarmForm> alarms;
        public ObservableCollection<AlarmForm> Alarms
        {
            get => alarms;
            set => SetProperty(ref alarms, value);
        }

        private AlarmForm? selectedItem;
        public AlarmForm? SelectedItem
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

        public AlarmsVM(IAlarmService alarmService)
        {
            _alarmService = alarmService;

            alarms = new();
            IsListDisabled = false;

            IsActive = true;
        }

        #region Messages

        #endregion

        #region Commands

        private AsyncRelayCommand? loadAlarms;
        public ICommand LoadAlarms => loadAlarms ??
            (loadAlarms = new AsyncRelayCommand(LoadAlarmsAsync));

        private RelayCommand<AlarmForm>? openAlarmCreation;
        public ICommand OpenAlarmCreation => openAlarmCreation ??
            (openAlarmCreation = new RelayCommand<AlarmForm>(alarm =>
            {

            }));

        private AsyncRelayCommand<AlarmForm>? switchAlarmEnability;
        public ICommand SwitchAlarmEnability => switchAlarmEnability ??
            (switchAlarmEnability = new AsyncRelayCommand<AlarmForm>(SwitchAlarmEnabilityAsync));

        private AsyncRelayCommand<AlarmForm>? deleteAlarm;
        public ICommand DeleteAlarm => deleteAlarm ??
            (deleteAlarm = new AsyncRelayCommand<AlarmForm>(DeleteAlarmAsync));


        public async Task LoadAlarmsAsync()
        {
            var _alarms = await _alarmService.GetAlarms();

            if (_alarms.StatusCode == Domain.Enums.StatusCode.Error)
                return;

            foreach (var note in _alarms.Data)
            {
                alarms.Add(note);
            }
        }

        private async Task DeleteAlarmAsync(AlarmForm? alarm)
        {
            if (alarm is null)
                return;

            var res = await _alarmService.DeleteAlarm(alarm.Id);

            if (res.StatusCode == Domain.Enums.StatusCode.Error)
            {

                return;
            }

            Alarms.Remove(alarm);
        }

        private async Task SwitchAlarmEnabilityAsync(AlarmForm? alarm)
        {

        }

        #endregion
    }
}
