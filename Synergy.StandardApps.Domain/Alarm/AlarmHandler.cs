using Synergy.StandardApps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Domain.Alarm
{
    public static class AlarmHandler
    {
        public static bool IsAlarmedDay(AlarmRecord alarm, DateTime date)
        {
            return IsAlarmedDay(alarm.DayMask, date);
        }

        public static bool IsAlarmedDay(WeekDay mask, DateTime date)
        {
            var day = DayOfWeekToWeekDay(date.DayOfWeek);

            return mask.HasFlag(day);
        }

        public static bool IsTimeToAlarm(AlarmRecord alarm, DateTime time)
        {
            return IsTimeToAlarm(alarm.Time, time);
        }

        public static bool IsTimeToAlarm(TimeOnly alarmTime, DateTime time)
        {
            if (alarmTime.Hour < time.Hour)
                return true;

            if (alarmTime.Hour > time.Hour)
                return true;

            if (alarmTime.Minute <= time.Minute)
                return true;

            return false;
        }

        public static void SetDay(AlarmRecord alarm, DayOfWeek day)
        {
            var mask = alarm.DayMask;

            SetDay(ref mask, day);

            alarm.DayMask = mask;
        }

        public static void SetDay(ref WeekDay mask, DayOfWeek day)
        {
            var _day = DayOfWeekToWeekDay(day);

            mask ^= _day;
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
