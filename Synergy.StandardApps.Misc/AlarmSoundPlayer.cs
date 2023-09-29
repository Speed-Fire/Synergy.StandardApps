using Synergy.StandardApps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Synergy.StandardApps.Misc
{
    public static class AlarmSoundPlayer
    {
        private static Dictionary<AlarmSound, string> _alarmSounds = new()
        {
            {AlarmSound.None, "" },
            {AlarmSound.Alarm1, "ms-winsoundevent:Notification.Looping.Alarm" },
            {AlarmSound.Alarm2, "ms-winsoundevent:Notification.Looping.Alarm2" },
            {AlarmSound.Alarm3, "ms-winsoundevent:Notification.Looping.Alarm3" },
            {AlarmSound.Alarm4, "ms-winsoundevent:Notification.Looping.Alarm4" },
            {AlarmSound.Alarm5, "ms-winsoundevent:Notification.Looping.Alarm5" },
        };

        public static string GetPath(AlarmSound sound)
        {
            return _alarmSounds[sound];
        }

        public static void PlaySound(AlarmSound sound)
        {
            //if (sound == AlarmSound.None)
            //    return;

            //var player = new MediaPlayer();
            //player.Open(new Uri(GetPath(sound)));
            //player.Play();
            //player.Close();
        }
    }
}
