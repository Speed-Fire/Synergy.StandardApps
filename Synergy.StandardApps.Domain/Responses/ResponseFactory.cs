using Synergy.StandardApps.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.StandardApps.Domain.Responses
{
    public static class ResponseFactory
    {
        public static IResponse<T> BadResponse<T>(Exception ex)
        {
            return new BaseResponse<T>()
            {
                StatusCode = Domain.Enums.StatusCode.Error,
                Error = ex
            };
        }

        public static IResponse<T> OK<T>(T data)
        {
            return new BaseResponse<T>()
            {
                StatusCode = StatusCode.OK,
                Data = data
            };
        }
    }
}
