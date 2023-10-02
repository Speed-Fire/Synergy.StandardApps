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
    /// Логика взаимодействия для CalendarEventView.xaml
    /// </summary>
    public partial class CalendarEventView :
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

        public CalendarEventView()
        {
            InitializeComponent();
        }

        #region Methods

        private void Init()
        {
            var vm = DataContext as CalendarEventVM;
            DataContext = null;

            SeasonColor = Misc.SeasonColor.Get(vm.MonthNum);

            DataContext = vm;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Event handlers

        private void CalendarEventViewer_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        #endregion
    }
}
