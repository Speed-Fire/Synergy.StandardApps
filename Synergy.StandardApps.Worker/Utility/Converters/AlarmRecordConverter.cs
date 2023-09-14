using Synergy.StandardApps.Domain.Alarm;
using Synergy.StandardApps.Utility.Converters;

namespace Synergy.StandardApps.Worker.Utility.Converters
{
    public class AlarmRecordConverter : IConverter<AlarmRequest, AlarmRecord>
    {
        public AlarmRecord Convert(AlarmRequest entity)
        {
            return new AlarmRecord
            {
                Id = entity.Id,
                Name = entity.Name,
                Time = TimeOnly.FromDateTime(entity.Time.ToDateTime()),
                DayMask = (Domain.Enums.WeekDay)entity.DayMask
            };
        }
    }
}
