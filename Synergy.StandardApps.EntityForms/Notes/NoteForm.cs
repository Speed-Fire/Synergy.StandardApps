using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.EntityForms.Notes
{
    public class NoteForm : ObservableObject
    {
        private long id;
        private string name;
        private string description;
        private int color;
        private DateOnly created;
        private DateOnly updated;

        public long Id
        {
            get => id;
            internal set => id = value;
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public int Color
        {
            get => color;
            set => SetProperty(ref color, value);
        }

        public DateOnly Created
        {
            get => created;
            set => created = value;
        }

        public DateOnly Updated
        {
            get => updated;
            set => SetProperty(ref updated, value);
        }

		public Color GetColor()
		{
			var r = (byte)(color & 255);
			var g = (byte)((color & (255 << 8)) >> 8);
			var b = (byte)((color & (255 << 16)) >> 16);

			return System.Windows.Media.Color.FromRgb(r, g, b);
		}
	}
}
