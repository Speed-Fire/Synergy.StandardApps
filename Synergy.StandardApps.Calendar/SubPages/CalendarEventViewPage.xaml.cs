using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Calendar.Messages;
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
    /// Логика взаимодействия для CalendarEventViewPage.xaml
    /// </summary>
    public partial class CalendarEventViewPage :
        PageFunction<String>,
        IRecipient<OpenCalendarEventMessage>
    {
        public CalendarEventViewPage()
        {
            InitializeComponent();
        }

        #region Messages

        void IRecipient<OpenCalendarEventMessage>.Receive(OpenCalendarEventMessage message)
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region Page loading handlers

        private void PageFunction_Loaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default
                .Register(this);
        }

        private void PageFunction_Unloaded(object sender, RoutedEventArgs e)
        {
            WeakReferenceMessenger.Default
                .UnregisterAll(this);
        }

        #endregion
    }
}
