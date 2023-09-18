using CommunityToolkit.Mvvm.Messaging.Messages;
using Synergy.StandardApps.Domain.Alarm;

namespace Synergy.StandardApps.Worker.Messages
{
    public class EnableAlarmMessage : ValueChangedMessage<AlarmRecord>
    {
        public EnableAlarmMessage(AlarmRecord value) : base(value)
        {
        }
    }
}
