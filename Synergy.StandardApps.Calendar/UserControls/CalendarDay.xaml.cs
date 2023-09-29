using Synergy.StandardApps.EntityForms.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Synergy.StandardApps.Calendar.UserControls
{
    /// <summary>
    /// Логика взаимодействия для CalendarDay.xaml
    /// </summary>
    public partial class CalendarDay : UserControl
    {
        public int Day { get; set; }
        public Color Color { get; set; }
        public string EventName { get; set; } = "";


        public CalendarDay(int day)
        {
            InitializeComponent();

            Color = Colors.OrangeRed;
        }

        public CalendarDay(CalendarEventForm form)
        {
            InitializeComponent();

            Day = form.Day;
            EventName = form.Title;
            Color = form.GetColor();
        }

#if DEBUG

        public CalendarDay()
        {
            InitializeComponent();

            Day = 31;
            Color = Colors.DarkRed;
            EventName = "New Year";
        }

#endif
    }
}
