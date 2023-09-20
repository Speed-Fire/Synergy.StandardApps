using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Alarms.Messages;
using Synergy.StandardApps.Alarms.ViewModels;
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

namespace Synergy.StandardApps.Alarms
{
    /// <summary>
    /// Логика взаимодействия для AlarmsPage.xaml
    /// </summary>
    public partial class AlarmsPage : Page, IRecipient<AlarmNavigateMessage>
    {
        private readonly AlarmsVM alarmsVM;

        public AlarmsPage(AlarmsVM alarmsVM)
        {
            InitializeComponent();
            DataContext = this.alarmsVM = alarmsVM;

            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        void IRecipient<AlarmNavigateMessage>.Receive(AlarmNavigateMessage message)
        {
            var navserv = AlarmFrame.NavigationService;

            if (message.Value is null)
            {
                if (navserv.CanGoBack)
                    navserv.GoBack();
            }
            else
                navserv.Navigate(message.Value);
        }
    }
}
