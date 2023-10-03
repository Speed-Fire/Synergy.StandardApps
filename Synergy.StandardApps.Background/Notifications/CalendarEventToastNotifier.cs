using Microsoft.Toolkit.Uwp.Notifications;
using Synergy.StandardApps.Domain.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace Synergy.StandardApps.Background.Notifications
{
    public class CalendarEventToastNotifier : INotifier<CalendarEvent>
    {
        internal static readonly int notificationId = 4894106;

        public void Notify(CalendarEvent entity)
        {
            var builder = new ToastContentBuilder();

            builder
                .AddArgument("action", "calendarEvent")
                .AddArgument("calendarEventId", notificationId)
                .AddText(entity.Title)
                .AddText(entity.Description)
                .SetToastScenario(ToastScenario.Reminder)
                .Show(toast =>
                {
                    toast.ExpirationTime = DateTime.Now.AddMinutes(1);
                    toast.ExpiresOnReboot = true;
                    toast.Priority = ToastNotificationPriority.High;
                });
        }
    }
}
