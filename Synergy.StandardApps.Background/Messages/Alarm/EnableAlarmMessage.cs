using CommunityToolkit.Mvvm.Messaging.Messages;
using Synergy.StandardApps.Domain.Alarm;

namespace Synergy.StandardApps.Background.Messages.Alarm
{
    public class EnableAlarmMessage : ValueChangedMessage<AlarmRecord>
    {
        public EnableAlarmMessage(AlarmRecord value) : base(value)
        {
        }
    }
}
