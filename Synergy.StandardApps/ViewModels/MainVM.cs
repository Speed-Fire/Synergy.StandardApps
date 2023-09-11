using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Synergy.StandardApps.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Synergy.StandardApps.ViewModels
{
    public class MainVM : ObservableObject
    {

        private RelayCommand<Frame> navigateToNotes;
        public RelayCommand<Frame> NavigateToNotes => navigateToNotes ??
            (navigateToNotes = new RelayCommand<Frame>(frame =>
            {
                var page = Program.AppHost.Services
                    .GetRequiredService<NotesPage>();

                frame.Navigate(page);
            }));
    }
}
