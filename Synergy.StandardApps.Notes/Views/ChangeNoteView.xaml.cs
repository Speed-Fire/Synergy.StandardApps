using Synergy.StandardApps.Notes.ViewModels;
using Synergy.StandardApps.Notes.ViewModels.ChangeNoteVMs;
using Synergy.StandardApps.Service.Notes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Synergy.StandardApps.Notes.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateNotePage.xaml
    /// </summary>
    public partial class ChangeNoteView : UserControl
    {
        public ChangeNoteView()
        {
            InitializeComponent();
        }

		#region Methods

		private void Init()
		{
			if (DataContext is not ChangeNoteVM vm) return;

			DataContext = null;

			if (!SetCurrentColorButton(vm.ProtoNote.Color))
			{
				vm.ProtoNote.Color = Colors.Yellow;
				ColorBtn0.IsChecked = true;
			}

			DataContext = vm;
		}

		private bool SetCurrentColorButton(Color color)
		{
			var colors = new List<Color>()
			{
				Colors.Yellow, Colors.Red, Colors.OliveDrab,
				Colors.CornflowerBlue, Colors.YellowGreen, Colors.BlueViolet,
				Colors.HotPink
			};

			var btns = new List<RadioButton>()
			{
				ColorBtn0, ColorBtn1, ColorBtn2, ColorBtn3,
				ColorBtn4, ColorBtn5, ColorBtn6,
			};

			var itr = 0;

			foreach (var clr in colors)
			{
				if (color.R == clr.R && color.G == clr.G && color.B == clr.B)
				{
					btns[itr].IsChecked = true;
					return true;
				}

				itr++;
			}

			return false;
		}

		#endregion

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			Init();
        }
    }
}
