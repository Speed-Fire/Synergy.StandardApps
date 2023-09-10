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
        public byte DayMask { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public static bool IsAlarmedDay(AlarmRecord alarm, DateTime date)
        {
            return (DayOfWeek)((byte)date.DayOfWeek & alarm.DayMask) == date.DayOfWeek;
        }
    }
}
