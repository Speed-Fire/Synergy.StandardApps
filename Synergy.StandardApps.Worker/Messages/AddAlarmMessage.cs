using CommunityToolkit.Mvvm.Messaging.Messages;
using Synergy.StandardApps.Domain.Alarm;

namespace Synergy.StandardApps.Worker.Messages
{
    public class AddAlarmMessage : ValueChangedMessage<AlarmRecord>
    {
        public AddAlarmMessage(AlarmRecord value) : base(value)
        {
        }
    }
}
