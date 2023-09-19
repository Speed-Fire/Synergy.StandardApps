using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Synergy.StandardApps.EntityForms.Alarm;
using Synergy.StandardApps.Service.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Synergy.StandardApps.Alarms.ViewModels.ChangeAlarmVMs
{
    public abstract class ChangeAlarmVM : ObservableObject
    {
        protected readonly IAlarmService _alarmService;

        private AlarmCreationForm _form;
        public AlarmCreationForm Form => _form;

        public IEnumerable<DayOfWeek> AlarmedDays => Form.GetAlarmedDays();

        protected ChangeAlarmVM(IAlarmService alarmService, AlarmCreationForm form)
        {
            _alarmService = alarmService;
            _form = form;
        }

        public abstract ICommand Save { get; }

        private RelayCommand<int>? setDay;
        public ICommand SetDay => setDay ??
            (setDay = new RelayCommand<int>(param =>
            {
                var day = (DayOfWeek)param;
                
                Form.SetDay(day);
            }));
    }
}
