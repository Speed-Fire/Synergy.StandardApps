using Synergy.StandardApps.EntityForms.Calendar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class CalendarDay : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int day;
        public int Day
        {
            get
            {
                return day;
            }
            set
            {
                day = value;
                OnPropertyChanged(nameof(Day));
            }
        }

        private Color color;
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        private string? eventName;
        public string EventName
        {
            get
            {
                return eventName;
            }
            set
            {
                eventName = value;
                OnPropertyChanged(nameof(EventName));
            }
        }

        private Color seasonColor;
        public Color SeasonColor
        {
            get
            {
                return seasonColor;
            }
            set
            {
                seasonColor = value;
                OnPropertyChanged(nameof(SeasonColor));
            }
        }

        public CalendarDay(int day, int month, bool outOfMonth = false)
        {
            InitializeComponent();

            Day = day;
            EventName = "";

            if (!outOfMonth)
            {
                SetSeasonColor(month);
                Color = Colors.OrangeRed;
            }
            else
            {
                SetSeasonColor(13);
                Color = Colors.Brown;
                IsEnabled = false;
            }
        }

        public CalendarDay(CalendarEventForm form)
        {
            InitializeComponent();

            SetSeasonColor(form.Month);

            Day = form.Day;
            EventName = form.Title;
            Color = form.GetColor();
        }

#if DEBUG

        public CalendarDay()
        {
            InitializeComponent();

            SetSeasonColor(1);

            Day = 31;
            Color = Colors.DarkRed;
            EventName = "New Year";
        }

#endif

        private void SetSeasonColor(int month)
        {
            switch (month)
            {
                // winter
                case 12:
                case 1:
                case 2:
                    SeasonColor = Colors.SkyBlue;
                    break;

                // spring
                case 3:
                case 4:
                case 5:
                    SeasonColor = Colors.LawnGreen;
                    break;

                // summer
                case 6:
                case 7:
                case 8:
                    SeasonColor = Colors.Orange;
                    break;

                // autumn
                case 9:
                case 10:
                case 11:
                    SeasonColor = Colors.DarkRed;
                    break;

                default:
                    SeasonColor = Colors.LightGray;
                    break;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
