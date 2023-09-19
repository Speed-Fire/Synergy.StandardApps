using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Synergy.StandardApps.Alarms;
using Synergy.StandardApps.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Synergy.StandardApps.ViewModels
{
    public class MainVM : ObservableObject
    {

        private RelayCommand<Frame> navigateToNotes;
        public ICommand NavigateToNotes => navigateToNotes ??
            (navigateToNotes = new RelayCommand<Frame>(frame =>
            {
                var page = Program.AppHost.Services
                    .GetRequiredService<NotesPage>();
                
                if(frame.CanGoBack)
                    frame.GoBack();

                frame.Navigate(page);
            }));

        private RelayCommand<Frame> navigateToAlarms;
        public ICommand NavigateToAlarms => navigateToAlarms ??
            (navigateToAlarms = new RelayCommand<Frame>(frame =>
            {
                var page = Program.AppHost.Services
                    .GetRequiredService<AlarmsPage>();

                if (frame.CanGoBack)
                    frame.GoBack();

                frame.Navigate(page);
            }));
    }
}
