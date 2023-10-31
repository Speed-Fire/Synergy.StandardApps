using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Synergy.StandardApps.EntityForms.Notes
{
    public class NoteCreationForm : Common.ObservableValidator
    {
        private string name;
        private string description = string.Empty;
        private int color;

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value, true);
        }

        //[Required]
        [MaxLength(500)]
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value, true);
        }

		public Color Color
		{
			get
			{
				var r = (byte)(color & 255);
				var g = (byte)((color & (255 << 8)) >> 8);
				var b = (byte)((color & (255 << 16)) >> 16);

				return Color.FromRgb(r, g, b);
			}

			set
			{
				var r = value.R;
				var g = value.G;
				var b = value.B;

				var clr = r | (g << 8) | (b << 16);
				SetProperty(ref color, clr);
			}
		}

		public int ColorNum => color;

		//public void Validate() => this.ValidateAllProperties();

		public static ValidationResult ValidateName(string name, ValidationContext context)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return new("Note's name is empty or has only whitespaces.");
            }

            return ValidationResult.Success;
        }
    }
}
