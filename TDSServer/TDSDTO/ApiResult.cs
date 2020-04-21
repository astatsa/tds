using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDSDTO
{
    public class ApiResult<T>
    {
        public T Result { get; set; }
        public string Error { get; set; }
    }
}
