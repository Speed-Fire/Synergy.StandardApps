using Synergy.StandardApps.Calendar.ViewModels.ChangeCalendarEventVMs;
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

namespace Synergy.StandardApps.Calendar.SubPages
{
    /// <summary>
    /// Логика взаимодействия для ChangeCalendarEventPage.xaml
    /// </summary>
    public partial class ChangeCalendarEventPage : Page
    {
        private readonly ChangeCalendarEventVM _vm;

        public ChangeCalendarEventPage(ChangeCalendarEventVM vm)
        {
            InitializeComponent();

            DataContext = _vm = vm;
        }
    }
}
