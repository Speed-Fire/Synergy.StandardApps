﻿using CommunityToolkit.Mvvm.Input;
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
    public class UpdateAlarmVM : ChangeAlarmVM
    {
        private long Id { get; set; }

        public UpdateAlarmVM(IAlarmService alarmService, AlarmCreationForm form, long id) : 
            base(alarmService, form)
        {
            Id = id;
        }

        private AsyncRelayCommand? save;
        public override ICommand Save => save ??
            (save = new AsyncRelayCommand(UpdateAlarm));

        private async Task UpdateAlarm()
        {
            if (Form.HasErrors)
                return;

            var res = await _alarmService.UpdateAlarm(Form, Id);
            if(res.StatusCode == Domain.Enums.StatusCode.Error)
            {
                await NotifyingGrid.ShowNotificationAsync("MainGrid",
                    "Error", "Specified time is already taken!",
                    System.Windows.MessageBoxButton.OK);

                return;
            }
        }
    }
}
