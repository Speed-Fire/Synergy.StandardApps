using Synergy.StandardApps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Domain.Alarm
{
    public class AlarmRecord
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public TimeOnly Time { get; set; }
        public WeekDay DayMask { get; set; }
        public bool IsEnabled { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public bool IsAlarmedDay(DateTime date)
        {
            var day = DayOfWeekToWeekDay(date.DayOfWeek);

            return DayMask.HasFlag(day);
        }

        public void SetDay(DayOfWeek day)
        {
            var _day = DayOfWeekToWeekDay(day);

            DayMask ^= _day;
        }

        private static WeekDay DayOfWeekToWeekDay(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return WeekDay.Sunday;
                case DayOfWeek.Monday:
                    return WeekDay.Monday;
                case DayOfWeek.Tuesday:
                    return WeekDay.Tuesday;
                case DayOfWeek.Wednesday:
                    return WeekDay.Wednesday;
                case DayOfWeek.Thursday:
                    return WeekDay.Thursday;
                case DayOfWeek.Friday:
                    return WeekDay.Friday;
                case DayOfWeek.Saturday:
                    return WeekDay.Saturday;
            }

            throw new ArgumentOutOfRangeException(nameof(dayOfWeek));
        }
    }
}
