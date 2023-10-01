using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Synergy.StandardApps.Alarms;
using Synergy.StandardApps.Calendar;
using Synergy.StandardApps.Notes;
using Synergy.WPF.Common.Extensions;
using Synergy.WPF.Common.Tray;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Synergy.StandardApps.ViewModels
{
    public class MainVM : ObservableObject
    {
        #region Properties

        private bool _showInTaskbar;
        public bool ShowInTaskbar
        {
            get => _showInTaskbar;
            set => SetProperty(ref _showInTaskbar, value);
        }

        private WindowState _windowState;
        public WindowState WindowState
        {
            get => _windowState;
            set
            {
                ShowInTaskbar = true;
                SetProperty(ref _windowState, value);
                ShowInTaskbar = value != WindowState.Minimized;
            }
        }

        private NotifyIconWrapper.NotifyRequestRecord notifyRequest;
        public NotifyIconWrapper.NotifyRequestRecord NotifyRequest
        {
            get => notifyRequest;
            set => SetProperty(ref notifyRequest, value);
        }

        #endregion

        #region Commands

        #region Navigation

        private RelayCommand<Frame> navigateToNotes;
        public ICommand NavigateToNotes => navigateToNotes ??
            (navigateToNotes = new RelayCommand<Frame>(frame =>
            {
                //var page = Program.AppHost.Services
                //    .GetRequiredService<NotesPage>();             

                frame.NavigateNoJournal(Program.AppHost.Services
                    .GetRequiredService<NotesPage>);
            }));

        private RelayCommand<Frame> navigateToAlarms;
        public ICommand NavigateToAlarms => navigateToAlarms ??
            (navigateToAlarms = new RelayCommand<Frame>(frame =>
            {
                //var page = Program.AppHost.Services
                //    .GetRequiredService<AlarmsPage>();

                frame.NavigateNoJournal(Program.AppHost.Services
                    .GetRequiredService<AlarmsPage>);
            }));

        private RelayCommand<Frame> navigateToCalendar;
        public ICommand NavigateToCalendar => navigateToCalendar ??
            (navigateToCalendar = new RelayCommand<Frame>(frame =>
            {
                //var page = Program.AppHost.Services
                //    .GetRequiredService<CalendarPage>();

                frame.NavigateNoJournal(Program.AppHost.Services
                    .GetRequiredService<CalendarPage>);
            }));

        #endregion

        #region Tray

        private RelayCommand<CancelEventArgs> closingCommand;
        public ICommand ClosingCommand => closingCommand ??
            (closingCommand = new RelayCommand<CancelEventArgs>(e =>
            {
                if (e is null)
                    return;

                e.Cancel = true;
                WindowState = WindowState.Minimized;
            }));

        private RelayCommand notifyIconOpen;
        public ICommand NotifyIconOpen => notifyIconOpen ??
            (notifyIconOpen = new RelayCommand(() =>
            {
                WindowState = WindowState.Normal;
            }));

        private RelayCommand notifyIconExit;
        public ICommand NotifyIconExit => notifyIconExit ??
            (notifyIconExit = new RelayCommand(() =>
            {
                Application.Current.Shutdown();
            }));

        private RelayCommand<string> trayNotify;
        public ICommand TrayNotify => trayNotify ??
            (trayNotify = new RelayCommand<string>(msg =>
            {
                NotifyRequest = new()
                {
                    Title = "Notification",
                    Message = msg
                };
            }));

        #endregion

        #endregion
    }
}
