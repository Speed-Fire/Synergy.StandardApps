using CommunityToolkit.Mvvm.Input;
using Synergy.StandardApps.Settings.Appliers;
using Synergy.WPF.Navigation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Synergy.StandardApps.Settings.ViewModels
{
    public class SettingsVM : ViewModel
    {
        public ISettingApplier<string> LanguageApplier { get; }
        public ISettingApplier<string> NotesCleaningApplier { get; }

        public SettingsVM()
        {
            LanguageApplier = new LanguageSettingAplier();
            NotesCleaningApplier = new NotesCleaningSettingApplier();
        }
    }
}
