using Synergy.StandardApps.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Settings.Appliers
{
    internal class LanguageSettingAplier : SettingApplier<string>
    {
        public override IEnumerable<string> Values => AppLanguageController.Instance.Languages;
        protected override string Init() => AppLanguageController.Instance.CurrentLanguage;

        protected override void Apply(string? value)
        {
            if (value is null)
                return;

            AppLanguageController.Instance.Set(value);
        }
    }
}
