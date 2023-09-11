using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Utility.Converters
{
    public interface IConverter<in From, out To>
    {
        To Convert(From entity);
    }
}
