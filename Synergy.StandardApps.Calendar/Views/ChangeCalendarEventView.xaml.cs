using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Calendar.Messages;
using Synergy.StandardApps.Calendar.ViewModels.CalendarEvent;
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

namespace Synergy.StandardApps.Calendar.Views
{
    /// <summary>
    /// Логика взаимодействия для ChangeCalendarEventView.xaml
    /// </summary>
    public partial class ChangeCalendarEventView :
        UserControl,
        INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

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

        public ChangeCalendarEventView()
        {
            InitializeComponent();
        }

        #region Methods

        private void Init()
        {
            var vm = DataContext as ChangeCalendarEventVM;
            DataContext = null;

            SeasonColor = Misc.SeasonColor.Get(vm.Form.Month);

            if (!SetCurrentColorButton(vm.Form.Color))
            {
                vm.Form.Color = Colors.BlueViolet;
                ColorBtn0.IsChecked = true;
            }

            DataContext = vm;
        }

        private bool SetCurrentColorButton(Color color)
        {
            var colors = new List<Color>()
            {
                Colors.BlueViolet, Colors.Red, Colors.OliveDrab,
                Colors.CornflowerBlue, Colors.DarkOrchid, Colors.Goldenrod,
                Colors.HotPink
            };

            var btns = new List<RadioButton>()
            {
                ColorBtn0, ColorBtn1, ColorBtn2, ColorBtn3,
                ColorBtn4, ColorBtn5, ColorBtn6,
            };

            var itr = 0;

            foreach(var clr in colors)
            {
                if(color.R == clr.R && color.G == clr.G && color.B == clr.B)
                {
                    btns[itr].IsChecked = true;
                    return true;
                }

                itr++;
            }

            return false;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Event handlers

        private void CalendarEventChanger_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        #endregion
    }
}
