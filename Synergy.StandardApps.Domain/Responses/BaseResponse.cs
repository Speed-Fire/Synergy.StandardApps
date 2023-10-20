using Synergy.StandardApps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Domain.Responses
{
    public class BaseResponse<T> : IResponse<T>
    {
        public StatusCode StatusCode { get; set; }
        public Exception? Error { get; set; }
        public T? Data { get; set; }

        internal BaseResponse(){}
    }
}
