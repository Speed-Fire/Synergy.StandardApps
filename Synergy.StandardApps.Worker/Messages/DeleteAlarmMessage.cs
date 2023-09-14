using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Synergy.StandardApps.Worker.Messages
{
    public class DeleteAlarmMessage : ValueChangedMessage<long>
    {
        public DeleteAlarmMessage(long value) : base(value)
        {
        }
    }
}
