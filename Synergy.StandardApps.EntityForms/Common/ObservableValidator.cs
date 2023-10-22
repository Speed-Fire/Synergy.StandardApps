using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.EntityForms.Common
{
	public class ObservableValidator : ObservableObject, INotifyDataErrorInfo
	{
		private readonly Dictionary<string, List<string>> _errors = new();

		public bool HasErrors => _errors.Any();

		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		public void Validate() => Validate(false);

		private void Validate(bool silent = false)
		{
			var properties = this.GetType().GetProperties();

			foreach(var property in properties)
			{
				var attributes = property
					.GetCustomAttributes(true)
					.Where(a => a.GetType().IsAssignableTo(typeof(ValidationAttribute)));

				foreach (ValidationAttribute attribute in attributes)
				{
					var val = property.GetValue(this);

					if (!attribute.IsValid(property.GetValue(this)))
						AddError(property.Name, attribute.ErrorMessage ?? "", silent);
				}
			}
		}

		private void Validate(string propertyName)
		{
			var property = this.GetType().GetProperty(propertyName)
				?? throw new Exception($"Property {propertyName} not found.");

			var attributes = property
				.GetCustomAttributes(true)
				.Where(a => a.GetType().IsAssignableTo(typeof(ValidationAttribute)));

			foreach(ValidationAttribute attribute in attributes)
			{
				if (!attribute.IsValid(property.GetValue(this)))
					AddError(propertyName, attribute.ErrorMessage ?? "");
			}
		}

		public IEnumerable GetErrors(string propertyName)
		{
			return _errors.GetValueOrDefault(propertyName);
		}

		private void AddError(string propertyName, string errorMessage, bool silent = false)
		{
			if (!_errors.ContainsKey(propertyName))
				_errors.Add(propertyName, new());

			_errors[propertyName].Add(errorMessage);

			if (!silent)
				OnErrorChanged(propertyName);
		}

		private void OnErrorChanged(string propertyName)
		{
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}

		protected bool SetProperty<T>(ref T field, T value, bool validate,
			[CallerMemberName]string propertyName = null)
		{
			_errors.Remove(propertyName);
			bool setted;
			if ((setted = SetProperty(ref field, value, propertyName)) && validate)
			{
				Validate(propertyName);
			}

			return setted;
		}
	}
}
