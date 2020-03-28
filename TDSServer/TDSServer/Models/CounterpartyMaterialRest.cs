using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDSServer.Models
{
    public class CounterpartyMaterialRest
    {
        public int CounterpartyId { get; set; }
        public int MaterialId { get; set; }
        public double Rest { get; set; }
    }
}
