using Synergy.StandardApps.Domain.Responses;
using Synergy.StandardApps.EntityForms.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Service.Alarm
{
    public interface IAlarmService
    {
        Task<IResponse<AlarmForm>> CreateAlarm(AlarmCreationForm form);
        Task<IResponse<AlarmForm>> UpdateAlarm(AlarmCreationForm form, long id);
        Task<IResponse<IEnumerable<AlarmForm>>> GetAlarms();
        Task<IResponse<bool>> DeleteAlarm(long id);
        Task<IResponse<bool>> SwitchAlarmEnability(long id, bool isEnabled);
    }
}
