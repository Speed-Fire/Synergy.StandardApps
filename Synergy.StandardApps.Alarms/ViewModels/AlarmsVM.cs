using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Alarms.Messages;
using Synergy.StandardApps.Alarms.SubPages;
using Synergy.StandardApps.Alarms.ViewModels.ChangeAlarmVMs;
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
    public class AlarmsVM :
        ObservableRecipient,
        IRecipient<AlarmCreatedMessage>,
        IRecipient<AlarmUpdatedMessage>,
        IRecipient<AlarmCreationCancelledMessage>
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

        void IRecipient<AlarmCreatedMessage>.Receive(AlarmCreatedMessage message)
        {
            Alarms.Add(message.Value);

            IsListDisabled = false;

            SelectedItem = message.Value;

            //WeakReferenceMessenger.Default
            //        .Send(new AlarmNavigateMessage(new ChangeAlarmPage(new UpdateAlarmVM(_alarmService,
            //            SelectedItem))));
        }

        void IRecipient<AlarmUpdatedMessage>.Receive(AlarmUpdatedMessage message)
        {
            var pos = alarms.IndexOf(alarms.First(a => a.Id == message.Value.Id));

            alarms.RemoveAt(pos);
            alarms.Insert(pos, message.Value);

            SelectedItem = message.Value;
        }

        #endregion

        #region Commands

        private AsyncRelayCommand? loadAlarms;
        public ICommand LoadAlarms => loadAlarms ??
            (loadAlarms = new AsyncRelayCommand(LoadAlarmsAsync));

        private RelayCommand<AlarmForm>? openAlarmCreation;
        public ICommand OpenAlarmCreation => openAlarmCreation ??
            (openAlarmCreation = new RelayCommand<AlarmForm>(alarm =>
            {
                IsListDisabled = true;

                WeakReferenceMessenger.Default
                    .Send(new AlarmNavigateMessage(new ChangeAlarmPage(new CreateAlarmVM(_alarmService))));
            }));

        void IRecipient<AlarmCreationCancelledMessage>.Receive(AlarmCreationCancelledMessage message)
        {
            IsListDisabled = false;
        }

        private RelayCommand<AlarmForm>? openAlarm;
        public ICommand OpenAlarm => openAlarm ??
            (openAlarm = new RelayCommand<AlarmForm>(alarm =>
            {
                if (alarm is null)
                {
                    WeakReferenceMessenger.Default
                    .Send(new AlarmNavigateMessage(null));

                    return;
                }

                WeakReferenceMessenger.Default
                    .Send(new AlarmNavigateMessage(new ChangeAlarmPage(new UpdateAlarmVM(_alarmService,
                        alarm))));
            }));

        private AsyncRelayCommand<AlarmForm>? switchAlarmEnability;
        public ICommand SwitchAlarmEnability => switchAlarmEnability ??
            (switchAlarmEnability = new AsyncRelayCommand<AlarmForm>(SwitchAlarmEnabilityAsync));

        private AsyncRelayCommand<AlarmForm>? deleteAlarm;
        public ICommand DeleteAlarm => deleteAlarm ??
            (deleteAlarm = new AsyncRelayCommand<AlarmForm>(DeleteAlarmAsync));


        public async Task LoadAlarmsAsync()
        {
            WeakReferenceMessenger.Default
                    .Send(new AlarmNavigateMessage(new BlankAlarmPage()));

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
            var res = await _alarmService.SwitchAlarmEnability(alarm.Id, !alarm.IsEnabled);
            alarm.IsEnabled = !alarm.IsEnabled;

            if (res.StatusCode == Domain.Enums.StatusCode.Error)
                return;
        }

        #endregion
    }
}
