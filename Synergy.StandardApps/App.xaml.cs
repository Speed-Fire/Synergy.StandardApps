using Microsoft.Extensions.DependencyInjection;
using Synergy.StandardApps.Resources;
using Synergy.WPF.Common.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace Synergy.StandardApps
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly AppThemeController _themeController;

        public App(AppThemeController themeController)
        {
            InitializeComponent();

            _themeController = themeController;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InitSettings();

            _themeController.SetTheme("Dark");

            MainWindow = Program.AppHost.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private static void InitSettings()
        {
            var settingsInit = Settings.Properties.CurrentLanguage;
            var langController = AppLanguageController.Instance;

            RegisterExceptions();
        }

        private static void RegisterExceptions()
        {
            var exceptionTranslator = ExceptionTranslator.Instance;

            exceptionTranslator
                .Register<Service.Exceptions.InvalidFormException>("Exceptions.Common.InvalidForm")
                .Register<Service.Exceptions.InvalidIdException>("Exceptions.Common.InvalidId")
                .Register<Service.Exceptions.NameIsAlreadyTakenExceptionException>("Exceptions.Common.NameIsAlreadyTaken")
                .Register<Service.Exceptions.CalendarDateIsAlreadyTakenException>("Exceptions.Common.CalendarDateIsAlreadyTaken")
                .Register<Service.Exceptions.AlarmTimeIsAlreadyTakenException>("Exceptions.Common.AlarmTimeIsAlreadyTaken");
        }
    }
}
