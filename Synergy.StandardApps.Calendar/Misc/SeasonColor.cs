using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Synergy.StandardApps.Calendar.Misc
{
    internal static class SeasonColor
    {
        public static Color Get(int month)
        {
            switch (month)
            {
                // winter
                case 12:
                case 1:
                case 2:
                    return Colors.SkyBlue;

                // spring
                case 3:
                case 4:
                case 5:
                    return Colors.LawnGreen;

                // summer
                case 6:
                case 7:
                case 8:
                    return Colors.Orange;

                // autumn
                case 9:
                case 10:
                case 11:
                    return Colors.DarkRed;

                default:
                    return Colors.LightGray;
            }
        }
    }
}
