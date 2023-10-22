using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Alarms.Messages;
using Synergy.StandardApps.Alarms.Messages.AlarmChanged;
using Synergy.StandardApps.EntityForms.Alarm;
using Synergy.StandardApps.Resources;
using Synergy.StandardApps.Service.Alarm;
using Synergy.WPF.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Synergy.StandardApps.Alarms.ViewModels.ChangeAlarmVMs
{
    public class UpdateAlarmVM : 
        ChangeAlarmVM,
        IRecipient<OpenAlarmEditMessage>
    {
        private long id;

        private AlarmCreationForm form;
        public override AlarmCreationForm Form 
        {
            get => form;
            protected set => SetProperty(ref form, value);
        }

        public UpdateAlarmVM(IAlarmService alarmService) : 
            base(alarmService)
        {
            IsUpdatingMode = true;

            form = new();

            id = -1;
        }

        private AsyncRelayCommand? saveCommand;
        public override ICommand SaveCommand => saveCommand ??
            (saveCommand = new AsyncRelayCommand(UpdateAlarm));

        private AsyncRelayCommand? deleteCommand;
        public override ICommand DeleteCommand => deleteCommand ??
            (deleteCommand = new AsyncRelayCommand(DeleteAlarmAsync));

        private async Task UpdateAlarm()
        {
            var res = await _alarmService.UpdateAlarm(Form, id);
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
                .Send(new AlarmUpdatedMessage(res.Data));
            WeakReferenceMessenger.Default
                .Send(new CloseAlarmChangingMessage(null));
        }

        private async Task DeleteAlarmAsync()
        {
            var res = await _alarmService.DeleteAlarm(id);

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
                .Send(new AlarmDeletedMessage(id));
            WeakReferenceMessenger.Default
                .Send(new CloseAlarmChangingMessage(null));
        }

        void IRecipient<OpenAlarmEditMessage>.Receive(OpenAlarmEditMessage message)
        {
            var form = message.Value;

            Form = new AlarmCreationForm()
            {
                Name = form?.Name ?? "",
                Time = form?.Time ?? TimeOnly.Parse("00:00:00"),
                DayMask = form?.DayMask ?? 0,
                IsSoundEnabled = form?.IsSoundEnabled ?? true,
                Sound = form?.Sound ?? Domain.Enums.AlarmSound.Alarm1
            };

            id = form?.Id ?? -1;
        }
    }
}
