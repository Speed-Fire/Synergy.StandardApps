using Microsoft.Extensions.DependencyInjection;
using Synergy.WPF.Common.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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

            _themeController.SetTheme("Dark");

            MainWindow = Program.AppHost.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }
    }
}
