using Synergy.StandardApps.EntityForms.Notes;
using Synergy.StandardApps.Notes.ViewModels.ChangeNoteVMs;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Synergy.StandardApps.Notes.UserControls
{
    /// <summary>
    /// Логика взаимодействия для NoteControl.xaml
    /// </summary>
    public partial class NoteControl : UserControl
    {
        private readonly NoteVM _vm;

        private readonly DoubleAnimation _updatingAnimation;

        public NoteControl(NoteForm form)
        {
            InitializeComponent();

            DataContext = _vm = new NoteVM(form);

            _updatingAnimation = new()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };
        }

        #region UC loading

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _vm.UpdateStart += _vm_UpdateStart;
            _vm.UpdateEnd += _vm_UpdateEnd;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _vm.UpdateStart -= _vm_UpdateStart;
            _vm.UpdateEnd -= _vm_UpdateEnd;
        }

        #endregion

        #region VM event handlers

        private void _vm_UpdateStart()
        {
            Opacity = 0;
        }

        private void _vm_UpdateEnd()
        {
            BeginAnimation(Control.OpacityProperty, _updatingAnimation);
        }

        #endregion
    }
}
