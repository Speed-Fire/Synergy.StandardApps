using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Synergy.StandardApps.Background.Messages.Alarm
{
    public class DeleteAlarmMessage : ValueChangedMessage<long>
    {
        public DeleteAlarmMessage(long value) : base(value)
        {
        }
    }
}
