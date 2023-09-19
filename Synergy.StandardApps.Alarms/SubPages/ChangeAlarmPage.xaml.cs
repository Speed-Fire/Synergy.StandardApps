using Synergy.StandardApps.Alarms.ViewModels.ChangeAlarmVMs;
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

namespace Synergy.StandardApps.Alarms.SubPages
{
    /// <summary>
    /// Логика взаимодействия для ChangeAlarmPage.xaml
    /// </summary>
    public partial class ChangeAlarmPage : Page
    {
        private readonly ChangeAlarmVM _vm;

        public ChangeAlarmPage(ChangeAlarmVM vm)
        {
            InitializeComponent();

            InitToggleButtons(vm.AlarmedDays);

            DataContext = _vm = vm;
        }

        private void InitToggleButtons(IEnumerable<DayOfWeek> days)
        {

        }
    }
}
