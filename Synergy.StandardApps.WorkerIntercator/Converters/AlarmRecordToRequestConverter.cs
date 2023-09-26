using Google.Protobuf.WellKnownTypes;
using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Utility.Converters;
using Synergy.StandardApps.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.WorkerInteractor.Converters
{
    internal class AlarmRecordToRequestConverter : IConverter<AlarmRecord, AlarmRequest>
    {
        public AlarmRequest Convert(AlarmRecord entity)
        {
            var dt = new DateTime(2020, 1, 1) + entity.Time.ToTimeSpan();
            var specified = DateTime.SpecifyKind(dt, DateTimeKind.Utc);

            return new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Time = Timestamp.FromDateTime(specified),
                DayMask = (uint)entity.DayMask,
            };
        }
    }
}
