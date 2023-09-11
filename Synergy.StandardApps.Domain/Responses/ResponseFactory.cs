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
            //if (ex is BaseException _ex)
            //{
            //    return new BaseResponse<T>()
            //    {
            //        StatusCode = Domain.Enums.StatusCode.Error,
            //        ErrorCode = _ex.ErrorCode
            //    };
            //}

            return new BaseResponse<T>()
            {
                StatusCode = Domain.Enums.StatusCode.Error,
                ErrorCode = Domain.Enums.ErrorCode.UnknownError
            };
        }

        public static IResponse<T> BadResponse<T>(ErrorCode ec)
        {
            return new BaseResponse<T>()
            {
                StatusCode = Domain.Enums.StatusCode.Error,
                ErrorCode = ec
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
