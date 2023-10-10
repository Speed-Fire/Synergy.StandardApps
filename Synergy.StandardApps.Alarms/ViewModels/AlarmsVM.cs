using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Alarms.Messages;
using Synergy.StandardApps.Alarms.ViewModels.ChangeAlarmVMs;
using Synergy.StandardApps.EntityForms.Alarm;
using Synergy.StandardApps.EntityForms.Notes;
using Synergy.StandardApps.Service.Alarm;
using Synergy.WPF.Navigation.Services.Local;
using Synergy.WPF.Navigation.Services;
using Synergy.WPF.Navigation.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Synergy.StandardApps.Alarms.Messages.AlarmChanged;
using System.Security.Claims;

namespace Synergy.StandardApps.Alarms.ViewModels
{
    public class AlarmsVM :
        ViewModel,
        IRecipient<AlarmCreatedMessage>,
        IRecipient<AlarmEnabilityChangedMessage>,
        IRecipient<AlarmDeletedMessage>,
        IRecipient<CloseAlarmChangingMessage>,
        IRecipient<RightSidePanelClosedMessage>
    {
        private readonly IAlarmService _alarmService;
        private readonly INavigationService _navigationService;
        private ILocalNavigationService _localNavigationService;

        #region Properties

        public ILocalNavigationService LocalNavigationService
        {
            get => _localNavigationService;
            set => SetProperty(ref _localNavigationService, value);
        }

        private ObservableCollection<AlarmVM> alarms;
        public ObservableCollection<AlarmVM> Alarms
        {
            get => alarms;
            set => SetProperty(ref alarms, value);
        }

        #endregion

        public AlarmsVM(IAlarmService alarmService,
            INavigationService navigationService,
            ILocalNavigationService localNavigationService)
        {
            _alarmService = alarmService;
            _navigationService = navigationService;
            LocalNavigationService = localNavigationService;

            alarms = new();

            IsActive = true;
        }

        #region Messages

        #region CRUD

        void IRecipient<AlarmCreatedMessage>.Receive(AlarmCreatedMessage message)
        {
            Alarms.Add(new AlarmVM(message.Value));
        }

        async void IRecipient<AlarmEnabilityChangedMessage>.Receive(AlarmEnabilityChangedMessage message)
        {
            var res = await _alarmService.SwitchAlarmEnability(message.Value.Item1, message.Value.Item2);

            if (res.StatusCode == Domain.Enums.StatusCode.Error)
                return;
        }

        void IRecipient<AlarmDeletedMessage>.Receive(AlarmDeletedMessage message)
        {
            var alarm = alarms.FirstOrDefault(a => a.Id == message.Value);
            if (alarm is null) return;
            Alarms.Remove(alarm);
        }

        #endregion

        void IRecipient<RightSidePanelClosedMessage>.Receive(RightSidePanelClosedMessage message)
        {
            if (_localNavigationService.CurrentView is null ||
                _localNavigationService.CurrentView.GetType() != typeof(UpdateAlarmVM))
            {
                _localNavigationService.NavigateTo<UpdateAlarmVM>(_alarmService);
            }

            WeakReferenceMessenger.Default
                .Send(new OpenAlarmEditMessage(null));
        }

        void IRecipient<CloseAlarmChangingMessage>.Receive(CloseAlarmChangingMessage message)
        {
            WeakReferenceMessenger.Default
                .Send(new SetRightSidePanelVisibilityMessage(false));
        }

        #endregion

        #region Commands

        private AsyncRelayCommand? loadAlarmsCommand;
        public ICommand LoadAlarmsCommand => loadAlarmsCommand ??
            (loadAlarmsCommand = new AsyncRelayCommand(LoadAlarmsAsync));

        private RelayCommand<AlarmForm>? openAlarmCreationCommand;
        public ICommand OpenAlarmCreationCommand => openAlarmCreationCommand ??
            (openAlarmCreationCommand = new RelayCommand<AlarmForm>(alarm =>
            {
                _localNavigationService
                    .NavigateTo<CreateAlarmVM>(_alarmService);
                WeakReferenceMessenger.Default
                    .Send(new SetRightSidePanelVisibilityMessage(true));
            }));

        private RelayCommand<long>? openAlarmEditCommand;
        public ICommand OpenAlarmEditCommand => openAlarmEditCommand ??
            (openAlarmEditCommand = new RelayCommand<long>(id =>
            {
                var alarm = alarms.FirstOrDefault(x => x.Id == id);
                if (alarm is null)
                    return;

                WeakReferenceMessenger.Default
                    .Send(new OpenAlarmEditMessage(alarm.Form));
                WeakReferenceMessenger.Default
                    .Send(new SetRightSidePanelVisibilityMessage(true));
            }));

        public async Task LoadAlarmsAsync()
        {
            Alarms.Clear();

            var _alarms = await _alarmService.GetAlarms();

            if (_alarms.StatusCode == Domain.Enums.StatusCode.Error)
                return;

            foreach (var alarm in _alarms.Data)
            {
                Alarms.Add(new AlarmVM(alarm));
            }

            _localNavigationService
                    .NavigateTo<UpdateAlarmVM>(_alarmService);
        }

        #endregion
    }
}
