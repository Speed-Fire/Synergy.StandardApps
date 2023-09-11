using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.EntityForms.Notes
{
    public class NoteCreationForm : ObservableValidator
    {
        private string name;
        private string description;

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value, true);
        }

        [Required]
        [MaxLength(200)]
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value, true);
        }

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
