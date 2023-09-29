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

namespace Synergy.StandardApps.Alarms.ViewModels.ChangeAlarmVMs
{
    public abstract class ChangeAlarmVM : ObservableObject
    {
        protected readonly IAlarmService _alarmService;

        private AlarmCreationForm _form;
        public AlarmCreationForm Form => _form;

        public IEnumerable<DayOfWeek> AlarmedDays => Form.GetAlarmedDays();

        private bool isCancelEnabled;
        public bool IsCancelEnabled
        {
            get => isCancelEnabled;
            set => SetProperty(ref isCancelEnabled, value);
        }

        protected ChangeAlarmVM(IAlarmService alarmService, AlarmForm form)
        {
            _alarmService = alarmService;
            _form = new AlarmCreationForm()
            {
                Name = form.Name,
                Time = form.Time,
                DayMask = form.DayMask,
            };
            IsCancelEnabled = false;
        }

        protected ChangeAlarmVM(IAlarmService alarmService)
        {
            _alarmService = alarmService;

            _form = new AlarmCreationForm()
            {
                Name = "",
                Time = new TimeOnly(),
            };
        }

        public abstract ICommand Save { get; }

        private RelayCommand<string>? setDay;
        public ICommand SetDay => setDay ??
            (setDay = new RelayCommand<string>(param =>
            {
                var day = (DayOfWeek)int.Parse(param);
                
                Form.SetDay(day);
            }));

        private RelayCommand<string>? setAlarmSound;
        public ICommand SetAlarmSound => setAlarmSound ??
            (setAlarmSound = new RelayCommand<string>(str =>
            {
                var sound = (AlarmSound)int.Parse(str);

                Form.Sound = sound;
            }));

        private RelayCommand? playAlarmSound;
        public ICommand PlayAlarmSound => playAlarmSound ??
            (playAlarmSound = new RelayCommand(() =>
            {
                AlarmSoundPlayer.PlaySound(Form.Sound);
            }));

        public abstract ICommand GoBack { get; }
    }
}
