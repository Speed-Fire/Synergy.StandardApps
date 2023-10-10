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

namespace Synergy.StandardApps.Alarms.UserControls
{
    /// <summary>
    /// Логика взаимодействия для AlarmControl.xaml
    /// </summary>
    public partial class AlarmControl : UserControl
    {
        public AlarmControl()
        {
            InitializeComponent();
        }

        private void SliderButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
