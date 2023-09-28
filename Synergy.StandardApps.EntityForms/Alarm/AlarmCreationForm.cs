using CommunityToolkit.Mvvm.ComponentModel;
using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Domain.Enums;
using System;
using System.Collections.Generic;
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
    }
}
