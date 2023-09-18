using CommunityToolkit.Mvvm.ComponentModel;
using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.EntityForms.Alarm
{
    public class AlarmForm : ObservableObject
    {
        private long _id;
        private string _name;
        private TimeOnly _time;
        private WeekDay _dayMask;
        private bool _isEnabled;

        public long Id
        {
            get => _id;
            internal set => _id = value;
        }

        public string Name
        {
            get => _name;
            internal set => _name = value;
        }

        public TimeOnly Time
        {
            get => _time;
            internal set => _time = value;
        }

        public WeekDay DayMask
        {
            get => _dayMask; 
            internal set => _dayMask = value;
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
    }
}
