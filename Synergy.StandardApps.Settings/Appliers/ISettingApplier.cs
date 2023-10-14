using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Settings.Appliers
{
    public interface ISettingApplier<T>
    {
        IEnumerable<T> Values { get; }
        T? Value { get; set; }
    }

    public abstract class SettingApplier<T> : ObservableObject, ISettingApplier<T>
    {
        public abstract IEnumerable<T> Values { get; }

        private T? _value;
        public T? Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public SettingApplier()
        {
            _value = Init();
        }

        protected abstract T? Init();
        protected abstract void Apply(T? value);

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            Apply(Value);
        }
    }
}
