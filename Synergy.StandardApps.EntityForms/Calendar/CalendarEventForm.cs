using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.EntityForms.Calendar
{
    public class CalendarEventForm
    {
        private long _id;
        private string _title;
        private string _description;
        private int _color;
        private int _month;
        private int _day;

        public long Id
        {
            get => _id;
            set => _id = value;
        }

        public string Title
        {
            get => _title;
            set => _title = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public int Color
        {
            get => _color;
            set => _color = value;
        }

        public int Month
        {
            get => _month;
            set => _month = value;
        }

        public int Day
        {
            get => _day;
            set => _day = value;
        }

        public Color GetColor()
        {
            var r = (byte)(_color & 255);
            var g =  (byte)((_color & (255 << 8)) >> 8);
            var b =  (byte)((_color & (255 << 16)) >> 16);

            return System.Windows.Media.Color.FromRgb(r, g, b);
        }
    }
}
