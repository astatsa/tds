using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Api
{
    class ApiResult<T>
    {
        public T Result { get; set; }
        public string Error { get; set; }
    }
}
