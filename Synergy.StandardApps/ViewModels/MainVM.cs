using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Synergy.StandardApps.Alarms;
using Synergy.StandardApps.Alarms.ViewModels;
using Synergy.StandardApps.Calendar;
using Synergy.StandardApps.Calendar.ViewModels;
using Synergy.StandardApps.Notes;
using Synergy.StandardApps.Notes.ViewModels;
using Synergy.WPF.Common.Extensions;
using Synergy.WPF.Common.Tray;
using Synergy.WPF.Navigation.Services;
using Synergy.WPF.Navigation.ViewModels;
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
    public class MainVM : ViewModel
    {
        #region Properties

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get => _navigation;
            set => SetProperty(ref _navigation, value);
        }

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

        public MainVM(INavigationService navigationService)
        {
            Navigation = navigationService;
        }

        #region Commands

        #region Loading

        private RelayCommand viewLoadedCommand;
        public ICommand ViewLoadedCommand => viewLoadedCommand ??
            (viewLoadedCommand = new(() =>
            {
                //NavigateToNotesCommand.Execute(null);
            }));

        #endregion

        #region Navigation

        private RelayCommand navigateToNotesCommand;
        public ICommand NavigateToNotesCommand => navigateToNotesCommand ??
            (navigateToNotesCommand = new RelayCommand(() =>
            {
                Navigation.NavigateTo<NotesVM>();
            }));

        private RelayCommand navigateToAlarmsCommand;
        public ICommand NavigateToAlarmsCommand => navigateToAlarmsCommand ??
            (navigateToAlarmsCommand = new RelayCommand(() =>
            {
                Navigation.NavigateTo<AlarmsVM>();
            }));

        private RelayCommand navigateToCalendarCommand;
        public ICommand NavigateToCalendarCommand => navigateToCalendarCommand ??
            (navigateToCalendarCommand = new RelayCommand(() =>
            {
                Navigation.NavigateTo<CalendarVM>();
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

        private RelayCommand notifyIconOpenCommand;
        public ICommand NotifyIconOpenCommand => notifyIconOpenCommand ??
            (notifyIconOpenCommand = new RelayCommand(() =>
            {
                WindowState = WindowState.Normal;
            }));

        private RelayCommand notifyIconExitCommand;
        public ICommand NotifyIconExitCommand => notifyIconExitCommand ??
            (notifyIconExitCommand = new RelayCommand(() =>
            {
                Application.Current.Shutdown();
            }));

        private RelayCommand<string> trayNotifyCommand;
        public ICommand TrayNotifyCommand => trayNotifyCommand ??
            (trayNotifyCommand = new RelayCommand<string>(msg =>
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
