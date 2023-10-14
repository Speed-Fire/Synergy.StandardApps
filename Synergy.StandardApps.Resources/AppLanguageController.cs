using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace Synergy.StandardApps.Resources
{
    public class AppLanguageController
    {
        private readonly Dictionary<string, string> _languagesDictionary;
        private ResourceDictionary _currentDictionary;

        public IEnumerable<string> Languages => _languagesDictionary.Keys;
        public string CurrentLanguage => Settings.Properties.CurrentLanguage ?? "";

        private static AppLanguageController _instance = null;
        public static AppLanguageController Instance
        {
            get
            {
                _instance ??= new AppLanguageController();

                return _instance;
            }
        }

        private AppLanguageController()
        {
            _languagesDictionary = new()
            {
                { "English", "pack://application:,,,/Synergy.StandardApps.Resources;component/Dictionaries/en.xaml" },
                { "Русский", "pack://application:,,,/Synergy.StandardApps.Resources;component/Dictionaries/ru.xaml" }
            };

            if(!string.IsNullOrEmpty(CurrentLanguage))
            {
                Set_impl(CurrentLanguage);
            }
        }

        public void Set(string language)
        {
            if (CurrentLanguage == language)
                return;

            Set_impl(language);

            Settings.Properties.CurrentLanguage = language;
            Settings.Properties.Save();
        }

        private void Set_impl(string language)
        {
            if (!_languagesDictionary.ContainsKey(language))
                throw new KeyNotFoundException(language);

            if (_currentDictionary != null)
                Application.Current.Resources.MergedDictionaries.Remove(_currentDictionary);

            _currentDictionary = new()
            {
                Source = new(_languagesDictionary[language])
            };

            Application.Current.Resources.MergedDictionaries.Add(_currentDictionary);
        }
    }
}
