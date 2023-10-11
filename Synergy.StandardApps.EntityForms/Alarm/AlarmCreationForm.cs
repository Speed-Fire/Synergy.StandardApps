using CommunityToolkit.Mvvm.ComponentModel;
using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.EntityForms.Alarm
{
    public class AlarmCreationForm : ObservableValidator
    {
        private string name;
        private TimeOnly time;
        private WeekDay dayMask;
        private AlarmSound sound;
        private bool isSoundEnabled;

        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        [Required]
        public TimeOnly Time
        {
            get => time;
            set => SetProperty(ref time, value);
        }

        [CustomValidation(typeof(AlarmCreationForm), "ValidateDayMask")]
        public WeekDay DayMask
        {
            get => dayMask;
            set => SetProperty(ref dayMask, value);
        }

        #region Days

        private bool _isInitRunning = false;

        private bool monday;
        public bool Monday
        {
            get => monday;
            set => SetProperty(ref monday, value);
        }

        private bool tuesday;
        public bool Tuesday
        {
            get => tuesday;
            set => SetProperty(ref tuesday, value);
        }

        private bool wednesday;
        public bool Wednesday
        {
            get => wednesday;
            set => SetProperty(ref wednesday, value);
        }

        private bool thursday;
        public bool Thursday
        {
            get => thursday;
            set => SetProperty(ref thursday, value);
        }

        private bool friday;
        public bool Friday
        {
            get => friday;
            set => SetProperty(ref friday, value);
        }

        private bool saturday;
        public bool Saturday
        {
            get => saturday;
            set => SetProperty(ref saturday, value);
        }

        private bool sunday;
        public bool Sunday
        {
            get => sunday;
            set => SetProperty(ref sunday, value);
        }

        private void InitDays(WeekDay mask)
        {
            _isInitRunning = true;

            Monday = AlarmHandler.IsAlarmedDay(mask, DayOfWeek.Monday);
            Tuesday = AlarmHandler.IsAlarmedDay(mask, DayOfWeek.Tuesday);
            Wednesday = AlarmHandler.IsAlarmedDay(mask, DayOfWeek.Wednesday);
            Thursday = AlarmHandler.IsAlarmedDay(mask, DayOfWeek.Thursday);
            Friday = AlarmHandler.IsAlarmedDay(mask, DayOfWeek.Friday);
            Saturday = AlarmHandler.IsAlarmedDay(mask, DayOfWeek.Saturday);
            Sunday = AlarmHandler.IsAlarmedDay(mask, DayOfWeek.Sunday);

            _isInitRunning = false;
        }

        #endregion

        [Required]
        public AlarmSound Sound
        {
            get => sound;
            set => SetProperty(ref sound, value);
        }

        [Required]
        public bool IsSoundEnabled
        {
            get => isSoundEnabled;
            set => SetProperty(ref isSoundEnabled, value);
        }

        public AlarmCreationForm()
        {
            name = "";
            Time = new TimeOnly(0, 0);
            sound = AlarmSound.Alarm1;
            isSoundEnabled = true;
        }

        public IEnumerable<DayOfWeek> GetAlarmedDays()
        {
            var res = new List<DayOfWeek>();

            foreach(var day in Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>())
            {
                if(AlarmHandler.IsAlarmedDay(DayMask, day))
                    res.Add(day);
            }

            return res;
        }

        public void SetDay(DayOfWeek day)
        {
            AlarmHandler.SetDay(ref dayMask, day);
        }

        public static ValidationResult ValidateDayMask(WeekDay dayMask, ValidationContext context)
        {
            if (dayMask.Equals(0))
            {
                return new("Alarm must have at least one alarmed day!");
            }

            return ValidationResult.Success;
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            switch (e.PropertyName)
            {
                case nameof(DayMask):
                    InitDays(DayMask);
                    break;

                case nameof(Monday):
                    if (!_isInitRunning)
                        SetDay(DayOfWeek.Monday);
                    break;

                case nameof(Tuesday):
                    if (!_isInitRunning)
                        SetDay(DayOfWeek.Tuesday);
                    break;

                case nameof(Wednesday):
                    if (!_isInitRunning)
                        SetDay(DayOfWeek.Wednesday);
                    break;

                case nameof(Thursday):
                    if (!_isInitRunning)
                        SetDay(DayOfWeek.Thursday);
                    break;

                case nameof(Friday):
                    if (!_isInitRunning)
                        SetDay(DayOfWeek.Friday);
                    break;

                case nameof(Saturday):
                    if (!_isInitRunning)
                        SetDay(DayOfWeek.Saturday);
                    break;

                case nameof(Sunday):
                    if (!_isInitRunning)
                        SetDay(DayOfWeek.Sunday);
                    break;

                default:
                    break;
            }
        }
    }
}
