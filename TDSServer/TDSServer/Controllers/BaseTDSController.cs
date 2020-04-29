using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TDSDTO;

namespace TDSServer.Controllers
{
    public abstract class BaseTDSController : ControllerBase
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
