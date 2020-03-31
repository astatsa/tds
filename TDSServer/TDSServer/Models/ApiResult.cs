using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Models
{
    public class ApiResult<T>
    {
        public ApiResult() : this(default, null)
        {
        }
        public ApiResult(T result, string error = null)
        {
            Result = result;
            Error = error;
        }
        public T Result { get; set; }
        public string Error { get; set; }
    }
}
