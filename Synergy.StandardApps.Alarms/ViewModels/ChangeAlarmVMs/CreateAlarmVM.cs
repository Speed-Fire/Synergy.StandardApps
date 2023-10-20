using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Alarms.Messages;
using Synergy.StandardApps.EntityForms.Alarm;
using Synergy.StandardApps.Resources;
using Synergy.StandardApps.Service.Alarm;
using Synergy.WPF.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Synergy.StandardApps.Alarms.ViewModels.ChangeAlarmVMs
{
    public class CreateAlarmVM :
        ChangeAlarmVM
    {
        private AlarmCreationForm form;
        public override AlarmCreationForm Form
        {
            get => form;
            protected set => SetProperty(ref form, value);
        }

        public CreateAlarmVM(IAlarmService alarmService) :
            base(alarmService)
        {
            IsUpdatingMode = false;

            form = new();
        }

        private AsyncRelayCommand? saveCommand;
        public override ICommand SaveCommand => saveCommand ??
            (saveCommand = new AsyncRelayCommand(CreateAlarm));

        private RelayCommand? deleteCommand;
        public override ICommand DeleteCommand => deleteCommand ??
            (deleteCommand = new(() => { }));

        private async Task CreateAlarm()
        {
            if (Form.HasErrors)
                return;

            var res = await _alarmService.CreateAlarm(Form);
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
                .Send(new AlarmCreatedMessage(res.Data));
            WeakReferenceMessenger.Default
                .Send(new CloseAlarmChangingMessage(null));
        }
    }
}
