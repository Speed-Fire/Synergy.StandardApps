using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Synergy.StandardApps.Settings.Appliers
{
    internal class NotesCleaningSettingApplier : SettingApplier<string>
    {
        private Dictionary<string, int> _mapper = new();
        private List<string> _values = new();
        public override IEnumerable<string> Values => _values;

        protected override void Apply(string? value)
        {
            if (value is null)
                return;

            Settings.Properties.NoteMaxAge = _mapper[value];
            Settings.Properties.Save();
        }

        protected override string? Init()
        {
            var days_30 = (string)Application.Current.FindResource("Strings.30Days");
            var days_60 = (string)Application.Current.FindResource("Strings.60Days");
            var never = (string)Application.Current.FindResource("Strings.Never");

            _mapper.Add(days_30, 30);
            _mapper.Add(days_60, 60);
            _mapper.Add(never, int.MaxValue);

            _values.Add(days_30);
            _values.Add(days_60);
            _values.Add(never);

            return _mapper.First(p => p.Value == Settings.Properties.NoteMaxAge).Key;
        }
    }
}
