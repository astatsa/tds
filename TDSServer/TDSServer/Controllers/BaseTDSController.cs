﻿using Microsoft.AspNetCore.Mvc;
using TDSServer.Models;

namespace TDSServer.Controllers
{
    public class BaseTDSController : ControllerBase
    {
        protected ApiResult<T> ApiResult<T>(T result, string error = null)
        {
            return new ApiResult<T>(result, error);
        }

        protected ApiResult<T> ApiResult<T>()
        {
            return new ApiResult<T>();
        }

    }
}