using CommunityToolkit.Mvvm.Input;
using Synergy.StandardApps.EntityForms.Alarm;
using Synergy.StandardApps.Service.Alarm;
using Synergy.WPF.Common.Controls.NotifyingGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Synergy.StandardApps.Alarms.ViewModels.ChangeAlarmVMs
{
    public class CreateAlarmVM : ChangeAlarmVM
    {
        public CreateAlarmVM(IAlarmService alarmService, AlarmCreationForm form) :
            base(alarmService, form)
        {
        }

        private AsyncRelayCommand? save;
        public override ICommand Save => save ??
            (save = new AsyncRelayCommand(CreateAlarm));

        private async Task CreateAlarm()
        {
            if (Form.HasErrors)
                return;

            var res = await _alarmService.CreateAlarm(Form);
            if (res.StatusCode == Domain.Enums.StatusCode.Error)
            {
                await NotifyingGrid.ShowNotificationAsync("MainGrid",
                    "Error", "Specified time is already taken!",
                    System.Windows.MessageBoxButton.OK);

                return;
            }
        }
    }
}
