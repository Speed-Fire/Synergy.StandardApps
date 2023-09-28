using Synergy.StandardApps.Alarms.ViewModels.ChangeAlarmVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Synergy.StandardApps.Alarms.SubPages
{
    /// <summary>
    /// Логика взаимодействия для ChangeAlarmPage.xaml
    /// </summary>
    public partial class ChangeAlarmPage : Page
    {
        private readonly ChangeAlarmVM _vm;
        private readonly List<ToggleButton> _dayRepetingButtons;

        public ChangeAlarmPage(ChangeAlarmVM vm)
        {
            InitializeComponent();

            _dayRepetingButtons = new()
            {
                SundayBtn, MondayBtn, TuesdayBtn,
                WednesdayBtn, ThursdayBtn, FridayBtn,
                SundayBtn,
            };

            InitToggleButtons(vm.AlarmedDays);

            DataContext = _vm = vm;
        }

        private void InitToggleButtons(IEnumerable<DayOfWeek> days)
        {
            foreach (var day in days)
            {
                switch (day)
                {
                    case DayOfWeek.Sunday:
                        SundayBtn.IsChecked = true;
                        break;
                    case DayOfWeek.Monday:
                        MondayBtn.IsChecked = true;
                        break;
                    case DayOfWeek.Tuesday:
                        TuesdayBtn.IsChecked = true;
                        break;
                    case DayOfWeek.Wednesday:
                        WednesdayBtn.IsChecked = true;
                        break;
                    case DayOfWeek.Thursday:
                        ThursdayBtn.IsChecked = true;
                        break;
                    case DayOfWeek.Friday:
                        FridayBtn.IsChecked = true;
                        break;
                    case DayOfWeek.Saturday:
                        SaturdayBtn.IsChecked = true;
                        break;
                }
            }
        }

        private void EverydayBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach(var btn in _dayRepetingButtons)
            {
                if (btn.IsChecked != true)
                    btn.IsChecked = true;
            }
        }
    }
}
