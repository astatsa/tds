using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Models
{
    public class Measure : BaseModel
    {
        public string Name { get; set; }
        public string FullName { get; set; }
    }
}
