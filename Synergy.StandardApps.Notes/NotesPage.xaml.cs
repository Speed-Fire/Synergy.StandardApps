using CommunityToolkit.Mvvm.Messaging;
using Synergy.StandardApps.Notes.Messages;
using Synergy.StandardApps.Notes.SubPages;
using Synergy.StandardApps.Notes.ViewModels;
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

namespace Synergy.StandardApps.Notes
{
    /// <summary>
    /// Логика взаимодействия для NotesPage.xaml
    /// </summary>
    public partial class NotesPage : Page, IRecipient<NoteNavigateMessage>
    {
        private readonly NotesVM notesVM;

        public NotesPage(NotesVM vm)
        {
            DataContext = notesVM = vm;

            InitializeComponent();

            WeakReferenceMessenger.Default.Register(this);

            //NoteFrame.Navigate(new Uri("pack://application:,,,/Synergy.StandardApps.Notes;component/SubPages/NoteViewPage.xaml"));
            NoteFrame.Navigate(new NoteViewPage());
        }

        void IRecipient<NoteNavigateMessage>.Receive(NoteNavigateMessage message)
        {
            var navserv = NoteFrame.NavigationService;

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
