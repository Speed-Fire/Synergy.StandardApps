﻿using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Calendar.Messages;
using Synergy.StandardApps.Calendar.ViewModels;
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

namespace Synergy.StandardApps.Calendar
{
    /// <summary>
    /// Логика взаимодействия для CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : 
        Page,
        IRecipient<MonthLoadedMessage>
    {
        private readonly CalendarVM _vm;

        private Thickness ItemMargin { get; }

        public CalendarPage(CalendarVM vm)
        {
            InitializeComponent();
            ItemMargin = new Thickness(15, 10, 15, 10);

            WeakReferenceMessenger.Default
                .RegisterAll(this);

            DataContext = _vm = vm;
        }

        #region Messages

        void IRecipient<MonthLoadedMessage>.Receive(MonthLoadedMessage message)
        {
            LoadMonth();
        }

        #endregion

        private void LoadMonth()
        {

        }
    }
}