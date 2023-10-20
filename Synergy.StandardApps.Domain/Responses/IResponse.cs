using Synergy.StandardApps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Domain.Responses
{
    public interface IResponse<T>
    {
        StatusCode StatusCode { get; }
        Exception? Error { get; }
        T? Data { get; }
    }
}
