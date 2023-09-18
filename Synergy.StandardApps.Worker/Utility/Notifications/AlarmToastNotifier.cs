using Microsoft.Toolkit.Uwp.Notifications;
using Synergy.StandardApps.Domain.Alarm;
using Windows.UI.Notifications;

namespace Synergy.StandardApps.Worker.Utility.Notifications
{
    public class AlarmToastNotifier : INotifier<AlarmRecord>
    {
        internal static readonly int notificationId = 897340;

        public void Notify(AlarmRecord entity)
        {
            var builder = new ToastContentBuilder();

            builder
                .AddArgument("action", "alarm")
                .AddArgument("alarmId", notificationId)
                .AddText(entity.Name)
                .AddText("Your alarm rings.")
                .AddInlineImage(new Uri("Resources/alarm.png"))
                .AddButton(new ToastButton()
                    {
                        ActivationOptions = new()
                        {
                            AfterActivationBehavior = ToastAfterActivationBehavior.Default
                        }
                    }
                    .SetContent("Stop")
                    .AddArgument("action", "stop"))
                .AddAudio(new Uri("Resources/tyriam_tyriam.mp3"), true)
                .Show(toast =>
                {
                    toast.ExpirationTime = DateTime.Now.AddMinutes(1);
                });
        }
    }
}
