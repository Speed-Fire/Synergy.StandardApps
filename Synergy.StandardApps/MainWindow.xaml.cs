using Synergy.StandardApps.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sidebar.SelectedIndex = 0;
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
            System.Drawing.Rectangle r = Screen.GetWorkingArea(new System.Drawing.Point((int)this.Left, (int)this.Top));
            this.MaxWidth = r.Width;
            this.MaxHeight = r.Height;
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
