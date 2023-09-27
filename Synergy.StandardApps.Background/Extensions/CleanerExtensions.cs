using Microsoft.Toolkit.Uwp.Notifications;
using Synergy.StandardApps.Utility.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Background.Extensions
{
    public static class CleanerExtensions
    {
        public static void CleanNotifications(this Cleaner cleaner)
        {
            ToastNotificationManagerCompat.History.Clear();
        }
    }
}
