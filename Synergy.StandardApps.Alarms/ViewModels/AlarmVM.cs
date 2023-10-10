using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Alarms.Messages;
using Synergy.StandardApps.Alarms.Messages.AlarmChanged;
using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Domain.Enums;
using Synergy.StandardApps.EntityForms.Alarm;
using Synergy.WPF.Navigation.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Synergy.StandardApps.Alarms.ViewModels
{
    public class AlarmVM : 
        ViewModel,
        IRecipient<AlarmUpdatedMessage>
    {
        public class Day : ObservableObject
        {
            private bool _value = false;
            public bool Value
            {
                get => _value;
                set => SetProperty(ref _value, value);
            }
        }

        #region Properties

        public AlarmForm Form { get; private set; }

        private long _id;
        public long Id
        {
            get => _id;
            set => _id = value;
        }

        private string _title = "";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _time = "00:00";
        public string Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        private List<Day> _days = new();
        public List<Day> Days
        {
            get => _days; 
            set => SetProperty(ref _days, value);
        }

        private bool _isAlarmEnabled = false;
        public bool IsAlarmEnabled
        {
            get => _isAlarmEnabled;
            set => SetProperty(ref _isAlarmEnabled, value);
        }

        #endregion

        public AlarmVM(AlarmForm form)
        {
            for(int i = 0; i < 7; i++)
            {
                Days.Add(new());
            }

            Init(form);
        }

        #region Commands

        private RelayCommand? viewLoadedCommand;
        public ICommand ViewLoadedCommand => viewLoadedCommand ??
            (viewLoadedCommand = new RelayCommand(() =>
            {
                IsActive = true;
            }));

        private RelayCommand? viewUnloadedCommand;
        public ICommand ViewUnloadedCommand => viewUnloadedCommand ??
            (viewUnloadedCommand = new RelayCommand(() =>
            {
                IsActive = false;
            }));

        #endregion

        #region Methods

        private void Init(AlarmForm form)
        {
            _isInitRunning = true;

            Form = form;

            Id = form.Id;
            Title = form.Name;
            Time = form.Time.ToString("HH:mm");
            IsAlarmEnabled = form.IsEnabled;

            InitDays(form.DayMask);

            _isInitRunning = false;
        }

        private void InitDays(WeekDay dayMask)
        {
            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                Days[(int)day].Value = AlarmHandler.IsAlarmedDay(dayMask, day);
            }
        }

        #endregion

        #region Messages

        public void Receive(AlarmUpdatedMessage message)
        {
            if(message.Value.Id == Id)
            {
                Init(message.Value);
            }
        }

        #endregion

        private bool _isInitRunning = false;

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if(e.PropertyName == nameof(IsAlarmEnabled))
            {
                if (_isInitRunning) return;

                WeakReferenceMessenger.Default
                    .Send(new AlarmEnabilityChangedMessage(Tuple.Create(Id, IsAlarmEnabled)));
            }
        }
    }
}
