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

        #region DragMove

        private volatile int _dragState = 0;

		private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && _dragState == 0)
            {
				_dragState = 1;
			}
        }

		private void Border_MouseMove(object sender, MouseEventArgs e)
		{
            if (_dragState != 1) return;

            _dragState = 2;

			if (this.WindowState == WindowState.Maximized)
				this.WindowState = WindowState.Normal;

			MoveBottomRightEdgeOfWindowToMousePosition();


			this.DragMove();
		}

		private void Border_MouseUp(object sender, MouseButtonEventArgs e)
		{
            _dragState = 0;
		}

		private void MoveBottomRightEdgeOfWindowToMousePosition()
		{
			var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
			var mouse = transform.Transform(GetMousePosition());
			Left = mouse.X - ActualWidth / 2;
			Top = mouse.Y;
		}

		public System.Windows.Point GetMousePosition()
		{
			System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
			return new System.Windows.Point(point.X, point.Y);
		}

		#endregion

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
