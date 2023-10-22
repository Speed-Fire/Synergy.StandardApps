using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Synergy.StandardApps.EntityForms.Calendar
{
    public class CalendarEventCreationForm : Common.ObservableValidator
    {
        private string title;
        private string description;
        private int color;
        private int month;
        private int day;

        [Required]
        [CustomValidation(typeof(CalendarEventCreationForm), "ValidateTitle")]
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value, true);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value, true);
        }

        [CustomValidation(typeof(CalendarEventCreationForm), "ValidateDayMonth")]
        public int Month
        {
            get => month;
            set => SetProperty(ref month, value, true);
        }

        [CustomValidation(typeof(CalendarEventCreationForm), "ValidateDayMonth")]
        public int Day
        {
            get => day;
            set => SetProperty(ref day, value, true);
        }

        public Color Color
        {
            get
            {
                var r = (byte)( color & 255);
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

        public static ValidationResult ValidateTitle(string title, ValidationContext context)
        {
            if(string.IsNullOrWhiteSpace(title))
            {
                return new("Empty event name!");
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidateDayMonth(int dm, ValidationContext context)
        {
            if(dm <= 0)
            {
                return new("Property can't be equal to or less than null!");
            }

            return ValidationResult.Success;
        }
    }
}
