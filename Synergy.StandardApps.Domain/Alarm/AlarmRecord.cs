using Synergy.StandardApps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Domain.Alarm
{
    public class AlarmRecord
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public TimeOnly Time { get; set; }
        public AlarmSound Sound { get; set; } = AlarmSound.Alarm1;
        public bool IsSoundEnabled { get; set; } = true;
        public WeekDay DayMask { get; set; }
        public bool IsEnabled { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
