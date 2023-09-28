using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Utility.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.EntityForms.Alarm.Converters
{
    public class AlarmRecordToAlarmFormConverter : IConverter<AlarmRecord, AlarmForm>
    {
        public AlarmForm Convert(AlarmRecord entity)
        {
            return new AlarmForm()
            {
                Id = entity.Id,
                Name = entity.Name,
                Time = entity.Time,
                DayMask = entity.DayMask,
                Sound = entity.Sound,
                IsEnabled = entity.IsEnabled
            };
        }
    }
}
