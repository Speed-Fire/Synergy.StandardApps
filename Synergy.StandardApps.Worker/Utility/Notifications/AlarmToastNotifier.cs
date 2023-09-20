using Microsoft.Toolkit.Uwp.Notifications;
using Synergy.StandardApps.Domain.Alarm;
using System.Reflection;
using Windows.UI.Notifications;

namespace Synergy.StandardApps.Worker.Utility.Notifications
{
    public class AlarmToastNotifier : INotifier<AlarmRecord>
    {
        internal static readonly int notificationId = 897340;

        public void Notify(AlarmRecord entity)
        {
            var builder = new ToastContentBuilder();

            var audio = new ToastAudio()
            {
                Loop = true,
                Silent = false,
                Src = new Uri("ms-winsoundevent:Notification.Looping.Alarm")
            };

            builder
                .AddArgument("action", "alarm")
                .AddArgument("alarmId", notificationId)
                .AddText(entity.Name)
                .AddText("Your alarm is ringing.")
                .AddInlineImage(new Uri(GetAppAbsolutePath("Resources/alarm.png")))
                .AddButton(new ToastButton()
                {
                    ActivationOptions = new()
                    {
                        AfterActivationBehavior = ToastAfterActivationBehavior.Default
                    }
                }
                    .SetContent("Stop")
                    .AddArgument("action", "stop"))
                .AddAudio(audio)
                .Show(toast =>
                {
                    toast.ExpirationTime = DateTime.Now.AddMinutes(1);
                    toast.ExpiresOnReboot = true;
                    toast.Priority = ToastNotificationPriority.High;
                });
        }

        /// <summary>
        /// This method seems a bit complicated for fetching a file's path,
        /// but it's flexible enough to fetch a path for both console 
        /// applications and service applications.
        /// </summary>
        /// <param name="relativePath">Relative path to a resource.</param>
        /// <returns>Absolute path.</returns>
        private string GetAppAbsolutePath(string relativePath)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), relativePath);
        }
    }
}
