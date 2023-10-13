using Synergy.StandardApps.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Synergy.StandardApps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainVM vm)
        {
            InitializeComponent();

            DataContext = vm;

            this.MaxWidth = SystemParameters.WorkArea.Width;
            this.MaxHeight = SystemParameters.WorkArea.Height;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sidebar.SelectedIndex = 0;
            this.WindowState = WindowState.Maximized;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (this.WindowState == WindowState.Maximized)
                    this.WindowState = WindowState.Normal;

                this.DragMove();
            }
        }

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void MaximizeCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void RestoreCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void MinimizeCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            //this.WindowState = WindowState.Minimized;
        }
    }
}
